using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCombineTransformsModifier Signatire: 0xfd1f0b79 size: 192 flags: FLAGS_NONE

    // translationOut class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // rotationOut class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // leftTranslation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // leftRotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // rightTranslation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // rightRotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // invertLeftTransform class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // invertRightTransform class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 177 flags: FLAGS_NONE enum: 
    // invertResult class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 178 flags: FLAGS_NONE enum: 
    public partial class hkbCombineTransformsModifier : hkbModifier, IEquatable<hkbCombineTransformsModifier?>
    {
        public Vector4 translationOut { set; get; }
        public Quaternion rotationOut { set; get; }
        public Vector4 leftTranslation { set; get; }
        public Quaternion leftRotation { set; get; }
        public Vector4 rightTranslation { set; get; }
        public Quaternion rightRotation { set; get; }
        public bool invertLeftTransform { set; get; }
        public bool invertRightTransform { set; get; }
        public bool invertResult { set; get; }

        public override uint Signature { set; get; } = 0xfd1f0b79;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            translationOut = br.ReadVector4();
            rotationOut = des.ReadQuaternion(br);
            leftTranslation = br.ReadVector4();
            leftRotation = des.ReadQuaternion(br);
            rightTranslation = br.ReadVector4();
            rightRotation = des.ReadQuaternion(br);
            invertLeftTransform = br.ReadBoolean();
            invertRightTransform = br.ReadBoolean();
            invertResult = br.ReadBoolean();
            br.Position += 13;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(translationOut);
            s.WriteQuaternion(bw, rotationOut);
            bw.WriteVector4(leftTranslation);
            s.WriteQuaternion(bw, leftRotation);
            bw.WriteVector4(rightTranslation);
            s.WriteQuaternion(bw, rightRotation);
            bw.WriteBoolean(invertLeftTransform);
            bw.WriteBoolean(invertRightTransform);
            bw.WriteBoolean(invertResult);
            bw.Position += 13;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            translationOut = xd.ReadVector4(xe, nameof(translationOut));
            rotationOut = xd.ReadQuaternion(xe, nameof(rotationOut));
            leftTranslation = xd.ReadVector4(xe, nameof(leftTranslation));
            leftRotation = xd.ReadQuaternion(xe, nameof(leftRotation));
            rightTranslation = xd.ReadVector4(xe, nameof(rightTranslation));
            rightRotation = xd.ReadQuaternion(xe, nameof(rightRotation));
            invertLeftTransform = xd.ReadBoolean(xe, nameof(invertLeftTransform));
            invertRightTransform = xd.ReadBoolean(xe, nameof(invertRightTransform));
            invertResult = xd.ReadBoolean(xe, nameof(invertResult));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(translationOut), translationOut);
            xs.WriteQuaternion(xe, nameof(rotationOut), rotationOut);
            xs.WriteVector4(xe, nameof(leftTranslation), leftTranslation);
            xs.WriteQuaternion(xe, nameof(leftRotation), leftRotation);
            xs.WriteVector4(xe, nameof(rightTranslation), rightTranslation);
            xs.WriteQuaternion(xe, nameof(rightRotation), rightRotation);
            xs.WriteBoolean(xe, nameof(invertLeftTransform), invertLeftTransform);
            xs.WriteBoolean(xe, nameof(invertRightTransform), invertRightTransform);
            xs.WriteBoolean(xe, nameof(invertResult), invertResult);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCombineTransformsModifier);
        }

        public bool Equals(hkbCombineTransformsModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   translationOut.Equals(other.translationOut) &&
                   rotationOut.Equals(other.rotationOut) &&
                   leftTranslation.Equals(other.leftTranslation) &&
                   leftRotation.Equals(other.leftRotation) &&
                   rightTranslation.Equals(other.rightTranslation) &&
                   rightRotation.Equals(other.rightRotation) &&
                   invertLeftTransform.Equals(other.invertLeftTransform) &&
                   invertRightTransform.Equals(other.invertRightTransform) &&
                   invertResult.Equals(other.invertResult) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(translationOut);
            hashcode.Add(rotationOut);
            hashcode.Add(leftTranslation);
            hashcode.Add(leftRotation);
            hashcode.Add(rightTranslation);
            hashcode.Add(rightRotation);
            hashcode.Add(invertLeftTransform);
            hashcode.Add(invertRightTransform);
            hashcode.Add(invertResult);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

