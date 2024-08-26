using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBlendingTransitionEffectInternalState Signatire: 0xb18c70c2 size: 48 flags: FLAGS_NONE

    // characterPoseAtBeginningOfTransition class:  Type.TYPE_ARRAY Type.TYPE_QSTRANSFORM arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // timeRemaining class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // timeInTransition class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // applySelfTransition class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // initializeCharacterPose class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 41 flags: FLAGS_NONE enum: 
    public partial class hkbBlendingTransitionEffectInternalState : hkReferencedObject, IEquatable<hkbBlendingTransitionEffectInternalState?>
    {
        public IList<Matrix4x4> characterPoseAtBeginningOfTransition { set; get; } = Array.Empty<Matrix4x4>();
        public float timeRemaining { set; get; }
        public float timeInTransition { set; get; }
        public bool applySelfTransition { set; get; }
        public bool initializeCharacterPose { set; get; }

        public override uint Signature { set; get; } = 0xb18c70c2;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterPoseAtBeginningOfTransition = des.ReadQSTransformArray(br);
            timeRemaining = br.ReadSingle();
            timeInTransition = br.ReadSingle();
            applySelfTransition = br.ReadBoolean();
            initializeCharacterPose = br.ReadBoolean();
            br.Position += 6;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteQSTransformArray(bw, characterPoseAtBeginningOfTransition);
            bw.WriteSingle(timeRemaining);
            bw.WriteSingle(timeInTransition);
            bw.WriteBoolean(applySelfTransition);
            bw.WriteBoolean(initializeCharacterPose);
            bw.Position += 6;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterPoseAtBeginningOfTransition = xd.ReadQSTransformArray(xe, nameof(characterPoseAtBeginningOfTransition));
            timeRemaining = xd.ReadSingle(xe, nameof(timeRemaining));
            timeInTransition = xd.ReadSingle(xe, nameof(timeInTransition));
            applySelfTransition = xd.ReadBoolean(xe, nameof(applySelfTransition));
            initializeCharacterPose = xd.ReadBoolean(xe, nameof(initializeCharacterPose));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteQSTransformArray(xe, nameof(characterPoseAtBeginningOfTransition), characterPoseAtBeginningOfTransition);
            xs.WriteFloat(xe, nameof(timeRemaining), timeRemaining);
            xs.WriteFloat(xe, nameof(timeInTransition), timeInTransition);
            xs.WriteBoolean(xe, nameof(applySelfTransition), applySelfTransition);
            xs.WriteBoolean(xe, nameof(initializeCharacterPose), initializeCharacterPose);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBlendingTransitionEffectInternalState);
        }

        public bool Equals(hkbBlendingTransitionEffectInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterPoseAtBeginningOfTransition.SequenceEqual(other.characterPoseAtBeginningOfTransition) &&
                   timeRemaining.Equals(other.timeRemaining) &&
                   timeInTransition.Equals(other.timeInTransition) &&
                   applySelfTransition.Equals(other.applySelfTransition) &&
                   initializeCharacterPose.Equals(other.initializeCharacterPose) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterPoseAtBeginningOfTransition.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(timeRemaining);
            hashcode.Add(timeInTransition);
            hashcode.Add(applySelfTransition);
            hashcode.Add(initializeCharacterPose);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

