using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSetBehaviorCommand Signatire: 0xe18b74b9 size: 72 flags: FLAGS_NONE

    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // behavior class: hkbBehaviorGraph Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // rootGenerator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // referencedBehaviors class: hkbBehaviorGraph Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // startStateIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // randomizeSimulation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // padding class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkbSetBehaviorCommand : hkReferencedObject, IEquatable<hkbSetBehaviorCommand?>
    {
        public ulong characterId { set; get; }
        public hkbBehaviorGraph? behavior { set; get; }
        public hkbGenerator? rootGenerator { set; get; }
        public IList<hkbBehaviorGraph> referencedBehaviors { set; get; } = Array.Empty<hkbBehaviorGraph>();
        public int startStateIndex { set; get; }
        public bool randomizeSimulation { set; get; }
        public int padding { set; get; }

        public override uint Signature { set; get; } = 0xe18b74b9;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterId = br.ReadUInt64();
            behavior = des.ReadClassPointer<hkbBehaviorGraph>(br);
            rootGenerator = des.ReadClassPointer<hkbGenerator>(br);
            referencedBehaviors = des.ReadClassPointerArray<hkbBehaviorGraph>(br);
            startStateIndex = br.ReadInt32();
            randomizeSimulation = br.ReadBoolean();
            br.Position += 3;
            padding = br.ReadInt32();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(characterId);
            s.WriteClassPointer(bw, behavior);
            s.WriteClassPointer(bw, rootGenerator);
            s.WriteClassPointerArray(bw, referencedBehaviors);
            bw.WriteInt32(startStateIndex);
            bw.WriteBoolean(randomizeSimulation);
            bw.Position += 3;
            bw.WriteInt32(padding);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            behavior = xd.ReadClassPointer<hkbBehaviorGraph>(this, xe, nameof(behavior));
            rootGenerator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(rootGenerator));
            referencedBehaviors = xd.ReadClassPointerArray<hkbBehaviorGraph>(this, xe, nameof(referencedBehaviors));
            startStateIndex = xd.ReadInt32(xe, nameof(startStateIndex));
            randomizeSimulation = xd.ReadBoolean(xe, nameof(randomizeSimulation));
            padding = xd.ReadInt32(xe, nameof(padding));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteClassPointer(xe, nameof(behavior), behavior);
            xs.WriteClassPointer(xe, nameof(rootGenerator), rootGenerator);
            xs.WriteClassPointerArray(xe, nameof(referencedBehaviors), referencedBehaviors!);
            xs.WriteNumber(xe, nameof(startStateIndex), startStateIndex);
            xs.WriteBoolean(xe, nameof(randomizeSimulation), randomizeSimulation);
            xs.WriteNumber(xe, nameof(padding), padding);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSetBehaviorCommand);
        }

        public bool Equals(hkbSetBehaviorCommand? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterId.Equals(other.characterId) &&
                   ((behavior is null && other.behavior is null) || (behavior is not null && other.behavior is not null && behavior.Equals((IHavokObject)other.behavior))) &&
                   ((rootGenerator is null && other.rootGenerator is null) || (rootGenerator is not null && other.rootGenerator is not null && rootGenerator.Equals((IHavokObject)other.rootGenerator))) &&
                   referencedBehaviors.SequenceEqual(other.referencedBehaviors) &&
                   startStateIndex.Equals(other.startStateIndex) &&
                   randomizeSimulation.Equals(other.randomizeSimulation) &&
                   padding.Equals(other.padding) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterId);
            hashcode.Add(behavior);
            hashcode.Add(rootGenerator);
            hashcode.Add(referencedBehaviors.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(startStateIndex);
            hashcode.Add(randomizeSimulation);
            hashcode.Add(padding);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

