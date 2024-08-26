using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSIStateManagerModifierBSIStateManagerStateListener Signatire: 0x99463586 size: 24 flags: FLAGS_NONE

    // pStateManager class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSIStateManagerModifierBSIStateManagerStateListener : hkbStateListener, IEquatable<BSIStateManagerModifierBSIStateManagerStateListener?>
    {
        private object? pStateManager { set; get; }

        public override uint Signature { set; get; } = 0x99463586;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(pStateManager));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSIStateManagerModifierBSIStateManagerStateListener);
        }

        public bool Equals(BSIStateManagerModifierBSIStateManagerStateListener? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

