using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterSteppedInfo Signatire: 0x2eda84f8 size: 112 flags: FLAGS_NONE

    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // deltaTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // worldFromModel class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // poseModelSpace class:  Type.TYPE_ARRAY Type.TYPE_QSTRANSFORM arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // rigidAttachmentTransforms class:  Type.TYPE_ARRAY Type.TYPE_QSTRANSFORM arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    public partial class hkbCharacterSteppedInfo : hkReferencedObject, IEquatable<hkbCharacterSteppedInfo?>
    {
        public ulong characterId { set; get; }
        public float deltaTime { set; get; }
        public Matrix4x4 worldFromModel { set; get; }
        public IList<Matrix4x4> poseModelSpace { set; get; } = Array.Empty<Matrix4x4>();
        public IList<Matrix4x4> rigidAttachmentTransforms { set; get; } = Array.Empty<Matrix4x4>();

        public override uint Signature { set; get; } = 0x2eda84f8;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterId = br.ReadUInt64();
            deltaTime = br.ReadSingle();
            br.Position += 4;
            worldFromModel = des.ReadQSTransform(br);
            poseModelSpace = des.ReadQSTransformArray(br);
            rigidAttachmentTransforms = des.ReadQSTransformArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(characterId);
            bw.WriteSingle(deltaTime);
            bw.Position += 4;
            s.WriteQSTransform(bw, worldFromModel);
            s.WriteQSTransformArray(bw, poseModelSpace);
            s.WriteQSTransformArray(bw, rigidAttachmentTransforms);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            deltaTime = xd.ReadSingle(xe, nameof(deltaTime));
            worldFromModel = xd.ReadQSTransform(xe, nameof(worldFromModel));
            poseModelSpace = xd.ReadQSTransformArray(xe, nameof(poseModelSpace));
            rigidAttachmentTransforms = xd.ReadQSTransformArray(xe, nameof(rigidAttachmentTransforms));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteFloat(xe, nameof(deltaTime), deltaTime);
            xs.WriteQSTransform(xe, nameof(worldFromModel), worldFromModel);
            xs.WriteQSTransformArray(xe, nameof(poseModelSpace), poseModelSpace);
            xs.WriteQSTransformArray(xe, nameof(rigidAttachmentTransforms), rigidAttachmentTransforms);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterSteppedInfo);
        }

        public bool Equals(hkbCharacterSteppedInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterId.Equals(other.characterId) &&
                   deltaTime.Equals(other.deltaTime) &&
                   worldFromModel.Equals(other.worldFromModel) &&
                   poseModelSpace.SequenceEqual(other.poseModelSpace) &&
                   rigidAttachmentTransforms.SequenceEqual(other.rigidAttachmentTransforms) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterId);
            hashcode.Add(deltaTime);
            hashcode.Add(worldFromModel);
            hashcode.Add(poseModelSpace.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(rigidAttachmentTransforms.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

