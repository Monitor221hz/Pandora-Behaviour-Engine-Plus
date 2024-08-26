using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBehaviorGraph Signatire: 0xb1218f86 size: 304 flags: FLAGS_NONE

    // variableMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 72 flags: FLAGS_NONE enum: VariableMode
    // uniqueIdPool class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 80 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // idToStateMachineTemplateMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // mirroredExternalIdMap class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 104 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // pseudoRandomGenerator class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 120 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // rootGenerator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // data class: hkbBehaviorGraphData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // rootGeneratorClone class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // activeNodes class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 152 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // activeNodeTemplateToIndexMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 160 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // activeNodesChildrenIndices class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 168 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // globalTransitionData class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 176 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // eventIdMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 184 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // attributeIdMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 192 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // variableIdMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 200 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // characterPropertyIdMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 208 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // variableValueSet class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 216 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // nodeTemplateToCloneMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 224 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // nodeCloneToTemplateMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 232 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // stateListenerTemplateToCloneMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 240 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // nodePartitionInfo class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 248 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // numIntermediateOutputs class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 256 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // jobs class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 264 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // allPartitionMemory class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 280 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // numStaticNodes class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 296 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // nextUniqueId class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 298 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // isActive class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 300 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // isLinked class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 301 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // updateActiveNodes class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 302 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // stateOrTransitionChanged class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 303 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbBehaviorGraph : hkbGenerator, IEquatable<hkbBehaviorGraph?>
    {
        public sbyte variableMode { set; get; }
        public IList<object> uniqueIdPool { set; get; } = Array.Empty<object>();
        private object? idToStateMachineTemplateMap { set; get; }
        public IList<object> mirroredExternalIdMap { set; get; } = Array.Empty<object>();
        private object? pseudoRandomGenerator { set; get; }
        public hkbGenerator? rootGenerator { set; get; }
        public hkbBehaviorGraphData? data { set; get; }
        private object? rootGeneratorClone { set; get; }
        private object? activeNodes { set; get; }
        private object? activeNodeTemplateToIndexMap { set; get; }
        private object? activeNodesChildrenIndices { set; get; }
        private object? globalTransitionData { set; get; }
        private object? eventIdMap { set; get; }
        private object? attributeIdMap { set; get; }
        private object? variableIdMap { set; get; }
        private object? characterPropertyIdMap { set; get; }
        private object? variableValueSet { set; get; }
        private object? nodeTemplateToCloneMap { set; get; }
        private object? nodeCloneToTemplateMap { set; get; }
        private object? stateListenerTemplateToCloneMap { set; get; }
        private object? nodePartitionInfo { set; get; }
        private int numIntermediateOutputs { set; get; }
        public IList<object> jobs { set; get; } = Array.Empty<object>();
        public IList<object> allPartitionMemory { set; get; } = Array.Empty<object>();
        private short numStaticNodes { set; get; }
        private short nextUniqueId { set; get; }
        private bool isActive { set; get; }
        private bool isLinked { set; get; }
        private bool updateActiveNodes { set; get; }
        private bool stateOrTransitionChanged { set; get; }

        public override uint Signature { set; get; } = 0xb1218f86;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            variableMode = br.ReadSByte();
            br.Position += 7;
            des.ReadEmptyArray(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyPointer(br);
            rootGenerator = des.ReadClassPointer<hkbGenerator>(br);
            data = des.ReadClassPointer<hkbBehaviorGraphData>(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            numIntermediateOutputs = br.ReadInt32();
            br.Position += 4;
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            numStaticNodes = br.ReadInt16();
            nextUniqueId = br.ReadInt16();
            isActive = br.ReadBoolean();
            isLinked = br.ReadBoolean();
            updateActiveNodes = br.ReadBoolean();
            stateOrTransitionChanged = br.ReadBoolean();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSByte(variableMode);
            bw.Position += 7;
            s.WriteVoidArray(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidPointer(bw);
            s.WriteClassPointer(bw, rootGenerator);
            s.WriteClassPointer(bw, data);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            bw.WriteInt32(numIntermediateOutputs);
            bw.Position += 4;
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            bw.WriteInt16(numStaticNodes);
            bw.WriteInt16(nextUniqueId);
            bw.WriteBoolean(isActive);
            bw.WriteBoolean(isLinked);
            bw.WriteBoolean(updateActiveNodes);
            bw.WriteBoolean(stateOrTransitionChanged);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            variableMode = xd.ReadFlag<VariableMode, sbyte>(xe, nameof(variableMode));
            rootGenerator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(rootGenerator));
            data = xd.ReadClassPointer<hkbBehaviorGraphData>(this, xe, nameof(data));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<VariableMode, sbyte>(xe, nameof(variableMode), variableMode);
            xs.WriteSerializeIgnored(xe, nameof(uniqueIdPool));
            xs.WriteSerializeIgnored(xe, nameof(idToStateMachineTemplateMap));
            xs.WriteSerializeIgnored(xe, nameof(mirroredExternalIdMap));
            xs.WriteSerializeIgnored(xe, nameof(pseudoRandomGenerator));
            xs.WriteClassPointer(xe, nameof(rootGenerator), rootGenerator);
            xs.WriteClassPointer(xe, nameof(data), data);
            xs.WriteSerializeIgnored(xe, nameof(rootGeneratorClone));
            xs.WriteSerializeIgnored(xe, nameof(activeNodes));
            xs.WriteSerializeIgnored(xe, nameof(activeNodeTemplateToIndexMap));
            xs.WriteSerializeIgnored(xe, nameof(activeNodesChildrenIndices));
            xs.WriteSerializeIgnored(xe, nameof(globalTransitionData));
            xs.WriteSerializeIgnored(xe, nameof(eventIdMap));
            xs.WriteSerializeIgnored(xe, nameof(attributeIdMap));
            xs.WriteSerializeIgnored(xe, nameof(variableIdMap));
            xs.WriteSerializeIgnored(xe, nameof(characterPropertyIdMap));
            xs.WriteSerializeIgnored(xe, nameof(variableValueSet));
            xs.WriteSerializeIgnored(xe, nameof(nodeTemplateToCloneMap));
            xs.WriteSerializeIgnored(xe, nameof(nodeCloneToTemplateMap));
            xs.WriteSerializeIgnored(xe, nameof(stateListenerTemplateToCloneMap));
            xs.WriteSerializeIgnored(xe, nameof(nodePartitionInfo));
            xs.WriteSerializeIgnored(xe, nameof(numIntermediateOutputs));
            xs.WriteSerializeIgnored(xe, nameof(jobs));
            xs.WriteSerializeIgnored(xe, nameof(allPartitionMemory));
            xs.WriteSerializeIgnored(xe, nameof(numStaticNodes));
            xs.WriteSerializeIgnored(xe, nameof(nextUniqueId));
            xs.WriteSerializeIgnored(xe, nameof(isActive));
            xs.WriteSerializeIgnored(xe, nameof(isLinked));
            xs.WriteSerializeIgnored(xe, nameof(updateActiveNodes));
            xs.WriteSerializeIgnored(xe, nameof(stateOrTransitionChanged));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBehaviorGraph);
        }

        public bool Equals(hkbBehaviorGraph? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   variableMode.Equals(other.variableMode) &&
                   ((rootGenerator is null && other.rootGenerator is null) || (rootGenerator is not null && other.rootGenerator is not null && rootGenerator.Equals((IHavokObject)other.rootGenerator))) &&
                   ((data is null && other.data is null) || (data is not null && other.data is not null && data.Equals((IHavokObject)other.data))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(variableMode);
            hashcode.Add(rootGenerator);
            hashcode.Add(data);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

