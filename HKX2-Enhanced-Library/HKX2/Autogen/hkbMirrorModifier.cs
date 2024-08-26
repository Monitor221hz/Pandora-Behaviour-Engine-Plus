using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbMirrorModifier Signatire: 0xa9a271ea size: 88 flags: FLAGS_NONE

    // isAdditive class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkbMirrorModifier : hkbModifier, IEquatable<hkbMirrorModifier?>
    {
        public bool isAdditive { set; get; }

        public override uint Signature { set; get; } = 0xa9a271ea;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isAdditive = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(isAdditive);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isAdditive = xd.ReadBoolean(xe, nameof(isAdditive));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(isAdditive), isAdditive);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbMirrorModifier);
        }

        public bool Equals(hkbMirrorModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isAdditive.Equals(other.isAdditive) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(isAdditive);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

