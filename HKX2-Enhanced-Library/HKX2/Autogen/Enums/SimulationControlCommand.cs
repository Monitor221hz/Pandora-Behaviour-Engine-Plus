namespace HKX2E
{
    public enum SimulationControlCommand : byte
    {
        COMMAND_PLAY = 0,
        COMMAND_PAUSE = 1,
        COMMAND_STEP = 2,
        COMMAND_STOP = 3,
        COMMAND_ACCUMULATE_MOTION = 4,
        COMMAND_DO_NOT_ACCUMULATE_MOTION = 5,
    }
}

