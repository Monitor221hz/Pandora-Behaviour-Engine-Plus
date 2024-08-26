using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpRackAndPinionConstraintDataAtoms Signatire: 0xa58a9659 size: 160 flags: FLAGS_NONE

    // transforms class: hkpSetLocalTransformsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // rackAndPinion class: hkpRackAndPinionConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    public partial class hkpRackAndPinionConstraintDataAtoms : IHavokObject, IEquatable<hkpRackAndPinionConstraintDataAtoms?>
    {
        public hkpSetLocalTransformsConstraintAtom transforms { set; get; } = new();
        public hkpRackAndPinionConstraintAtom rackAndPinion { set; get; } = new();

        public virtual uint Signature { set; get; } = 0xa58a9659;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            transforms.Read(des, br);
            rackAndPinion.Read(des, br);
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            transforms.Write(s, bw);
            rackAndPinion.Write(s, bw);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            transforms = xd.ReadClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms));
            rackAndPinion = xd.ReadClass<hkpRackAndPinionConstraintAtom>(xe, nameof(rackAndPinion));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms), transforms);
            xs.WriteClass<hkpRackAndPinionConstraintAtom>(xe, nameof(rackAndPinion), rackAndPinion);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpRackAndPinionConstraintDataAtoms);
        }

        public bool Equals(hkpRackAndPinionConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((transforms is null && other.transforms is null) || (transforms is not null && other.transforms is not null && transforms.Equals((IHavokObject)other.transforms))) &&
                   ((rackAndPinion is null && other.rackAndPinion is null) || (rackAndPinion is not null && other.rackAndPinion is not null && rackAndPinion.Equals((IHavokObject)other.rackAndPinion))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(transforms);
            hashcode.Add(rackAndPinion);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

