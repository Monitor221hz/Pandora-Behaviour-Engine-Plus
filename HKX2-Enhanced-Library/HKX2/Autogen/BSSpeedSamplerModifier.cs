using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSSpeedSamplerModifier Signatire: 0xd297fda9 size: 96 flags: FLAGS_NONE

    // state class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // direction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // goalSpeed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // speedOut class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    public partial class BSSpeedSamplerModifier : hkbModifier, IEquatable<BSSpeedSamplerModifier?>
    {
        public int state { set; get; }
        public float direction { set; get; }
        public float goalSpeed { set; get; }
        public float speedOut { set; get; }

        public override uint Signature { set; get; } = 0xd297fda9;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            state = br.ReadInt32();
            direction = br.ReadSingle();
            goalSpeed = br.ReadSingle();
            speedOut = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(state);
            bw.WriteSingle(direction);
            bw.WriteSingle(goalSpeed);
            bw.WriteSingle(speedOut);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            state = xd.ReadInt32(xe, nameof(state));
            direction = xd.ReadSingle(xe, nameof(direction));
            goalSpeed = xd.ReadSingle(xe, nameof(goalSpeed));
            speedOut = xd.ReadSingle(xe, nameof(speedOut));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(state), state);
            xs.WriteFloat(xe, nameof(direction), direction);
            xs.WriteFloat(xe, nameof(goalSpeed), goalSpeed);
            xs.WriteFloat(xe, nameof(speedOut), speedOut);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSSpeedSamplerModifier);
        }

        public bool Equals(BSSpeedSamplerModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   state.Equals(other.state) &&
                   direction.Equals(other.direction) &&
                   goalSpeed.Equals(other.goalSpeed) &&
                   speedOut.Equals(other.speedOut) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(state);
            hashcode.Add(direction);
            hashcode.Add(goalSpeed);
            hashcode.Add(speedOut);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

