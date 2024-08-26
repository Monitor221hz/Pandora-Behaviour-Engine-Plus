using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEventBase Signatire: 0x76bddb31 size: 16 flags: FLAGS_NONE

    // id class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // payload class: hkbEventPayload Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkbEventBase : IHavokObject, IEquatable<hkbEventBase?>
    {
        public int id { set; get; }
        public hkbEventPayload? payload { set; get; }

        public virtual uint Signature { set; get; } = 0x76bddb31;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            id = br.ReadInt32();
            br.Position += 4;
            payload = des.ReadClassPointer<hkbEventPayload>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(id);
            bw.Position += 4;
            s.WriteClassPointer(bw, payload);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            id = xd.ReadInt32(xe, nameof(id));
            payload = xd.ReadClassPointer<hkbEventPayload>(this, xe, nameof(payload));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(id), id);
            xs.WriteClassPointer(xe, nameof(payload), payload);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEventBase);
        }

        public bool Equals(hkbEventBase? other)
        {
            return other is not null &&
                   id.Equals(other.id) &&
                   ((payload is null && other.payload is null) || (payload is not null && other.payload is not null && payload.Equals((IHavokObject)other.payload))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(id);
            hashcode.Add(payload);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

