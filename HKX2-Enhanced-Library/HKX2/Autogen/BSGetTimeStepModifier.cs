using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSGetTimeStepModifier Signatire: 0xbda33bfe size: 88 flags: FLAGS_NONE

    // timeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class BSGetTimeStepModifier : hkbModifier, IEquatable<BSGetTimeStepModifier?>
    {
        public float timeStep { set; get; }

        public override uint Signature { set; get; } = 0xbda33bfe;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            timeStep = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(timeStep);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            timeStep = xd.ReadSingle(xe, nameof(timeStep));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(timeStep), timeStep);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSGetTimeStepModifier);
        }

        public bool Equals(BSGetTimeStepModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   timeStep.Equals(other.timeStep) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(timeStep);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

