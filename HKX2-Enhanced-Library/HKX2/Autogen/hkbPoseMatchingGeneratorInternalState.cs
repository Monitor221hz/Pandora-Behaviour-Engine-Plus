using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbPoseMatchingGeneratorInternalState Signatire: 0x552d9dd4 size: 40 flags: FLAGS_NONE

    // currentMatch class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // bestMatch class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // timeSinceBetterMatch class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // error class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // resetCurrentMatchLocalTime class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkbPoseMatchingGeneratorInternalState : hkReferencedObject, IEquatable<hkbPoseMatchingGeneratorInternalState?>
    {
        public int currentMatch { set; get; }
        public int bestMatch { set; get; }
        public float timeSinceBetterMatch { set; get; }
        public float error { set; get; }
        public bool resetCurrentMatchLocalTime { set; get; }

        public override uint Signature { set; get; } = 0x552d9dd4;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            currentMatch = br.ReadInt32();
            bestMatch = br.ReadInt32();
            timeSinceBetterMatch = br.ReadSingle();
            error = br.ReadSingle();
            resetCurrentMatchLocalTime = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(currentMatch);
            bw.WriteInt32(bestMatch);
            bw.WriteSingle(timeSinceBetterMatch);
            bw.WriteSingle(error);
            bw.WriteBoolean(resetCurrentMatchLocalTime);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            currentMatch = xd.ReadInt32(xe, nameof(currentMatch));
            bestMatch = xd.ReadInt32(xe, nameof(bestMatch));
            timeSinceBetterMatch = xd.ReadSingle(xe, nameof(timeSinceBetterMatch));
            error = xd.ReadSingle(xe, nameof(error));
            resetCurrentMatchLocalTime = xd.ReadBoolean(xe, nameof(resetCurrentMatchLocalTime));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(currentMatch), currentMatch);
            xs.WriteNumber(xe, nameof(bestMatch), bestMatch);
            xs.WriteFloat(xe, nameof(timeSinceBetterMatch), timeSinceBetterMatch);
            xs.WriteFloat(xe, nameof(error), error);
            xs.WriteBoolean(xe, nameof(resetCurrentMatchLocalTime), resetCurrentMatchLocalTime);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbPoseMatchingGeneratorInternalState);
        }

        public bool Equals(hkbPoseMatchingGeneratorInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   currentMatch.Equals(other.currentMatch) &&
                   bestMatch.Equals(other.bestMatch) &&
                   timeSinceBetterMatch.Equals(other.timeSinceBetterMatch) &&
                   error.Equals(other.error) &&
                   resetCurrentMatchLocalTime.Equals(other.resetCurrentMatchLocalTime) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(currentMatch);
            hashcode.Add(bestMatch);
            hashcode.Add(timeSinceBetterMatch);
            hashcode.Add(error);
            hashcode.Add(resetCurrentMatchLocalTime);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

