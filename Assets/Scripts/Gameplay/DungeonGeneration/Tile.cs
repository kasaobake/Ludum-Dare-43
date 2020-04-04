using Kameosa.Cartesian;

namespace DungeonGeneration
{
    public class Tile
    {
        private Room room;
        private Coordinate roomCoordinate;
        private Coordinate dungeonCoordinate;
        private char type;

        #region Properties
        public char Type
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

        public Coordinate RoomCoordinate
        {
            get
            {
                return this.roomCoordinate;
            }
        }

        public Coordinate DungeonCoordinate
        {
            get
            {
                return this.dungeonCoordinate;
            }
        }
        #endregion

        public Tile(Room room, int x, int y, char type = TileType.HOLLOW)
        {
            this.room = room;
            this.roomCoordinate = new Coordinate(x, y);
            this.dungeonCoordinate = new Coordinate((this.room.Coordinate.X * this.room.Width) + x, (this.room.Coordinate.Y * this.room.Height) + y);
            this.type = type;
        }

        public Tile(int x, int y, char type = TileType.HOLLOW)
        {
            this.dungeonCoordinate = new Coordinate(x, y);
            this.type = type;
        }
    }
}
