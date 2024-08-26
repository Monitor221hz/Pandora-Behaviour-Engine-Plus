using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkUiAttribute Signatire: 0xeb6e96e3 size: 40 flags: FLAGS_NONE

    // visible class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // hideInModeler class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 1 flags: FLAGS_NONE enum: HideInModeler
    // label class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // group class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // hideBaseClassMembers class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // endGroup class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // endGroup2 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 33 flags: FLAGS_NONE enum: 
    // advanced class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 34 flags: FLAGS_NONE enum: 
    public partial class hkUiAttribute : IHavokObject, IEquatable<hkUiAttribute?>
    {
        public bool visible { set; get; }
        public sbyte hideInModeler { set; get; }
        public string label { set; get; } = "";
        public string group { set; get; } = "";
        public string hideBaseClassMembers { set; get; } = "";
        public bool endGroup { set; get; }
        public bool endGroup2 { set; get; }
        public bool advanced { set; get; }

        public virtual uint Signature { set; get; } = 0xeb6e96e3;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            visible = br.ReadBoolean();
            hideInModeler = br.ReadSByte();
            br.Position += 6;
            label = des.ReadCString(br);
            group = des.ReadCString(br);
            hideBaseClassMembers = des.ReadCString(br);
            endGroup = br.ReadBoolean();
            endGroup2 = br.ReadBoolean();
            advanced = br.ReadBoolean();
            br.Position += 5;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteBoolean(visible);
            bw.WriteSByte(hideInModeler);
            bw.Position += 6;
            s.WriteCString(bw, label);
            s.WriteCString(bw, group);
            s.WriteCString(bw, hideBaseClassMembers);
            bw.WriteBoolean(endGroup);
            bw.WriteBoolean(endGroup2);
            bw.WriteBoolean(advanced);
            bw.Position += 5;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            visible = xd.ReadBoolean(xe, nameof(visible));
            hideInModeler = xd.ReadFlag<HideInModeler, sbyte>(xe, nameof(hideInModeler));
            label = xd.ReadString(xe, nameof(label));
            group = xd.ReadString(xe, nameof(group));
            hideBaseClassMembers = xd.ReadString(xe, nameof(hideBaseClassMembers));
            endGroup = xd.ReadBoolean(xe, nameof(endGroup));
            endGroup2 = xd.ReadBoolean(xe, nameof(endGroup2));
            advanced = xd.ReadBoolean(xe, nameof(advanced));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteBoolean(xe, nameof(visible), visible);
            xs.WriteEnum<HideInModeler, sbyte>(xe, nameof(hideInModeler), hideInModeler);
            xs.WriteString(xe, nameof(label), label);
            xs.WriteString(xe, nameof(group), group);
            xs.WriteString(xe, nameof(hideBaseClassMembers), hideBaseClassMembers);
            xs.WriteBoolean(xe, nameof(endGroup), endGroup);
            xs.WriteBoolean(xe, nameof(endGroup2), endGroup2);
            xs.WriteBoolean(xe, nameof(advanced), advanced);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkUiAttribute);
        }

        public bool Equals(hkUiAttribute? other)
        {
            return other is not null &&
                   visible.Equals(other.visible) &&
                   hideInModeler.Equals(other.hideInModeler) &&
                   (label is null && other.label is null || label == other.label || label is null && other.label == "" || label == "" && other.label is null) &&
                   (group is null && other.group is null || group == other.group || group is null && other.group == "" || group == "" && other.group is null) &&
                   (hideBaseClassMembers is null && other.hideBaseClassMembers is null || hideBaseClassMembers == other.hideBaseClassMembers || hideBaseClassMembers is null && other.hideBaseClassMembers == "" || hideBaseClassMembers == "" && other.hideBaseClassMembers is null) &&
                   endGroup.Equals(other.endGroup) &&
                   endGroup2.Equals(other.endGroup2) &&
                   advanced.Equals(other.advanced) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(visible);
            hashcode.Add(hideInModeler);
            hashcode.Add(label);
            hashcode.Add(group);
            hashcode.Add(hideBaseClassMembers);
            hashcode.Add(endGroup);
            hashcode.Add(endGroup2);
            hashcode.Add(advanced);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

