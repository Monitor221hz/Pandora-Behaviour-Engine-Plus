using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbModifier Signatire: 0x96ec5ced size: 80 flags: FLAGS_NONE

    // enable class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // padModifier class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 3 offset: 73 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbModifier : hkbNode, IEquatable<hkbModifier?>
    {
        public bool enable { set; get; }
        public bool[] padModifier = new bool[3];

        public override uint Signature { set; get; } = 0x96ec5ced;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            enable = br.ReadBoolean();
            padModifier = des.ReadBooleanCStyleArray(br, 3);
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(enable);
            s.WriteBooleanCStyleArray(bw, padModifier);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            enable = xd.ReadBoolean(xe, nameof(enable));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(enable), enable);
            xs.WriteSerializeIgnored(xe, nameof(padModifier));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbModifier);
        }

        public bool Equals(hkbModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   enable.Equals(other.enable) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(enable);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

