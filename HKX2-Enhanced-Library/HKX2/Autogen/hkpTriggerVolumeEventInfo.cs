using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpTriggerVolumeEventInfo Signatire: 0xeb60f431 size: 24 flags: FLAGS_NONE

    // sortValue class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // body class: hkpRigidBody Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // operation class:  Type.TYPE_ENUM Type.TYPE_INT32 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: Operation
    public partial class hkpTriggerVolumeEventInfo : IHavokObject, IEquatable<hkpTriggerVolumeEventInfo?>
    {
        public ulong sortValue { set; get; }
        public hkpRigidBody? body { set; get; }
        public int operation { set; get; }

        public virtual uint Signature { set; get; } = 0xeb60f431;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            sortValue = br.ReadUInt64();
            body = des.ReadClassPointer<hkpRigidBody>(br);
            operation = br.ReadInt32();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt64(sortValue);
            s.WriteClassPointer(bw, body);
            bw.WriteInt32(operation);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            sortValue = xd.ReadUInt64(xe, nameof(sortValue));
            body = xd.ReadClassPointer<hkpRigidBody>(this, xe, nameof(body));
            operation = xd.ReadFlag<Operation, int>(xe, nameof(operation));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(sortValue), sortValue);
            xs.WriteClassPointer(xe, nameof(body), body);
            xs.WriteEnum<Operation, int>(xe, nameof(operation), operation);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpTriggerVolumeEventInfo);
        }

        public bool Equals(hkpTriggerVolumeEventInfo? other)
        {
            return other is not null &&
                   sortValue.Equals(other.sortValue) &&
                   ((body is null && other.body is null) || (body is not null && other.body is not null && body.Equals((IHavokObject)other.body))) &&
                   operation.Equals(other.operation) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(sortValue);
            hashcode.Add(body);
            hashcode.Add(operation);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

