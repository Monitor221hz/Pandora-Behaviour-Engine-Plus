using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpDisplayBindingDataRigidBody Signatire: 0xfe16e2a3 size: 96 flags: FLAGS_NONE

    // rigidBody class: hkpRigidBody Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // displayObjectPtr class: hkReferencedObject Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // rigidBodyFromDisplayObjectTransform class:  Type.TYPE_MATRIX4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkpDisplayBindingDataRigidBody : hkReferencedObject, IEquatable<hkpDisplayBindingDataRigidBody?>
    {
        public hkpRigidBody? rigidBody { set; get; }
        public hkReferencedObject? displayObjectPtr { set; get; }
        public Matrix4x4 rigidBodyFromDisplayObjectTransform { set; get; }

        public override uint Signature { set; get; } = 0xfe16e2a3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rigidBody = des.ReadClassPointer<hkpRigidBody>(br);
            displayObjectPtr = des.ReadClassPointer<hkReferencedObject>(br);
            rigidBodyFromDisplayObjectTransform = des.ReadMatrix4(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, rigidBody);
            s.WriteClassPointer(bw, displayObjectPtr);
            s.WriteMatrix4(bw, rigidBodyFromDisplayObjectTransform);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rigidBody = xd.ReadClassPointer<hkpRigidBody>(this, xe, nameof(rigidBody));
            displayObjectPtr = xd.ReadClassPointer<hkReferencedObject>(this, xe, nameof(displayObjectPtr));
            rigidBodyFromDisplayObjectTransform = xd.ReadMatrix4(xe, nameof(rigidBodyFromDisplayObjectTransform));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(rigidBody), rigidBody);
            xs.WriteClassPointer(xe, nameof(displayObjectPtr), displayObjectPtr);
            xs.WriteMatrix4(xe, nameof(rigidBodyFromDisplayObjectTransform), rigidBodyFromDisplayObjectTransform);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpDisplayBindingDataRigidBody);
        }

        public bool Equals(hkpDisplayBindingDataRigidBody? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((rigidBody is null && other.rigidBody is null) || (rigidBody is not null && other.rigidBody is not null && rigidBody.Equals((IHavokObject)other.rigidBody))) &&
                   ((displayObjectPtr is null && other.displayObjectPtr is null) || (displayObjectPtr is not null && other.displayObjectPtr is not null && displayObjectPtr.Equals((IHavokObject)other.displayObjectPtr))) &&
                   rigidBodyFromDisplayObjectTransform.Equals(other.rigidBodyFromDisplayObjectTransform) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rigidBody);
            hashcode.Add(displayObjectPtr);
            hashcode.Add(rigidBodyFromDisplayObjectTransform);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

