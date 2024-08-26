using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSEventOnFalseToTrueModifier Signatire: 0x81d0777a size: 160 flags: FLAGS_NONE

    // bEnableEvent1 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // bVariableToTest1 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 81 flags: FLAGS_NONE enum: 
    // EventToSend1 class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // bEnableEvent2 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // bVariableToTest2 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 105 flags: FLAGS_NONE enum: 
    // EventToSend2 class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // bEnableEvent3 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // bVariableToTest3 class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 129 flags: FLAGS_NONE enum: 
    // EventToSend3 class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // bSlot1ActivatedLastFrame class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 152 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // bSlot2ActivatedLastFrame class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 153 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // bSlot3ActivatedLastFrame class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 154 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSEventOnFalseToTrueModifier : hkbModifier, IEquatable<BSEventOnFalseToTrueModifier?>
    {
        public bool bEnableEvent1 { set; get; }
        public bool bVariableToTest1 { set; get; }
        public hkbEventProperty EventToSend1 { set; get; } = new();
        public bool bEnableEvent2 { set; get; }
        public bool bVariableToTest2 { set; get; }
        public hkbEventProperty EventToSend2 { set; get; } = new();
        public bool bEnableEvent3 { set; get; }
        public bool bVariableToTest3 { set; get; }
        public hkbEventProperty EventToSend3 { set; get; } = new();
        private bool bSlot1ActivatedLastFrame { set; get; }
        private bool bSlot2ActivatedLastFrame { set; get; }
        private bool bSlot3ActivatedLastFrame { set; get; }

        public override uint Signature { set; get; } = 0x81d0777a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            bEnableEvent1 = br.ReadBoolean();
            bVariableToTest1 = br.ReadBoolean();
            br.Position += 6;
            EventToSend1.Read(des, br);
            bEnableEvent2 = br.ReadBoolean();
            bVariableToTest2 = br.ReadBoolean();
            br.Position += 6;
            EventToSend2.Read(des, br);
            bEnableEvent3 = br.ReadBoolean();
            bVariableToTest3 = br.ReadBoolean();
            br.Position += 6;
            EventToSend3.Read(des, br);
            bSlot1ActivatedLastFrame = br.ReadBoolean();
            bSlot2ActivatedLastFrame = br.ReadBoolean();
            bSlot3ActivatedLastFrame = br.ReadBoolean();
            br.Position += 5;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(bEnableEvent1);
            bw.WriteBoolean(bVariableToTest1);
            bw.Position += 6;
            EventToSend1.Write(s, bw);
            bw.WriteBoolean(bEnableEvent2);
            bw.WriteBoolean(bVariableToTest2);
            bw.Position += 6;
            EventToSend2.Write(s, bw);
            bw.WriteBoolean(bEnableEvent3);
            bw.WriteBoolean(bVariableToTest3);
            bw.Position += 6;
            EventToSend3.Write(s, bw);
            bw.WriteBoolean(bSlot1ActivatedLastFrame);
            bw.WriteBoolean(bSlot2ActivatedLastFrame);
            bw.WriteBoolean(bSlot3ActivatedLastFrame);
            bw.Position += 5;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            bEnableEvent1 = xd.ReadBoolean(xe, nameof(bEnableEvent1));
            bVariableToTest1 = xd.ReadBoolean(xe, nameof(bVariableToTest1));
            EventToSend1 = xd.ReadClass<hkbEventProperty>(xe, nameof(EventToSend1));
            bEnableEvent2 = xd.ReadBoolean(xe, nameof(bEnableEvent2));
            bVariableToTest2 = xd.ReadBoolean(xe, nameof(bVariableToTest2));
            EventToSend2 = xd.ReadClass<hkbEventProperty>(xe, nameof(EventToSend2));
            bEnableEvent3 = xd.ReadBoolean(xe, nameof(bEnableEvent3));
            bVariableToTest3 = xd.ReadBoolean(xe, nameof(bVariableToTest3));
            EventToSend3 = xd.ReadClass<hkbEventProperty>(xe, nameof(EventToSend3));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(bEnableEvent1), bEnableEvent1);
            xs.WriteBoolean(xe, nameof(bVariableToTest1), bVariableToTest1);
            xs.WriteClass<hkbEventProperty>(xe, nameof(EventToSend1), EventToSend1);
            xs.WriteBoolean(xe, nameof(bEnableEvent2), bEnableEvent2);
            xs.WriteBoolean(xe, nameof(bVariableToTest2), bVariableToTest2);
            xs.WriteClass<hkbEventProperty>(xe, nameof(EventToSend2), EventToSend2);
            xs.WriteBoolean(xe, nameof(bEnableEvent3), bEnableEvent3);
            xs.WriteBoolean(xe, nameof(bVariableToTest3), bVariableToTest3);
            xs.WriteClass<hkbEventProperty>(xe, nameof(EventToSend3), EventToSend3);
            xs.WriteSerializeIgnored(xe, nameof(bSlot1ActivatedLastFrame));
            xs.WriteSerializeIgnored(xe, nameof(bSlot2ActivatedLastFrame));
            xs.WriteSerializeIgnored(xe, nameof(bSlot3ActivatedLastFrame));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSEventOnFalseToTrueModifier);
        }

        public bool Equals(BSEventOnFalseToTrueModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   bEnableEvent1.Equals(other.bEnableEvent1) &&
                   bVariableToTest1.Equals(other.bVariableToTest1) &&
                   ((EventToSend1 is null && other.EventToSend1 is null) || (EventToSend1 is not null && other.EventToSend1 is not null && EventToSend1.Equals((IHavokObject)other.EventToSend1))) &&
                   bEnableEvent2.Equals(other.bEnableEvent2) &&
                   bVariableToTest2.Equals(other.bVariableToTest2) &&
                   ((EventToSend2 is null && other.EventToSend2 is null) || (EventToSend2 is not null && other.EventToSend2 is not null && EventToSend2.Equals((IHavokObject)other.EventToSend2))) &&
                   bEnableEvent3.Equals(other.bEnableEvent3) &&
                   bVariableToTest3.Equals(other.bVariableToTest3) &&
                   ((EventToSend3 is null && other.EventToSend3 is null) || (EventToSend3 is not null && other.EventToSend3 is not null && EventToSend3.Equals((IHavokObject)other.EventToSend3))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(bEnableEvent1);
            hashcode.Add(bVariableToTest1);
            hashcode.Add(EventToSend1);
            hashcode.Add(bEnableEvent2);
            hashcode.Add(bVariableToTest2);
            hashcode.Add(EventToSend2);
            hashcode.Add(bEnableEvent3);
            hashcode.Add(bVariableToTest3);
            hashcode.Add(EventToSend3);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

