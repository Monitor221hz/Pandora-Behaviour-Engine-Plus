using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterControllerControlData Signatire: 0x5b6c03d9 size: 32 flags: FLAGS_NONE

    // desiredVelocity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // verticalGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // horizontalCatchUpGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // maxVerticalSeparation class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // maxHorizontalSeparation class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    public partial class hkbCharacterControllerControlData : IHavokObject, IEquatable<hkbCharacterControllerControlData?>
    {
        public Vector4 desiredVelocity { set; get; }
        public float verticalGain { set; get; }
        public float horizontalCatchUpGain { set; get; }
        public float maxVerticalSeparation { set; get; }
        public float maxHorizontalSeparation { set; get; }

        public virtual uint Signature { set; get; } = 0x5b6c03d9;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            desiredVelocity = br.ReadVector4();
            verticalGain = br.ReadSingle();
            horizontalCatchUpGain = br.ReadSingle();
            maxVerticalSeparation = br.ReadSingle();
            maxHorizontalSeparation = br.ReadSingle();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(desiredVelocity);
            bw.WriteSingle(verticalGain);
            bw.WriteSingle(horizontalCatchUpGain);
            bw.WriteSingle(maxVerticalSeparation);
            bw.WriteSingle(maxHorizontalSeparation);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            desiredVelocity = xd.ReadVector4(xe, nameof(desiredVelocity));
            verticalGain = xd.ReadSingle(xe, nameof(verticalGain));
            horizontalCatchUpGain = xd.ReadSingle(xe, nameof(horizontalCatchUpGain));
            maxVerticalSeparation = xd.ReadSingle(xe, nameof(maxVerticalSeparation));
            maxHorizontalSeparation = xd.ReadSingle(xe, nameof(maxHorizontalSeparation));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(desiredVelocity), desiredVelocity);
            xs.WriteFloat(xe, nameof(verticalGain), verticalGain);
            xs.WriteFloat(xe, nameof(horizontalCatchUpGain), horizontalCatchUpGain);
            xs.WriteFloat(xe, nameof(maxVerticalSeparation), maxVerticalSeparation);
            xs.WriteFloat(xe, nameof(maxHorizontalSeparation), maxHorizontalSeparation);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterControllerControlData);
        }

        public bool Equals(hkbCharacterControllerControlData? other)
        {
            return other is not null &&
                   desiredVelocity.Equals(other.desiredVelocity) &&
                   verticalGain.Equals(other.verticalGain) &&
                   horizontalCatchUpGain.Equals(other.horizontalCatchUpGain) &&
                   maxVerticalSeparation.Equals(other.maxVerticalSeparation) &&
                   maxHorizontalSeparation.Equals(other.maxHorizontalSeparation) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(desiredVelocity);
            hashcode.Add(verticalGain);
            hashcode.Add(horizontalCatchUpGain);
            hashcode.Add(maxVerticalSeparation);
            hashcode.Add(maxHorizontalSeparation);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

