using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaMeshBinding Signatire: 0x81d9950b size: 72 flags: FLAGS_NONE

    // mesh class: hkxMesh Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // originalSkeletonName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // skeleton class: hkaSkeleton Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // mappings class: hkaMeshBindingMapping Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // boneFromSkinMeshTransforms class:  Type.TYPE_ARRAY Type.TYPE_TRANSFORM arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    public partial class hkaMeshBinding : hkReferencedObject, IEquatable<hkaMeshBinding?>
    {
        public hkxMesh? mesh { set; get; }
        public string originalSkeletonName { set; get; } = "";
        public hkaSkeleton? skeleton { set; get; }
        public IList<hkaMeshBindingMapping> mappings { set; get; } = Array.Empty<hkaMeshBindingMapping>();
        public IList<Matrix4x4> boneFromSkinMeshTransforms { set; get; } = Array.Empty<Matrix4x4>();

        public override uint Signature { set; get; } = 0x81d9950b;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            mesh = des.ReadClassPointer<hkxMesh>(br);
            originalSkeletonName = des.ReadStringPointer(br);
            skeleton = des.ReadClassPointer<hkaSkeleton>(br);
            mappings = des.ReadClassArray<hkaMeshBindingMapping>(br);
            boneFromSkinMeshTransforms = des.ReadTransformArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, mesh);
            s.WriteStringPointer(bw, originalSkeletonName);
            s.WriteClassPointer(bw, skeleton);
            s.WriteClassArray(bw, mappings);
            s.WriteTransformArray(bw, boneFromSkinMeshTransforms);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            mesh = xd.ReadClassPointer<hkxMesh>(this, xe, nameof(mesh));
            originalSkeletonName = xd.ReadString(xe, nameof(originalSkeletonName));
            skeleton = xd.ReadClassPointer<hkaSkeleton>(this, xe, nameof(skeleton));
            mappings = xd.ReadClassArray<hkaMeshBindingMapping>(xe, nameof(mappings));
            boneFromSkinMeshTransforms = xd.ReadTransformArray(xe, nameof(boneFromSkinMeshTransforms));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(mesh), mesh);
            xs.WriteString(xe, nameof(originalSkeletonName), originalSkeletonName);
            xs.WriteClassPointer(xe, nameof(skeleton), skeleton);
            xs.WriteClassArray(xe, nameof(mappings), mappings);
            xs.WriteTransformArray(xe, nameof(boneFromSkinMeshTransforms), boneFromSkinMeshTransforms);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaMeshBinding);
        }

        public bool Equals(hkaMeshBinding? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((mesh is null && other.mesh is null) || (mesh is not null && other.mesh is not null && mesh.Equals((IHavokObject)other.mesh))) &&
                   (originalSkeletonName is null && other.originalSkeletonName is null || originalSkeletonName == other.originalSkeletonName || originalSkeletonName is null && other.originalSkeletonName == "" || originalSkeletonName == "" && other.originalSkeletonName is null) &&
                   ((skeleton is null && other.skeleton is null) || (skeleton is not null && other.skeleton is not null && skeleton.Equals((IHavokObject)other.skeleton))) &&
                   mappings.SequenceEqual(other.mappings) &&
                   boneFromSkinMeshTransforms.SequenceEqual(other.boneFromSkinMeshTransforms) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(mesh);
            hashcode.Add(originalSkeletonName);
            hashcode.Add(skeleton);
            hashcode.Add(mappings.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(boneFromSkinMeshTransforms.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

