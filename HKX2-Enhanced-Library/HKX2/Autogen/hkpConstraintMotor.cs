using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConstraintMotor Signatire: 0x6a44c317 size: 24 flags: FLAGS_NONE

    // type class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: MotorType
    public partial class hkpConstraintMotor : hkReferencedObject, IEquatable<hkpConstraintMotor?>
    {
        public sbyte type { set; get; }

        public override uint Signature { set; get; } = 0x6a44c317;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            type = br.ReadSByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSByte(type);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            type = xd.ReadFlag<MotorType, sbyte>(xe, nameof(type));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<MotorType, sbyte>(xe, nameof(type), type);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConstraintMotor);
        }

        public bool Equals(hkpConstraintMotor? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   type.Equals(other.type) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(type);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

