using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPhysicsSystem Signatire: 0xff724c17 size: 104 flags: FLAGS_NONE

    // rigidBodies class: hkpRigidBody Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // constraints class: hkpConstraintInstance Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // actions class: hkpAction Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // phantoms class: hkpPhantom Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // userData class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // active class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    public partial class hkpPhysicsSystem : hkReferencedObject, IEquatable<hkpPhysicsSystem?>
    {
        public IList<hkpRigidBody> rigidBodies { set; get; } = Array.Empty<hkpRigidBody>();
        public IList<hkpConstraintInstance> constraints { set; get; } = Array.Empty<hkpConstraintInstance>();
        public IList<hkpAction> actions { set; get; } = Array.Empty<hkpAction>();
        public IList<hkpPhantom> phantoms { set; get; } = Array.Empty<hkpPhantom>();
        public string name { set; get; } = "";
        public ulong userData { set; get; }
        public bool active { set; get; }

        public override uint Signature { set; get; } = 0xff724c17;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rigidBodies = des.ReadClassPointerArray<hkpRigidBody>(br);
            constraints = des.ReadClassPointerArray<hkpConstraintInstance>(br);
            actions = des.ReadClassPointerArray<hkpAction>(br);
            phantoms = des.ReadClassPointerArray<hkpPhantom>(br);
            name = des.ReadStringPointer(br);
            userData = br.ReadUInt64();
            active = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, rigidBodies);
            s.WriteClassPointerArray(bw, constraints);
            s.WriteClassPointerArray(bw, actions);
            s.WriteClassPointerArray(bw, phantoms);
            s.WriteStringPointer(bw, name);
            bw.WriteUInt64(userData);
            bw.WriteBoolean(active);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rigidBodies = xd.ReadClassPointerArray<hkpRigidBody>(this, xe, nameof(rigidBodies));
            constraints = xd.ReadClassPointerArray<hkpConstraintInstance>(this, xe, nameof(constraints));
            actions = xd.ReadClassPointerArray<hkpAction>(this, xe, nameof(actions));
            phantoms = xd.ReadClassPointerArray<hkpPhantom>(this, xe, nameof(phantoms));
            name = xd.ReadString(xe, nameof(name));
            userData = xd.ReadUInt64(xe, nameof(userData));
            active = xd.ReadBoolean(xe, nameof(active));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(rigidBodies), rigidBodies!);
            xs.WriteClassPointerArray(xe, nameof(constraints), constraints!);
            xs.WriteClassPointerArray(xe, nameof(actions), actions!);
            xs.WriteClassPointerArray(xe, nameof(phantoms), phantoms!);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteNumber(xe, nameof(userData), userData);
            xs.WriteBoolean(xe, nameof(active), active);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPhysicsSystem);
        }

        public bool Equals(hkpPhysicsSystem? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rigidBodies.SequenceEqual(other.rigidBodies) &&
                   constraints.SequenceEqual(other.constraints) &&
                   actions.SequenceEqual(other.actions) &&
                   phantoms.SequenceEqual(other.phantoms) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   userData.Equals(other.userData) &&
                   active.Equals(other.active) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rigidBodies.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(constraints.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(actions.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(phantoms.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(name);
            hashcode.Add(userData);
            hashcode.Add(active);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

