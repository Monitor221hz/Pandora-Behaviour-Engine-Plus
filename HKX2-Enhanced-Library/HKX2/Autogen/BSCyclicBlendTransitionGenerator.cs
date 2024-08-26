using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSCyclicBlendTransitionGenerator Signatire: 0x5119eb06 size: 176 flags: FLAGS_NONE

    // pBlenderGenerator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: ALIGN_16|FLAGS_NONE enum: 
    // EventToFreezeBlendValue class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // EventToCrossBlend class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // fBlendParameter class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // fTransitionDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 124 flags: FLAGS_NONE enum: 
    // eBlendCurve class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 128 flags: FLAGS_NONE enum: BlendCurve
    // pTransitionBlenderGenerator class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|ALIGN_16|FLAGS_NONE enum: 
    // pTransitionEffect class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 160 flags: SERIALIZE_IGNORED|ALIGN_16|FLAGS_NONE enum: 
    // currentMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 168 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSCyclicBlendTransitionGenerator : hkbGenerator, IEquatable<BSCyclicBlendTransitionGenerator?>
    {
        public hkbGenerator? pBlenderGenerator { set; get; }
        public hkbEventProperty EventToFreezeBlendValue { set; get; } = new();
        public hkbEventProperty EventToCrossBlend { set; get; } = new();
        public float fBlendParameter { set; get; }
        public float fTransitionDuration { set; get; }
        public sbyte eBlendCurve { set; get; }
        private object? pTransitionBlenderGenerator { set; get; }
        private object? pTransitionEffect { set; get; }
        private sbyte currentMode { set; get; }

        public override uint Signature { set; get; } = 0x5119eb06;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            pBlenderGenerator = des.ReadClassPointer<hkbGenerator>(br);
            EventToFreezeBlendValue.Read(des, br);
            EventToCrossBlend.Read(des, br);
            fBlendParameter = br.ReadSingle();
            fTransitionDuration = br.ReadSingle();
            eBlendCurve = br.ReadSByte();
            br.Position += 15;
            des.ReadEmptyPointer(br);
            br.Position += 8;
            des.ReadEmptyPointer(br);
            currentMode = br.ReadSByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            s.WriteClassPointer(bw, pBlenderGenerator);
            EventToFreezeBlendValue.Write(s, bw);
            EventToCrossBlend.Write(s, bw);
            bw.WriteSingle(fBlendParameter);
            bw.WriteSingle(fTransitionDuration);
            bw.WriteSByte(eBlendCurve);
            bw.Position += 15;
            s.WriteVoidPointer(bw);
            bw.Position += 8;
            s.WriteVoidPointer(bw);
            bw.WriteSByte(currentMode);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pBlenderGenerator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(pBlenderGenerator));
            EventToFreezeBlendValue = xd.ReadClass<hkbEventProperty>(xe, nameof(EventToFreezeBlendValue));
            EventToCrossBlend = xd.ReadClass<hkbEventProperty>(xe, nameof(EventToCrossBlend));
            fBlendParameter = xd.ReadSingle(xe, nameof(fBlendParameter));
            fTransitionDuration = xd.ReadSingle(xe, nameof(fTransitionDuration));
            eBlendCurve = xd.ReadFlag<BlendCurve, sbyte>(xe, nameof(eBlendCurve));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(pBlenderGenerator), pBlenderGenerator);
            xs.WriteClass<hkbEventProperty>(xe, nameof(EventToFreezeBlendValue), EventToFreezeBlendValue);
            xs.WriteClass<hkbEventProperty>(xe, nameof(EventToCrossBlend), EventToCrossBlend);
            xs.WriteFloat(xe, nameof(fBlendParameter), fBlendParameter);
            xs.WriteFloat(xe, nameof(fTransitionDuration), fTransitionDuration);
            xs.WriteEnum<BlendCurve, sbyte>(xe, nameof(eBlendCurve), eBlendCurve);
            xs.WriteSerializeIgnored(xe, nameof(pTransitionBlenderGenerator));
            xs.WriteSerializeIgnored(xe, nameof(pTransitionEffect));
            xs.WriteSerializeIgnored(xe, nameof(currentMode));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSCyclicBlendTransitionGenerator);
        }

        public bool Equals(BSCyclicBlendTransitionGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((pBlenderGenerator is null && other.pBlenderGenerator is null) || (pBlenderGenerator is not null && other.pBlenderGenerator is not null && pBlenderGenerator.Equals((IHavokObject)other.pBlenderGenerator))) &&
                   ((EventToFreezeBlendValue is null && other.EventToFreezeBlendValue is null) || (EventToFreezeBlendValue is not null && other.EventToFreezeBlendValue is not null && EventToFreezeBlendValue.Equals((IHavokObject)other.EventToFreezeBlendValue))) &&
                   ((EventToCrossBlend is null && other.EventToCrossBlend is null) || (EventToCrossBlend is not null && other.EventToCrossBlend is not null && EventToCrossBlend.Equals((IHavokObject)other.EventToCrossBlend))) &&
                   fBlendParameter.Equals(other.fBlendParameter) &&
                   fTransitionDuration.Equals(other.fTransitionDuration) &&
                   eBlendCurve.Equals(other.eBlendCurve) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pBlenderGenerator);
            hashcode.Add(EventToFreezeBlendValue);
            hashcode.Add(EventToCrossBlend);
            hashcode.Add(fBlendParameter);
            hashcode.Add(fTransitionDuration);
            hashcode.Add(eBlendCurve);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

