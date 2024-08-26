using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPointToPlaneConstraintDataAtoms Signatire: 0x749bc260 size: 160 flags: FLAGS_NONE

    // transforms class: hkpSetLocalTransformsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // lin class: hkpLinConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    public partial class hkpPointToPlaneConstraintDataAtoms : IHavokObject, IEquatable<hkpPointToPlaneConstraintDataAtoms?>
    {
        public hkpSetLocalTransformsConstraintAtom transforms { set; get; } = new();
        public hkpLinConstraintAtom lin { set; get; } = new();

        public virtual uint Signature { set; get; } = 0x749bc260;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            transforms.Read(des, br);
            lin.Read(des, br);
            br.Position += 12;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            transforms.Write(s, bw);
            lin.Write(s, bw);
            bw.Position += 12;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            transforms = xd.ReadClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms));
            lin = xd.ReadClass<hkpLinConstraintAtom>(xe, nameof(lin));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms), transforms);
            xs.WriteClass<hkpLinConstraintAtom>(xe, nameof(lin), lin);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPointToPlaneConstraintDataAtoms);
        }

        public bool Equals(hkpPointToPlaneConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((transforms is null && other.transforms is null) || (transforms is not null && other.transforms is not null && transforms.Equals((IHavokObject)other.transforms))) &&
                   ((lin is null && other.lin is null) || (lin is not null && other.lin is not null && lin.Equals((IHavokObject)other.lin))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(transforms);
            hashcode.Add(lin);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

