using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpTriangleShape Signatire: 0x95ad1a25 size: 112 flags: FLAGS_NONE

    // weldingInfo class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // weldingType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 42 flags: FLAGS_NONE enum: WeldingType
    // isExtruded class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 43 flags: FLAGS_NONE enum: 
    // vertexA class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // vertexB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // vertexC class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // extrusion class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    public partial class hkpTriangleShape : hkpConvexShape, IEquatable<hkpTriangleShape?>
    {
        public ushort weldingInfo { set; get; }
        public byte weldingType { set; get; }
        public byte isExtruded { set; get; }
        public Vector4 vertexA { set; get; }
        public Vector4 vertexB { set; get; }
        public Vector4 vertexC { set; get; }
        public Vector4 extrusion { set; get; }

        public override uint Signature { set; get; } = 0x95ad1a25;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            weldingInfo = br.ReadUInt16();
            weldingType = br.ReadByte();
            isExtruded = br.ReadByte();
            br.Position += 4;
            vertexA = br.ReadVector4();
            vertexB = br.ReadVector4();
            vertexC = br.ReadVector4();
            extrusion = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt16(weldingInfo);
            bw.WriteByte(weldingType);
            bw.WriteByte(isExtruded);
            bw.Position += 4;
            bw.WriteVector4(vertexA);
            bw.WriteVector4(vertexB);
            bw.WriteVector4(vertexC);
            bw.WriteVector4(extrusion);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            weldingInfo = xd.ReadUInt16(xe, nameof(weldingInfo));
            weldingType = xd.ReadFlag<WeldingType, byte>(xe, nameof(weldingType));
            isExtruded = xd.ReadByte(xe, nameof(isExtruded));
            vertexA = xd.ReadVector4(xe, nameof(vertexA));
            vertexB = xd.ReadVector4(xe, nameof(vertexB));
            vertexC = xd.ReadVector4(xe, nameof(vertexC));
            extrusion = xd.ReadVector4(xe, nameof(extrusion));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(weldingInfo), weldingInfo);
            xs.WriteEnum<WeldingType, byte>(xe, nameof(weldingType), weldingType);
            xs.WriteNumber(xe, nameof(isExtruded), isExtruded);
            xs.WriteVector4(xe, nameof(vertexA), vertexA);
            xs.WriteVector4(xe, nameof(vertexB), vertexB);
            xs.WriteVector4(xe, nameof(vertexC), vertexC);
            xs.WriteVector4(xe, nameof(extrusion), extrusion);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpTriangleShape);
        }

        public bool Equals(hkpTriangleShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   weldingInfo.Equals(other.weldingInfo) &&
                   weldingType.Equals(other.weldingType) &&
                   isExtruded.Equals(other.isExtruded) &&
                   vertexA.Equals(other.vertexA) &&
                   vertexB.Equals(other.vertexB) &&
                   vertexC.Equals(other.vertexC) &&
                   extrusion.Equals(other.extrusion) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(weldingInfo);
            hashcode.Add(weldingType);
            hashcode.Add(isExtruded);
            hashcode.Add(vertexA);
            hashcode.Add(vertexB);
            hashcode.Add(vertexC);
            hashcode.Add(extrusion);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

