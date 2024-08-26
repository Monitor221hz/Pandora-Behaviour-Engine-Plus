using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpStiffSpringConstraintAtom Signatire: 0x6c128096 size: 8 flags: FLAGS_NONE

    // length class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    public partial class hkpStiffSpringConstraintAtom : hkpConstraintAtom, IEquatable<hkpStiffSpringConstraintAtom?>
    {
        public float length { set; get; }

        public override uint Signature { set; get; } = 0x6c128096;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 2;
            length = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 2;
            bw.WriteSingle(length);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            length = xd.ReadSingle(xe, nameof(length));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(length), length);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpStiffSpringConstraintAtom);
        }

        public bool Equals(hkpStiffSpringConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   length.Equals(other.length) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(length);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

