namespace HKX2E
{
    public enum StateMachineSelfTransitionMode : sbyte
    {
        SELF_TRANSITION_MODE_NO_TRANSITION = 0,
        SELF_TRANSITION_MODE_TRANSITION_TO_START_STATE = 1,
        SELF_TRANSITION_MODE_FORCE_TRANSITION_TO_START_STATE = 2,
    }
}

