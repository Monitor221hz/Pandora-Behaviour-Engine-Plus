using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbGeneratorSyncInfo Signatire: 0xa3c341f8 size: 80 flags: FLAGS_NONE

    // syncPoints class: hkbGeneratorSyncInfoSyncPoint Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 8 offset: 0 flags: FLAGS_NONE enum: 
    // baseFrequency class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // localTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // playbackSpeed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // numSyncPoints class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    // isCyclic class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 77 flags: FLAGS_NONE enum: 
    // isMirrored class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 78 flags: FLAGS_NONE enum: 
    // isAdditive class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 79 flags: FLAGS_NONE enum: 
    public partial class hkbGeneratorSyncInfo : IHavokObject, IEquatable<hkbGeneratorSyncInfo?>
    {
        public hkbGeneratorSyncInfoSyncPoint[] syncPoints = new hkbGeneratorSyncInfoSyncPoint[8];
        public float baseFrequency { set; get; }
        public float localTime { set; get; }
        public float playbackSpeed { set; get; }
        public sbyte numSyncPoints { set; get; }
        public bool isCyclic { set; get; }
        public bool isMirrored { set; get; }
        public bool isAdditive { set; get; }

        public virtual uint Signature { set; get; } = 0xa3c341f8;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            syncPoints = des.ReadStructCStyleArray<hkbGeneratorSyncInfoSyncPoint>(br, 8);
            br.Position += 8;
            baseFrequency = br.ReadSingle();
            localTime = br.ReadSingle();
            playbackSpeed = br.ReadSingle();
            numSyncPoints = br.ReadSByte();
            isCyclic = br.ReadBoolean();
            isMirrored = br.ReadBoolean();
            isAdditive = br.ReadBoolean();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteStructCStyleArray(bw, syncPoints);
            bw.Position += 8;
            bw.WriteSingle(baseFrequency);
            bw.WriteSingle(localTime);
            bw.WriteSingle(playbackSpeed);
            bw.WriteSByte(numSyncPoints);
            bw.WriteBoolean(isCyclic);
            bw.WriteBoolean(isMirrored);
            bw.WriteBoolean(isAdditive);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            syncPoints = xd.ReadClassCStyleArray<hkbGeneratorSyncInfoSyncPoint>(xe, nameof(syncPoints), 8);
            baseFrequency = xd.ReadSingle(xe, nameof(baseFrequency));
            localTime = xd.ReadSingle(xe, nameof(localTime));
            playbackSpeed = xd.ReadSingle(xe, nameof(playbackSpeed));
            numSyncPoints = xd.ReadSByte(xe, nameof(numSyncPoints));
            isCyclic = xd.ReadBoolean(xe, nameof(isCyclic));
            isMirrored = xd.ReadBoolean(xe, nameof(isMirrored));
            isAdditive = xd.ReadBoolean(xe, nameof(isAdditive));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassArray<hkbGeneratorSyncInfoSyncPoint>(xe, nameof(syncPoints), syncPoints);
            xs.WriteFloat(xe, nameof(baseFrequency), baseFrequency);
            xs.WriteFloat(xe, nameof(localTime), localTime);
            xs.WriteFloat(xe, nameof(playbackSpeed), playbackSpeed);
            xs.WriteNumber(xe, nameof(numSyncPoints), numSyncPoints);
            xs.WriteBoolean(xe, nameof(isCyclic), isCyclic);
            xs.WriteBoolean(xe, nameof(isMirrored), isMirrored);
            xs.WriteBoolean(xe, nameof(isAdditive), isAdditive);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbGeneratorSyncInfo);
        }

        public bool Equals(hkbGeneratorSyncInfo? other)
        {
            return other is not null &&
                   syncPoints.SequenceEqual(other.syncPoints) &&
                   baseFrequency.Equals(other.baseFrequency) &&
                   localTime.Equals(other.localTime) &&
                   playbackSpeed.Equals(other.playbackSpeed) &&
                   numSyncPoints.Equals(other.numSyncPoints) &&
                   isCyclic.Equals(other.isCyclic) &&
                   isMirrored.Equals(other.isMirrored) &&
                   isAdditive.Equals(other.isAdditive) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(syncPoints.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(baseFrequency);
            hashcode.Add(localTime);
            hashcode.Add(playbackSpeed);
            hashcode.Add(numSyncPoints);
            hashcode.Add(isCyclic);
            hashcode.Add(isMirrored);
            hashcode.Add(isAdditive);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

