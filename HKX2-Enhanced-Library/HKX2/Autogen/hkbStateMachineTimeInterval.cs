using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachineTimeInterval Signatire: 0x60a881e5 size: 16 flags: FLAGS_NONE

    // enterEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // exitEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // enterTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // exitTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkbStateMachineTimeInterval : IHavokObject, IEquatable<hkbStateMachineTimeInterval?>
    {
        public static hkbStateMachineTimeInterval GetDefault() => new()
        {
            enterEventId = -1, 
            exitEventId = -1,
            enterTime = 0.0f,
            exitTime = 0.0f,
        };
        public void SetDefault()
        {
            enterEventId = -1;
            exitEventId = -1;
            enterTime = 0.0f;
            exitTime = 0.0f;
        }
        public int enterEventId { set; get; }
        public int exitEventId { set; get; }
        public float enterTime { set; get; }
        public float exitTime { set; get; }

        public virtual uint Signature { set; get; } = 0x60a881e5;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            enterEventId = br.ReadInt32();
            exitEventId = br.ReadInt32();
            enterTime = br.ReadSingle();
            exitTime = br.ReadSingle();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(enterEventId);
            bw.WriteInt32(exitEventId);
            bw.WriteSingle(enterTime);
            bw.WriteSingle(exitTime);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            enterEventId = xd.ReadInt32(xe, nameof(enterEventId));
            exitEventId = xd.ReadInt32(xe, nameof(exitEventId));
            enterTime = xd.ReadSingle(xe, nameof(enterTime));
            exitTime = xd.ReadSingle(xe, nameof(exitTime));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(enterEventId), enterEventId);
            xs.WriteNumber(xe, nameof(exitEventId), exitEventId);
            xs.WriteFloat(xe, nameof(enterTime), enterTime);
            xs.WriteFloat(xe, nameof(exitTime), exitTime);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachineTimeInterval);
        }

        public bool Equals(hkbStateMachineTimeInterval? other)
        {
            return other is not null &&
                   enterEventId.Equals(other.enterEventId) &&
                   exitEventId.Equals(other.exitEventId) &&
                   enterTime.Equals(other.enterTime) &&
                   exitTime.Equals(other.exitTime) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(enterEventId);
            hashcode.Add(exitEventId);
            hashcode.Add(enterTime);
            hashcode.Add(exitTime);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

