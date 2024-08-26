using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpDisplayBindingData Signatire: 0xdc46c906 size: 48 flags: FLAGS_NONE

    // rigidBodyBindings class: hkpDisplayBindingDataRigidBody Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // physicsSystemBindings class: hkpDisplayBindingDataPhysicsSystem Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkpDisplayBindingData : hkReferencedObject, IEquatable<hkpDisplayBindingData?>
    {
        public IList<hkpDisplayBindingDataRigidBody> rigidBodyBindings { set; get; } = Array.Empty<hkpDisplayBindingDataRigidBody>();
        public IList<hkpDisplayBindingDataPhysicsSystem> physicsSystemBindings { set; get; } = Array.Empty<hkpDisplayBindingDataPhysicsSystem>();

        public override uint Signature { set; get; } = 0xdc46c906;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rigidBodyBindings = des.ReadClassPointerArray<hkpDisplayBindingDataRigidBody>(br);
            physicsSystemBindings = des.ReadClassPointerArray<hkpDisplayBindingDataPhysicsSystem>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, rigidBodyBindings);
            s.WriteClassPointerArray(bw, physicsSystemBindings);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rigidBodyBindings = xd.ReadClassPointerArray<hkpDisplayBindingDataRigidBody>(this, xe, nameof(rigidBodyBindings));
            physicsSystemBindings = xd.ReadClassPointerArray<hkpDisplayBindingDataPhysicsSystem>(this, xe, nameof(physicsSystemBindings));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(rigidBodyBindings), rigidBodyBindings!);
            xs.WriteClassPointerArray(xe, nameof(physicsSystemBindings), physicsSystemBindings!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpDisplayBindingData);
        }

        public bool Equals(hkpDisplayBindingData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rigidBodyBindings.SequenceEqual(other.rigidBodyBindings) &&
                   physicsSystemBindings.SequenceEqual(other.physicsSystemBindings) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rigidBodyBindings.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(physicsSystemBindings.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

