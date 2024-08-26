using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkMoppBvTreeShapeBase Signatire: 0x7c338c66 size: 80 flags: FLAGS_NONE

    // code class: hkpMoppCode Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // moppData class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 48 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // moppDataSize class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // codeInfoCopy class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkMoppBvTreeShapeBase : hkpBvTreeShape, IEquatable<hkMoppBvTreeShapeBase?>
    {
        public hkpMoppCode? code { set; get; }
        private object? moppData { set; get; }
        private uint moppDataSize { set; get; }
        private Vector4 codeInfoCopy { set; get; }

        public override uint Signature { set; get; } = 0x7c338c66;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            code = des.ReadClassPointer<hkpMoppCode>(br);
            des.ReadEmptyPointer(br);
            moppDataSize = br.ReadUInt32();
            br.Position += 4;
            codeInfoCopy = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, code);
            s.WriteVoidPointer(bw);
            bw.WriteUInt32(moppDataSize);
            bw.Position += 4;
            bw.WriteVector4(codeInfoCopy);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            code = xd.ReadClassPointer<hkpMoppCode>(this, xe, nameof(code));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(code), code);
            xs.WriteSerializeIgnored(xe, nameof(moppData));
            xs.WriteSerializeIgnored(xe, nameof(moppDataSize));
            xs.WriteSerializeIgnored(xe, nameof(codeInfoCopy));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkMoppBvTreeShapeBase);
        }

        public bool Equals(hkMoppBvTreeShapeBase? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((code is null && other.code is null) || (code is not null && other.code is not null && code.Equals((IHavokObject)other.code))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(code);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

