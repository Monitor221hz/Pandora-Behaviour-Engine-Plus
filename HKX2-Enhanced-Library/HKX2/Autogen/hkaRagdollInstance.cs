using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaRagdollInstance Signatire: 0x154948e8 size: 72 flags: FLAGS_NONE

    // rigidBodies class: hkpRigidBody Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // constraints class: hkpConstraintInstance Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // boneToRigidBodyMap class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // skeleton class: hkaSkeleton Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkaRagdollInstance : hkReferencedObject, IEquatable<hkaRagdollInstance?>
    {
        public IList<hkpRigidBody> rigidBodies { set; get; } = Array.Empty<hkpRigidBody>();
        public IList<hkpConstraintInstance> constraints { set; get; } = Array.Empty<hkpConstraintInstance>();
        public IList<int> boneToRigidBodyMap { set; get; } = Array.Empty<int>();
        public hkaSkeleton? skeleton { set; get; }

        public override uint Signature { set; get; } = 0x154948e8;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rigidBodies = des.ReadClassPointerArray<hkpRigidBody>(br);
            constraints = des.ReadClassPointerArray<hkpConstraintInstance>(br);
            boneToRigidBodyMap = des.ReadInt32Array(br);
            skeleton = des.ReadClassPointer<hkaSkeleton>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, rigidBodies);
            s.WriteClassPointerArray(bw, constraints);
            s.WriteInt32Array(bw, boneToRigidBodyMap);
            s.WriteClassPointer(bw, skeleton);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rigidBodies = xd.ReadClassPointerArray<hkpRigidBody>(this, xe, nameof(rigidBodies));
            constraints = xd.ReadClassPointerArray<hkpConstraintInstance>(this, xe, nameof(constraints));
            boneToRigidBodyMap = xd.ReadInt32Array(xe, nameof(boneToRigidBodyMap));
            skeleton = xd.ReadClassPointer<hkaSkeleton>(this, xe, nameof(skeleton));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(rigidBodies), rigidBodies!);
            xs.WriteClassPointerArray(xe, nameof(constraints), constraints!);
            xs.WriteNumberArray(xe, nameof(boneToRigidBodyMap), boneToRigidBodyMap);
            xs.WriteClassPointer(xe, nameof(skeleton), skeleton);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaRagdollInstance);
        }

        public bool Equals(hkaRagdollInstance? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rigidBodies.SequenceEqual(other.rigidBodies) &&
                   constraints.SequenceEqual(other.constraints) &&
                   boneToRigidBodyMap.SequenceEqual(other.boneToRigidBodyMap) &&
                   ((skeleton is null && other.skeleton is null) || (skeleton is not null && other.skeleton is not null && skeleton.Equals((IHavokObject)other.skeleton))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rigidBodies.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(constraints.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(boneToRigidBodyMap.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(skeleton);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

