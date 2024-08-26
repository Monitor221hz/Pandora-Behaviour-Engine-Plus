using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpGenericConstraintDataScheme Signatire: 0x11fd6f6c size: 80 flags: FLAGS_NONE

    // info class: hkpGenericConstraintDataSchemeConstraintInfo Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // data class:  Type.TYPE_ARRAY Type.TYPE_VECTOR4 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // commands class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // modifiers class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 48 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // motors class: hkpConstraintMotor Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpGenericConstraintDataScheme : IHavokObject, IEquatable<hkpGenericConstraintDataScheme?>
    {
        public hkpGenericConstraintDataSchemeConstraintInfo info { set; get; } = new();
        public IList<Vector4> data { set; get; } = Array.Empty<Vector4>();
        public IList<int> commands { set; get; } = Array.Empty<int>();
        public IList<object> modifiers { set; get; } = Array.Empty<object>();
        public IList<hkpConstraintMotor> motors { set; get; } = Array.Empty<hkpConstraintMotor>();

        public virtual uint Signature { set; get; } = 0x11fd6f6c;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            info.Read(des, br);
            data = des.ReadVector4Array(br);
            commands = des.ReadInt32Array(br);
            des.ReadEmptyArray(br);
            motors = des.ReadClassPointerArray<hkpConstraintMotor>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            info.Write(s, bw);
            s.WriteVector4Array(bw, data);
            s.WriteInt32Array(bw, commands);
            s.WriteVoidArray(bw);
            s.WriteClassPointerArray(bw, motors);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            data = xd.ReadVector4Array(xe, nameof(data));
            commands = xd.ReadInt32Array(xe, nameof(commands));
            motors = xd.ReadClassPointerArray<hkpConstraintMotor>(this, xe, nameof(motors));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(info));
            xs.WriteVector4Array(xe, nameof(data), data);
            xs.WriteNumberArray(xe, nameof(commands), commands);
            xs.WriteSerializeIgnored(xe, nameof(modifiers));
            xs.WriteClassPointerArray(xe, nameof(motors), motors!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpGenericConstraintDataScheme);
        }

        public bool Equals(hkpGenericConstraintDataScheme? other)
        {
            return other is not null &&
                   data.SequenceEqual(other.data) &&
                   commands.SequenceEqual(other.commands) &&
                   motors.SequenceEqual(other.motors) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(commands.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(motors.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

