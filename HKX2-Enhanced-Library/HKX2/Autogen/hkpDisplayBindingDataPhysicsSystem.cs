using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpDisplayBindingDataPhysicsSystem Signatire: 0xc8ae86a7 size: 40 flags: FLAGS_NONE

    // bindings class: hkpDisplayBindingDataRigidBody Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // system class: hkpPhysicsSystem Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkpDisplayBindingDataPhysicsSystem : hkReferencedObject, IEquatable<hkpDisplayBindingDataPhysicsSystem?>
    {
        public IList<hkpDisplayBindingDataRigidBody> bindings { set; get; } = Array.Empty<hkpDisplayBindingDataRigidBody>();
        public hkpPhysicsSystem? system { set; get; }

        public override uint Signature { set; get; } = 0xc8ae86a7;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            bindings = des.ReadClassPointerArray<hkpDisplayBindingDataRigidBody>(br);
            system = des.ReadClassPointer<hkpPhysicsSystem>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, bindings);
            s.WriteClassPointer(bw, system);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            bindings = xd.ReadClassPointerArray<hkpDisplayBindingDataRigidBody>(this, xe, nameof(bindings));
            system = xd.ReadClassPointer<hkpPhysicsSystem>(this, xe, nameof(system));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(bindings), bindings!);
            xs.WriteClassPointer(xe, nameof(system), system);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpDisplayBindingDataPhysicsSystem);
        }

        public bool Equals(hkpDisplayBindingDataPhysicsSystem? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   bindings.SequenceEqual(other.bindings) &&
                   ((system is null && other.system is null) || (system is not null && other.system is not null && system.Equals((IHavokObject)other.system))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(bindings.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(system);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

