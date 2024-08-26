using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaSkeletonMapperData Signatire: 0x95687ea0 size: 128 flags: FLAGS_NONE

    // skeletonA class: hkaSkeleton Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // skeletonB class: hkaSkeleton Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // simpleMappings class: hkaSkeletonMapperDataSimpleMapping Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // chainMappings class: hkaSkeletonMapperDataChainMapping Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // unmappedBones class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // extractedMotionMapping class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // keepUnmappedLocal class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // mappingType class:  Type.TYPE_ENUM Type.TYPE_INT32 arrSize: 0 offset: 116 flags: FLAGS_NONE enum: MappingType
    public partial class hkaSkeletonMapperData : IHavokObject, IEquatable<hkaSkeletonMapperData?>
    {
        public hkaSkeleton? skeletonA { set; get; }
        public hkaSkeleton? skeletonB { set; get; }
        public IList<hkaSkeletonMapperDataSimpleMapping> simpleMappings { set; get; } = Array.Empty<hkaSkeletonMapperDataSimpleMapping>();
        public IList<hkaSkeletonMapperDataChainMapping> chainMappings { set; get; } = Array.Empty<hkaSkeletonMapperDataChainMapping>();
        public IList<short> unmappedBones { set; get; } = Array.Empty<short>();
        public Matrix4x4 extractedMotionMapping { set; get; }
        public bool keepUnmappedLocal { set; get; }
        public int mappingType { set; get; }

        public virtual uint Signature { set; get; } = 0x95687ea0;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            skeletonA = des.ReadClassPointer<hkaSkeleton>(br);
            skeletonB = des.ReadClassPointer<hkaSkeleton>(br);
            simpleMappings = des.ReadClassArray<hkaSkeletonMapperDataSimpleMapping>(br);
            chainMappings = des.ReadClassArray<hkaSkeletonMapperDataChainMapping>(br);
            unmappedBones = des.ReadInt16Array(br);
            extractedMotionMapping = des.ReadQSTransform(br);
            keepUnmappedLocal = br.ReadBoolean();
            br.Position += 3;
            mappingType = br.ReadInt32();
            br.Position += 8;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer(bw, skeletonA);
            s.WriteClassPointer(bw, skeletonB);
            s.WriteClassArray(bw, simpleMappings);
            s.WriteClassArray(bw, chainMappings);
            s.WriteInt16Array(bw, unmappedBones);
            s.WriteQSTransform(bw, extractedMotionMapping);
            bw.WriteBoolean(keepUnmappedLocal);
            bw.Position += 3;
            bw.WriteInt32(mappingType);
            bw.Position += 8;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            skeletonA = xd.ReadClassPointer<hkaSkeleton>(this, xe, nameof(skeletonA));
            skeletonB = xd.ReadClassPointer<hkaSkeleton>(this, xe, nameof(skeletonB));
            simpleMappings = xd.ReadClassArray<hkaSkeletonMapperDataSimpleMapping>(xe, nameof(simpleMappings));
            chainMappings = xd.ReadClassArray<hkaSkeletonMapperDataChainMapping>(xe, nameof(chainMappings));
            unmappedBones = xd.ReadInt16Array(xe, nameof(unmappedBones));
            extractedMotionMapping = xd.ReadQSTransform(xe, nameof(extractedMotionMapping));
            keepUnmappedLocal = xd.ReadBoolean(xe, nameof(keepUnmappedLocal));
            mappingType = xd.ReadFlag<MappingType, int>(xe, nameof(mappingType));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassPointer(xe, nameof(skeletonA), skeletonA);
            xs.WriteClassPointer(xe, nameof(skeletonB), skeletonB);
            xs.WriteClassArray(xe, nameof(simpleMappings), simpleMappings);
            xs.WriteClassArray(xe, nameof(chainMappings), chainMappings);
            xs.WriteNumberArray(xe, nameof(unmappedBones), unmappedBones);
            xs.WriteQSTransform(xe, nameof(extractedMotionMapping), extractedMotionMapping);
            xs.WriteBoolean(xe, nameof(keepUnmappedLocal), keepUnmappedLocal);
            xs.WriteEnum<MappingType, int>(xe, nameof(mappingType), mappingType);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaSkeletonMapperData);
        }

        public bool Equals(hkaSkeletonMapperData? other)
        {
            return other is not null &&
                   ((skeletonA is null && other.skeletonA is null) || (skeletonA is not null && other.skeletonA is not null && skeletonA.Equals((IHavokObject)other.skeletonA))) &&
                   ((skeletonB is null && other.skeletonB is null) || (skeletonB is not null && other.skeletonB is not null && skeletonB.Equals((IHavokObject)other.skeletonB))) &&
                   simpleMappings.SequenceEqual(other.simpleMappings) &&
                   chainMappings.SequenceEqual(other.chainMappings) &&
                   unmappedBones.SequenceEqual(other.unmappedBones) &&
                   extractedMotionMapping.Equals(other.extractedMotionMapping) &&
                   keepUnmappedLocal.Equals(other.keepUnmappedLocal) &&
                   mappingType.Equals(other.mappingType) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(skeletonA);
            hashcode.Add(skeletonB);
            hashcode.Add(simpleMappings.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(chainMappings.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(unmappedBones.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(extractedMotionMapping);
            hashcode.Add(keepUnmappedLocal);
            hashcode.Add(mappingType);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

