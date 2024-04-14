namespace DefaultNamespace
{
    public enum SongDifficulty
    {
        Easy = 0,
        Normal,
        Hard
    }

    public static class GlobalSettings
    {
        public static SongDifficulty Difficulty { get; set; } = SongDifficulty.Normal;
        public static NoteCalculationType NoteTiming { get; set; } = NoteCalculationType.Normal;
        public static int NoteOffsetMillis { get; set; } = 0;
        public static bool PendingRestart { get; set; }
    }
}