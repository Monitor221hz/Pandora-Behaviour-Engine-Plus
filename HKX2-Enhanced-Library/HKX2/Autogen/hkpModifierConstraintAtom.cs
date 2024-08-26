using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpModifierConstraintAtom Signatire: 0xb13fef1f size: 48 flags: FLAGS_NONE

    // modifierAtomSize class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 16 flags: ALIGN_16|FLAGS_NONE enum: 
    // childSize class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 18 flags: FLAGS_NONE enum: 
    // child class: hkpConstraintAtom Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // pad class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 2 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkpModifierConstraintAtom : hkpConstraintAtom, IEquatable<hkpModifierConstraintAtom?>
    {
        public ushort modifierAtomSize { set; get; }
        public ushort childSize { set; get; }
        public hkpConstraintAtom? child { set; get; }
        public uint[] pad = new uint[2];

        public override uint Signature { set; get; } = 0xb13fef1f;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 14;
            modifierAtomSize = br.ReadUInt16();
            childSize = br.ReadUInt16();
            br.Position += 4;
            child = des.ReadClassPointer<hkpConstraintAtom>(br);
            pad = des.ReadUInt32CStyleArray(br, 2);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 14;
            bw.WriteUInt16(modifierAtomSize);
            bw.WriteUInt16(childSize);
            bw.Position += 4;
            s.WriteClassPointer(bw, child);
            s.WriteUInt32CStyleArray(bw, pad);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            modifierAtomSize = xd.ReadUInt16(xe, nameof(modifierAtomSize));
            childSize = xd.ReadUInt16(xe, nameof(childSize));
            child = xd.ReadClassPointer<hkpConstraintAtom>(this, xe, nameof(child));
            pad = xd.ReadUInt32CStyleArray(xe, nameof(pad), 2);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(modifierAtomSize), modifierAtomSize);
            xs.WriteNumber(xe, nameof(childSize), childSize);
            xs.WriteClassPointer(xe, nameof(child), child);
            xs.WriteNumberArray(xe, nameof(pad), pad);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpModifierConstraintAtom);
        }

        public bool Equals(hkpModifierConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   modifierAtomSize.Equals(other.modifierAtomSize) &&
                   childSize.Equals(other.childSize) &&
                   ((child is null && other.child is null) || (child is not null && other.child is not null && child.Equals((IHavokObject)other.child))) &&
                   pad.SequenceEqual(other.pad) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(modifierAtomSize);
            hashcode.Add(childSize);
            hashcode.Add(child);
            hashcode.Add(pad.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

