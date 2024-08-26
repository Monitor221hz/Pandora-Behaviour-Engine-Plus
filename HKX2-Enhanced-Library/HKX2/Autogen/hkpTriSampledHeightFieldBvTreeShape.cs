using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpTriSampledHeightFieldBvTreeShape Signatire: 0x58e1e585 size: 80 flags: FLAGS_NONE

    // childContainer class: hkpSingleShapeContainer Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // childSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // wantAabbRejectionTest class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // padding class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 12 offset: 61 flags: FLAGS_NONE enum: 
    public partial class hkpTriSampledHeightFieldBvTreeShape : hkpBvTreeShape, IEquatable<hkpTriSampledHeightFieldBvTreeShape?>
    {
        public hkpSingleShapeContainer childContainer { set; get; } = new();
        private int childSize { set; get; }
        public bool wantAabbRejectionTest { set; get; }
        public byte[] padding = new byte[12];

        public override uint Signature { set; get; } = 0x58e1e585;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            childContainer.Read(des, br);
            childSize = br.ReadInt32();
            wantAabbRejectionTest = br.ReadBoolean();
            padding = des.ReadByteCStyleArray(br, 12);
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            childContainer.Write(s, bw);
            bw.WriteInt32(childSize);
            bw.WriteBoolean(wantAabbRejectionTest);
            s.WriteByteCStyleArray(bw, padding);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            childContainer = xd.ReadClass<hkpSingleShapeContainer>(xe, nameof(childContainer));
            wantAabbRejectionTest = xd.ReadBoolean(xe, nameof(wantAabbRejectionTest));
            padding = xd.ReadByteCStyleArray(xe, nameof(padding), 12);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpSingleShapeContainer>(xe, nameof(childContainer), childContainer);
            xs.WriteSerializeIgnored(xe, nameof(childSize));
            xs.WriteBoolean(xe, nameof(wantAabbRejectionTest), wantAabbRejectionTest);
            xs.WriteNumberArray(xe, nameof(padding), padding);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpTriSampledHeightFieldBvTreeShape);
        }

        public bool Equals(hkpTriSampledHeightFieldBvTreeShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((childContainer is null && other.childContainer is null) || (childContainer is not null && other.childContainer is not null && childContainer.Equals((IHavokObject)other.childContainer))) &&
                   wantAabbRejectionTest.Equals(other.wantAabbRejectionTest) &&
                   padding.SequenceEqual(other.padding) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(childContainer);
            hashcode.Add(wantAabbRejectionTest);
            hashcode.Add(padding.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

