using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMeshShape Signatire: 0x3bf12c0f size: 128 flags: FLAGS_NONE

    // scaling class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // numBitsForSubpartIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // subparts class: hkpMeshShapeSubpart Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // weldingInfo class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // weldingType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 104 flags: FLAGS_NONE enum: WeldingType
    // radius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // pad class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 3 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkpMeshShape : hkpShapeCollection, IEquatable<hkpMeshShape?>
    {
        public Vector4 scaling { set; get; }
        public int numBitsForSubpartIndex { set; get; }
        public IList<hkpMeshShapeSubpart> subparts { set; get; } = Array.Empty<hkpMeshShapeSubpart>();
        public IList<ushort> weldingInfo { set; get; } = Array.Empty<ushort>();
        public byte weldingType { set; get; }
        public float radius { set; get; }
        public int[] pad = new int[3];

        public override uint Signature { set; get; } = 0x3bf12c0f;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            scaling = br.ReadVector4();
            numBitsForSubpartIndex = br.ReadInt32();
            br.Position += 4;
            subparts = des.ReadClassArray<hkpMeshShapeSubpart>(br);
            weldingInfo = des.ReadUInt16Array(br);
            weldingType = br.ReadByte();
            br.Position += 3;
            radius = br.ReadSingle();
            pad = des.ReadInt32CStyleArray(br, 3);
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(scaling);
            bw.WriteInt32(numBitsForSubpartIndex);
            bw.Position += 4;
            s.WriteClassArray(bw, subparts);
            s.WriteUInt16Array(bw, weldingInfo);
            bw.WriteByte(weldingType);
            bw.Position += 3;
            bw.WriteSingle(radius);
            s.WriteInt32CStyleArray(bw, pad);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            scaling = xd.ReadVector4(xe, nameof(scaling));
            numBitsForSubpartIndex = xd.ReadInt32(xe, nameof(numBitsForSubpartIndex));
            subparts = xd.ReadClassArray<hkpMeshShapeSubpart>(xe, nameof(subparts));
            weldingInfo = xd.ReadUInt16Array(xe, nameof(weldingInfo));
            weldingType = xd.ReadFlag<WeldingType, byte>(xe, nameof(weldingType));
            radius = xd.ReadSingle(xe, nameof(radius));
            pad = xd.ReadInt32CStyleArray(xe, nameof(pad), 3);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(scaling), scaling);
            xs.WriteNumber(xe, nameof(numBitsForSubpartIndex), numBitsForSubpartIndex);
            xs.WriteClassArray(xe, nameof(subparts), subparts);
            xs.WriteNumberArray(xe, nameof(weldingInfo), weldingInfo);
            xs.WriteEnum<WeldingType, byte>(xe, nameof(weldingType), weldingType);
            xs.WriteFloat(xe, nameof(radius), radius);
            xs.WriteNumberArray(xe, nameof(pad), pad);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMeshShape);
        }

        public bool Equals(hkpMeshShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   scaling.Equals(other.scaling) &&
                   numBitsForSubpartIndex.Equals(other.numBitsForSubpartIndex) &&
                   subparts.SequenceEqual(other.subparts) &&
                   weldingInfo.SequenceEqual(other.weldingInfo) &&
                   weldingType.Equals(other.weldingType) &&
                   radius.Equals(other.radius) &&
                   pad.SequenceEqual(other.pad) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(scaling);
            hashcode.Add(numBitsForSubpartIndex);
            hashcode.Add(subparts.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(weldingInfo.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(weldingType);
            hashcode.Add(radius);
            hashcode.Add(pad.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

