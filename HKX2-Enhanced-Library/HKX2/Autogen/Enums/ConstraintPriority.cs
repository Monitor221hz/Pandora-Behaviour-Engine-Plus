namespace HKX2E
{
    public enum ConstraintPriority : byte
    {
        PRIORITY_INVALID = 0,
        PRIORITY_PSI = 1,
        PRIORITY_SIMPLIFIED_TOI_UNUSED = 2,
        PRIORITY_TOI = 3,
        PRIORITY_TOI_HIGHER = 4,
        PRIORITY_TOI_FORCED = 5,
        NUM_PRIORITIES = 6,
    }
}

