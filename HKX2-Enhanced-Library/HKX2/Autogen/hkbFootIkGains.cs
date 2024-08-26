using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbFootIkGains Signatire: 0xa681b7f0 size: 48 flags: FLAGS_NONE

    // onOffGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // groundAscendingGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // groundDescendingGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // footPlantedGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // footRaisedGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // footUnlockGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // worldFromModelFeedbackGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // errorUpDownBias class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // alignWorldFromModelGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // hipOrientationGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // maxKneeAngleDifference class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // ankleOrientationGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    public partial class hkbFootIkGains : IHavokObject, IEquatable<hkbFootIkGains?>
    {
        public float onOffGain { set; get; }
        public float groundAscendingGain { set; get; }
        public float groundDescendingGain { set; get; }
        public float footPlantedGain { set; get; }
        public float footRaisedGain { set; get; }
        public float footUnlockGain { set; get; }
        public float worldFromModelFeedbackGain { set; get; }
        public float errorUpDownBias { set; get; }
        public float alignWorldFromModelGain { set; get; }
        public float hipOrientationGain { set; get; }
        public float maxKneeAngleDifference { set; get; }
        public float ankleOrientationGain { set; get; }

        public virtual uint Signature { set; get; } = 0xa681b7f0;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            onOffGain = br.ReadSingle();
            groundAscendingGain = br.ReadSingle();
            groundDescendingGain = br.ReadSingle();
            footPlantedGain = br.ReadSingle();
            footRaisedGain = br.ReadSingle();
            footUnlockGain = br.ReadSingle();
            worldFromModelFeedbackGain = br.ReadSingle();
            errorUpDownBias = br.ReadSingle();
            alignWorldFromModelGain = br.ReadSingle();
            hipOrientationGain = br.ReadSingle();
            maxKneeAngleDifference = br.ReadSingle();
            ankleOrientationGain = br.ReadSingle();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(onOffGain);
            bw.WriteSingle(groundAscendingGain);
            bw.WriteSingle(groundDescendingGain);
            bw.WriteSingle(footPlantedGain);
            bw.WriteSingle(footRaisedGain);
            bw.WriteSingle(footUnlockGain);
            bw.WriteSingle(worldFromModelFeedbackGain);
            bw.WriteSingle(errorUpDownBias);
            bw.WriteSingle(alignWorldFromModelGain);
            bw.WriteSingle(hipOrientationGain);
            bw.WriteSingle(maxKneeAngleDifference);
            bw.WriteSingle(ankleOrientationGain);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            onOffGain = xd.ReadSingle(xe, nameof(onOffGain));
            groundAscendingGain = xd.ReadSingle(xe, nameof(groundAscendingGain));
            groundDescendingGain = xd.ReadSingle(xe, nameof(groundDescendingGain));
            footPlantedGain = xd.ReadSingle(xe, nameof(footPlantedGain));
            footRaisedGain = xd.ReadSingle(xe, nameof(footRaisedGain));
            footUnlockGain = xd.ReadSingle(xe, nameof(footUnlockGain));
            worldFromModelFeedbackGain = xd.ReadSingle(xe, nameof(worldFromModelFeedbackGain));
            errorUpDownBias = xd.ReadSingle(xe, nameof(errorUpDownBias));
            alignWorldFromModelGain = xd.ReadSingle(xe, nameof(alignWorldFromModelGain));
            hipOrientationGain = xd.ReadSingle(xe, nameof(hipOrientationGain));
            maxKneeAngleDifference = xd.ReadSingle(xe, nameof(maxKneeAngleDifference));
            ankleOrientationGain = xd.ReadSingle(xe, nameof(ankleOrientationGain));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(onOffGain), onOffGain);
            xs.WriteFloat(xe, nameof(groundAscendingGain), groundAscendingGain);
            xs.WriteFloat(xe, nameof(groundDescendingGain), groundDescendingGain);
            xs.WriteFloat(xe, nameof(footPlantedGain), footPlantedGain);
            xs.WriteFloat(xe, nameof(footRaisedGain), footRaisedGain);
            xs.WriteFloat(xe, nameof(footUnlockGain), footUnlockGain);
            xs.WriteFloat(xe, nameof(worldFromModelFeedbackGain), worldFromModelFeedbackGain);
            xs.WriteFloat(xe, nameof(errorUpDownBias), errorUpDownBias);
            xs.WriteFloat(xe, nameof(alignWorldFromModelGain), alignWorldFromModelGain);
            xs.WriteFloat(xe, nameof(hipOrientationGain), hipOrientationGain);
            xs.WriteFloat(xe, nameof(maxKneeAngleDifference), maxKneeAngleDifference);
            xs.WriteFloat(xe, nameof(ankleOrientationGain), ankleOrientationGain);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbFootIkGains);
        }

        public bool Equals(hkbFootIkGains? other)
        {
            return other is not null &&
                   onOffGain.Equals(other.onOffGain) &&
                   groundAscendingGain.Equals(other.groundAscendingGain) &&
                   groundDescendingGain.Equals(other.groundDescendingGain) &&
                   footPlantedGain.Equals(other.footPlantedGain) &&
                   footRaisedGain.Equals(other.footRaisedGain) &&
                   footUnlockGain.Equals(other.footUnlockGain) &&
                   worldFromModelFeedbackGain.Equals(other.worldFromModelFeedbackGain) &&
                   errorUpDownBias.Equals(other.errorUpDownBias) &&
                   alignWorldFromModelGain.Equals(other.alignWorldFromModelGain) &&
                   hipOrientationGain.Equals(other.hipOrientationGain) &&
                   maxKneeAngleDifference.Equals(other.maxKneeAngleDifference) &&
                   ankleOrientationGain.Equals(other.ankleOrientationGain) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(onOffGain);
            hashcode.Add(groundAscendingGain);
            hashcode.Add(groundDescendingGain);
            hashcode.Add(footPlantedGain);
            hashcode.Add(footRaisedGain);
            hashcode.Add(footUnlockGain);
            hashcode.Add(worldFromModelFeedbackGain);
            hashcode.Add(errorUpDownBias);
            hashcode.Add(alignWorldFromModelGain);
            hashcode.Add(hipOrientationGain);
            hashcode.Add(maxKneeAngleDifference);
            hashcode.Add(ankleOrientationGain);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

