using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpDefaultWorldMemoryWatchDog Signatire: 0x77d6b19f size: 24 flags: FLAGS_NONE

    // freeHeapMemoryRequested class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpDefaultWorldMemoryWatchDog : hkWorldMemoryAvailableWatchDog, IEquatable<hkpDefaultWorldMemoryWatchDog?>
    {
        public int freeHeapMemoryRequested { set; get; }

        public override uint Signature { set; get; } = 0x77d6b19f;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            freeHeapMemoryRequested = br.ReadInt32();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(freeHeapMemoryRequested);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            freeHeapMemoryRequested = xd.ReadInt32(xe, nameof(freeHeapMemoryRequested));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(freeHeapMemoryRequested), freeHeapMemoryRequested);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpDefaultWorldMemoryWatchDog);
        }

        public bool Equals(hkpDefaultWorldMemoryWatchDog? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   freeHeapMemoryRequested.Equals(other.freeHeapMemoryRequested) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(freeHeapMemoryRequested);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

