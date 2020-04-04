using Kameosa.Cartesian;
using Kameosa.Collections.IList;
using Kameosa.Services;
using Kameosa.Enumerables;
using UnityEngine;

namespace DungeonGeneration
{
    [RequireComponent(typeof(DungeonInstantiationComponent))]
    public class DungeonGenerationComponent : MonoBehaviour
    {
        #region Inspector Variables
        [Header("Generation Parameters")]
        [SerializeField]
        [Range(1, 10)]
        private int dungeonHeightInRooms = 4;
        [SerializeField]
        [Range(1, 10)]
        private int dungeonWidthInRooms = 4;
        [SerializeField]
        [Range(1, 10)]
        private int roomHeightInTiles = 8;
        [SerializeField]
        [Range(1, 10)]
        private int roomWidthInTiles = 10;
        [SerializeField]
        private string templatesFilename = "RoomTemplates";
        [SerializeField]

        [Header("References")]
        private DungeonInstantiationComponent dungeonInstantiationComponent;
        #endregion

        #region Private Variables
        private Dungeon dungeon;
        private Templates templates;
        #endregion

        #region Properties
        public Dungeon Dungeon
        {
            get
            {
                return this.dungeon;
            }

            set
            {
                this.dungeon = value;
            }
        }
        #endregion

        #region MonoBehaviour Functions
        private void Awake()
        {
            if (this.dungeonInstantiationComponent == null)
            {
                this.dungeonInstantiationComponent = GetComponent<DungeonInstantiationComponent>();
            }
        }
        #endregion

        #region Public Functions
        // http://tinysubversions.com/spelunkyGen/
        public Dungeon Generate()
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

            this.dungeon = new Dungeon(this.dungeonHeightInRooms, this.dungeonWidthInRooms, this.roomHeightInTiles, this.roomWidthInTiles);
            this.templates = FileService.JsonLoad<Templates>(Application.streamingAssetsPath, this.templatesFilename);

            GenerateOptimalSolutionPath();
            GenerateRoomTemplateForAllRooms();
            GenerateObstacleBlockTemplateForAllRooms();
            HandleReservedAndProbabilisticTilesForAllRooms();
            GenerateDungeonBorder();

            this.dungeonInstantiationComponent.Instantiate(this.dungeon);

            return this.dungeon;
        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            this.dungeonInstantiationComponent.DrawGizmos();
        }
#endif
        #endregion

        #region Private Functions
        private void GenerateOptimalSolutionPath()
        {
            Coordinate currentCoordinate = new Coordinate(Roll(0, this.dungeon.WidthInRooms - 1), this.dungeon.HeightInRooms - 1);
            Kameosa.Enumerables.Direction fromDirection = Kameosa.Enumerables.Direction.Up;
            this.dungeon.StartRoomCoordinate = currentCoordinate;

            do
            {
                Room currentRoom = this.dungeon.Rooms[currentCoordinate.X, currentCoordinate.Y];
                currentRoom.Type = currentRoom.Type == RoomType.Side ? RoomType.LeftRight : currentRoom.Type;
                Kameosa.Enumerables.Direction nextDirection;

                do
                {
                    nextDirection = GetNextDirection();
                } while (nextDirection == fromDirection);

                if ((nextDirection == Kameosa.Enumerables.Direction.Left && currentCoordinate.X == 0)
                    || (nextDirection == Kameosa.Enumerables.Direction.Right && currentCoordinate.X == this.dungeon.WidthInRooms - 1)
                    || (nextDirection == Kameosa.Enumerables.Direction.Down))
                {
                    currentRoom.Type = currentRoom.Type == RoomType.LeftRightUp ? RoomType.LeftRightUpDown : RoomType.LeftRightDown;

                    fromDirection = Kameosa.Enumerables.Direction.Up;
                    currentCoordinate.Y--;

                    if (currentCoordinate.Y >= 0)
                    {
                        currentRoom = this.dungeon.Rooms[currentCoordinate.X, currentCoordinate.Y];
                        currentRoom.Type = RoomType.LeftRightUp;
                    }

                    if (nextDirection == Kameosa.Enumerables.Direction.Left || nextDirection == Kameosa.Enumerables.Direction.Right)
                    {
                        fromDirection = nextDirection;
                        currentCoordinate.X += nextDirection == Kameosa.Enumerables.Direction.Right ? -1 : 1;
                    }
                }
                else
                {
                    currentCoordinate.X += nextDirection == Kameosa.Enumerables.Direction.Right ? 1 : -1;
                }
            } while (currentCoordinate.Y >= 0);

            currentCoordinate.Y++;
            this.dungeon.ExitRoomCoordinate = currentCoordinate;
        }

