using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCompressedMeshShapeBigTriangle Signatire: 0xcbfc95a4 size: 16 flags: FLAGS_NONE

    // a class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // b class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // c class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // material class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // weldingInfo class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // transformIndex class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 14 flags: FLAGS_NONE enum: 
    public partial class hkpCompressedMeshShapeBigTriangle : IHavokObject, IEquatable<hkpCompressedMeshShapeBigTriangle?>
    {
        public ushort a { set; get; }
        public ushort b { set; get; }
        public ushort c { set; get; }
        public uint material { set; get; }
        public ushort weldingInfo { set; get; }
        public ushort transformIndex { set; get; }

        public virtual uint Signature { set; get; } = 0xcbfc95a4;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            a = br.ReadUInt16();
            b = br.ReadUInt16();
            c = br.ReadUInt16();
            br.Position += 2;
            material = br.ReadUInt32();
            weldingInfo = br.ReadUInt16();
            transformIndex = br.ReadUInt16();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt16(a);
            bw.WriteUInt16(b);
            bw.WriteUInt16(c);
            bw.Position += 2;
            bw.WriteUInt32(material);
            bw.WriteUInt16(weldingInfo);
            bw.WriteUInt16(transformIndex);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            a = xd.ReadUInt16(xe, nameof(a));
            b = xd.ReadUInt16(xe, nameof(b));
            c = xd.ReadUInt16(xe, nameof(c));
            material = xd.ReadUInt32(xe, nameof(material));
            weldingInfo = xd.ReadUInt16(xe, nameof(weldingInfo));
            transformIndex = xd.ReadUInt16(xe, nameof(transformIndex));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(a), a);
            xs.WriteNumber(xe, nameof(b), b);
            xs.WriteNumber(xe, nameof(c), c);
            xs.WriteNumber(xe, nameof(material), material);
            xs.WriteNumber(xe, nameof(weldingInfo), weldingInfo);
            xs.WriteNumber(xe, nameof(transformIndex), transformIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCompressedMeshShapeBigTriangle);
        }

        public bool Equals(hkpCompressedMeshShapeBigTriangle? other)
        {
            return other is not null &&
                   a.Equals(other.a) &&
                   b.Equals(other.b) &&
                   c.Equals(other.c) &&
                   material.Equals(other.material) &&
                   weldingInfo.Equals(other.weldingInfo) &&
                   transformIndex.Equals(other.transformIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(a);
            hashcode.Add(b);
            hashcode.Add(c);
            hashcode.Add(material);
            hashcode.Add(weldingInfo);
            hashcode.Add(transformIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

