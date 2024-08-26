using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbComputeDirectionModifierInternalState Signatire: 0x6ac054d7 size: 48 flags: FLAGS_NONE

    // pointOut class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // groundAngleOut class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // upAngleOut class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // computedOutput class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    public partial class hkbComputeDirectionModifierInternalState : hkReferencedObject, IEquatable<hkbComputeDirectionModifierInternalState?>
    {
        public Vector4 pointOut { set; get; }
        public float groundAngleOut { set; get; }
        public float upAngleOut { set; get; }
        public bool computedOutput { set; get; }

        public override uint Signature { set; get; } = 0x6ac054d7;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            pointOut = br.ReadVector4();
            groundAngleOut = br.ReadSingle();
            upAngleOut = br.ReadSingle();
            computedOutput = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(pointOut);
            bw.WriteSingle(groundAngleOut);
            bw.WriteSingle(upAngleOut);
            bw.WriteBoolean(computedOutput);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pointOut = xd.ReadVector4(xe, nameof(pointOut));
            groundAngleOut = xd.ReadSingle(xe, nameof(groundAngleOut));
            upAngleOut = xd.ReadSingle(xe, nameof(upAngleOut));
            computedOutput = xd.ReadBoolean(xe, nameof(computedOutput));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(pointOut), pointOut);
            xs.WriteFloat(xe, nameof(groundAngleOut), groundAngleOut);
            xs.WriteFloat(xe, nameof(upAngleOut), upAngleOut);
            xs.WriteBoolean(xe, nameof(computedOutput), computedOutput);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbComputeDirectionModifierInternalState);
        }

        public bool Equals(hkbComputeDirectionModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   pointOut.Equals(other.pointOut) &&
                   groundAngleOut.Equals(other.groundAngleOut) &&
                   upAngleOut.Equals(other.upAngleOut) &&
                   computedOutput.Equals(other.computedOutput) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pointOut);
            hashcode.Add(groundAngleOut);
            hashcode.Add(upAngleOut);
            hashcode.Add(computedOutput);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

