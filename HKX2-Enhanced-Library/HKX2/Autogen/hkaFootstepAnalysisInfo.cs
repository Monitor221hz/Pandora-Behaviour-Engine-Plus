using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaFootstepAnalysisInfo Signatire: 0x824faf75 size: 208 flags: FLAGS_NONE

    // name class:  Type.TYPE_ARRAY Type.TYPE_CHAR arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // nameStrike class:  Type.TYPE_ARRAY Type.TYPE_CHAR arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // nameLift class:  Type.TYPE_ARRAY Type.TYPE_CHAR arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // nameLock class:  Type.TYPE_ARRAY Type.TYPE_CHAR arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // nameUnlock class:  Type.TYPE_ARRAY Type.TYPE_CHAR arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // minPos class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // maxPos class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // minVel class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // maxVel class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // allBonesDown class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // anyBonesDown class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // posTol class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 192 flags: FLAGS_NONE enum: 
    // velTol class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 196 flags: FLAGS_NONE enum: 
    // duration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 200 flags: FLAGS_NONE enum: 
    public partial class hkaFootstepAnalysisInfo : hkReferencedObject, IEquatable<hkaFootstepAnalysisInfo?>
    {
        public string name { set; get; } = "";
        public string nameStrike { set; get; } = "";
        public string nameLift { set; get; } = "";
        public string nameLock { set; get; } = "";
        public string nameUnlock { set; get; } = "";
        public IList<float> minPos { set; get; } = Array.Empty<float>();
        public IList<float> maxPos { set; get; } = Array.Empty<float>();
        public IList<float> minVel { set; get; } = Array.Empty<float>();
        public IList<float> maxVel { set; get; } = Array.Empty<float>();
        public IList<float> allBonesDown { set; get; } = Array.Empty<float>();
        public IList<float> anyBonesDown { set; get; } = Array.Empty<float>();
        public float posTol { set; get; }
        public float velTol { set; get; }
        public float duration { set; get; }

        public override uint Signature { set; get; } = 0x824faf75;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            name = br.ReadASCII();
            nameStrike = br.ReadASCII();
            nameLift = br.ReadASCII();
            nameLock = br.ReadASCII();
            nameUnlock = br.ReadASCII();
            minPos = des.ReadSingleArray(br);
            maxPos = des.ReadSingleArray(br);
            minVel = des.ReadSingleArray(br);
            maxVel = des.ReadSingleArray(br);
            allBonesDown = des.ReadSingleArray(br);
            anyBonesDown = des.ReadSingleArray(br);
            posTol = br.ReadSingle();
            velTol = br.ReadSingle();
            duration = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteASCII(name, true);
            bw.WriteASCII(nameStrike, true);
            bw.WriteASCII(nameLift, true);
            bw.WriteASCII(nameLock, true);
            bw.WriteASCII(nameUnlock, true);
            s.WriteSingleArray(bw, minPos);
            s.WriteSingleArray(bw, maxPos);
            s.WriteSingleArray(bw, minVel);
            s.WriteSingleArray(bw, maxVel);
            s.WriteSingleArray(bw, allBonesDown);
            s.WriteSingleArray(bw, anyBonesDown);
            bw.WriteSingle(posTol);
            bw.WriteSingle(velTol);
            bw.WriteSingle(duration);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            name = xd.ReadString(xe, nameof(name));
            nameStrike = xd.ReadString(xe, nameof(nameStrike));
            nameLift = xd.ReadString(xe, nameof(nameLift));
            nameLock = xd.ReadString(xe, nameof(nameLock));
            nameUnlock = xd.ReadString(xe, nameof(nameUnlock));
            minPos = xd.ReadSingleArray(xe, nameof(minPos));
            maxPos = xd.ReadSingleArray(xe, nameof(maxPos));
            minVel = xd.ReadSingleArray(xe, nameof(minVel));
            maxVel = xd.ReadSingleArray(xe, nameof(maxVel));
            allBonesDown = xd.ReadSingleArray(xe, nameof(allBonesDown));
            anyBonesDown = xd.ReadSingleArray(xe, nameof(anyBonesDown));
            posTol = xd.ReadSingle(xe, nameof(posTol));
            velTol = xd.ReadSingle(xe, nameof(velTol));
            duration = xd.ReadSingle(xe, nameof(duration));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteString(xe, nameof(nameStrike), nameStrike);
            xs.WriteString(xe, nameof(nameLift), nameLift);
            xs.WriteString(xe, nameof(nameLock), nameLock);
            xs.WriteString(xe, nameof(nameUnlock), nameUnlock);
            xs.WriteFloatArray(xe, nameof(minPos), minPos);
            xs.WriteFloatArray(xe, nameof(maxPos), maxPos);
            xs.WriteFloatArray(xe, nameof(minVel), minVel);
            xs.WriteFloatArray(xe, nameof(maxVel), maxVel);
            xs.WriteFloatArray(xe, nameof(allBonesDown), allBonesDown);
            xs.WriteFloatArray(xe, nameof(anyBonesDown), anyBonesDown);
            xs.WriteFloat(xe, nameof(posTol), posTol);
            xs.WriteFloat(xe, nameof(velTol), velTol);
            xs.WriteFloat(xe, nameof(duration), duration);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaFootstepAnalysisInfo);
        }

        public bool Equals(hkaFootstepAnalysisInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   name.SequenceEqual(other.name) &&
                   nameStrike.SequenceEqual(other.nameStrike) &&
                   nameLift.SequenceEqual(other.nameLift) &&
                   nameLock.SequenceEqual(other.nameLock) &&
                   nameUnlock.SequenceEqual(other.nameUnlock) &&
                   minPos.SequenceEqual(other.minPos) &&
                   maxPos.SequenceEqual(other.maxPos) &&
                   minVel.SequenceEqual(other.minVel) &&
                   maxVel.SequenceEqual(other.maxVel) &&
                   allBonesDown.SequenceEqual(other.allBonesDown) &&
                   anyBonesDown.SequenceEqual(other.anyBonesDown) &&
                   posTol.Equals(other.posTol) &&
                   velTol.Equals(other.velTol) &&
                   duration.Equals(other.duration) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(name.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(nameStrike.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(nameLift.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(nameLock.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(nameUnlock.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(minPos.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(maxPos.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(minVel.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(maxVel.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(allBonesDown.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(anyBonesDown.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(posTol);
            hashcode.Add(velTol);
            hashcode.Add(duration);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

