using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpAngConstraintAtom Signatire: 0x35bb3cd0 size: 4 flags: FLAGS_NONE

    // firstConstrainedAxis class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // numConstrainedAxes class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 3 flags: FLAGS_NONE enum: 
    public partial class hkpAngConstraintAtom : hkpConstraintAtom, IEquatable<hkpAngConstraintAtom?>
    {
        public byte firstConstrainedAxis { set; get; }
        public byte numConstrainedAxes { set; get; }

        public override uint Signature { set; get; } = 0x35bb3cd0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            firstConstrainedAxis = br.ReadByte();
            numConstrainedAxes = br.ReadByte();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(firstConstrainedAxis);
            bw.WriteByte(numConstrainedAxes);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            firstConstrainedAxis = xd.ReadByte(xe, nameof(firstConstrainedAxis));
            numConstrainedAxes = xd.ReadByte(xe, nameof(numConstrainedAxes));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(firstConstrainedAxis), firstConstrainedAxis);
            xs.WriteNumber(xe, nameof(numConstrainedAxes), numConstrainedAxes);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpAngConstraintAtom);
        }

        public bool Equals(hkpAngConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   firstConstrainedAxis.Equals(other.firstConstrainedAxis) &&
                   numConstrainedAxes.Equals(other.numConstrainedAxes) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(firstConstrainedAxis);
            hashcode.Add(numConstrainedAxes);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

