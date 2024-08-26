using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSetWordVariableCommand Signatire: 0xf3ae5fca size: 64 flags: FLAGS_NONE

    // quadValue class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // variableId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // value class: hkbVariableValue Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    // type class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: VariableType
    // global class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 49 flags: FLAGS_NONE enum: 
    public partial class hkbSetWordVariableCommand : hkReferencedObject, IEquatable<hkbSetWordVariableCommand?>
    {
        public Vector4 quadValue { set; get; }
        public ulong characterId { set; get; }
        public int variableId { set; get; }
        public hkbVariableValue value { set; get; } = new();
        public byte type { set; get; }
        public bool global { set; get; }

        public override uint Signature { set; get; } = 0xf3ae5fca;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            quadValue = br.ReadVector4();
            characterId = br.ReadUInt64();
            variableId = br.ReadInt32();
            value.Read(des, br);
            type = br.ReadByte();
            global = br.ReadBoolean();
            br.Position += 14;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(quadValue);
            bw.WriteUInt64(characterId);
            bw.WriteInt32(variableId);
            value.Write(s, bw);
            bw.WriteByte(type);
            bw.WriteBoolean(global);
            bw.Position += 14;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            quadValue = xd.ReadVector4(xe, nameof(quadValue));
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            variableId = xd.ReadInt32(xe, nameof(variableId));
            value = xd.ReadClass<hkbVariableValue>(xe, nameof(value));
            type = xd.ReadFlag<VariableType, byte>(xe, nameof(type));
            global = xd.ReadBoolean(xe, nameof(global));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(quadValue), quadValue);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteNumber(xe, nameof(variableId), variableId);
            xs.WriteClass<hkbVariableValue>(xe, nameof(value), value);
            xs.WriteEnum<VariableType, byte>(xe, nameof(type), type);
            xs.WriteBoolean(xe, nameof(global), global);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSetWordVariableCommand);
        }

        public bool Equals(hkbSetWordVariableCommand? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   quadValue.Equals(other.quadValue) &&
                   characterId.Equals(other.characterId) &&
                   variableId.Equals(other.variableId) &&
                   ((value is null && other.value is null) || (value is not null && other.value is not null && value.Equals((IHavokObject)other.value))) &&
                   type.Equals(other.type) &&
                   global.Equals(other.global) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(quadValue);
            hashcode.Add(characterId);
            hashcode.Add(variableId);
            hashcode.Add(value);
            hashcode.Add(type);
            hashcode.Add(global);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

