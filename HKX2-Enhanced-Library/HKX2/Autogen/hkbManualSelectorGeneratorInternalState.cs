using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbManualSelectorGeneratorInternalState Signatire: 0x492c6137 size: 24 flags: FLAGS_NONE

    // currentGeneratorIndex class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbManualSelectorGeneratorInternalState : hkReferencedObject, IEquatable<hkbManualSelectorGeneratorInternalState?>
    {
        public sbyte currentGeneratorIndex { set; get; }

        public override uint Signature { set; get; } = 0x492c6137;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            currentGeneratorIndex = br.ReadSByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSByte(currentGeneratorIndex);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            currentGeneratorIndex = xd.ReadSByte(xe, nameof(currentGeneratorIndex));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(currentGeneratorIndex), currentGeneratorIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbManualSelectorGeneratorInternalState);
        }

        public bool Equals(hkbManualSelectorGeneratorInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   currentGeneratorIndex.Equals(other.currentGeneratorIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(currentGeneratorIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

