using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkSweptTransform Signatire: 0xb4e5770 size: 80 flags: FLAGS_NONE

    // centerOfMass0 class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // centerOfMass1 class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // rotation0 class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // rotation1 class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // centerOfMassLocal class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkSweptTransform : IHavokObject, IEquatable<hkSweptTransform?>
    {
        public Vector4 centerOfMass0 { set; get; }
        public Vector4 centerOfMass1 { set; get; }
        public Quaternion rotation0 { set; get; }
        public Quaternion rotation1 { set; get; }
        public Vector4 centerOfMassLocal { set; get; }

        public virtual uint Signature { set; get; } = 0xb4e5770;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            centerOfMass0 = br.ReadVector4();
            centerOfMass1 = br.ReadVector4();
            rotation0 = des.ReadQuaternion(br);
            rotation1 = des.ReadQuaternion(br);
            centerOfMassLocal = br.ReadVector4();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(centerOfMass0);
            bw.WriteVector4(centerOfMass1);
            s.WriteQuaternion(bw, rotation0);
            s.WriteQuaternion(bw, rotation1);
            bw.WriteVector4(centerOfMassLocal);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            centerOfMass0 = xd.ReadVector4(xe, nameof(centerOfMass0));
            centerOfMass1 = xd.ReadVector4(xe, nameof(centerOfMass1));
            rotation0 = xd.ReadQuaternion(xe, nameof(rotation0));
            rotation1 = xd.ReadQuaternion(xe, nameof(rotation1));
            centerOfMassLocal = xd.ReadVector4(xe, nameof(centerOfMassLocal));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(centerOfMass0), centerOfMass0);
            xs.WriteVector4(xe, nameof(centerOfMass1), centerOfMass1);
            xs.WriteQuaternion(xe, nameof(rotation0), rotation0);
            xs.WriteQuaternion(xe, nameof(rotation1), rotation1);
            xs.WriteVector4(xe, nameof(centerOfMassLocal), centerOfMassLocal);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkSweptTransform);
        }

        public bool Equals(hkSweptTransform? other)
        {
            return other is not null &&
                   centerOfMass0.Equals(other.centerOfMass0) &&
                   centerOfMass1.Equals(other.centerOfMass1) &&
                   rotation0.Equals(other.rotation0) &&
                   rotation1.Equals(other.rotation1) &&
                   centerOfMassLocal.Equals(other.centerOfMassLocal) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(centerOfMass0);
            hashcode.Add(centerOfMass1);
            hashcode.Add(rotation0);
            hashcode.Add(rotation1);
            hashcode.Add(centerOfMassLocal);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

