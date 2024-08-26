using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpListShape Signatire: 0xa1937cbd size: 144 flags: FLAGS_NONE

    // childInfo class: hkpListShapeChildInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // flags class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // numDisabledChildren class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 66 flags: FLAGS_NONE enum: 
    // aabbHalfExtents class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // aabbCenter class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // enabledChildren class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 8 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkpListShape : hkpShapeCollection, IEquatable<hkpListShape?>
    {
        public IList<hkpListShapeChildInfo> childInfo { set; get; } = Array.Empty<hkpListShapeChildInfo>();
        public ushort flags { set; get; }
        public ushort numDisabledChildren { set; get; }
        public Vector4 aabbHalfExtents { set; get; }
        public Vector4 aabbCenter { set; get; }
        public uint[] enabledChildren = new uint[8];

        public override uint Signature { set; get; } = 0xa1937cbd;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            childInfo = des.ReadClassArray<hkpListShapeChildInfo>(br);
            flags = br.ReadUInt16();
            numDisabledChildren = br.ReadUInt16();
            br.Position += 12;
            aabbHalfExtents = br.ReadVector4();
            aabbCenter = br.ReadVector4();
            enabledChildren = des.ReadUInt32CStyleArray(br, 8);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, childInfo);
            bw.WriteUInt16(flags);
            bw.WriteUInt16(numDisabledChildren);
            bw.Position += 12;
            bw.WriteVector4(aabbHalfExtents);
            bw.WriteVector4(aabbCenter);
            s.WriteUInt32CStyleArray(bw, enabledChildren);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            childInfo = xd.ReadClassArray<hkpListShapeChildInfo>(xe, nameof(childInfo));
            flags = xd.ReadUInt16(xe, nameof(flags));
            numDisabledChildren = xd.ReadUInt16(xe, nameof(numDisabledChildren));
            aabbHalfExtents = xd.ReadVector4(xe, nameof(aabbHalfExtents));
            aabbCenter = xd.ReadVector4(xe, nameof(aabbCenter));
            enabledChildren = xd.ReadUInt32CStyleArray(xe, nameof(enabledChildren), 8);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(childInfo), childInfo);
            xs.WriteNumber(xe, nameof(flags), flags);
            xs.WriteNumber(xe, nameof(numDisabledChildren), numDisabledChildren);
            xs.WriteVector4(xe, nameof(aabbHalfExtents), aabbHalfExtents);
            xs.WriteVector4(xe, nameof(aabbCenter), aabbCenter);
            xs.WriteNumberArray(xe, nameof(enabledChildren), enabledChildren);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpListShape);
        }

        public bool Equals(hkpListShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   childInfo.SequenceEqual(other.childInfo) &&
                   flags.Equals(other.flags) &&
                   numDisabledChildren.Equals(other.numDisabledChildren) &&
                   aabbHalfExtents.Equals(other.aabbHalfExtents) &&
                   aabbCenter.Equals(other.aabbCenter) &&
                   enabledChildren.SequenceEqual(other.enabledChildren) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(childInfo.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(flags);
            hashcode.Add(numDisabledChildren);
            hashcode.Add(aabbHalfExtents);
            hashcode.Add(aabbCenter);
            hashcode.Add(enabledChildren.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

