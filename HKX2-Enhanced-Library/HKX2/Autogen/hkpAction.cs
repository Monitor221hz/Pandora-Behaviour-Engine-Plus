using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpAction Signatire: 0xbdf70a51 size: 48 flags: FLAGS_NONE

    // world class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // island class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 24 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // userData class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    public partial class hkpAction : hkReferencedObject, IEquatable<hkpAction?>
    {
        private object? world { set; get; }
        private object? island { set; get; }
        public ulong userData { set; get; }
        public string name { set; get; } = "";

        public override uint Signature { set; get; } = 0xbdf70a51;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            userData = br.ReadUInt64();
            name = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            bw.WriteUInt64(userData);
            s.WriteStringPointer(bw, name);
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
            xs.WriteSerializeIgnored(xe, nameof(world));
            xs.WriteSerializeIgnored(xe, nameof(island));
            xs.WriteNumber(xe, nameof(userData), userData);
            xs.WriteString(xe, nameof(name), name);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpAction);
        }

        public bool Equals(hkpAction? other)
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

