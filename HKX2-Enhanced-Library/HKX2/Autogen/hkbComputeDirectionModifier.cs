using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbComputeDirectionModifier Signatire: 0xdf358bd3 size: 144 flags: FLAGS_NONE

    // pointIn class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // pointOut class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // groundAngleOut class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // upAngleOut class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 116 flags: FLAGS_NONE enum: 
    // verticalOffset class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // reverseGroundAngle class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 124 flags: FLAGS_NONE enum: 
    // reverseUpAngle class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 125 flags: FLAGS_NONE enum: 
    // projectPoint class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 126 flags: FLAGS_NONE enum: 
    // normalizePoint class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 127 flags: FLAGS_NONE enum: 
    // computeOnlyOnce class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // computedOutput class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 129 flags: FLAGS_NONE enum: 
    public partial class hkbComputeDirectionModifier : hkbModifier, IEquatable<hkbComputeDirectionModifier?>
    {
        public Vector4 pointIn { set; get; }
        public Vector4 pointOut { set; get; }
        public float groundAngleOut { set; get; }
        public float upAngleOut { set; get; }
        public float verticalOffset { set; get; }
        public bool reverseGroundAngle { set; get; }
        public bool reverseUpAngle { set; get; }
        public bool projectPoint { set; get; }
        public bool normalizePoint { set; get; }
        public bool computeOnlyOnce { set; get; }
        public bool computedOutput { set; get; }

        public override uint Signature { set; get; } = 0xdf358bd3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            pointIn = br.ReadVector4();
            pointOut = br.ReadVector4();
            groundAngleOut = br.ReadSingle();
            upAngleOut = br.ReadSingle();
            verticalOffset = br.ReadSingle();
            reverseGroundAngle = br.ReadBoolean();
            reverseUpAngle = br.ReadBoolean();
            projectPoint = br.ReadBoolean();
            normalizePoint = br.ReadBoolean();
            computeOnlyOnce = br.ReadBoolean();
            computedOutput = br.ReadBoolean();
            br.Position += 14;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(pointIn);
            bw.WriteVector4(pointOut);
            bw.WriteSingle(groundAngleOut);
            bw.WriteSingle(upAngleOut);
            bw.WriteSingle(verticalOffset);
            bw.WriteBoolean(reverseGroundAngle);
            bw.WriteBoolean(reverseUpAngle);
            bw.WriteBoolean(projectPoint);
            bw.WriteBoolean(normalizePoint);
            bw.WriteBoolean(computeOnlyOnce);
            bw.WriteBoolean(computedOutput);
            bw.Position += 14;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pointIn = xd.ReadVector4(xe, nameof(pointIn));
            pointOut = xd.ReadVector4(xe, nameof(pointOut));
            groundAngleOut = xd.ReadSingle(xe, nameof(groundAngleOut));
            upAngleOut = xd.ReadSingle(xe, nameof(upAngleOut));
            verticalOffset = xd.ReadSingle(xe, nameof(verticalOffset));
            reverseGroundAngle = xd.ReadBoolean(xe, nameof(reverseGroundAngle));
            reverseUpAngle = xd.ReadBoolean(xe, nameof(reverseUpAngle));
            projectPoint = xd.ReadBoolean(xe, nameof(projectPoint));
            normalizePoint = xd.ReadBoolean(xe, nameof(normalizePoint));
            computeOnlyOnce = xd.ReadBoolean(xe, nameof(computeOnlyOnce));
            computedOutput = xd.ReadBoolean(xe, nameof(computedOutput));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(pointIn), pointIn);
            xs.WriteVector4(xe, nameof(pointOut), pointOut);
            xs.WriteFloat(xe, nameof(groundAngleOut), groundAngleOut);
            xs.WriteFloat(xe, nameof(upAngleOut), upAngleOut);
            xs.WriteFloat(xe, nameof(verticalOffset), verticalOffset);
            xs.WriteBoolean(xe, nameof(reverseGroundAngle), reverseGroundAngle);
            xs.WriteBoolean(xe, nameof(reverseUpAngle), reverseUpAngle);
            xs.WriteBoolean(xe, nameof(projectPoint), projectPoint);
            xs.WriteBoolean(xe, nameof(normalizePoint), normalizePoint);
            xs.WriteBoolean(xe, nameof(computeOnlyOnce), computeOnlyOnce);
            xs.WriteBoolean(xe, nameof(computedOutput), computedOutput);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbComputeDirectionModifier);
        }

        public bool Equals(hkbComputeDirectionModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   pointIn.Equals(other.pointIn) &&
                   pointOut.Equals(other.pointOut) &&
                   groundAngleOut.Equals(other.groundAngleOut) &&
                   upAngleOut.Equals(other.upAngleOut) &&
                   verticalOffset.Equals(other.verticalOffset) &&
                   reverseGroundAngle.Equals(other.reverseGroundAngle) &&
                   reverseUpAngle.Equals(other.reverseUpAngle) &&
                   projectPoint.Equals(other.projectPoint) &&
                   normalizePoint.Equals(other.normalizePoint) &&
                   computeOnlyOnce.Equals(other.computeOnlyOnce) &&
                   computedOutput.Equals(other.computedOutput) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pointIn);
            hashcode.Add(pointOut);
            hashcode.Add(groundAngleOut);
            hashcode.Add(upAngleOut);
            hashcode.Add(verticalOffset);
            hashcode.Add(reverseGroundAngle);
            hashcode.Add(reverseUpAngle);
            hashcode.Add(projectPoint);
            hashcode.Add(normalizePoint);
            hashcode.Add(computeOnlyOnce);
            hashcode.Add(computedOutput);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

