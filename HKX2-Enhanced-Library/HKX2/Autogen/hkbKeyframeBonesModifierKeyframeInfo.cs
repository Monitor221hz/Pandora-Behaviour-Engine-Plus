using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbKeyframeBonesModifierKeyframeInfo Signatire: 0x72deb7a6 size: 48 flags: FLAGS_NONE

    // keyframedPosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // keyframedRotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // boneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // isValid class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 34 flags: FLAGS_NONE enum: 
    public partial class hkbKeyframeBonesModifierKeyframeInfo : IHavokObject, IEquatable<hkbKeyframeBonesModifierKeyframeInfo?>
    {
        public Vector4 keyframedPosition { set; get; }
        public Quaternion keyframedRotation { set; get; }
        public short boneIndex { set; get; }
        public bool isValid { set; get; }

        public virtual uint Signature { set; get; } = 0x72deb7a6;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            keyframedPosition = br.ReadVector4();
            keyframedRotation = des.ReadQuaternion(br);
            boneIndex = br.ReadInt16();
            isValid = br.ReadBoolean();
            br.Position += 13;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(keyframedPosition);
            s.WriteQuaternion(bw, keyframedRotation);
            bw.WriteInt16(boneIndex);
            bw.WriteBoolean(isValid);
            bw.Position += 13;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            keyframedPosition = xd.ReadVector4(xe, nameof(keyframedPosition));
            keyframedRotation = xd.ReadQuaternion(xe, nameof(keyframedRotation));
            boneIndex = xd.ReadInt16(xe, nameof(boneIndex));
            isValid = xd.ReadBoolean(xe, nameof(isValid));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(keyframedPosition), keyframedPosition);
            xs.WriteQuaternion(xe, nameof(keyframedRotation), keyframedRotation);
            xs.WriteNumber(xe, nameof(boneIndex), boneIndex);
            xs.WriteBoolean(xe, nameof(isValid), isValid);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbKeyframeBonesModifierKeyframeInfo);
        }

        public bool Equals(hkbKeyframeBonesModifierKeyframeInfo? other)
        {
            return other is not null &&
                   keyframedPosition.Equals(other.keyframedPosition) &&
                   keyframedRotation.Equals(other.keyframedRotation) &&
                   boneIndex.Equals(other.boneIndex) &&
                   isValid.Equals(other.isValid) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(keyframedPosition);
            hashcode.Add(keyframedRotation);
            hashcode.Add(boneIndex);
            hashcode.Add(isValid);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

