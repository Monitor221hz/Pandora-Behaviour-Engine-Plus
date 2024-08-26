using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaSkeleton Signatire: 0x366e8220 size: 120 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // parentIndices class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // bones class: hkaBone Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // referencePose class:  Type.TYPE_ARRAY Type.TYPE_QSTRANSFORM arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // referenceFloats class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // floatSlots class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // localFrames class: hkaSkeletonLocalFrameOnBone Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    public partial class hkaSkeleton : hkReferencedObject, IEquatable<hkaSkeleton?>
    {
        public string name { set; get; } = "";
        public IList<short> parentIndices { set; get; } = Array.Empty<short>();
        public IList<hkaBone> bones { set; get; } = Array.Empty<hkaBone>();
        public IList<Matrix4x4> referencePose { set; get; } = Array.Empty<Matrix4x4>();
        public IList<float> referenceFloats { set; get; } = Array.Empty<float>();
        public IList<string> floatSlots { set; get; } = Array.Empty<string>();
        public IList<hkaSkeletonLocalFrameOnBone> localFrames { set; get; } = Array.Empty<hkaSkeletonLocalFrameOnBone>();

        public override uint Signature { set; get; } = 0x366e8220;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            name = des.ReadStringPointer(br);
            parentIndices = des.ReadInt16Array(br);
            bones = des.ReadClassArray<hkaBone>(br);
            referencePose = des.ReadQSTransformArray(br);
            referenceFloats = des.ReadSingleArray(br);
            floatSlots = des.ReadStringPointerArray(br);
            localFrames = des.ReadClassArray<hkaSkeletonLocalFrameOnBone>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, name);
            s.WriteInt16Array(bw, parentIndices);
            s.WriteClassArray(bw, bones);
            s.WriteQSTransformArray(bw, referencePose);
            s.WriteSingleArray(bw, referenceFloats);
            s.WriteStringPointerArray(bw, floatSlots);
            s.WriteClassArray(bw, localFrames);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            name = xd.ReadString(xe, nameof(name));
            parentIndices = xd.ReadInt16Array(xe, nameof(parentIndices));
            bones = xd.ReadClassArray<hkaBone>(xe, nameof(bones));
            referencePose = xd.ReadQSTransformArray(xe, nameof(referencePose));
            referenceFloats = xd.ReadSingleArray(xe, nameof(referenceFloats));
            floatSlots = xd.ReadStringArray(xe, nameof(floatSlots));
            localFrames = xd.ReadClassArray<hkaSkeletonLocalFrameOnBone>(xe, nameof(localFrames));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteNumberArray(xe, nameof(parentIndices), parentIndices);
            xs.WriteClassArray(xe, nameof(bones), bones);
            xs.WriteQSTransformArray(xe, nameof(referencePose), referencePose);
            xs.WriteFloatArray(xe, nameof(referenceFloats), referenceFloats);
            xs.WriteStringArray(xe, nameof(floatSlots), floatSlots);
            xs.WriteClassArray(xe, nameof(localFrames), localFrames);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaSkeleton);
        }

        public bool Equals(hkaSkeleton? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   parentIndices.SequenceEqual(other.parentIndices) &&
                   bones.SequenceEqual(other.bones) &&
                   referencePose.SequenceEqual(other.referencePose) &&
                   referenceFloats.SequenceEqual(other.referenceFloats) &&
                   floatSlots.SequenceEqual(other.floatSlots) &&
                   localFrames.SequenceEqual(other.localFrames) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(name);
            hashcode.Add(parentIndices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(bones.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(referencePose.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(referenceFloats.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(floatSlots.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(localFrames.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

