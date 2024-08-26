namespace HKX2E
{
    public enum NodeType : byte
    {
        NODE_TYPE_UNKNOWN = 0,
        NODE_TYPE_NODE = 1,
        NODE_TYPE_TRANSITION = 2,
        NODE_TYPE_WILDCARD_TRANSITION = 3,
        NODE_TYPE_STATE = 4,
        NODE_TYPE_STATE_MACHINE = 5,
        NODE_TYPE_MODIFIER_GENERATOR = 6,
        NODE_TYPE_MODIFIER = 7,
        NODE_TYPE_CLIP = 8,
        NODE_TYPE_BLEND = 9,
        NODE_TYPE_TRANSITION_EFFECT = 10,
    }
}

