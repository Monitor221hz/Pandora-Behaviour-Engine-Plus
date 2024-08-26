using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBlenderGeneratorChildInternalState Signatire: 0xff7327c0 size: 2 flags: FLAGS_NONE

    // isActive class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // syncNextFrame class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 1 flags: FLAGS_NONE enum: 
    public partial class hkbBlenderGeneratorChildInternalState : IHavokObject, IEquatable<hkbBlenderGeneratorChildInternalState?>
    {
        public bool isActive { set; get; }
        public bool syncNextFrame { set; get; }

        public virtual uint Signature { set; get; } = 0xff7327c0;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            isActive = br.ReadBoolean();
            syncNextFrame = br.ReadBoolean();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteBoolean(isActive);
            bw.WriteBoolean(syncNextFrame);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            isActive = xd.ReadBoolean(xe, nameof(isActive));
            syncNextFrame = xd.ReadBoolean(xe, nameof(syncNextFrame));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteBoolean(xe, nameof(isActive), isActive);
            xs.WriteBoolean(xe, nameof(syncNextFrame), syncNextFrame);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBlenderGeneratorChildInternalState);
        }

        public bool Equals(hkbBlenderGeneratorChildInternalState? other)
        {
            return other is not null &&
                   isActive.Equals(other.isActive) &&
                   syncNextFrame.Equals(other.syncNextFrame) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(isActive);
            hashcode.Add(syncNextFrame);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

