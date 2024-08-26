using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaKeyFrameHierarchyUtility Signatire: 0x7bd5c66f size: 1 flags: FLAGS_NONE


    public partial class hkaKeyFrameHierarchyUtility : IHavokObject, IEquatable<hkaKeyFrameHierarchyUtility?>
    {
        private byte[] unk0 = new byte[1];

        public virtual uint Signature { set; get; } = 0x7bd5c66f;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            unk0 = br.ReadBytes(1);
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
            return Equals(obj as hkaKeyFrameHierarchyUtility);
        }

        public bool Equals(hkaKeyFrameHierarchyUtility? other)
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

