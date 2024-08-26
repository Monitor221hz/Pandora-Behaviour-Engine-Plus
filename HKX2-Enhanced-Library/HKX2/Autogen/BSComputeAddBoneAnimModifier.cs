using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // BSComputeAddBoneAnimModifier Signatire: 0xa67f8c46 size: 160 flags: FLAGS_NONE

    // boneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // translationLSOut class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // rotationLSOut class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // scaleLSOut class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // pSkeletonMemory class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSComputeAddBoneAnimModifier : hkbModifier, IEquatable<BSComputeAddBoneAnimModifier?>
    {
        public short boneIndex { set; get; }
        public Vector4 translationLSOut { set; get; }
        public Quaternion rotationLSOut { set; get; }
        public Vector4 scaleLSOut { set; get; }
        private object? pSkeletonMemory { set; get; }

        public override uint Signature { set; get; } = 0xa67f8c46;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            boneIndex = br.ReadInt16();
            br.Position += 14;
            translationLSOut = br.ReadVector4();
            rotationLSOut = des.ReadQuaternion(br);
            scaleLSOut = br.ReadVector4();
            des.ReadEmptyPointer(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt16(boneIndex);
            bw.Position += 14;
            bw.WriteVector4(translationLSOut);
            s.WriteQuaternion(bw, rotationLSOut);
            bw.WriteVector4(scaleLSOut);
            s.WriteVoidPointer(bw);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            boneIndex = xd.ReadInt16(xe, nameof(boneIndex));
            translationLSOut = xd.ReadVector4(xe, nameof(translationLSOut));
            rotationLSOut = xd.ReadQuaternion(xe, nameof(rotationLSOut));
            scaleLSOut = xd.ReadVector4(xe, nameof(scaleLSOut));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(boneIndex), boneIndex);
            xs.WriteVector4(xe, nameof(translationLSOut), translationLSOut);
            xs.WriteQuaternion(xe, nameof(rotationLSOut), rotationLSOut);
            xs.WriteVector4(xe, nameof(scaleLSOut), scaleLSOut);
            xs.WriteSerializeIgnored(xe, nameof(pSkeletonMemory));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSComputeAddBoneAnimModifier);
        }

        public bool Equals(BSComputeAddBoneAnimModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   boneIndex.Equals(other.boneIndex) &&
                   translationLSOut.Equals(other.translationLSOut) &&
                   rotationLSOut.Equals(other.rotationLSOut) &&
                   scaleLSOut.Equals(other.scaleLSOut) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(boneIndex);
            hashcode.Add(translationLSOut);
            hashcode.Add(rotationLSOut);
            hashcode.Add(scaleLSOut);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

