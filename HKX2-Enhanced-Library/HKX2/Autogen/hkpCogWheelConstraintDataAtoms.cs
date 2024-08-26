using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCogWheelConstraintDataAtoms Signatire: 0xf855ba44 size: 160 flags: FLAGS_NONE

    // transforms class: hkpSetLocalTransformsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // cogWheels class: hkpCogWheelConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    public partial class hkpCogWheelConstraintDataAtoms : IHavokObject, IEquatable<hkpCogWheelConstraintDataAtoms?>
    {
        public hkpSetLocalTransformsConstraintAtom transforms { set; get; } = new();
        public hkpCogWheelConstraintAtom cogWheels { set; get; } = new();

        public virtual uint Signature { set; get; } = 0xf855ba44;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            transforms.Read(des, br);
            cogWheels.Read(des, br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            transforms.Write(s, bw);
            cogWheels.Write(s, bw);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            transforms = xd.ReadClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms));
            cogWheels = xd.ReadClass<hkpCogWheelConstraintAtom>(xe, nameof(cogWheels));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms), transforms);
            xs.WriteClass<hkpCogWheelConstraintAtom>(xe, nameof(cogWheels), cogWheels);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCogWheelConstraintDataAtoms);
        }

        public bool Equals(hkpCogWheelConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((transforms is null && other.transforms is null) || (transforms is not null && other.transforms is not null && transforms.Equals((IHavokObject)other.transforms))) &&
                   ((cogWheels is null && other.cogWheels is null) || (cogWheels is not null && other.cogWheels is not null && cogWheels.Equals((IHavokObject)other.cogWheels))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(transforms);
            hashcode.Add(cogWheels);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

