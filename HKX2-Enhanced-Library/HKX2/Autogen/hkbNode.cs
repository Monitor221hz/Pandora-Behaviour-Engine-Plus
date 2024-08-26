using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbNode Signatire: 0x6d26f61d size: 72 flags: FLAGS_NONE

    // userData class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // id class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 64 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // cloneState class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 66 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // padNode class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 1 offset: 67 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbNode : hkbBindable, IEquatable<hkbNode?>
    {
        public ulong userData { set; get; }
        public string name { set; get; } = "";
        private short id { set; get; }
        private sbyte cloneState { set; get; }
        public bool[] padNode = new bool[1];

        public override uint Signature { set; get; } = 0x6d26f61d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            userData = br.ReadUInt64();
            name = des.ReadStringPointer(br);
            id = br.ReadInt16();
            cloneState = br.ReadSByte();
            padNode = des.ReadBooleanCStyleArray(br, 1);
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(userData);
            s.WriteStringPointer(bw, name);
            bw.WriteInt16(id);
            bw.WriteSByte(cloneState);
            s.WriteBooleanCStyleArray(bw, padNode);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            userData = xd.ReadUInt64(xe, nameof(userData));
            name = xd.ReadString(xe, nameof(name));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(userData), userData);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteSerializeIgnored(xe, nameof(id));
            xs.WriteSerializeIgnored(xe, nameof(cloneState));
            xs.WriteSerializeIgnored(xe, nameof(padNode));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbNode);
        }

        public bool Equals(hkbNode? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   userData.Equals(other.userData) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(userData);
            hashcode.Add(name);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

