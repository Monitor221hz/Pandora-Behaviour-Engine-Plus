using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCompiledExpressionSetToken Signatire: 0xc6aaccc8 size: 8 flags: FLAGS_NONE

    // data class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // type class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 4 flags: FLAGS_NONE enum: TokenType
    // @operator class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 5 flags: FLAGS_NONE enum: Operator
    public partial class hkbCompiledExpressionSetToken : IHavokObject, IEquatable<hkbCompiledExpressionSetToken?>
    {
        public float data { set; get; }
        public sbyte type { set; get; }
        public sbyte @operator { set; get; }

        public virtual uint Signature { set; get; } = 0xc6aaccc8;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            data = br.ReadSingle();
            type = br.ReadSByte();
            @operator = br.ReadSByte();
            br.Position += 2;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(data);
            bw.WriteSByte(type);
            bw.WriteSByte(@operator);
            bw.Position += 2;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            data = xd.ReadSingle(xe, nameof(data));
            type = xd.ReadFlag<TokenType, sbyte>(xe, nameof(type));
            @operator = xd.ReadFlag<Operator, sbyte>(xe, nameof(@operator));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(data), data);
            xs.WriteEnum<TokenType, sbyte>(xe, nameof(type), type);
            xs.WriteEnum<Operator, sbyte>(xe, nameof(@operator), @operator);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCompiledExpressionSetToken);
        }

        public bool Equals(hkbCompiledExpressionSetToken? other)
        {
            return other is not null &&
                   data.Equals(other.data) &&
                   type.Equals(other.type) &&
                   @operator.Equals(other.@operator) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(data);
            hashcode.Add(type);
            hashcode.Add(@operator);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

