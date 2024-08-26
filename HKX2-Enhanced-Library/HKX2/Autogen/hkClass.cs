using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkClass Signatire: 0x75585ef6 size: 80 flags: FLAGS_NONE

    // name class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // parent class: hkClass Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // objectSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // numImplementedInterfaces class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // declaredEnums class: hkClassEnum Type.TYPE_SIMPLEARRAY Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // declaredMembers class: hkClassMember Type.TYPE_SIMPLEARRAY Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // defaults class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 56 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // attributes class: hkCustomAttributes Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 64 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // flags class:  Type.TYPE_FLAGS Type.TYPE_UINT32 arrSize: 0 offset: 72 flags: FLAGS_NONE enum: FlagValues
    // describedVersion class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    public partial class hkClass : IHavokObject, IEquatable<hkClass?>
    {
        public string name { set; get; } = "";
        public hkClass? parent { set; get; }
        public int objectSize { set; get; }
        public int numImplementedInterfaces { set; get; }
        public object? declaredEnums { set; get; }
        public object? declaredMembers { set; get; }
        private object? defaults { set; get; }
        private hkCustomAttributes? attributes { set; get; }
        public uint flags { set; get; }
        public int describedVersion { set; get; }

        public virtual uint Signature { set; get; } = 0x75585ef6;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            name = des.ReadCString(br);
            parent = des.ReadClassPointer<hkClass>(br);
            objectSize = br.ReadInt32();
            numImplementedInterfaces = br.ReadInt32();
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            //des.ReadEmptyPointer(br);
            //attributes = des.ReadClassPointer<hkCustomAttributes>(br);
            //flags = br.ReadUInt32();
            //describedVersion = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteCString(bw, name);
            s.WriteClassPointer(bw, parent);
            bw.WriteInt32(objectSize);
            bw.WriteInt32(numImplementedInterfaces);
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            //s.WriteVoidPointer(bw);
            //s.WriteClassPointer(bw, attributes);
            //bw.WriteUInt32(flags);
            //bw.WriteInt32(describedVersion);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            name = xd.ReadString(xe, nameof(name));
            parent = xd.ReadClassPointer<hkClass>(this, xe, nameof(parent));
            objectSize = xd.ReadInt32(xe, nameof(objectSize));
            numImplementedInterfaces = xd.ReadInt32(xe, nameof(numImplementedInterfaces));
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            //flags = xd.ReadFlag<FlagValues, uint>(xe, nameof(flags));
            //describedVersion = xd.ReadInt32(xe, nameof(describedVersion));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(name), name);
            xs.WriteClassPointer(xe, nameof(parent), parent);
            xs.WriteNumber(xe, nameof(objectSize), objectSize);
            xs.WriteNumber(xe, nameof(numImplementedInterfaces), numImplementedInterfaces);
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            //xs.WriteSerializeIgnored(xe, nameof(defaults));
            //xs.WriteSerializeIgnored(xe, nameof(attributes));
            //xs.WriteFlag<FlagValues, uint>(xe, nameof(flags), flags);
            //xs.WriteNumber(xe, nameof(describedVersion), describedVersion);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkClass);
        }

        public bool Equals(hkClass? other)
        {
            return other is not null &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   ((parent is null && other.parent is null) || (parent is not null && other.parent is not null && parent.Equals((IHavokObject)other.parent))) &&
                   objectSize.Equals(other.objectSize) &&
                   numImplementedInterfaces.Equals(other.numImplementedInterfaces) &&
                   declaredEnums!.Equals(other.declaredEnums!) &&
                   declaredMembers!.Equals(other.declaredMembers) &&
                   flags.Equals(other.flags) &&
                   describedVersion.Equals(other.describedVersion) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(name);
            hashcode.Add(parent);
            hashcode.Add(objectSize);
            hashcode.Add(numImplementedInterfaces);
            hashcode.Add(declaredEnums);
            hashcode.Add(declaredMembers);
            hashcode.Add(flags);
            hashcode.Add(describedVersion);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

