using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaQuantizedAnimation Signatire: 0x3920f053 size: 88 flags: FLAGS_NONE

    // data class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // endian class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // skeleton class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 80 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkaQuantizedAnimation : hkaAnimation, IEquatable<hkaQuantizedAnimation?>
    {
        public IList<byte> data { set; get; } = Array.Empty<byte>();
        public uint endian { set; get; }
        private object? skeleton { set; get; }

        public override uint Signature { set; get; } = 0x3920f053;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            data = des.ReadByteArray(br);
            endian = br.ReadUInt32();
            br.Position += 4;
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteByteArray(bw, data);
            bw.WriteUInt32(endian);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            data = xd.ReadByteArray(xe, nameof(data));
            endian = xd.ReadUInt32(xe, nameof(endian));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(data), data);
            xs.WriteNumber(xe, nameof(endian), endian);
            xs.WriteSerializeIgnored(xe, nameof(skeleton));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaQuantizedAnimation);
        }

        public bool Equals(hkaQuantizedAnimation? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   data.SequenceEqual(other.data) &&
                   endian.Equals(other.endian) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(endian);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

