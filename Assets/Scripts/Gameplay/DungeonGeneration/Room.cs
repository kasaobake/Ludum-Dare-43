using System.Collections;
using Kameosa.Cartesian;

namespace DungeonGeneration
{
    public class Room : IEnumerable
    {
        #region Private Variables
        private Dungeon dungeon;
        private Coordinate coordinate;
        private Coordinate startTileCoordinate;
        private Coordinate exitTileCoordinate;
        private Tile[,] tiles;
        private RoomType type;
        #endregion

        #region Properties
        public RoomType Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
            }
        }

        public Coordinate Coordinate
        {
            get
            {
                return this.coordinate;
            }

            set
            {
                this.coordinate = new Coordinate(value.X, value.Y);
            }
        }

        public int Width
        {
            get
            {
                return this.tiles.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return this.tiles.GetLength(1);
            }
        }

        public Tile[,] Tiles
        {
            get
            {
                return this.tiles;
            }
        }

        public Coordinate StartTileCoordinate
        {
            get
            {
                return this.startTileCoordinate;
            }

            set
            {
                this.startTileCoordinate = value;
            }
        }

        public Coordinate ExitTileCoordinate
        {
            get
            {
                return this.exitTileCoordinate;
            }

            set
            {
                this.exitTileCoordinate = value;
            }
        }

        public Tile StartTile 
        {
            get
            {
                return this.tiles[this.startTileCoordinate.X, this.startTileCoordinate.Y];
            }
        }

        public Tile ExitTile
        {
            get
            {
                return this.tiles[this.exitTileCoordinate.X, this.exitTileCoordinate.Y];
            }
        }
        #endregion

        public Room(Dungeon dungeon, int x, int y, int roomHeightInTiles = 8, int roomWidthInTiles = 10, RoomType type = RoomType.Side)
        {
            this.dungeon = dungeon;
            this.coordinate = new Coordinate(x, y);
            this.type = type;

            this.tiles = new Tile[roomWidthInTiles, roomHeightInTiles];

            for (int _x = 0; _x < roomWidthInTiles; _x++)
            {
                for (int _y = 0; _y < roomHeightInTiles; _y++)
                {
                    this.tiles[_x, _y] = new Tile(this, _x, _y);
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (int x = 0; x < this.tiles.GetLength(0); x++)
            {
                for (int y = 0; y < this.tiles.GetLength(1); y++)
                {
                    yield return this.tiles[x, y];
                }
            }
        }
    }
}
