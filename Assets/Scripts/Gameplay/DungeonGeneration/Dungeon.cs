using System.Collections;
using Kameosa.Cartesian;
using UnityEngine;

namespace DungeonGeneration
{
    public class Dungeon : IEnumerable
    {
        Tile[] borderTiles;
        Room[,] rooms;
        Coordinate startRoomCoordinate;
        Coordinate exitRoomCoordinate;

        int roomWidthInTiles;
        int roomHeightInTiles;

        #region properties

        public Room[,] Rooms
        {
            get
            {
                return this.rooms;
            }

            set
            {
                this.rooms = value;
            }
        }

        public Coordinate StartRoomCoordinate
        {
            get
            {
                return this.startRoomCoordinate;
            }

            set
            {
                this.startRoomCoordinate = new Coordinate(value.X, value.Y);
            }
        }

        public Coordinate ExitRoomCoordinate
        {
            get
            {
                return this.exitRoomCoordinate;
            }

            set
            {
                this.exitRoomCoordinate = new Coordinate(value.X, value.Y);
            }
        }

        public Coordinate StartCoordinate
        {
            get
            {
                return this.rooms[this.startRoomCoordinate.X, this.startRoomCoordinate.Y].StartTile.DungeonCoordinate;
            }
        }

        public Coordinate ExitCoordinate
        {
            get
            {
                return this.rooms[this.exitRoomCoordinate.X, this.exitRoomCoordinate.Y].ExitTile.DungeonCoordinate;
            }
        }

        public int WidthInRooms
        {
            get
            {
                return this.rooms.GetLength(0);
            }
        }

        public int HeightInRooms
        {
            get
            {
                return this.rooms.GetLength(1);
            }
        }

        public int WidthInTiles
        {
            get
            {
                return WidthInRooms * this.roomWidthInTiles;
            }
        }

        public int HeightInTiles
        {
            get
            {
                return HeightInRooms * this.roomHeightInTiles;
            }
        }

        public Tile[] BorderTiles
        {
            get
            {
                return this.borderTiles;
            }

            set
            {
                this.borderTiles = value;
            }
        }

        public int RoomWidthInTiles
        {
            get
            {
                return this.roomWidthInTiles;
            }
        }

        public int RoomHeightInTiles
        {
            get
            {
                return this.roomHeightInTiles;
            }
        }

        #endregion

        // http://tinysubversions.com/spelunkyGen/
        public Dungeon(int dungeonHeightInRooms = 4, int dungeonWidthInRooms = 4, int roomHeightInTiles = 8, int roomWidthInTiles = 10)
        {
            this.rooms = new Room[dungeonWidthInRooms, dungeonHeightInRooms];
            this.roomWidthInTiles = roomWidthInTiles;
            this.roomHeightInTiles = roomHeightInTiles;

            for (int x = 0; x < dungeonWidthInRooms; x++)
            {
                for (int y = 0; y < dungeonHeightInRooms; y++)
                {
                    this.rooms[x, y] = new Room(this, x, y, roomHeightInTiles, roomWidthInTiles);
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (int x = 0; x < this.rooms.GetLength(0); x++)
            {
                for (int y = 0; y < this.rooms.GetLength(1); y++)
                {
                    yield return this.rooms[x, y];
                }
            }
        }
    }
}