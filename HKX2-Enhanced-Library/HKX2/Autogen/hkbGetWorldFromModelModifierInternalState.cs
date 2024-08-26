using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbGetWorldFromModelModifierInternalState Signatire: 0xa92ed39f size: 48 flags: FLAGS_NONE

    // translationOut class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // rotationOut class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkbGetWorldFromModelModifierInternalState : hkReferencedObject, IEquatable<hkbGetWorldFromModelModifierInternalState?>
    {
        public Vector4 translationOut { set; get; }
        public Quaternion rotationOut { set; get; }

        public override uint Signature { set; get; } = 0xa92ed39f;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            translationOut = br.ReadVector4();
            rotationOut = des.ReadQuaternion(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(translationOut);
            s.WriteQuaternion(bw, rotationOut);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            translationOut = xd.ReadVector4(xe, nameof(translationOut));
            rotationOut = xd.ReadQuaternion(xe, nameof(rotationOut));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(translationOut), translationOut);
            xs.WriteQuaternion(xe, nameof(rotationOut), rotationOut);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbGetWorldFromModelModifierInternalState);
        }

        public bool Equals(hkbGetWorldFromModelModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   translationOut.Equals(other.translationOut) &&
                   rotationOut.Equals(other.rotationOut) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(translationOut);
            hashcode.Add(rotationOut);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

