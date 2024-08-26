using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSetupStabilizationAtom Signatire: 0xf05d137e size: 16 flags: FLAGS_NONE

    // enabled class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // maxAngle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // padding class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 8 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkpSetupStabilizationAtom : hkpConstraintAtom, IEquatable<hkpSetupStabilizationAtom?>
    {
        public bool enabled { set; get; }
        public float maxAngle { set; get; }
        public byte[] padding = new byte[8];

        public override uint Signature { set; get; } = 0xf05d137e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            enabled = br.ReadBoolean();
            br.Position += 1;
            maxAngle = br.ReadSingle();
            padding = des.ReadByteCStyleArray(br, 8);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(enabled);
            bw.Position += 1;
            bw.WriteSingle(maxAngle);
            s.WriteByteCStyleArray(bw, padding);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            enabled = xd.ReadBoolean(xe, nameof(enabled));
            maxAngle = xd.ReadSingle(xe, nameof(maxAngle));
            padding = xd.ReadByteCStyleArray(xe, nameof(padding), 8);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(enabled), enabled);
            xs.WriteFloat(xe, nameof(maxAngle), maxAngle);
            xs.WriteNumberArray(xe, nameof(padding), padding);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSetupStabilizationAtom);
        }

        public bool Equals(hkpSetupStabilizationAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   enabled.Equals(other.enabled) &&
                   maxAngle.Equals(other.maxAngle) &&
                   padding.SequenceEqual(other.padding) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(enabled);
            hashcode.Add(maxAngle);
            hashcode.Add(padding.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

