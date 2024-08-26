using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCameraShakeEventPayload Signatire: 0x64136982 size: 24 flags: FLAGS_NONE

    // amplitude class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // halfLife class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    public partial class hkbCameraShakeEventPayload : hkbEventPayload, IEquatable<hkbCameraShakeEventPayload?>
    {
        public float amplitude { set; get; }
        public float halfLife { set; get; }

        public override uint Signature { set; get; } = 0x64136982;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            amplitude = br.ReadSingle();
            halfLife = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(amplitude);
            bw.WriteSingle(halfLife);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            amplitude = xd.ReadSingle(xe, nameof(amplitude));
            halfLife = xd.ReadSingle(xe, nameof(halfLife));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(amplitude), amplitude);
            xs.WriteFloat(xe, nameof(halfLife), halfLife);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCameraShakeEventPayload);
        }

        public bool Equals(hkbCameraShakeEventPayload? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   amplitude.Equals(other.amplitude) &&
                   halfLife.Equals(other.halfLife) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(amplitude);
            hashcode.Add(halfLife);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

