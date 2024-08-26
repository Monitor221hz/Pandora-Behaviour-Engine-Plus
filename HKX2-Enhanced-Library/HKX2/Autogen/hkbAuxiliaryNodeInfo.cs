using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbAuxiliaryNodeInfo Signatire: 0xca0888ca size: 48 flags: FLAGS_NONE

    // type class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: NodeType
    // depth class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 17 flags: FLAGS_NONE enum: 
    // referenceBehaviorName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // selfTransitionNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkbAuxiliaryNodeInfo : hkReferencedObject, IEquatable<hkbAuxiliaryNodeInfo?>
    {
        public byte type { set; get; }
        public byte depth { set; get; }
        public string referenceBehaviorName { set; get; } = "";
        public IList<string> selfTransitionNames { set; get; } = Array.Empty<string>();

        public override uint Signature { set; get; } = 0xca0888ca;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            type = br.ReadByte();
            depth = br.ReadByte();
            br.Position += 6;
            referenceBehaviorName = des.ReadStringPointer(br);
            selfTransitionNames = des.ReadStringPointerArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(type);
            bw.WriteByte(depth);
            bw.Position += 6;
            s.WriteStringPointer(bw, referenceBehaviorName);
            s.WriteStringPointerArray(bw, selfTransitionNames);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            type = xd.ReadFlag<NodeType, byte>(xe, nameof(type));
            depth = xd.ReadByte(xe, nameof(depth));
            referenceBehaviorName = xd.ReadString(xe, nameof(referenceBehaviorName));
            selfTransitionNames = xd.ReadStringArray(xe, nameof(selfTransitionNames));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<NodeType, byte>(xe, nameof(type), type);
            xs.WriteNumber(xe, nameof(depth), depth);
            xs.WriteString(xe, nameof(referenceBehaviorName), referenceBehaviorName);
            xs.WriteStringArray(xe, nameof(selfTransitionNames), selfTransitionNames);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbAuxiliaryNodeInfo);
        }

        public bool Equals(hkbAuxiliaryNodeInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   type.Equals(other.type) &&
                   depth.Equals(other.depth) &&
                   (referenceBehaviorName is null && other.referenceBehaviorName is null || referenceBehaviorName == other.referenceBehaviorName || referenceBehaviorName is null && other.referenceBehaviorName == "" || referenceBehaviorName == "" && other.referenceBehaviorName is null) &&
                   selfTransitionNames.SequenceEqual(other.selfTransitionNames) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(type);
            hashcode.Add(depth);
            hashcode.Add(referenceBehaviorName);
            hashcode.Add(selfTransitionNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

