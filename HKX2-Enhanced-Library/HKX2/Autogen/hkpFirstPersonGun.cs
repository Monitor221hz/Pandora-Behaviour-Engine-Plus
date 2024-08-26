using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpFirstPersonGun Signatire: 0x852ab70b size: 56 flags: FLAGS_NONE

    // type class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // keyboardKey class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: KeyboardKey
    // listeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 40 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpFirstPersonGun : hkReferencedObject, IEquatable<hkpFirstPersonGun?>
    {
        private byte type { set; get; }
        public string name { set; get; } = "";
        public byte keyboardKey { set; get; }
        public IList<object> listeners { set; get; } = Array.Empty<object>();

        public override uint Signature { set; get; } = 0x852ab70b;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            type = br.ReadByte();
            br.Position += 7;
            name = des.ReadStringPointer(br);
            keyboardKey = br.ReadByte();
            br.Position += 7;
            des.ReadEmptyArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(type);
            bw.Position += 7;
            s.WriteStringPointer(bw, name);
            bw.WriteByte(keyboardKey);
            bw.Position += 7;
            s.WriteVoidArray(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            name = xd.ReadString(xe, nameof(name));
            keyboardKey = xd.ReadFlag<KeyboardKey, byte>(xe, nameof(keyboardKey));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(type));
            xs.WriteString(xe, nameof(name), name);
            xs.WriteEnum<KeyboardKey, byte>(xe, nameof(keyboardKey), keyboardKey);
            xs.WriteSerializeIgnored(xe, nameof(listeners));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpFirstPersonGun);
        }

        public bool Equals(hkpFirstPersonGun? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   keyboardKey.Equals(other.keyboardKey) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(name);
            hashcode.Add(keyboardKey);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

