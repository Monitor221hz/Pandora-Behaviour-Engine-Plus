using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbFootIkDriverInfoLeg Signatire: 0x224b18d1 size: 96 flags: FLAGS_NONE

    // prevAnkleRotLS class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // kneeAxisLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // footEndLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // footPlantedAnkleHeightMS class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // footRaisedAnkleHeightMS class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 52 flags: FLAGS_NONE enum: 
    // maxAnkleHeightMS class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // minAnkleHeightMS class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // maxKneeAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // minKneeAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // maxAnkleAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // hipIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    // kneeIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 78 flags: FLAGS_NONE enum: 
    // ankleIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkbFootIkDriverInfoLeg : IHavokObject, IEquatable<hkbFootIkDriverInfoLeg?>
    {
        private Quaternion prevAnkleRotLS { set; get; }
        public Vector4 kneeAxisLS { set; get; }
        public Vector4 footEndLS { set; get; }
        public float footPlantedAnkleHeightMS { set; get; }
        public float footRaisedAnkleHeightMS { set; get; }
        public float maxAnkleHeightMS { set; get; }
        public float minAnkleHeightMS { set; get; }
        public float maxKneeAngleDegrees { set; get; }
        public float minKneeAngleDegrees { set; get; }
        public float maxAnkleAngleDegrees { set; get; }
        public short hipIndex { set; get; }
        public short kneeIndex { set; get; }
        public short ankleIndex { set; get; }

        public virtual uint Signature { set; get; } = 0x224b18d1;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            prevAnkleRotLS = des.ReadQuaternion(br);
            kneeAxisLS = br.ReadVector4();
            footEndLS = br.ReadVector4();
            footPlantedAnkleHeightMS = br.ReadSingle();
            footRaisedAnkleHeightMS = br.ReadSingle();
            maxAnkleHeightMS = br.ReadSingle();
            minAnkleHeightMS = br.ReadSingle();
            maxKneeAngleDegrees = br.ReadSingle();
            minKneeAngleDegrees = br.ReadSingle();
            maxAnkleAngleDegrees = br.ReadSingle();
            hipIndex = br.ReadInt16();
            kneeIndex = br.ReadInt16();
            ankleIndex = br.ReadInt16();
            br.Position += 14;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteQuaternion(bw, prevAnkleRotLS);
            bw.WriteVector4(kneeAxisLS);
            bw.WriteVector4(footEndLS);
            bw.WriteSingle(footPlantedAnkleHeightMS);
            bw.WriteSingle(footRaisedAnkleHeightMS);
            bw.WriteSingle(maxAnkleHeightMS);
            bw.WriteSingle(minAnkleHeightMS);
            bw.WriteSingle(maxKneeAngleDegrees);
            bw.WriteSingle(minKneeAngleDegrees);
            bw.WriteSingle(maxAnkleAngleDegrees);
            bw.WriteInt16(hipIndex);
            bw.WriteInt16(kneeIndex);
            bw.WriteInt16(ankleIndex);
            bw.Position += 14;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            kneeAxisLS = xd.ReadVector4(xe, nameof(kneeAxisLS));
            footEndLS = xd.ReadVector4(xe, nameof(footEndLS));
            footPlantedAnkleHeightMS = xd.ReadSingle(xe, nameof(footPlantedAnkleHeightMS));
            footRaisedAnkleHeightMS = xd.ReadSingle(xe, nameof(footRaisedAnkleHeightMS));
            maxAnkleHeightMS = xd.ReadSingle(xe, nameof(maxAnkleHeightMS));
            minAnkleHeightMS = xd.ReadSingle(xe, nameof(minAnkleHeightMS));
            maxKneeAngleDegrees = xd.ReadSingle(xe, nameof(maxKneeAngleDegrees));
            minKneeAngleDegrees = xd.ReadSingle(xe, nameof(minKneeAngleDegrees));
            maxAnkleAngleDegrees = xd.ReadSingle(xe, nameof(maxAnkleAngleDegrees));
            hipIndex = xd.ReadInt16(xe, nameof(hipIndex));
            kneeIndex = xd.ReadInt16(xe, nameof(kneeIndex));
            ankleIndex = xd.ReadInt16(xe, nameof(ankleIndex));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(prevAnkleRotLS));
            xs.WriteVector4(xe, nameof(kneeAxisLS), kneeAxisLS);
            xs.WriteVector4(xe, nameof(footEndLS), footEndLS);
            xs.WriteFloat(xe, nameof(footPlantedAnkleHeightMS), footPlantedAnkleHeightMS);
            xs.WriteFloat(xe, nameof(footRaisedAnkleHeightMS), footRaisedAnkleHeightMS);
            xs.WriteFloat(xe, nameof(maxAnkleHeightMS), maxAnkleHeightMS);
            xs.WriteFloat(xe, nameof(minAnkleHeightMS), minAnkleHeightMS);
            xs.WriteFloat(xe, nameof(maxKneeAngleDegrees), maxKneeAngleDegrees);
            xs.WriteFloat(xe, nameof(minKneeAngleDegrees), minKneeAngleDegrees);
            xs.WriteFloat(xe, nameof(maxAnkleAngleDegrees), maxAnkleAngleDegrees);
            xs.WriteNumber(xe, nameof(hipIndex), hipIndex);
            xs.WriteNumber(xe, nameof(kneeIndex), kneeIndex);
            xs.WriteNumber(xe, nameof(ankleIndex), ankleIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbFootIkDriverInfoLeg);
        }

        public bool Equals(hkbFootIkDriverInfoLeg? other)
        {
            return other is not null &&
                   kneeAxisLS.Equals(other.kneeAxisLS) &&
                   footEndLS.Equals(other.footEndLS) &&
                   footPlantedAnkleHeightMS.Equals(other.footPlantedAnkleHeightMS) &&
                   footRaisedAnkleHeightMS.Equals(other.footRaisedAnkleHeightMS) &&
                   maxAnkleHeightMS.Equals(other.maxAnkleHeightMS) &&
                   minAnkleHeightMS.Equals(other.minAnkleHeightMS) &&
                   maxKneeAngleDegrees.Equals(other.maxKneeAngleDegrees) &&
                   minKneeAngleDegrees.Equals(other.minKneeAngleDegrees) &&
                   maxAnkleAngleDegrees.Equals(other.maxAnkleAngleDegrees) &&
                   hipIndex.Equals(other.hipIndex) &&
                   kneeIndex.Equals(other.kneeIndex) &&
                   ankleIndex.Equals(other.ankleIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(kneeAxisLS);
            hashcode.Add(footEndLS);
            hashcode.Add(footPlantedAnkleHeightMS);
            hashcode.Add(footRaisedAnkleHeightMS);
            hashcode.Add(maxAnkleHeightMS);
            hashcode.Add(minAnkleHeightMS);
            hashcode.Add(maxKneeAngleDegrees);
            hashcode.Add(minKneeAngleDegrees);
            hashcode.Add(maxAnkleAngleDegrees);
            hashcode.Add(hipIndex);
            hashcode.Add(kneeIndex);
            hashcode.Add(ankleIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

