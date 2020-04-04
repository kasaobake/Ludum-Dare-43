using System.Collections.Generic;
using Kameosa.Collections.IList;
using Kameosa.GameObject;
using UnityEditor;
using UnityEngine;

namespace DungeonGeneration
{
    public class DungeonInstantiationComponent : MonoBehaviour
    {
        private const float TILE_GIZMO_SIZE = 0.4f;

        #region Inspector Variables
        [SerializeField]
        private float tileSize = 1f;
        [SerializeField]
        private Vector3 offset = Vector3.zero;
        [SerializeField]
        private DungeonOrientation orientation = DungeonOrientation.Horizontal;
        [Space(10)]

        [Header("Prefabs")]
        [SerializeField]
        private UnityEngine.GameObject hollowTilePrefab;
        [SerializeField]
        private List<UnityEngine.GameObject> solidTilePrefabs;
        [SerializeField]
        private UnityEngine.GameObject cabinet3x2TilePrefab;
        [SerializeField]
        private UnityEngine.GameObject cabinet2x3TilePrefab;
        [SerializeField]
        private UnityEngine.GameObject exitTilePrefab;
        [Space(10)]

        [Header("Gizmos")]
        [SerializeField]
        private bool isShowGizmo = true;
        [SerializeField]
        private bool isShowRoomTypeGizmo = true;
        [SerializeField]
        private bool isShowTileTypeGizmo = true;
        #endregion

        #region Private Variables
        private Dungeon dungeon;
        private string containerName = "Container";
        private Transform container;
        #endregion

        #region Properties
        public DungeonOrientation Orientation
        {
            get
            {
                return this.orientation;
            }

            set
            {
                this.orientation = value;
            }
        }

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
        private void Start()
        {
            //this.tileSize = this.solidTilePrefabs.First().GetComponent<Collider>().bounds.size.x;
        }

        private void OnDrawGizmos()
        {
            if ((this.dungeon != null) && this.isShowGizmo)
            {
                foreach (Room room in this.dungeon)
                {
                    foreach (Tile tile in room)
                    {
                        UnityEngine.Vector3 position = this.orientation == DungeonOrientation.Horizontal ? tile.DungeonCoordinate.XZVector3 : tile.DungeonCoordinate.XZVector3;
                        position *= this.tileSize;

                        UnityEngine.Gizmos.color = Color.white;

                        if (this.dungeon.StartRoomCoordinate.Equals(room.Coordinate))
                        {
                            UnityEngine.Gizmos.color = Color.green;
                        }
                        else if (this.dungeon.ExitRoomCoordinate.Equals(room.Coordinate))
                        {
                            UnityEngine.Gizmos.color = Color.red;
                        }
                        else if (room.Type == RoomType.Side)
                        {
                            UnityEngine.Gizmos.color = Color.black;
                        }

                        if (this.isShowRoomTypeGizmo)
                        {
                            GUIStyle style = new GUIStyle();
                            style.normal.textColor = Color.green;
                            Handles.Label(position, ((int)room.Type).ToString(), style);
                        }
                        else if (this.isShowTileTypeGizmo)
                        {
                            GUIStyle style = new GUIStyle();
                            style.normal.textColor = Color.green;
                            Handles.Label(position, ((char)tile.Type).ToString(), style);
                        }
                        else
                        {
                            UnityEngine.Gizmos.DrawCube(position, UnityEngine.Vector3.one * TILE_GIZMO_SIZE);
                        }
                    }
                }
            }
        }
        #endregion

        #region Public Functions
        public void Instantiate(Dungeon dungeon)
        {
            this.dungeon = dungeon;

            Reset();
            InstantiateRooms();
            InstantiateBoundaries();
            InstantiateExit();

            SetOffset();
        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            OnDrawGizmos();
        }
#endif
        #endregion

        #region Private Functions
        private void Reset()
        {
            if (this.transform.Find(this.containerName))
            {
                DestroyImmediate(this.transform.Find(this.containerName).gameObject);
            }

            this.container = new GameObject(this.containerName).transform;
            this.container.SetParent(this.transform);
        }

        private void InstantiateRooms()
        {
            foreach (Room room in this.dungeon)
            {
                UnityEngine.GameObject roomGameObject = new UnityEngine.GameObject("Room " + room.Coordinate.ToString());
                roomGameObject.transform.SetParent(this.container);

                foreach (Tile tile in room)
                {
                    UnityEngine.GameObject tilePrefab;

                    switch (tile.Type)
                    {
                        case TileType.SOLID:
                            tilePrefab = this.solidTilePrefabs.GetRandom();
                            break;

                        case TileType.CABINET_3X2:
                            tilePrefab = this.cabinet3x2TilePrefab;
                            break;

                        case TileType.CABINET_2X3:
                            tilePrefab = this.cabinet2x3TilePrefab;
                            break;

                        default:
                            tilePrefab = this.hollowTilePrefab;
                            break;
                    }

                    UnityEngine.GameObject.Instantiate(
                        tilePrefab,
                        (this.orientation == DungeonOrientation.Horizontal ? tile.DungeonCoordinate.XZVector3 : tile.DungeonCoordinate.XYVector3) * this.tileSize,
                        Quaternion.identity,
                        roomGameObject.transform);
                }
            }
        }

        private void InstantiateBoundaries()
        {
            UnityEngine.GameObject boundaryGameObject = new UnityEngine.GameObject("Dungeon Boundaries");
            boundaryGameObject.transform.SetParent(this.container);

            foreach (Tile tile in this.dungeon.BorderTiles)
            {
                UnityEngine.GameObject tilePrefab = this.solidTilePrefabs.GetRandom();
                UnityEngine.GameObject.Instantiate(
                    tilePrefab,
                    (this.orientation == DungeonOrientation.Horizontal ? tile.DungeonCoordinate.XZVector3 : tile.DungeonCoordinate.XYVector3) * this.tileSize,
                    Quaternion.identity,
                    boundaryGameObject.transform);
            }
        }

        private void InstantiateExit()
        {
            UnityEngine.GameObject.Instantiate(
                this.exitTilePrefab,
                (this.orientation == DungeonOrientation.Horizontal ? this.dungeon.ExitCoordinate.XZVector3 : this.dungeon.ExitCoordinate.XZVector3) * this.tileSize,
                Quaternion.identity,
                this.container);
        }

        private void SetOffset()
        {
            this.container.transform.position = this.offset;
        }
        #endregion
    }
}