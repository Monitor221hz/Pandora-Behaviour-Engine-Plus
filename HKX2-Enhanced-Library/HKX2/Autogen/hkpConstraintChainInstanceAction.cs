using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConstraintChainInstanceAction Signatire: 0xc3971189 size: 56 flags: FLAGS_NONE

    // constraintInstance class: hkpConstraintChainInstance Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: NOT_OWNED|FLAGS_NONE enum: 
    public partial class hkpConstraintChainInstanceAction : hkpAction, IEquatable<hkpConstraintChainInstanceAction?>
    {
        public hkpConstraintChainInstance? constraintInstance { set; get; }

        public override uint Signature { set; get; } = 0xc3971189;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            constraintInstance = des.ReadClassPointer<hkpConstraintChainInstance>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, constraintInstance);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            constraintInstance = xd.ReadClassPointer<hkpConstraintChainInstance>(this, xe, nameof(constraintInstance));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(constraintInstance), constraintInstance);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConstraintChainInstanceAction);
        }

        public bool Equals(hkpConstraintChainInstanceAction? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((constraintInstance is null && other.constraintInstance is null) || (constraintInstance is not null && other.constraintInstance is not null && constraintInstance.Equals((IHavokObject)other.constraintInstance))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(constraintInstance);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

