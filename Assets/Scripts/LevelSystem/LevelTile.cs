namespace GG.LevelSystem
{
    [System.Serializable]
    public class LevelTile
    {
        public int id;
        public TilePosition position;
        public string character;
        public int[] children;
    }
}