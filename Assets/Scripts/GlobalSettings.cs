namespace DefaultNamespace
{
    public enum SongDifficulty
    {
        Easy = 0,
        Medium,
        Hard
    }

    public static class GlobalSettings
    {
        public static SongDifficulty Difficulty { get; set; } = SongDifficulty.Medium;
    }
}