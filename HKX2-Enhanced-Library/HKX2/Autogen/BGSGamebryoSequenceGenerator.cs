using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // BGSGamebryoSequenceGenerator Signatire: 0xc8df2d77 size: 112 flags: FLAGS_NONE

    // pSequence class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // eBlendModeFunction class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 80 flags: FLAGS_NONE enum: BlendModeFunction
    // fPercent class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // events class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 88 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // fTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // bDelayedActivate class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 108 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // bLooping class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 109 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BGSGamebryoSequenceGenerator : hkbGenerator, IEquatable<BGSGamebryoSequenceGenerator?>
    {
        public string pSequence { set; get; } = "";
        public sbyte eBlendModeFunction { set; get; }
        public float fPercent { set; get; }
        public IList<object> events { set; get; } = Array.Empty<object>();
        private float fTime { set; get; }
        private bool bDelayedActivate { set; get; }
        private bool bLooping { set; get; }

        public override uint Signature { set; get; } = 0xc8df2d77;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            pSequence = des.ReadCString(br);
            eBlendModeFunction = br.ReadSByte();
            br.Position += 3;
            fPercent = br.ReadSingle();
            des.ReadEmptyArray(br);
            fTime = br.ReadSingle();
            bDelayedActivate = br.ReadBoolean();
            bLooping = br.ReadBoolean();
            br.Position += 2;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteCString(bw, pSequence);
            bw.WriteSByte(eBlendModeFunction);
            bw.Position += 3;
            bw.WriteSingle(fPercent);
            s.WriteVoidArray(bw);
            bw.WriteSingle(fTime);
            bw.WriteBoolean(bDelayedActivate);
            bw.WriteBoolean(bLooping);
            bw.Position += 2;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pSequence = xd.ReadString(xe, nameof(pSequence));
            eBlendModeFunction = xd.ReadFlag<BlendModeFunction, sbyte>(xe, nameof(eBlendModeFunction));
            fPercent = xd.ReadSingle(xe, nameof(fPercent));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(pSequence), pSequence);
            xs.WriteEnum<BlendModeFunction, sbyte>(xe, nameof(eBlendModeFunction), eBlendModeFunction);
            xs.WriteFloat(xe, nameof(fPercent), fPercent);
            xs.WriteSerializeIgnored(xe, nameof(events));
            xs.WriteSerializeIgnored(xe, nameof(fTime));
            xs.WriteSerializeIgnored(xe, nameof(bDelayedActivate));
            xs.WriteSerializeIgnored(xe, nameof(bLooping));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BGSGamebryoSequenceGenerator);
        }

        public bool Equals(BGSGamebryoSequenceGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (pSequence is null && other.pSequence is null || pSequence == other.pSequence || pSequence is null && other.pSequence == "" || pSequence == "" && other.pSequence is null) &&
                   eBlendModeFunction.Equals(other.eBlendModeFunction) &&
                   fPercent.Equals(other.fPercent) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pSequence);
            hashcode.Add(eBlendModeFunction);
            hashcode.Add(fPercent);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

