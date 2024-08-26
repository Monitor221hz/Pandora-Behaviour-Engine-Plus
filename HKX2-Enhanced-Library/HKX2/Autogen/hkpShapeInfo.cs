using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpShapeInfo Signatire: 0xea7f1d08 size: 128 flags: FLAGS_NONE

    // shape class: hkpShape Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // isHierarchicalCompound class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // hkdShapesCollected class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 25 flags: FLAGS_NONE enum: 
    // childShapeNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // childTransforms class:  Type.TYPE_ARRAY Type.TYPE_TRANSFORM arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // transform class:  Type.TYPE_TRANSFORM Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpShapeInfo : hkReferencedObject, IEquatable<hkpShapeInfo?>
    {
        public hkpShape? shape { set; get; }
        public bool isHierarchicalCompound { set; get; }
        public bool hkdShapesCollected { set; get; }
        public IList<string> childShapeNames { set; get; } = Array.Empty<string>();
        public IList<Matrix4x4> childTransforms { set; get; } = Array.Empty<Matrix4x4>();
        public Matrix4x4 transform { set; get; }

        public override uint Signature { set; get; } = 0xea7f1d08;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            shape = des.ReadClassPointer<hkpShape>(br);
            isHierarchicalCompound = br.ReadBoolean();
            hkdShapesCollected = br.ReadBoolean();
            br.Position += 6;
            childShapeNames = des.ReadStringPointerArray(br);
            childTransforms = des.ReadTransformArray(br);
            transform = des.ReadTransform(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, shape);
            bw.WriteBoolean(isHierarchicalCompound);
            bw.WriteBoolean(hkdShapesCollected);
            bw.Position += 6;
            s.WriteStringPointerArray(bw, childShapeNames);
            s.WriteTransformArray(bw, childTransforms);
            s.WriteTransform(bw, transform);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            shape = xd.ReadClassPointer<hkpShape>(this, xe, nameof(shape));
            isHierarchicalCompound = xd.ReadBoolean(xe, nameof(isHierarchicalCompound));
            hkdShapesCollected = xd.ReadBoolean(xe, nameof(hkdShapesCollected));
            childShapeNames = xd.ReadStringArray(xe, nameof(childShapeNames));
            childTransforms = xd.ReadTransformArray(xe, nameof(childTransforms));
            transform = xd.ReadTransform(xe, nameof(transform));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(shape), shape);
            xs.WriteBoolean(xe, nameof(isHierarchicalCompound), isHierarchicalCompound);
            xs.WriteBoolean(xe, nameof(hkdShapesCollected), hkdShapesCollected);
            xs.WriteStringArray(xe, nameof(childShapeNames), childShapeNames);
            xs.WriteTransformArray(xe, nameof(childTransforms), childTransforms);
            xs.WriteTransform(xe, nameof(transform), transform);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpShapeInfo);
        }

        public bool Equals(hkpShapeInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((shape is null && other.shape is null) || (shape is not null && other.shape is not null && shape.Equals((IHavokObject)other.shape))) &&
                   isHierarchicalCompound.Equals(other.isHierarchicalCompound) &&
                   hkdShapesCollected.Equals(other.hkdShapesCollected) &&
                   childShapeNames.SequenceEqual(other.childShapeNames) &&
                   childTransforms.SequenceEqual(other.childTransforms) &&
                   transform.Equals(other.transform) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(shape);
            hashcode.Add(isHierarchicalCompound);
            hashcode.Add(hkdShapesCollected);
            hashcode.Add(childShapeNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(childTransforms.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(transform);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

