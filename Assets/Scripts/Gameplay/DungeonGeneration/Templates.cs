namespace DungeonGeneration
{
    [System.Serializable]
    public class Templates
    {
        public Template[] sideRooms;
        public Template[] leftRightRooms;
        public Template[] leftRightDownRooms;
        public Template[] leftRightUpRooms;
        public Template[] leftRightUpDownRooms;

        public Template[] obstacle5x5Blocks;
    }
}