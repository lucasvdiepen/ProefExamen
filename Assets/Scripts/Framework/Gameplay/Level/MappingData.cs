namespace ProefExamen.Framework.Gameplay.Level
{
    /// <summary>
    /// A struct holding mapping data seperatly so that it can be defined for each difficulty.
    /// </summary>
    [System.Serializable]
    public struct MappingData
    {
        public Difficulty difficulty;
        public float[] timestamps;
        public int[] laneIDs;
    }
}
