using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbGeneratorTransitionEffectInternalState Signatire: 0xd6692b5d size: 40 flags: FLAGS_NONE

    // timeInTransition class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // duration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // effectiveBlendInDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // effectiveBlendOutDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // toGeneratorState class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: ToGeneratorState
    // echoTransitionGenerator class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 33 flags: FLAGS_NONE enum: 
    // echoToGenerator class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 34 flags: FLAGS_NONE enum: 
    // justActivated class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 35 flags: FLAGS_NONE enum: 
    // updateActiveNodes class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // stage class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 37 flags: FLAGS_NONE enum: Stage
    public partial class hkbGeneratorTransitionEffectInternalState : hkReferencedObject, IEquatable<hkbGeneratorTransitionEffectInternalState?>
    {
        public float timeInTransition { set; get; }
        public float duration { set; get; }
        public float effectiveBlendInDuration { set; get; }
        public float effectiveBlendOutDuration { set; get; }
        public sbyte toGeneratorState { set; get; }
        public bool echoTransitionGenerator { set; get; }
        public bool echoToGenerator { set; get; }
        public bool justActivated { set; get; }
        public bool updateActiveNodes { set; get; }
        public sbyte stage { set; get; }

        public override uint Signature { set; get; } = 0xd6692b5d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
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
            timeInTransition = xd.ReadSingle(xe, nameof(timeInTransition));
            duration = xd.ReadSingle(xe, nameof(duration));
            effectiveBlendInDuration = xd.ReadSingle(xe, nameof(effectiveBlendInDuration));
            effectiveBlendOutDuration = xd.ReadSingle(xe, nameof(effectiveBlendOutDuration));
            toGeneratorState = xd.ReadFlag<ToGeneratorState, sbyte>(xe, nameof(toGeneratorState));
            echoTransitionGenerator = xd.ReadBoolean(xe, nameof(echoTransitionGenerator));
            echoToGenerator = xd.ReadBoolean(xe, nameof(echoToGenerator));
            justActivated = xd.ReadBoolean(xe, nameof(justActivated));
            updateActiveNodes = xd.ReadBoolean(xe, nameof(updateActiveNodes));
            stage = xd.ReadFlag<Stage, sbyte>(xe, nameof(stage));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(timeInTransition), timeInTransition);
            xs.WriteFloat(xe, nameof(duration), duration);
            xs.WriteFloat(xe, nameof(effectiveBlendInDuration), effectiveBlendInDuration);
            xs.WriteFloat(xe, nameof(effectiveBlendOutDuration), effectiveBlendOutDuration);
            xs.WriteEnum<ToGeneratorState, sbyte>(xe, nameof(toGeneratorState), toGeneratorState);
            xs.WriteBoolean(xe, nameof(echoTransitionGenerator), echoTransitionGenerator);
            xs.WriteBoolean(xe, nameof(echoToGenerator), echoToGenerator);
            xs.WriteBoolean(xe, nameof(justActivated), justActivated);
            xs.WriteBoolean(xe, nameof(updateActiveNodes), updateActiveNodes);
            xs.WriteEnum<Stage, sbyte>(xe, nameof(stage), stage);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbGeneratorTransitionEffectInternalState);
        }

        public bool Equals(hkbGeneratorTransitionEffectInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   timeInTransition.Equals(other.timeInTransition) &&
                   duration.Equals(other.duration) &&
                   effectiveBlendInDuration.Equals(other.effectiveBlendInDuration) &&
                   effectiveBlendOutDuration.Equals(other.effectiveBlendOutDuration) &&
                   toGeneratorState.Equals(other.toGeneratorState) &&
                   echoTransitionGenerator.Equals(other.echoTransitionGenerator) &&
                   echoToGenerator.Equals(other.echoToGenerator) &&
                   justActivated.Equals(other.justActivated) &&
                   updateActiveNodes.Equals(other.updateActiveNodes) &&
                   stage.Equals(other.stage) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(timeInTransition);
            hashcode.Add(duration);
            hashcode.Add(effectiveBlendInDuration);
            hashcode.Add(effectiveBlendOutDuration);
            hashcode.Add(toGeneratorState);
            hashcode.Add(echoTransitionGenerator);
            hashcode.Add(echoToGenerator);
            hashcode.Add(justActivated);
            hashcode.Add(updateActiveNodes);
            hashcode.Add(stage);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

