using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterAddedInfo Signatire: 0x3544e182 size: 128 flags: FLAGS_NONE

    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // instanceName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // templateName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // fullPathToProject class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // skeleton class: hkaSkeleton Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // worldFromModel class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // poseModelSpace class:  Type.TYPE_ARRAY Type.TYPE_QSTRANSFORM arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkbCharacterAddedInfo : hkReferencedObject, IEquatable<hkbCharacterAddedInfo?>
    {
        public ulong characterId { set; get; }
        public string instanceName { set; get; } = "";
        public string templateName { set; get; } = "";
        public string fullPathToProject { set; get; } = "";
        public hkaSkeleton? skeleton { set; get; }
        public Matrix4x4 worldFromModel { set; get; }
        public IList<Matrix4x4> poseModelSpace { set; get; } = Array.Empty<Matrix4x4>();

        public override uint Signature { set; get; } = 0x3544e182;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterId = br.ReadUInt64();
            instanceName = des.ReadStringPointer(br);
            templateName = des.ReadStringPointer(br);
            fullPathToProject = des.ReadStringPointer(br);
            skeleton = des.ReadClassPointer<hkaSkeleton>(br);
            br.Position += 8;
            worldFromModel = des.ReadQSTransform(br);
            poseModelSpace = des.ReadQSTransformArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(characterId);
            s.WriteStringPointer(bw, instanceName);
            s.WriteStringPointer(bw, templateName);
            s.WriteStringPointer(bw, fullPathToProject);
            s.WriteClassPointer(bw, skeleton);
            bw.Position += 8;
            s.WriteQSTransform(bw, worldFromModel);
            s.WriteQSTransformArray(bw, poseModelSpace);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            instanceName = xd.ReadString(xe, nameof(instanceName));
            templateName = xd.ReadString(xe, nameof(templateName));
            fullPathToProject = xd.ReadString(xe, nameof(fullPathToProject));
            skeleton = xd.ReadClassPointer<hkaSkeleton>(this, xe, nameof(skeleton));
            worldFromModel = xd.ReadQSTransform(xe, nameof(worldFromModel));
            poseModelSpace = xd.ReadQSTransformArray(xe, nameof(poseModelSpace));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteString(xe, nameof(instanceName), instanceName);
            xs.WriteString(xe, nameof(templateName), templateName);
            xs.WriteString(xe, nameof(fullPathToProject), fullPathToProject);
            xs.WriteClassPointer(xe, nameof(skeleton), skeleton);
            xs.WriteQSTransform(xe, nameof(worldFromModel), worldFromModel);
            xs.WriteQSTransformArray(xe, nameof(poseModelSpace), poseModelSpace);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterAddedInfo);
        }

        public bool Equals(hkbCharacterAddedInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterId.Equals(other.characterId) &&
                   (instanceName is null && other.instanceName is null || instanceName == other.instanceName || instanceName is null && other.instanceName == "" || instanceName == "" && other.instanceName is null) &&
                   (templateName is null && other.templateName is null || templateName == other.templateName || templateName is null && other.templateName == "" || templateName == "" && other.templateName is null) &&
                   (fullPathToProject is null && other.fullPathToProject is null || fullPathToProject == other.fullPathToProject || fullPathToProject is null && other.fullPathToProject == "" || fullPathToProject == "" && other.fullPathToProject is null) &&
                   ((skeleton is null && other.skeleton is null) || (skeleton is not null && other.skeleton is not null && skeleton.Equals((IHavokObject)other.skeleton))) &&
                   worldFromModel.Equals(other.worldFromModel) &&
                   poseModelSpace.SequenceEqual(other.poseModelSpace) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterId);
            hashcode.Add(instanceName);
            hashcode.Add(templateName);
            hashcode.Add(fullPathToProject);
            hashcode.Add(skeleton);
            hashcode.Add(worldFromModel);
            hashcode.Add(poseModelSpace.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

