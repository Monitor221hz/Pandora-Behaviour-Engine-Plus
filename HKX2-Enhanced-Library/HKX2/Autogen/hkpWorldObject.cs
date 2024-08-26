using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpWorldObject Signatire: 0x49fb6f2e size: 208 flags: FLAGS_NONE

    // world class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // userData class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // collidable class: hkpLinkedCollidable Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // multiThreadCheck class: hkMultiThreadCheck Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // properties class: hkpProperty Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    // treeData class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 200 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpWorldObject : hkReferencedObject, IEquatable<hkpWorldObject?>
    {
        private object? world { set; get; }
        public ulong userData { set; get; }
        public hkpLinkedCollidable collidable { set; get; } = new();
        public hkMultiThreadCheck multiThreadCheck { set; get; } = new();
        public string name { set; get; } = "";
        public IList<hkpProperty> properties { set; get; } = Array.Empty<hkpProperty>();
        private object? treeData { set; get; }

        public override uint Signature { set; get; } = 0x49fb6f2e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            des.ReadEmptyPointer(br);
            userData = br.ReadUInt64();
            collidable.Read(des, br);
            multiThreadCheck.Read(des, br);
            br.Position += 4;
            name = des.ReadStringPointer(br);
            properties = des.ReadClassArray<hkpProperty>(br);
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVoidPointer(bw);
            bw.WriteUInt64(userData);
            collidable.Write(s, bw);
            multiThreadCheck.Write(s, bw);
            bw.Position += 4;
            s.WriteStringPointer(bw, name);
            s.WriteClassArray(bw, properties);
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            userData = xd.ReadUInt64(xe, nameof(userData));
            collidable = xd.ReadClass<hkpLinkedCollidable>(xe, nameof(collidable));
            multiThreadCheck = xd.ReadClass<hkMultiThreadCheck>(xe, nameof(multiThreadCheck));
            name = xd.ReadString(xe, nameof(name));
            properties = xd.ReadClassArray<hkpProperty>(xe, nameof(properties));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(world));
            xs.WriteNumber(xe, nameof(userData), userData);
            xs.WriteClass<hkpLinkedCollidable>(xe, nameof(collidable), collidable);
            xs.WriteClass<hkMultiThreadCheck>(xe, nameof(multiThreadCheck), multiThreadCheck);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteClassArray(xe, nameof(properties), properties);
            xs.WriteSerializeIgnored(xe, nameof(treeData));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpWorldObject);
        }

        public bool Equals(hkpWorldObject? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   userData.Equals(other.userData) &&
                   ((collidable is null && other.collidable is null) || (collidable is not null && other.collidable is not null && collidable.Equals((IHavokObject)other.collidable))) &&
                   ((multiThreadCheck is null && other.multiThreadCheck is null) || (multiThreadCheck is not null && other.multiThreadCheck is not null && multiThreadCheck.Equals((IHavokObject)other.multiThreadCheck))) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   properties.SequenceEqual(other.properties) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(userData);
            hashcode.Add(collidable);
            hashcode.Add(multiThreadCheck);
            hashcode.Add(name);
            hashcode.Add(properties.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

