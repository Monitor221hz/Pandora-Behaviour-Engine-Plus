namespace HKX2E
{
    public enum ShaderType : byte
    {
        EFFECT_TYPE_INVALID = 0,
        EFFECT_TYPE_UNKNOWN = 1,
        EFFECT_TYPE_HLSL_INLINE = 2,
        EFFECT_TYPE_CG_INLINE = 3,
        EFFECT_TYPE_HLSL_FILENAME = 4,
        EFFECT_TYPE_CG_FILENAME = 5,
        EFFECT_TYPE_MAX_ID = 6,
    }
}

