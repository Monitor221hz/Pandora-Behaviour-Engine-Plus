using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSModifyOnceModifier Signatire: 0x1e20a97a size: 112 flags: FLAGS_NONE

    // pOnActivateModifier class: hkbModifier Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: ALIGN_16|FLAGS_NONE enum: 
    // pOnDeactivateModifier class: hkbModifier Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 96 flags: ALIGN_16|FLAGS_NONE enum: 
    public partial class BSModifyOnceModifier : hkbModifier, IEquatable<BSModifyOnceModifier?>
    {
        public hkbModifier? pOnActivateModifier { set; get; }
        public hkbModifier? pOnDeactivateModifier { set; get; }

        public override uint Signature { set; get; } = 0x1e20a97a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            pOnActivateModifier = des.ReadClassPointer<hkbModifier>(br);
            br.Position += 8;
            pOnDeactivateModifier = des.ReadClassPointer<hkbModifier>(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, pOnActivateModifier);
            bw.Position += 8;
            s.WriteClassPointer(bw, pOnDeactivateModifier);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pOnActivateModifier = xd.ReadClassPointer<hkbModifier>(this, xe, nameof(pOnActivateModifier));
            pOnDeactivateModifier = xd.ReadClassPointer<hkbModifier>(this, xe, nameof(pOnDeactivateModifier));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(pOnActivateModifier), pOnActivateModifier);
            xs.WriteClassPointer(xe, nameof(pOnDeactivateModifier), pOnDeactivateModifier);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSModifyOnceModifier);
        }

        public bool Equals(BSModifyOnceModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((pOnActivateModifier is null && other.pOnActivateModifier is null) || (pOnActivateModifier is not null && other.pOnActivateModifier is not null && pOnActivateModifier.Equals((IHavokObject)other.pOnActivateModifier))) &&
                   ((pOnDeactivateModifier is null && other.pOnDeactivateModifier is null) || (pOnDeactivateModifier is not null && other.pOnDeactivateModifier is not null && pOnDeactivateModifier.Equals((IHavokObject)other.pOnDeactivateModifier))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pOnActivateModifier);
            hashcode.Add(pOnDeactivateModifier);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

