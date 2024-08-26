using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBridgeConstraintAtom Signatire: 0x87a4f31b size: 24 flags: FLAGS_NONE

    // buildJacobianFunc class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 8 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // constraintData class: hkpConstraintData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: NOT_OWNED|FLAGS_NONE enum: 
    public partial class hkpBridgeConstraintAtom : hkpConstraintAtom, IEquatable<hkpBridgeConstraintAtom?>
    {
        private object? buildJacobianFunc { set; get; }
        public hkpConstraintData? constraintData { set; get; }

        public override uint Signature { set; get; } = 0x87a4f31b;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 6;
            des.ReadEmptyPointer(br);
            constraintData = des.ReadClassPointer<hkpConstraintData>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 6;
            s.WriteVoidPointer(bw);
            s.WriteClassPointer(bw, constraintData);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            constraintData = xd.ReadClassPointer<hkpConstraintData>(this, xe, nameof(constraintData));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(buildJacobianFunc));
            xs.WriteClassPointer(xe, nameof(constraintData), constraintData);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBridgeConstraintAtom);
        }

        public bool Equals(hkpBridgeConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((constraintData is null && other.constraintData is null) || (constraintData is not null && other.constraintData is not null && constraintData.Equals((IHavokObject)other.constraintData))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(constraintData);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

