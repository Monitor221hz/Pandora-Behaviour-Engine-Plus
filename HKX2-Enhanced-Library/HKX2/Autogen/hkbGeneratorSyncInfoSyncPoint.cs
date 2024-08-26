using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbGeneratorSyncInfoSyncPoint Signatire: 0xb597cf92 size: 8 flags: FLAGS_NONE

    // id class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    public partial class hkbGeneratorSyncInfoSyncPoint : IHavokObject, IEquatable<hkbGeneratorSyncInfoSyncPoint?>
    {
        public int id { set; get; }
        public float time { set; get; }

        public virtual uint Signature { set; get; } = 0xb597cf92;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            id = br.ReadInt32();
            time = br.ReadSingle();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(id);
            bw.WriteSingle(time);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            id = xd.ReadInt32(xe, nameof(id));
            time = xd.ReadSingle(xe, nameof(time));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(id), id);
            xs.WriteFloat(xe, nameof(time), time);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbGeneratorSyncInfoSyncPoint);
        }

        public bool Equals(hkbGeneratorSyncInfoSyncPoint? other)
        {
            return other is not null &&
                   id.Equals(other.id) &&
                   time.Equals(other.time) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(id);
            hashcode.Add(time);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

