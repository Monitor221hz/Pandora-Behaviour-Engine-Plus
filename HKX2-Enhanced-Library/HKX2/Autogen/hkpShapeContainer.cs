using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpShapeContainer Signatire: 0xe0708a00 size: 8 flags: FLAGS_NONE


    public partial class hkpShapeContainer : IHavokObject, IEquatable<hkpShapeContainer?>
    {
        private byte[] unk0 = new byte[8];

        public virtual uint Signature { set; get; } = 0xe0708a00;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            unk0 = br.ReadBytes(8);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteBytes(unk0);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {

        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {

        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpShapeContainer);
        }

        public bool Equals(hkpShapeContainer? other)
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

