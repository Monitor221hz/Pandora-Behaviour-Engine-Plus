using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbManualSelectorGenerator Signatire: 0xd932fab8 size: 96 flags: FLAGS_NONE

    // generators class: hkbGenerator Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // selectedGeneratorIndex class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // currentGeneratorIndex class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 89 flags: FLAGS_NONE enum: 
    public partial class hkbManualSelectorGenerator : hkbGenerator, IEquatable<hkbManualSelectorGenerator?>
    {
        public IList<hkbGenerator> generators { set; get; } = Array.Empty<hkbGenerator>();
        public sbyte selectedGeneratorIndex { set; get; }
        public sbyte currentGeneratorIndex { set; get; }

        public override uint Signature { set; get; } = 0xd932fab8;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            generators = des.ReadClassPointerArray<hkbGenerator>(br);
            selectedGeneratorIndex = br.ReadSByte();
            currentGeneratorIndex = br.ReadSByte();
            br.Position += 6;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, generators);
            bw.WriteSByte(selectedGeneratorIndex);
            bw.WriteSByte(currentGeneratorIndex);
            bw.Position += 6;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            generators = xd.ReadClassPointerArray<hkbGenerator>(this, xe, nameof(generators));
            selectedGeneratorIndex = xd.ReadSByte(xe, nameof(selectedGeneratorIndex));
            currentGeneratorIndex = xd.ReadSByte(xe, nameof(currentGeneratorIndex));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(generators), generators!);
            xs.WriteNumber(xe, nameof(selectedGeneratorIndex), selectedGeneratorIndex);
            xs.WriteNumber(xe, nameof(currentGeneratorIndex), currentGeneratorIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbManualSelectorGenerator);
        }

        public bool Equals(hkbManualSelectorGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   generators.SequenceEqual(other.generators) &&
                   selectedGeneratorIndex.Equals(other.selectedGeneratorIndex) &&
                   currentGeneratorIndex.Equals(other.currentGeneratorIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(generators.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(selectedGeneratorIndex);
            hashcode.Add(currentGeneratorIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

