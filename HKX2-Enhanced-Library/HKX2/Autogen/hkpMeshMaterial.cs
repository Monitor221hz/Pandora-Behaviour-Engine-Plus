using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMeshMaterial Signatire: 0x886cde0c size: 4 flags: FLAGS_NONE

    // filterInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkpMeshMaterial : IHavokObject, IEquatable<hkpMeshMaterial?>
    {
        public uint filterInfo { set; get; }

        public virtual uint Signature { set; get; } = 0x886cde0c;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            filterInfo = br.ReadUInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt32(filterInfo);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            filterInfo = xd.ReadUInt32(xe, nameof(filterInfo));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(filterInfo), filterInfo);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMeshMaterial);
        }

        public bool Equals(hkpMeshMaterial? other)
        {
            return other is not null &&
                   filterInfo.Equals(other.filterInfo) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(filterInfo);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

