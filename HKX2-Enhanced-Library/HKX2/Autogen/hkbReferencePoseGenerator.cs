using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbReferencePoseGenerator Signatire: 0x26a5675a size: 80 flags: FLAGS_NONE

    // skeleton class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 72 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbReferencePoseGenerator : hkbGenerator, IEquatable<hkbReferencePoseGenerator?>
    {
        private object? skeleton { set; get; }

        public override uint Signature { set; get; } = 0x26a5675a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(skeleton));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbReferencePoseGenerator);
        }

        public bool Equals(hkbReferencePoseGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

