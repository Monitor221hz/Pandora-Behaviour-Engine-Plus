namespace HKX2E
{
    public enum ToGeneratorState : sbyte
    {
        STATE_INACTIVE = 0,
        STATE_READY_FOR_SET_LOCAL_TIME = 1,
        STATE_READY_FOR_APPLY_SELF_TRANSITION_MODE = 2,
        STATE_ACTIVE = 3,
    }
}

