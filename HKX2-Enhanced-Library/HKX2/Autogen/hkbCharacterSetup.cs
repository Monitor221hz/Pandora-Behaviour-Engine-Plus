using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterSetup Signatire: 0xe5a2a413 size: 88 flags: FLAGS_NONE

    // retargetingSkeletonMappers class: hkaSkeletonMapper Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // animationSkeleton class: hkaSkeleton Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // ragdollToAnimationSkeletonMapper class: hkaSkeletonMapper Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // animationToRagdollSkeletonMapper class: hkaSkeletonMapper Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // animationBindingSet class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 56 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // data class: hkbCharacterData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // mirroredSkeleton class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 72 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // characterPropertyIdMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 80 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbCharacterSetup : hkReferencedObject, IEquatable<hkbCharacterSetup?>
    {
        public IList<hkaSkeletonMapper> retargetingSkeletonMappers { set; get; } = Array.Empty<hkaSkeletonMapper>();
        public hkaSkeleton? animationSkeleton { set; get; }
        public hkaSkeletonMapper? ragdollToAnimationSkeletonMapper { set; get; }
        public hkaSkeletonMapper? animationToRagdollSkeletonMapper { set; get; }
        private object? animationBindingSet { set; get; }
        public hkbCharacterData? data { set; get; }
        private object? mirroredSkeleton { set; get; }
        private object? characterPropertyIdMap { set; get; }

        public override uint Signature { set; get; } = 0xe5a2a413;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            retargetingSkeletonMappers = des.ReadClassPointerArray<hkaSkeletonMapper>(br);
            animationSkeleton = des.ReadClassPointer<hkaSkeleton>(br);
            ragdollToAnimationSkeletonMapper = des.ReadClassPointer<hkaSkeletonMapper>(br);
            animationToRagdollSkeletonMapper = des.ReadClassPointer<hkaSkeletonMapper>(br);
            des.ReadEmptyPointer(br);
            data = des.ReadClassPointer<hkbCharacterData>(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, retargetingSkeletonMappers);
            s.WriteClassPointer(bw, animationSkeleton);
            s.WriteClassPointer(bw, ragdollToAnimationSkeletonMapper);
            s.WriteClassPointer(bw, animationToRagdollSkeletonMapper);
            s.WriteVoidPointer(bw);
            s.WriteClassPointer(bw, data);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            retargetingSkeletonMappers = xd.ReadClassPointerArray<hkaSkeletonMapper>(this, xe, nameof(retargetingSkeletonMappers));
            animationSkeleton = xd.ReadClassPointer<hkaSkeleton>(this, xe, nameof(animationSkeleton));
            ragdollToAnimationSkeletonMapper = xd.ReadClassPointer<hkaSkeletonMapper>(this, xe, nameof(ragdollToAnimationSkeletonMapper));
            animationToRagdollSkeletonMapper = xd.ReadClassPointer<hkaSkeletonMapper>(this, xe, nameof(animationToRagdollSkeletonMapper));
            data = xd.ReadClassPointer<hkbCharacterData>(this, xe, nameof(data));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(retargetingSkeletonMappers), retargetingSkeletonMappers!);
            xs.WriteClassPointer(xe, nameof(animationSkeleton), animationSkeleton);
            xs.WriteClassPointer(xe, nameof(ragdollToAnimationSkeletonMapper), ragdollToAnimationSkeletonMapper);
            xs.WriteClassPointer(xe, nameof(animationToRagdollSkeletonMapper), animationToRagdollSkeletonMapper);
            xs.WriteSerializeIgnored(xe, nameof(animationBindingSet));
            xs.WriteClassPointer(xe, nameof(data), data);
            xs.WriteSerializeIgnored(xe, nameof(mirroredSkeleton));
            xs.WriteSerializeIgnored(xe, nameof(characterPropertyIdMap));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterSetup);
        }

        public bool Equals(hkbCharacterSetup? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   retargetingSkeletonMappers.SequenceEqual(other.retargetingSkeletonMappers) &&
                   ((animationSkeleton is null && other.animationSkeleton is null) || (animationSkeleton is not null && other.animationSkeleton is not null && animationSkeleton.Equals((IHavokObject)other.animationSkeleton))) &&
                   ((ragdollToAnimationSkeletonMapper is null && other.ragdollToAnimationSkeletonMapper is null) || (ragdollToAnimationSkeletonMapper is not null && other.ragdollToAnimationSkeletonMapper is not null && ragdollToAnimationSkeletonMapper.Equals((IHavokObject)other.ragdollToAnimationSkeletonMapper))) &&
                   ((animationToRagdollSkeletonMapper is null && other.animationToRagdollSkeletonMapper is null) || (animationToRagdollSkeletonMapper is not null && other.animationToRagdollSkeletonMapper is not null && animationToRagdollSkeletonMapper.Equals((IHavokObject)other.animationToRagdollSkeletonMapper))) &&
                   ((data is null && other.data is null) || (data is not null && other.data is not null && data.Equals((IHavokObject)other.data))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(retargetingSkeletonMappers.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(animationSkeleton);
            hashcode.Add(ragdollToAnimationSkeletonMapper);
            hashcode.Add(animationToRagdollSkeletonMapper);
            hashcode.Add(data);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

