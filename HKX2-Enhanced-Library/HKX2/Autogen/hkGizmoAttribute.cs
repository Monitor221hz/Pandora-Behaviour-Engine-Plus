using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkGizmoAttribute Signatire: 0x23aadfb6 size: 24 flags: FLAGS_NONE

    // visible class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // label class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // type class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: GizmoType
    public partial class hkGizmoAttribute : IHavokObject, IEquatable<hkGizmoAttribute?>
    {
        public bool visible { set; get; }
        public string label { set; get; } = "";
        public sbyte type { set; get; }

        public virtual uint Signature { set; get; } = 0x23aadfb6;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            visible = br.ReadBoolean();
            br.Position += 7;
            label = des.ReadCString(br);
            type = br.ReadSByte();
            br.Position += 7;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteBoolean(visible);
            bw.Position += 7;
            s.WriteCString(bw, label);
            bw.WriteSByte(type);
            bw.Position += 7;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            visible = xd.ReadBoolean(xe, nameof(visible));
            label = xd.ReadString(xe, nameof(label));
            type = xd.ReadFlag<GizmoType, sbyte>(xe, nameof(type));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteBoolean(xe, nameof(visible), visible);
            xs.WriteString(xe, nameof(label), label);
            xs.WriteEnum<GizmoType, sbyte>(xe, nameof(type), type);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkGizmoAttribute);
        }

        public bool Equals(hkGizmoAttribute? other)
        {
            return other is not null &&
                   visible.Equals(other.visible) &&
                   (label is null && other.label is null || label == other.label || label is null && other.label == "" || label == "" && other.label is null) &&
                   type.Equals(other.type) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(visible);
            hashcode.Add(label);
            hashcode.Add(type);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