        private void GenerateRoomTemplateForAllRooms()
        {
            foreach (Room room in this.dungeon)
            {
                Template template;

                switch (room.Type)
                {
                    case RoomType.LeftRight:
                        template = this.templates.leftRightRooms.GetRandom();
                        break;

                    case RoomType.LeftRightDown:
                        template = this.templates.leftRightDownRooms.GetRandom();
                        break;

                    case RoomType.LeftRightUp:
                        template = this.templates.leftRightUpRooms.GetRandom();
                        break;

                    case RoomType.LeftRightUpDown:
                        template = this.templates.leftRightUpDownRooms.GetRandom();
                        break;

                    default:
                        template = this.templates.sideRooms.GetRandom();
                        break;
                }

                for (int x = 0; x < template.value[0].Length; x++)
                {
                    for (int y = 0; y < template.value.Length; y++)
                    {
                        room.Tiles[x, y].Type = template.value[template.value.Length - y - 1][x];
                    }
                }
            }

        }

        private void GenerateObstacleBlockTemplateForAllRooms()
        {
            foreach (Room room in this.dungeon)
            {
                foreach (Tile tile in room.Tiles)
                {
                    if (tile.Type == TileType.OBSTACLE_5X5_BLOCK)
                    {
                        Template template = this.templates.obstacle5x5Blocks.GetRandom();

                        for (int x = tile.RoomCoordinate.X; x < tile.RoomCoordinate.X + template.value[0].Length; x++)
                        {
                            for (int y = tile.RoomCoordinate.Y; y < tile.RoomCoordinate.Y - template.value.Length; y--)
                            {
                                char type = HandleProbabilisticTileType(template.value[template.value.Length - y - 1][x]);
                                room.Tiles[x, y].Type = type;
                            }
                        }

                        for (int x = 0; x < template.value[0].Length; x++)
                        {
                            for (int y = 0; y < template.value.Length; y++)
                            {
                                room.Tiles[tile.RoomCoordinate.X + x, tile.RoomCoordinate.Y - y].Type = template.value[y][x];
                            }
                        }
                    }
                }
            }
        }

        private void HandleReservedAndProbabilisticTilesForAllRooms()
        {
            foreach (Room room in this.dungeon)
            {
                foreach (Tile tile in room.Tiles)
                {
                    switch (tile.Type)
                    {
                        case TileType.START:
                            //room.StartTileCoordinate = tile.RoomCoordinate;
                            tile.Type = TileType.HOLLOW;
                            break;

                        case TileType.EXIT:
                            room.ExitTileCoordinate = tile.RoomCoordinate;
                            tile.Type = TileType.SOLID;
                            break;

                        case TileType.SOLID_1_IN_2:
                            tile.Type = Roll(0, 1) == 0 ? TileType.SOLID : TileType.HOLLOW;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void GenerateDungeonBorder()
        {
            int borderTilesCount = (this.dungeon.WidthInTiles * 2) + (this.dungeon.HeightInTiles * 2) + 4;
            this.dungeon.BorderTiles = new Tile[borderTilesCount];

            int i = 0;

            for (int x = -1; x <= this.dungeon.WidthInTiles; x++)
            {
                this.dungeon.BorderTiles[i++] = new Tile(x, -1, TileType.SOLID);
                this.dungeon.BorderTiles[i++] = new Tile(x, this.dungeon.HeightInTiles, TileType.SOLID);
            }

            for (int y = 0; y < this.dungeon.HeightInTiles; y++)
            {
                this.dungeon.BorderTiles[i++] = new Tile(-1, y, TileType.SOLID);
                this.dungeon.BorderTiles[i++] = new Tile(this.dungeon.WidthInTiles, y, TileType.SOLID);
            }
        }

        /// <summary>
        /// Helper Functions
        /// </summary>
        /// <returns></returns>
        private Kameosa.Enumerables.Direction GetNextDirection()
        {
            int typeIndex = Roll(1, 5);

            switch (typeIndex)
            {
                case 1:
                case 2:
                    return Kameosa.Enumerables.Direction.Left;
                case 3:
                case 4:
                    return Kameosa.Enumerables.Direction.Right;
                default:
                    return Kameosa.Enumerables.Direction.Down;
            }
        }

        private char HandleProbabilisticTileType(char value)
        {
            char result = TileType.HOLLOW;

            switch (value)
            {
                case TileType.SOLID_1_IN_2:
                    break;

                default:
                    result = value;
                    break;
            }

            return result;
        }

        private int Roll(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
        #endregion
    }
}