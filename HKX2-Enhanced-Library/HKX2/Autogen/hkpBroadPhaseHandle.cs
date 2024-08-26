using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBroadPhaseHandle Signatire: 0x940569dc size: 4 flags: FLAGS_NONE

    // id class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpBroadPhaseHandle : IHavokObject, IEquatable<hkpBroadPhaseHandle?>
    {
        private uint id { set; get; }

        public virtual uint Signature { set; get; } = 0x940569dc;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            id = br.ReadUInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt32(id);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {

        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(id));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBroadPhaseHandle);
        }

        public bool Equals(hkpBroadPhaseHandle? other)
        {
            return other is not null &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();

            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

