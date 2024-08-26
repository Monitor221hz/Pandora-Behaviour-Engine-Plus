using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSiStateTaggingGenerator Signatire: 0xf0826fc1 size: 96 flags: FLAGS_NONE

    // pDefaultGenerator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: ALIGN_16|FLAGS_NONE enum: 
    // iStateToSetAs class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // iPriority class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    public partial class BSiStateTaggingGenerator : hkbGenerator, IEquatable<BSiStateTaggingGenerator?>
    {
        public hkbGenerator? pDefaultGenerator { set; get; }
        public int iStateToSetAs { set; get; }
        public int iPriority { set; get; }

        public override uint Signature { set; get; } = 0xf0826fc1;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            pDefaultGenerator = des.ReadClassPointer<hkbGenerator>(br);
            iStateToSetAs = br.ReadInt32();
            iPriority = br.ReadInt32();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            s.WriteClassPointer(bw, pDefaultGenerator);
            bw.WriteInt32(iStateToSetAs);
            bw.WriteInt32(iPriority);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pDefaultGenerator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(pDefaultGenerator));
            iStateToSetAs = xd.ReadInt32(xe, nameof(iStateToSetAs));
            iPriority = xd.ReadInt32(xe, nameof(iPriority));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(pDefaultGenerator), pDefaultGenerator);
            xs.WriteNumber(xe, nameof(iStateToSetAs), iStateToSetAs);
            xs.WriteNumber(xe, nameof(iPriority), iPriority);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSiStateTaggingGenerator);
        }

        public bool Equals(BSiStateTaggingGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((pDefaultGenerator is null && other.pDefaultGenerator is null) || (pDefaultGenerator is not null && other.pDefaultGenerator is not null && pDefaultGenerator.Equals((IHavokObject)other.pDefaultGenerator))) &&
                   iStateToSetAs.Equals(other.iStateToSetAs) &&
                   iPriority.Equals(other.iPriority) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pDefaultGenerator);
            hashcode.Add(iStateToSetAs);
            hashcode.Add(iPriority);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

