using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbClientCharacterState Signatire: 0xa2624c97 size: 272 flags: FLAGS_NONE

    // deformableSkinIds class:  Type.TYPE_ARRAY Type.TYPE_UINT64 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // rigidSkinIds class:  Type.TYPE_ARRAY Type.TYPE_UINT64 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // externalEventIds class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // auxiliaryInfo class: hkbAuxiliaryNodeInfo Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // activeEventIds class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // activeVariableIds class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // instanceName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // templateName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // fullPathToProject class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // behaviorData class: hkbBehaviorGraphData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // behaviorInternalState class: hkbBehaviorGraphInternalState Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    // nodeIdToInternalStateMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 160 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // visible class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // elapsedSimulationTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 172 flags: FLAGS_NONE enum: 
    // skeleton class: hkaSkeleton Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // worldFromModel class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 192 flags: FLAGS_NONE enum: 
    // poseModelSpace class:  Type.TYPE_ARRAY Type.TYPE_QSTRANSFORM arrSize: 0 offset: 240 flags: FLAGS_NONE enum: 
    // rigidAttachmentTransforms class:  Type.TYPE_ARRAY Type.TYPE_QSTRANSFORM arrSize: 0 offset: 256 flags: FLAGS_NONE enum: 
    public partial class hkbClientCharacterState : hkReferencedObject, IEquatable<hkbClientCharacterState?>
    {
        public IList<ulong> deformableSkinIds { set; get; } = Array.Empty<ulong>();
        public IList<ulong> rigidSkinIds { set; get; } = Array.Empty<ulong>();
        public IList<short> externalEventIds { set; get; } = Array.Empty<short>();
        public IList<hkbAuxiliaryNodeInfo> auxiliaryInfo { set; get; } = Array.Empty<hkbAuxiliaryNodeInfo>();
        public IList<short> activeEventIds { set; get; } = Array.Empty<short>();
        public IList<short> activeVariableIds { set; get; } = Array.Empty<short>();
        public ulong characterId { set; get; }
        public string instanceName { set; get; } = "";
        public string templateName { set; get; } = "";
        public string fullPathToProject { set; get; } = "";
        public hkbBehaviorGraphData? behaviorData { set; get; }
        public hkbBehaviorGraphInternalState? behaviorInternalState { set; get; }
        private object? nodeIdToInternalStateMap { set; get; }
        public bool visible { set; get; }
        public float elapsedSimulationTime { set; get; }
        public hkaSkeleton? skeleton { set; get; }
        public Matrix4x4 worldFromModel { set; get; }
        public IList<Matrix4x4> poseModelSpace { set; get; } = Array.Empty<Matrix4x4>();
        public IList<Matrix4x4> rigidAttachmentTransforms { set; get; } = Array.Empty<Matrix4x4>();

        public override uint Signature { set; get; } = 0xa2624c97;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            deformableSkinIds = des.ReadUInt64Array(br);
            rigidSkinIds = des.ReadUInt64Array(br);
            externalEventIds = des.ReadInt16Array(br);
            auxiliaryInfo = des.ReadClassPointerArray<hkbAuxiliaryNodeInfo>(br);
            activeEventIds = des.ReadInt16Array(br);
            activeVariableIds = des.ReadInt16Array(br);
            characterId = br.ReadUInt64();
            instanceName = des.ReadStringPointer(br);
            templateName = des.ReadStringPointer(br);
            fullPathToProject = des.ReadStringPointer(br);
            behaviorData = des.ReadClassPointer<hkbBehaviorGraphData>(br);
            behaviorInternalState = des.ReadClassPointer<hkbBehaviorGraphInternalState>(br);
            des.ReadEmptyPointer(br);
            visible = br.ReadBoolean();
            br.Position += 3;
            elapsedSimulationTime = br.ReadSingle();
            skeleton = des.ReadClassPointer<hkaSkeleton>(br);
            br.Position += 8;
            worldFromModel = des.ReadQSTransform(br);
            poseModelSpace = des.ReadQSTransformArray(br);
            rigidAttachmentTransforms = des.ReadQSTransformArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteUInt64Array(bw, deformableSkinIds);
            s.WriteUInt64Array(bw, rigidSkinIds);
            s.WriteInt16Array(bw, externalEventIds);
            s.WriteClassPointerArray(bw, auxiliaryInfo);
            s.WriteInt16Array(bw, activeEventIds);
            s.WriteInt16Array(bw, activeVariableIds);
            bw.WriteUInt64(characterId);
            s.WriteStringPointer(bw, instanceName);
            s.WriteStringPointer(bw, templateName);
            s.WriteStringPointer(bw, fullPathToProject);
            s.WriteClassPointer(bw, behaviorData);
            s.WriteClassPointer(bw, behaviorInternalState);
            s.WriteVoidPointer(bw);
            bw.WriteBoolean(visible);
            bw.Position += 3;
            bw.WriteSingle(elapsedSimulationTime);
            s.WriteClassPointer(bw, skeleton);
            bw.Position += 8;
            s.WriteQSTransform(bw, worldFromModel);
            s.WriteQSTransformArray(bw, poseModelSpace);
            s.WriteQSTransformArray(bw, rigidAttachmentTransforms);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            deformableSkinIds = xd.ReadUInt64Array(xe, nameof(deformableSkinIds));
            rigidSkinIds = xd.ReadUInt64Array(xe, nameof(rigidSkinIds));
            externalEventIds = xd.ReadInt16Array(xe, nameof(externalEventIds));
            auxiliaryInfo = xd.ReadClassPointerArray<hkbAuxiliaryNodeInfo>(this, xe, nameof(auxiliaryInfo));
            activeEventIds = xd.ReadInt16Array(xe, nameof(activeEventIds));
            activeVariableIds = xd.ReadInt16Array(xe, nameof(activeVariableIds));
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            instanceName = xd.ReadString(xe, nameof(instanceName));
            templateName = xd.ReadString(xe, nameof(templateName));
            fullPathToProject = xd.ReadString(xe, nameof(fullPathToProject));
            behaviorData = xd.ReadClassPointer<hkbBehaviorGraphData>(this, xe, nameof(behaviorData));
            behaviorInternalState = xd.ReadClassPointer<hkbBehaviorGraphInternalState>(this, xe, nameof(behaviorInternalState));
            visible = xd.ReadBoolean(xe, nameof(visible));
            elapsedSimulationTime = xd.ReadSingle(xe, nameof(elapsedSimulationTime));
            skeleton = xd.ReadClassPointer<hkaSkeleton>(this, xe, nameof(skeleton));
            worldFromModel = xd.ReadQSTransform(xe, nameof(worldFromModel));
            poseModelSpace = xd.ReadQSTransformArray(xe, nameof(poseModelSpace));
            rigidAttachmentTransforms = xd.ReadQSTransformArray(xe, nameof(rigidAttachmentTransforms));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(deformableSkinIds), deformableSkinIds);
            xs.WriteNumberArray(xe, nameof(rigidSkinIds), rigidSkinIds);
            xs.WriteNumberArray(xe, nameof(externalEventIds), externalEventIds);
            xs.WriteClassPointerArray(xe, nameof(auxiliaryInfo), auxiliaryInfo!);
            xs.WriteNumberArray(xe, nameof(activeEventIds), activeEventIds);
            xs.WriteNumberArray(xe, nameof(activeVariableIds), activeVariableIds);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteString(xe, nameof(instanceName), instanceName);
            xs.WriteString(xe, nameof(templateName), templateName);
            xs.WriteString(xe, nameof(fullPathToProject), fullPathToProject);
            xs.WriteClassPointer(xe, nameof(behaviorData), behaviorData);
            xs.WriteClassPointer(xe, nameof(behaviorInternalState), behaviorInternalState);
            xs.WriteSerializeIgnored(xe, nameof(nodeIdToInternalStateMap));
            xs.WriteBoolean(xe, nameof(visible), visible);
            xs.WriteFloat(xe, nameof(elapsedSimulationTime), elapsedSimulationTime);
            xs.WriteClassPointer(xe, nameof(skeleton), skeleton);
            xs.WriteQSTransform(xe, nameof(worldFromModel), worldFromModel);
            xs.WriteQSTransformArray(xe, nameof(poseModelSpace), poseModelSpace);
            xs.WriteQSTransformArray(xe, nameof(rigidAttachmentTransforms), rigidAttachmentTransforms);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbClientCharacterState);
        }

        public bool Equals(hkbClientCharacterState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   deformableSkinIds.SequenceEqual(other.deformableSkinIds) &&
                   rigidSkinIds.SequenceEqual(other.rigidSkinIds) &&
                   externalEventIds.SequenceEqual(other.externalEventIds) &&
                   auxiliaryInfo.SequenceEqual(other.auxiliaryInfo) &&
                   activeEventIds.SequenceEqual(other.activeEventIds) &&
                   activeVariableIds.SequenceEqual(other.activeVariableIds) &&
                   characterId.Equals(other.characterId) &&
                   (instanceName is null && other.instanceName is null || instanceName == other.instanceName || instanceName is null && other.instanceName == "" || instanceName == "" && other.instanceName is null) &&
                   (templateName is null && other.templateName is null || templateName == other.templateName || templateName is null && other.templateName == "" || templateName == "" && other.templateName is null) &&
                   (fullPathToProject is null && other.fullPathToProject is null || fullPathToProject == other.fullPathToProject || fullPathToProject is null && other.fullPathToProject == "" || fullPathToProject == "" && other.fullPathToProject is null) &&
                   ((behaviorData is null && other.behaviorData is null) || (behaviorData is not null && other.behaviorData is not null && behaviorData.Equals((IHavokObject)other.behaviorData))) &&
                   ((behaviorInternalState is null && other.behaviorInternalState is null) || (behaviorInternalState is not null && other.behaviorInternalState is not null && behaviorInternalState.Equals((IHavokObject)other.behaviorInternalState))) &&
                   visible.Equals(other.visible) &&
                   elapsedSimulationTime.Equals(other.elapsedSimulationTime) &&
                   ((skeleton is null && other.skeleton is null) || (skeleton is not null && other.skeleton is not null && skeleton.Equals((IHavokObject)other.skeleton))) &&
                   worldFromModel.Equals(other.worldFromModel) &&
                   poseModelSpace.SequenceEqual(other.poseModelSpace) &&
                   rigidAttachmentTransforms.SequenceEqual(other.rigidAttachmentTransforms) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(deformableSkinIds.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(rigidSkinIds.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(externalEventIds.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(auxiliaryInfo.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(activeEventIds.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(activeVariableIds.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(characterId);
            hashcode.Add(instanceName);
            hashcode.Add(templateName);
            hashcode.Add(fullPathToProject);
            hashcode.Add(behaviorData);
            hashcode.Add(behaviorInternalState);
            hashcode.Add(visible);
            hashcode.Add(elapsedSimulationTime);
            hashcode.Add(skeleton);
            hashcode.Add(worldFromModel);
            hashcode.Add(poseModelSpace.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(rigidAttachmentTransforms.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

