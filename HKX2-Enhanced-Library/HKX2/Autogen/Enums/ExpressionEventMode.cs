namespace HKX2E
{
    public enum ExpressionEventMode : sbyte
    {
        EVENT_MODE_SEND_ONCE = 0,
        EVENT_MODE_SEND_ON_TRUE = 1,
        EVENT_MODE_SEND_ON_FALSE_TO_TRUE = 2,
        EVENT_MODE_SEND_EVERY_FRAME_ONCE_TRUE = 3,
    }
}

