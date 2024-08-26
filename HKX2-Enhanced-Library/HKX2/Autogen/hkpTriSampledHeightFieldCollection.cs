using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpTriSampledHeightFieldCollection Signatire: 0xc291ddde size: 96 flags: FLAGS_NONE

    // heightfield class: hkpSampledHeightFieldShape Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // childSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // radius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // weldingInfo class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // triangleExtrusion class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkpTriSampledHeightFieldCollection : hkpShapeCollection, IEquatable<hkpTriSampledHeightFieldCollection?>
    {
        public hkpSampledHeightFieldShape? heightfield { set; get; }
        private int childSize { set; get; }
        public float radius { set; get; }
        public IList<ushort> weldingInfo { set; get; } = Array.Empty<ushort>();
        public Vector4 triangleExtrusion { set; get; }

        public override uint Signature { set; get; } = 0xc291ddde;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            heightfield = des.ReadClassPointer<hkpSampledHeightFieldShape>(br);
            childSize = br.ReadInt32();
            radius = br.ReadSingle();
            weldingInfo = des.ReadUInt16Array(br);
            triangleExtrusion = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, heightfield);
            bw.WriteInt32(childSize);
            bw.WriteSingle(radius);
            s.WriteUInt16Array(bw, weldingInfo);
            bw.WriteVector4(triangleExtrusion);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            heightfield = xd.ReadClassPointer<hkpSampledHeightFieldShape>(this, xe, nameof(heightfield));
            radius = xd.ReadSingle(xe, nameof(radius));
            weldingInfo = xd.ReadUInt16Array(xe, nameof(weldingInfo));
            triangleExtrusion = xd.ReadVector4(xe, nameof(triangleExtrusion));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(heightfield), heightfield);
            xs.WriteSerializeIgnored(xe, nameof(childSize));
            xs.WriteFloat(xe, nameof(radius), radius);
            xs.WriteNumberArray(xe, nameof(weldingInfo), weldingInfo);
            xs.WriteVector4(xe, nameof(triangleExtrusion), triangleExtrusion);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpTriSampledHeightFieldCollection);
        }

        public bool Equals(hkpTriSampledHeightFieldCollection? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((heightfield is null && other.heightfield is null) || (heightfield is not null && other.heightfield is not null && heightfield.Equals((IHavokObject)other.heightfield))) &&
                   radius.Equals(other.radius) &&
                   weldingInfo.SequenceEqual(other.weldingInfo) &&
                   triangleExtrusion.Equals(other.triangleExtrusion) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(heightfield);
            hashcode.Add(radius);
            hashcode.Add(weldingInfo.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(triangleExtrusion);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

