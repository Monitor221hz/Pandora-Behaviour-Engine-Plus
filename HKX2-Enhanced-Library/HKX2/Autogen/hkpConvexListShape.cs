using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConvexListShape Signatire: 0x450b26e8 size: 128 flags: FLAGS_NONE

    // minDistanceToUseConvexHullForGetClosestPoints class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // aabbHalfExtents class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // aabbCenter class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // useCachedAabb class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // childShapes class: hkpConvexShape Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    public partial class hkpConvexListShape : hkpConvexShape, IEquatable<hkpConvexListShape?>
    {
        public float minDistanceToUseConvexHullForGetClosestPoints { set; get; }
        public Vector4 aabbHalfExtents { set; get; }
        public Vector4 aabbCenter { set; get; }
        public bool useCachedAabb { set; get; }
        public IList<hkpConvexShape> childShapes { set; get; } = Array.Empty<hkpConvexShape>();

        public override uint Signature { set; get; } = 0x450b26e8;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            minDistanceToUseConvexHullForGetClosestPoints = br.ReadSingle();
            br.Position += 12;
            aabbHalfExtents = br.ReadVector4();
            aabbCenter = br.ReadVector4();
            useCachedAabb = br.ReadBoolean();
            br.Position += 7;
            childShapes = des.ReadClassPointerArray<hkpConvexShape>(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            bw.WriteSingle(minDistanceToUseConvexHullForGetClosestPoints);
            bw.Position += 12;
            bw.WriteVector4(aabbHalfExtents);
            bw.WriteVector4(aabbCenter);
            bw.WriteBoolean(useCachedAabb);
            bw.Position += 7;
            s.WriteClassPointerArray(bw, childShapes);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            minDistanceToUseConvexHullForGetClosestPoints = xd.ReadSingle(xe, nameof(minDistanceToUseConvexHullForGetClosestPoints));
            aabbHalfExtents = xd.ReadVector4(xe, nameof(aabbHalfExtents));
            aabbCenter = xd.ReadVector4(xe, nameof(aabbCenter));
            useCachedAabb = xd.ReadBoolean(xe, nameof(useCachedAabb));
            childShapes = xd.ReadClassPointerArray<hkpConvexShape>(this, xe, nameof(childShapes));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(minDistanceToUseConvexHullForGetClosestPoints), minDistanceToUseConvexHullForGetClosestPoints);
            xs.WriteVector4(xe, nameof(aabbHalfExtents), aabbHalfExtents);
            xs.WriteVector4(xe, nameof(aabbCenter), aabbCenter);
            xs.WriteBoolean(xe, nameof(useCachedAabb), useCachedAabb);
            xs.WriteClassPointerArray(xe, nameof(childShapes), childShapes!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConvexListShape);
        }

        public bool Equals(hkpConvexListShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   minDistanceToUseConvexHullForGetClosestPoints.Equals(other.minDistanceToUseConvexHullForGetClosestPoints) &&
                   aabbHalfExtents.Equals(other.aabbHalfExtents) &&
                   aabbCenter.Equals(other.aabbCenter) &&
                   useCachedAabb.Equals(other.useCachedAabb) &&
                   childShapes.SequenceEqual(other.childShapes) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(minDistanceToUseConvexHullForGetClosestPoints);
            hashcode.Add(aabbHalfExtents);
            hashcode.Add(aabbCenter);
            hashcode.Add(useCachedAabb);
            hashcode.Add(childShapes.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

