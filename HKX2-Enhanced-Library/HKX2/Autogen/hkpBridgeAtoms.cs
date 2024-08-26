using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBridgeAtoms Signatire: 0xde152a4d size: 24 flags: FLAGS_NONE

    // bridgeAtom class: hkpBridgeConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkpBridgeAtoms : IHavokObject, IEquatable<hkpBridgeAtoms?>
    {
        public hkpBridgeConstraintAtom bridgeAtom { set; get; } = new();

        public virtual uint Signature { set; get; } = 0xde152a4d;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            bridgeAtom.Read(des, br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bridgeAtom.Write(s, bw);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            bridgeAtom = xd.ReadClass<hkpBridgeConstraintAtom>(xe, nameof(bridgeAtom));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpBridgeConstraintAtom>(xe, nameof(bridgeAtom), bridgeAtom);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBridgeAtoms);
        }

        public bool Equals(hkpBridgeAtoms? other)
        {
            return other is not null &&
                   ((bridgeAtom is null && other.bridgeAtom is null) || (bridgeAtom is not null && other.bridgeAtom is not null && bridgeAtom.Equals((IHavokObject)other.bridgeAtom))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(bridgeAtom);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

