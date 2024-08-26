using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbFootIkModifierLeg Signatire: 0x9f3e3a04 size: 160 flags: FLAGS_NONE

    // originalAnkleTransformMS class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // prevAnkleRotLS class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 48 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // kneeAxisLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // footEndLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // ungroundedEvent class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // footPlantedAnkleHeightMS class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // footRaisedAnkleHeightMS class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 116 flags: FLAGS_NONE enum: 
    // maxAnkleHeightMS class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // minAnkleHeightMS class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 124 flags: FLAGS_NONE enum: 
    // maxKneeAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // minKneeAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 132 flags: FLAGS_NONE enum: 
    // verticalError class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // maxAnkleAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 140 flags: FLAGS_NONE enum: 
    // hipIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // kneeIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 146 flags: FLAGS_NONE enum: 
    // ankleIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 148 flags: FLAGS_NONE enum: 
    // hitSomething class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 150 flags: FLAGS_NONE enum: 
    // isPlantedMS class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 151 flags: FLAGS_NONE enum: 
    // isOriginalAnkleTransformMSSet class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    public partial class hkbFootIkModifierLeg : IHavokObject, IEquatable<hkbFootIkModifierLeg?>
    {
        public Matrix4x4 originalAnkleTransformMS { set; get; }
        private Quaternion prevAnkleRotLS { set; get; }
        public Vector4 kneeAxisLS { set; get; }
        public Vector4 footEndLS { set; get; }
        public hkbEventProperty ungroundedEvent { set; get; } = new();
        public float footPlantedAnkleHeightMS { set; get; }
        public float footRaisedAnkleHeightMS { set; get; }
        public float maxAnkleHeightMS { set; get; }
        public float minAnkleHeightMS { set; get; }
        public float maxKneeAngleDegrees { set; get; }
        public float minKneeAngleDegrees { set; get; }
        public float verticalError { set; get; }
        public float maxAnkleAngleDegrees { set; get; }
        public short hipIndex { set; get; }
        public short kneeIndex { set; get; }
        public short ankleIndex { set; get; }
        public bool hitSomething { set; get; }
        public bool isPlantedMS { set; get; }
        public bool isOriginalAnkleTransformMSSet { set; get; }

        public virtual uint Signature { set; get; } = 0x9f3e3a04;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            originalAnkleTransformMS = des.ReadQSTransform(br);
            prevAnkleRotLS = des.ReadQuaternion(br);
            kneeAxisLS = br.ReadVector4();
            footEndLS = br.ReadVector4();
            ungroundedEvent.Read(des, br);
            footPlantedAnkleHeightMS = br.ReadSingle();
            footRaisedAnkleHeightMS = br.ReadSingle();
            maxAnkleHeightMS = br.ReadSingle();
            minAnkleHeightMS = br.ReadSingle();
            maxKneeAngleDegrees = br.ReadSingle();
            minKneeAngleDegrees = br.ReadSingle();
            verticalError = br.ReadSingle();
            maxAnkleAngleDegrees = br.ReadSingle();
            hipIndex = br.ReadInt16();
            kneeIndex = br.ReadInt16();
            ankleIndex = br.ReadInt16();
            hitSomething = br.ReadBoolean();
            isPlantedMS = br.ReadBoolean();
            isOriginalAnkleTransformMSSet = br.ReadBoolean();
            br.Position += 7;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteQSTransform(bw, originalAnkleTransformMS);
            s.WriteQuaternion(bw, prevAnkleRotLS);
            bw.WriteVector4(kneeAxisLS);
            bw.WriteVector4(footEndLS);
            ungroundedEvent.Write(s, bw);
            bw.WriteSingle(footPlantedAnkleHeightMS);
            bw.WriteSingle(footRaisedAnkleHeightMS);
            bw.WriteSingle(maxAnkleHeightMS);
            bw.WriteSingle(minAnkleHeightMS);
            bw.WriteSingle(maxKneeAngleDegrees);
            bw.WriteSingle(minKneeAngleDegrees);
            bw.WriteSingle(verticalError);
            bw.WriteSingle(maxAnkleAngleDegrees);
            bw.WriteInt16(hipIndex);
            bw.WriteInt16(kneeIndex);
            bw.WriteInt16(ankleIndex);
            bw.WriteBoolean(hitSomething);
            bw.WriteBoolean(isPlantedMS);
            bw.WriteBoolean(isOriginalAnkleTransformMSSet);
            bw.Position += 7;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            originalAnkleTransformMS = xd.ReadQSTransform(xe, nameof(originalAnkleTransformMS));
            kneeAxisLS = xd.ReadVector4(xe, nameof(kneeAxisLS));
            footEndLS = xd.ReadVector4(xe, nameof(footEndLS));
            ungroundedEvent = xd.ReadClass<hkbEventProperty>(xe, nameof(ungroundedEvent));
            footPlantedAnkleHeightMS = xd.ReadSingle(xe, nameof(footPlantedAnkleHeightMS));
            footRaisedAnkleHeightMS = xd.ReadSingle(xe, nameof(footRaisedAnkleHeightMS));
            maxAnkleHeightMS = xd.ReadSingle(xe, nameof(maxAnkleHeightMS));
            minAnkleHeightMS = xd.ReadSingle(xe, nameof(minAnkleHeightMS));
            maxKneeAngleDegrees = xd.ReadSingle(xe, nameof(maxKneeAngleDegrees));
            minKneeAngleDegrees = xd.ReadSingle(xe, nameof(minKneeAngleDegrees));
            verticalError = xd.ReadSingle(xe, nameof(verticalError));
            maxAnkleAngleDegrees = xd.ReadSingle(xe, nameof(maxAnkleAngleDegrees));
            hipIndex = xd.ReadInt16(xe, nameof(hipIndex));
            kneeIndex = xd.ReadInt16(xe, nameof(kneeIndex));
            ankleIndex = xd.ReadInt16(xe, nameof(ankleIndex));
            hitSomething = xd.ReadBoolean(xe, nameof(hitSomething));
            isPlantedMS = xd.ReadBoolean(xe, nameof(isPlantedMS));
            isOriginalAnkleTransformMSSet = xd.ReadBoolean(xe, nameof(isOriginalAnkleTransformMSSet));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteQSTransform(xe, nameof(originalAnkleTransformMS), originalAnkleTransformMS);
            xs.WriteSerializeIgnored(xe, nameof(prevAnkleRotLS));
            xs.WriteVector4(xe, nameof(kneeAxisLS), kneeAxisLS);
            xs.WriteVector4(xe, nameof(footEndLS), footEndLS);
            xs.WriteClass<hkbEventProperty>(xe, nameof(ungroundedEvent), ungroundedEvent);
            xs.WriteFloat(xe, nameof(footPlantedAnkleHeightMS), footPlantedAnkleHeightMS);
            xs.WriteFloat(xe, nameof(footRaisedAnkleHeightMS), footRaisedAnkleHeightMS);
            xs.WriteFloat(xe, nameof(maxAnkleHeightMS), maxAnkleHeightMS);
            xs.WriteFloat(xe, nameof(minAnkleHeightMS), minAnkleHeightMS);
            xs.WriteFloat(xe, nameof(maxKneeAngleDegrees), maxKneeAngleDegrees);
            xs.WriteFloat(xe, nameof(minKneeAngleDegrees), minKneeAngleDegrees);
            xs.WriteFloat(xe, nameof(verticalError), verticalError);
            xs.WriteFloat(xe, nameof(maxAnkleAngleDegrees), maxAnkleAngleDegrees);
            xs.WriteNumber(xe, nameof(hipIndex), hipIndex);
            xs.WriteNumber(xe, nameof(kneeIndex), kneeIndex);
            xs.WriteNumber(xe, nameof(ankleIndex), ankleIndex);
            xs.WriteBoolean(xe, nameof(hitSomething), hitSomething);
            xs.WriteBoolean(xe, nameof(isPlantedMS), isPlantedMS);
            xs.WriteBoolean(xe, nameof(isOriginalAnkleTransformMSSet), isOriginalAnkleTransformMSSet);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbFootIkModifierLeg);
        }

        public bool Equals(hkbFootIkModifierLeg? other)
        {
            return other is not null &&
                   originalAnkleTransformMS.Equals(other.originalAnkleTransformMS) &&
                   kneeAxisLS.Equals(other.kneeAxisLS) &&
                   footEndLS.Equals(other.footEndLS) &&
                   ((ungroundedEvent is null && other.ungroundedEvent is null) || (ungroundedEvent is not null && other.ungroundedEvent is not null && ungroundedEvent.Equals((IHavokObject)other.ungroundedEvent))) &&
                   footPlantedAnkleHeightMS.Equals(other.footPlantedAnkleHeightMS) &&
                   footRaisedAnkleHeightMS.Equals(other.footRaisedAnkleHeightMS) &&
                   maxAnkleHeightMS.Equals(other.maxAnkleHeightMS) &&
                   minAnkleHeightMS.Equals(other.minAnkleHeightMS) &&
                   maxKneeAngleDegrees.Equals(other.maxKneeAngleDegrees) &&
                   minKneeAngleDegrees.Equals(other.minKneeAngleDegrees) &&
                   verticalError.Equals(other.verticalError) &&
                   maxAnkleAngleDegrees.Equals(other.maxAnkleAngleDegrees) &&
                   hipIndex.Equals(other.hipIndex) &&
                   kneeIndex.Equals(other.kneeIndex) &&
                   ankleIndex.Equals(other.ankleIndex) &&
                   hitSomething.Equals(other.hitSomething) &&
                   isPlantedMS.Equals(other.isPlantedMS) &&
                   isOriginalAnkleTransformMSSet.Equals(other.isOriginalAnkleTransformMSSet) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(originalAnkleTransformMS);
            hashcode.Add(kneeAxisLS);
            hashcode.Add(footEndLS);
            hashcode.Add(ungroundedEvent);
            hashcode.Add(footPlantedAnkleHeightMS);
            hashcode.Add(footRaisedAnkleHeightMS);
            hashcode.Add(maxAnkleHeightMS);
            hashcode.Add(minAnkleHeightMS);
            hashcode.Add(maxKneeAngleDegrees);
            hashcode.Add(minKneeAngleDegrees);
            hashcode.Add(verticalError);
            hashcode.Add(maxAnkleAngleDegrees);
            hashcode.Add(hipIndex);
            hashcode.Add(kneeIndex);
            hashcode.Add(ankleIndex);
            hashcode.Add(hitSomething);
            hashcode.Add(isPlantedMS);
            hashcode.Add(isOriginalAnkleTransformMSSet);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

