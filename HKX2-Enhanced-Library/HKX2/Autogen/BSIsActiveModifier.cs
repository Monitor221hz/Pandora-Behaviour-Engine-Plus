using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSIsActiveModifier Signatire: 0xb0fde45a size: 96 flags: FLAGS_NONE

    // bIsActive0 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // bInvertActive0 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 81 flags: FLAGS_NONE enum: 
    // bIsActive1 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 82 flags: FLAGS_NONE enum: 
    // bInvertActive1 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 83 flags: FLAGS_NONE enum: 
    // bIsActive2 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // bInvertActive2 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 85 flags: FLAGS_NONE enum: 
    // bIsActive3 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 86 flags: FLAGS_NONE enum: 
    // bInvertActive3 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 87 flags: FLAGS_NONE enum: 
    // bIsActive4 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // bInvertActive4 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 89 flags: FLAGS_NONE enum: 
    public partial class BSIsActiveModifier : hkbModifier, IEquatable<BSIsActiveModifier?>
    {
        public bool bIsActive0 { set; get; }
        public bool bInvertActive0 { set; get; }
        public bool bIsActive1 { set; get; }
        public bool bInvertActive1 { set; get; }
        public bool bIsActive2 { set; get; }
        public bool bInvertActive2 { set; get; }
        public bool bIsActive3 { set; get; }
        public bool bInvertActive3 { set; get; }
        public bool bIsActive4 { set; get; }
        public bool bInvertActive4 { set; get; }

        public override uint Signature { set; get; } = 0xb0fde45a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            bIsActive0 = br.ReadBoolean();
            bInvertActive0 = br.ReadBoolean();
            bIsActive1 = br.ReadBoolean();
            bInvertActive1 = br.ReadBoolean();
            bIsActive2 = br.ReadBoolean();
            bInvertActive2 = br.ReadBoolean();
            bIsActive3 = br.ReadBoolean();
            bInvertActive3 = br.ReadBoolean();
            bIsActive4 = br.ReadBoolean();
            bInvertActive4 = br.ReadBoolean();
            br.Position += 6;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(bIsActive0);
            bw.WriteBoolean(bInvertActive0);
            bw.WriteBoolean(bIsActive1);
            bw.WriteBoolean(bInvertActive1);
            bw.WriteBoolean(bIsActive2);
            bw.WriteBoolean(bInvertActive2);
            bw.WriteBoolean(bIsActive3);
            bw.WriteBoolean(bInvertActive3);
            bw.WriteBoolean(bIsActive4);
            bw.WriteBoolean(bInvertActive4);
            bw.Position += 6;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            bIsActive0 = xd.ReadBoolean(xe, nameof(bIsActive0));
            bInvertActive0 = xd.ReadBoolean(xe, nameof(bInvertActive0));
            bIsActive1 = xd.ReadBoolean(xe, nameof(bIsActive1));
            bInvertActive1 = xd.ReadBoolean(xe, nameof(bInvertActive1));
            bIsActive2 = xd.ReadBoolean(xe, nameof(bIsActive2));
            bInvertActive2 = xd.ReadBoolean(xe, nameof(bInvertActive2));
            bIsActive3 = xd.ReadBoolean(xe, nameof(bIsActive3));
            bInvertActive3 = xd.ReadBoolean(xe, nameof(bInvertActive3));
            bIsActive4 = xd.ReadBoolean(xe, nameof(bIsActive4));
            bInvertActive4 = xd.ReadBoolean(xe, nameof(bInvertActive4));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(bIsActive0), bIsActive0);
            xs.WriteBoolean(xe, nameof(bInvertActive0), bInvertActive0);
            xs.WriteBoolean(xe, nameof(bIsActive1), bIsActive1);
            xs.WriteBoolean(xe, nameof(bInvertActive1), bInvertActive1);
            xs.WriteBoolean(xe, nameof(bIsActive2), bIsActive2);
            xs.WriteBoolean(xe, nameof(bInvertActive2), bInvertActive2);
            xs.WriteBoolean(xe, nameof(bIsActive3), bIsActive3);
            xs.WriteBoolean(xe, nameof(bInvertActive3), bInvertActive3);
            xs.WriteBoolean(xe, nameof(bIsActive4), bIsActive4);
            xs.WriteBoolean(xe, nameof(bInvertActive4), bInvertActive4);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSIsActiveModifier);
        }

        public bool Equals(BSIsActiveModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   bIsActive0.Equals(other.bIsActive0) &&
                   bInvertActive0.Equals(other.bInvertActive0) &&
                   bIsActive1.Equals(other.bIsActive1) &&
                   bInvertActive1.Equals(other.bInvertActive1) &&
                   bIsActive2.Equals(other.bIsActive2) &&
                   bInvertActive2.Equals(other.bInvertActive2) &&
                   bIsActive3.Equals(other.bIsActive3) &&
                   bInvertActive3.Equals(other.bInvertActive3) &&
                   bIsActive4.Equals(other.bIsActive4) &&
                   bInvertActive4.Equals(other.bInvertActive4) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(bIsActive0);
            hashcode.Add(bInvertActive0);
            hashcode.Add(bIsActive1);
            hashcode.Add(bInvertActive1);
            hashcode.Add(bIsActive2);
            hashcode.Add(bInvertActive2);
            hashcode.Add(bIsActive3);
            hashcode.Add(bInvertActive3);
            hashcode.Add(bIsActive4);
            hashcode.Add(bInvertActive4);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

