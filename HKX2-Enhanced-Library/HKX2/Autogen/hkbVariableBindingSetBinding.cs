using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbVariableBindingSetBinding Signatire: 0x4d592f72 size: 40 flags: FLAGS_NONE

    // memberPath class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // memberClass class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 8 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // offsetInObjectPlusOne class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // offsetInArrayPlusOne class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 20 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // rootVariableIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 24 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // variableIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // bitIndex class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // bindingType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 33 flags: FLAGS_NONE enum: BindingType
    // memberType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 34 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // variableType class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 35 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // flags class:  Type.TYPE_FLAGS Type.TYPE_INT8 arrSize: 0 offset: 36 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbVariableBindingSetBinding : IHavokObject, IEquatable<hkbVariableBindingSetBinding?>
    {
        public string memberPath { set; get; } = "";
        private object? memberClass { set; get; }
        private int offsetInObjectPlusOne { set; get; }
        private int offsetInArrayPlusOne { set; get; }
        private int rootVariableIndex { set; get; }
        public int variableIndex { set; get; }
        public sbyte bitIndex { set; get; }
        public sbyte bindingType { set; get; }
        private byte memberType { set; get; }
        private sbyte variableType { set; get; }
        private sbyte flags { set; get; }

        public virtual uint Signature { set; get; } = 0x4d592f72;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            memberPath = des.ReadStringPointer(br);
            des.ReadEmptyPointer(br);
            offsetInObjectPlusOne = br.ReadInt32();
            offsetInArrayPlusOne = br.ReadInt32();
            rootVariableIndex = br.ReadInt32();
            variableIndex = br.ReadInt32();
            bitIndex = br.ReadSByte();
            bindingType = br.ReadSByte();
            memberType = br.ReadByte();
            variableType = br.ReadSByte();
            flags = br.ReadSByte();
            br.Position += 3;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteStringPointer(bw, memberPath);
            s.WriteVoidPointer(bw);
            bw.WriteInt32(offsetInObjectPlusOne);
            bw.WriteInt32(offsetInArrayPlusOne);
            bw.WriteInt32(rootVariableIndex);
            bw.WriteInt32(variableIndex);
            bw.WriteSByte(bitIndex);
            bw.WriteSByte(bindingType);
            bw.WriteByte(memberType);
            bw.WriteSByte(variableType);
            bw.WriteSByte(flags);
            bw.Position += 3;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            memberPath = xd.ReadString(xe, nameof(memberPath));
            variableIndex = xd.ReadInt32(xe, nameof(variableIndex));
            bitIndex = xd.ReadSByte(xe, nameof(bitIndex));
            bindingType = xd.ReadFlag<BindingType, sbyte>(xe, nameof(bindingType));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(memberPath), memberPath);
            xs.WriteSerializeIgnored(xe, nameof(memberClass));
            xs.WriteSerializeIgnored(xe, nameof(offsetInObjectPlusOne));
            xs.WriteSerializeIgnored(xe, nameof(offsetInArrayPlusOne));
            xs.WriteSerializeIgnored(xe, nameof(rootVariableIndex));
            xs.WriteNumber(xe, nameof(variableIndex), variableIndex);
            xs.WriteNumber(xe, nameof(bitIndex), bitIndex);
            xs.WriteEnum<BindingType, sbyte>(xe, nameof(bindingType), bindingType);
            xs.WriteSerializeIgnored(xe, nameof(memberType));
            xs.WriteSerializeIgnored(xe, nameof(variableType));
            xs.WriteSerializeIgnored(xe, nameof(flags));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbVariableBindingSetBinding);
        }

        public bool Equals(hkbVariableBindingSetBinding? other)
        {
            return other is not null &&
                   (memberPath is null && other.memberPath is null || memberPath == other.memberPath || memberPath is null && other.memberPath == "" || memberPath == "" && other.memberPath is null) &&
                   variableIndex.Equals(other.variableIndex) &&
                   bitIndex.Equals(other.bitIndex) &&
                   bindingType.Equals(other.bindingType) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(memberPath);
            hashcode.Add(variableIndex);
            hashcode.Add(bitIndex);
            hashcode.Add(bindingType);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

