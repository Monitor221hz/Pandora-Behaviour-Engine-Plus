using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpLinearParametricCurve Signatire: 0xd7b3be03 size: 80 flags: FLAGS_NONE

    // smoothingFactor class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // closedLoop class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // dirNotParallelToTangentAlongWholePath class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // points class:  Type.TYPE_ARRAY Type.TYPE_VECTOR4 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // distance class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpLinearParametricCurve : hkpParametricCurve, IEquatable<hkpLinearParametricCurve?>
    {
        public float smoothingFactor { set; get; }
        public bool closedLoop { set; get; }
        public Vector4 dirNotParallelToTangentAlongWholePath { set; get; }
        public IList<Vector4> points { set; get; } = Array.Empty<Vector4>();
        public IList<float> distance { set; get; } = Array.Empty<float>();

        public override uint Signature { set; get; } = 0xd7b3be03;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            smoothingFactor = br.ReadSingle();
            closedLoop = br.ReadBoolean();
            br.Position += 11;
            dirNotParallelToTangentAlongWholePath = br.ReadVector4();
            points = des.ReadVector4Array(br);
            distance = des.ReadSingleArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(smoothingFactor);
            bw.WriteBoolean(closedLoop);
            bw.Position += 11;
            bw.WriteVector4(dirNotParallelToTangentAlongWholePath);
            s.WriteVector4Array(bw, points);
            s.WriteSingleArray(bw, distance);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            smoothingFactor = xd.ReadSingle(xe, nameof(smoothingFactor));
            closedLoop = xd.ReadBoolean(xe, nameof(closedLoop));
            dirNotParallelToTangentAlongWholePath = xd.ReadVector4(xe, nameof(dirNotParallelToTangentAlongWholePath));
            points = xd.ReadVector4Array(xe, nameof(points));
            distance = xd.ReadSingleArray(xe, nameof(distance));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(smoothingFactor), smoothingFactor);
            xs.WriteBoolean(xe, nameof(closedLoop), closedLoop);
            xs.WriteVector4(xe, nameof(dirNotParallelToTangentAlongWholePath), dirNotParallelToTangentAlongWholePath);
            xs.WriteVector4Array(xe, nameof(points), points);
            xs.WriteFloatArray(xe, nameof(distance), distance);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpLinearParametricCurve);
        }

        public bool Equals(hkpLinearParametricCurve? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   smoothingFactor.Equals(other.smoothingFactor) &&
                   closedLoop.Equals(other.closedLoop) &&
                   dirNotParallelToTangentAlongWholePath.Equals(other.dirNotParallelToTangentAlongWholePath) &&
                   points.SequenceEqual(other.points) &&
                   distance.SequenceEqual(other.distance) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(smoothingFactor);
            hashcode.Add(closedLoop);
            hashcode.Add(dirNotParallelToTangentAlongWholePath);
            hashcode.Add(points.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(distance.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

