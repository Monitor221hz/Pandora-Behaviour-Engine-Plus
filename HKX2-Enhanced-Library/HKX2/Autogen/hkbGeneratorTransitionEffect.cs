using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbGeneratorTransitionEffect Signatire: 0x5f771b12 size: 144 flags: FLAGS_NONE

    // transitionGenerator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // blendInDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // blendOutDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // syncToGeneratorStartTime class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // fromGenerator class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 104 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // toGenerator class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // timeInTransition class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 120 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // duration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 124 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // effectiveBlendInDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 128 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // effectiveBlendOutDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 132 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // toGeneratorState class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 136 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // echoTransitionGenerator class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 137 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // echoToGenerator class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 138 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // justActivated class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 139 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // updateActiveNodes class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 140 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // stage class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 141 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbGeneratorTransitionEffect : hkbTransitionEffect, IEquatable<hkbGeneratorTransitionEffect?>
    {
        public hkbGenerator? transitionGenerator { set; get; }
        public float blendInDuration { set; get; }
        public float blendOutDuration { set; get; }
        public bool syncToGeneratorStartTime { set; get; }
        private object? fromGenerator { set; get; }
        private object? toGenerator { set; get; }
        private float timeInTransition { set; get; }
        private float duration { set; get; }
        private float effectiveBlendInDuration { set; get; }
        private float effectiveBlendOutDuration { set; get; }
        private sbyte toGeneratorState { set; get; }
        private bool echoTransitionGenerator { set; get; }
        private bool echoToGenerator { set; get; }
        private bool justActivated { set; get; }
        private bool updateActiveNodes { set; get; }
        private sbyte stage { set; get; }

        public override uint Signature { set; get; } = 0x5f771b12;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            transitionGenerator = des.ReadClassPointer<hkbGenerator>(br);
            blendInDuration = br.ReadSingle();
            blendOutDuration = br.ReadSingle();
            syncToGeneratorStartTime = br.ReadBoolean();
            br.Position += 7;
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            timeInTransition = br.ReadSingle();
            duration = br.ReadSingle();
            effectiveBlendInDuration = br.ReadSingle();
            effectiveBlendOutDuration = br.ReadSingle();
            toGeneratorState = br.ReadSByte();
            echoTransitionGenerator = br.ReadBoolean();
            echoToGenerator = br.ReadBoolean();
            justActivated = br.ReadBoolean();
            updateActiveNodes = br.ReadBoolean();
            stage = br.ReadSByte();
            br.Position += 2;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, transitionGenerator);
            bw.WriteSingle(blendInDuration);
            bw.WriteSingle(blendOutDuration);
            bw.WriteBoolean(syncToGeneratorStartTime);
            bw.Position += 7;
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            bw.WriteSingle(timeInTransition);
            bw.WriteSingle(duration);
            bw.WriteSingle(effectiveBlendInDuration);
            bw.WriteSingle(effectiveBlendOutDuration);
            bw.WriteSByte(toGeneratorState);
            bw.WriteBoolean(echoTransitionGenerator);
            bw.WriteBoolean(echoToGenerator);
            bw.WriteBoolean(justActivated);
            bw.WriteBoolean(updateActiveNodes);
            bw.WriteSByte(stage);
            bw.Position += 2;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            transitionGenerator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(transitionGenerator));
            blendInDuration = xd.ReadSingle(xe, nameof(blendInDuration));
            blendOutDuration = xd.ReadSingle(xe, nameof(blendOutDuration));
            syncToGeneratorStartTime = xd.ReadBoolean(xe, nameof(syncToGeneratorStartTime));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(transitionGenerator), transitionGenerator);
            xs.WriteFloat(xe, nameof(blendInDuration), blendInDuration);
            xs.WriteFloat(xe, nameof(blendOutDuration), blendOutDuration);
            xs.WriteBoolean(xe, nameof(syncToGeneratorStartTime), syncToGeneratorStartTime);
            xs.WriteSerializeIgnored(xe, nameof(fromGenerator));
            xs.WriteSerializeIgnored(xe, nameof(toGenerator));
            xs.WriteSerializeIgnored(xe, nameof(timeInTransition));
            xs.WriteSerializeIgnored(xe, nameof(duration));
            xs.WriteSerializeIgnored(xe, nameof(effectiveBlendInDuration));
            xs.WriteSerializeIgnored(xe, nameof(effectiveBlendOutDuration));
            xs.WriteSerializeIgnored(xe, nameof(toGeneratorState));
            xs.WriteSerializeIgnored(xe, nameof(echoTransitionGenerator));
            xs.WriteSerializeIgnored(xe, nameof(echoToGenerator));
            xs.WriteSerializeIgnored(xe, nameof(justActivated));
            xs.WriteSerializeIgnored(xe, nameof(updateActiveNodes));
            xs.WriteSerializeIgnored(xe, nameof(stage));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbGeneratorTransitionEffect);
        }

        public bool Equals(hkbGeneratorTransitionEffect? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((transitionGenerator is null && other.transitionGenerator is null) || (transitionGenerator is not null && other.transitionGenerator is not null && transitionGenerator.Equals((IHavokObject)other.transitionGenerator))) &&
                   blendInDuration.Equals(other.blendInDuration) &&
                   blendOutDuration.Equals(other.blendOutDuration) &&
                   syncToGeneratorStartTime.Equals(other.syncToGeneratorStartTime) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(transitionGenerator);
            hashcode.Add(blendInDuration);
            hashcode.Add(blendOutDuration);
            hashcode.Add(syncToGeneratorStartTime);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

