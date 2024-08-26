using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPhysicsData Signatire: 0xc2a461e4 size: 40 flags: FLAGS_NONE

    // worldCinfo class: hkpWorldCinfo Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // systems class: hkpPhysicsSystem Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    public partial class hkpPhysicsData : hkReferencedObject, IEquatable<hkpPhysicsData?>
    {
        public hkpWorldCinfo? worldCinfo { set; get; }
        public IList<hkpPhysicsSystem> systems { set; get; } = Array.Empty<hkpPhysicsSystem>();

        public override uint Signature { set; get; } = 0xc2a461e4;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            worldCinfo = des.ReadClassPointer<hkpWorldCinfo>(br);
            systems = des.ReadClassPointerArray<hkpPhysicsSystem>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, worldCinfo);
            s.WriteClassPointerArray(bw, systems);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            worldCinfo = xd.ReadClassPointer<hkpWorldCinfo>(this, xe, nameof(worldCinfo));
            systems = xd.ReadClassPointerArray<hkpPhysicsSystem>(this, xe, nameof(systems));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(worldCinfo), worldCinfo);
            xs.WriteClassPointerArray(xe, nameof(systems), systems!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPhysicsData);
        }

        public bool Equals(hkpPhysicsData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((worldCinfo is null && other.worldCinfo is null) || (worldCinfo is not null && other.worldCinfo is not null && worldCinfo.Equals((IHavokObject)other.worldCinfo))) &&
                   systems.SequenceEqual(other.systems) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(worldCinfo);
            hashcode.Add(systems.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

