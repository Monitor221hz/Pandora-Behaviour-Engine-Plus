using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConstraintInstance Signatire: 0x34eba5f size: 112 flags: FLAGS_NONE

    // owner class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // data class: hkpConstraintData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // constraintModifiers class: hkpModifierConstraintAtom Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // entities class: hkpEntity Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 2 offset: 40 flags: FLAGS_NONE enum: 
    // priority class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 56 flags: FLAGS_NONE enum: ConstraintPriority
    // wantRuntime class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 57 flags: FLAGS_NONE enum: 
    // destructionRemapInfo class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 58 flags: FLAGS_NONE enum: OnDestructionRemapInfo
    // listeners class: hkpConstraintInstanceSmallArraySerializeOverrideType Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 64 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // userData class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // internal class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // uid class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 104 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpConstraintInstance : hkReferencedObject, IEquatable<hkpConstraintInstance?>
    {
        private object? owner { set; get; }
        public hkpConstraintData? data { set; get; }
        public hkpModifierConstraintAtom? constraintModifiers { set; get; }
        public hkpEntity?[] entities = new hkpEntity?[2];
        public byte priority { set; get; }
        public bool wantRuntime { set; get; }
        public byte destructionRemapInfo { set; get; }
        public hkpConstraintInstanceSmallArraySerializeOverrideType listeners { set; get; } = new();
        public string name { set; get; } = "";
        public ulong userData { set; get; }
        private object? @internal { set; get; }
        private uint uid { set; get; }

        public override uint Signature { set; get; } = 0x34eba5f;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            des.ReadEmptyPointer(br);
            data = des.ReadClassPointer<hkpConstraintData>(br);
            constraintModifiers = des.ReadClassPointer<hkpModifierConstraintAtom>(br);
            entities = des.ReadClassPointerCStyleArray<hkpEntity>(br, 2);
            priority = br.ReadByte();
            wantRuntime = br.ReadBoolean();
            destructionRemapInfo = br.ReadByte();
            br.Position += 5;
            listeners.Read(des, br);
            name = des.ReadStringPointer(br);
            userData = br.ReadUInt64();
            des.ReadEmptyPointer(br);
            uid = br.ReadUInt32();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVoidPointer(bw);
            s.WriteClassPointer(bw, data);
            s.WriteClassPointer(bw, constraintModifiers);
            s.WriteClassPointerCStyleArray(bw, entities);
            bw.WriteByte(priority);
            bw.WriteBoolean(wantRuntime);
            bw.WriteByte(destructionRemapInfo);
            bw.Position += 5;
            listeners.Write(s, bw);
            s.WriteStringPointer(bw, name);
            bw.WriteUInt64(userData);
            s.WriteVoidPointer(bw);
            bw.WriteUInt32(uid);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            data = xd.ReadClassPointer<hkpConstraintData>(this, xe, nameof(data));
            constraintModifiers = xd.ReadClassPointer<hkpModifierConstraintAtom>(this, xe, nameof(constraintModifiers));
            entities = xd.ReadClassPointerCStyleArray<hkpEntity>(this, xe, nameof(entities), 2);
            priority = xd.ReadFlag<ConstraintPriority, byte>(xe, nameof(priority));
            wantRuntime = xd.ReadBoolean(xe, nameof(wantRuntime));
            destructionRemapInfo = xd.ReadFlag<OnDestructionRemapInfo, byte>(xe, nameof(destructionRemapInfo));
            name = xd.ReadString(xe, nameof(name));
            userData = xd.ReadUInt64(xe, nameof(userData));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(owner));
            xs.WriteClassPointer(xe, nameof(data), data);
            xs.WriteClassPointer(xe, nameof(constraintModifiers), constraintModifiers);
            xs.WriteClassPointerArray(xe, nameof(entities), entities);
            xs.WriteEnum<ConstraintPriority, byte>(xe, nameof(priority), priority);
            xs.WriteBoolean(xe, nameof(wantRuntime), wantRuntime);
            xs.WriteEnum<OnDestructionRemapInfo, byte>(xe, nameof(destructionRemapInfo), destructionRemapInfo);
            xs.WriteSerializeIgnored(xe, nameof(listeners));
            xs.WriteString(xe, nameof(name), name);
            xs.WriteNumber(xe, nameof(userData), userData);
            xs.WriteSerializeIgnored(xe, nameof(@internal));
            xs.WriteSerializeIgnored(xe, nameof(uid));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConstraintInstance);
        }

        public bool Equals(hkpConstraintInstance? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((data is null && other.data is null) || (data is not null && other.data is not null && data.Equals((IHavokObject)other.data))) &&
                   ((constraintModifiers is null && other.constraintModifiers is null) || (constraintModifiers is not null && other.constraintModifiers is not null && constraintModifiers.Equals((IHavokObject)other.constraintModifiers))) &&
                   entities.SequenceEqual(other.entities) &&
                   priority.Equals(other.priority) &&
                   wantRuntime.Equals(other.wantRuntime) &&
                   destructionRemapInfo.Equals(other.destructionRemapInfo) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   userData.Equals(other.userData) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(data);
            hashcode.Add(constraintModifiers);
            hashcode.Add(entities.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(priority);
            hashcode.Add(wantRuntime);
            hashcode.Add(destructionRemapInfo);
            hashcode.Add(name);
            hashcode.Add(userData);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

