using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // BSBoneSwitchGenerator Signatire: 0xf33d3eea size: 112 flags: FLAGS_NONE

    // pDefaultGenerator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: ALIGN_16|FLAGS_NONE enum: 
    // ChildrenA class: BSBoneSwitchGeneratorBoneData Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    public partial class BSBoneSwitchGenerator : hkbGenerator, IEquatable<BSBoneSwitchGenerator?>
    {
        public hkbGenerator? pDefaultGenerator { set; get; }
        public IList<BSBoneSwitchGeneratorBoneData> ChildrenA { set; get; } = Array.Empty<BSBoneSwitchGeneratorBoneData>();

        public override uint Signature { set; get; } = 0xf33d3eea;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            pDefaultGenerator = des.ReadClassPointer<hkbGenerator>(br);
            ChildrenA = des.ReadClassPointerArray<BSBoneSwitchGeneratorBoneData>(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            s.WriteClassPointer(bw, pDefaultGenerator);
            s.WriteClassPointerArray(bw, ChildrenA);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pDefaultGenerator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(pDefaultGenerator));
            ChildrenA = xd.ReadClassPointerArray<BSBoneSwitchGeneratorBoneData>(this, xe, nameof(ChildrenA));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(pDefaultGenerator), pDefaultGenerator);
            xs.WriteClassPointerArray(xe, nameof(ChildrenA), ChildrenA!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSBoneSwitchGenerator);
        }

        public bool Equals(BSBoneSwitchGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((pDefaultGenerator is null && other.pDefaultGenerator is null) || (pDefaultGenerator is not null && other.pDefaultGenerator is not null && pDefaultGenerator.Equals((IHavokObject)other.pDefaultGenerator))) &&
                   ChildrenA.SequenceEqual(other.ChildrenA) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pDefaultGenerator);
            hashcode.Add(ChildrenA.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

