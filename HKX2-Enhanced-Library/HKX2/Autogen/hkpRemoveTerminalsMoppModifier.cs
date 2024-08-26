using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpRemoveTerminalsMoppModifier Signatire: 0x91367f03 size: 48 flags: FLAGS_NONE

    // removeInfo class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // tempShapesToRemove class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 40 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpRemoveTerminalsMoppModifier : hkReferencedObject, IEquatable<hkpRemoveTerminalsMoppModifier?>
    {
        public IList<uint> removeInfo { set; get; } = Array.Empty<uint>();
        private object? tempShapesToRemove { set; get; }

        public override uint Signature { set; get; } = 0x91367f03;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            removeInfo = des.ReadUInt32Array(br);
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            s.WriteUInt32Array(bw, removeInfo);
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            removeInfo = xd.ReadUInt32Array(xe, nameof(removeInfo));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(removeInfo), removeInfo);
            xs.WriteSerializeIgnored(xe, nameof(tempShapesToRemove));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpRemoveTerminalsMoppModifier);
        }

        public bool Equals(hkpRemoveTerminalsMoppModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   removeInfo.SequenceEqual(other.removeInfo) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(removeInfo.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

