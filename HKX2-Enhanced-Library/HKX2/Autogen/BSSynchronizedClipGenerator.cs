using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // BSSynchronizedClipGenerator Signatire: 0xd83bea64 size: 304 flags: FLAGS_NONE

    // pClipGenerator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: ALIGN_16|FLAGS_NONE enum: 
    // SyncAnimPrefix class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // bSyncClipIgnoreMarkPlacement class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // fGetToMarkTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // fMarkErrorThreshold class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // bLeadCharacter class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // bReorientSupportChar class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 109 flags: FLAGS_NONE enum: 
    // bApplyMotionFromRoot class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 110 flags: FLAGS_NONE enum: 
    // pSyncScene class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // StartMarkWS class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 128 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // EndMarkWS class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 176 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // StartMarkMS class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 224 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // fCurrentLerp class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 272 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // pLocalSyncBinding class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 280 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // pEventMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 288 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // sAnimationBindingIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 296 flags: FLAGS_NONE enum: 
    // bAtMark class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 298 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // bAllCharactersInScene class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 299 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // bAllCharactersAtMarks class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 300 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSSynchronizedClipGenerator : hkbGenerator, IEquatable<BSSynchronizedClipGenerator?>
    {
        public hkbGenerator? pClipGenerator { set; get; }
        public string SyncAnimPrefix { set; get; } = "";
        public bool bSyncClipIgnoreMarkPlacement { set; get; }
        public float fGetToMarkTime { set; get; }
        public float fMarkErrorThreshold { set; get; }
        public bool bLeadCharacter { set; get; }
        public bool bReorientSupportChar { set; get; }
        public bool bApplyMotionFromRoot { set; get; }
        private object? pSyncScene { set; get; }
        private Matrix4x4 StartMarkWS { set; get; }
        private Matrix4x4 EndMarkWS { set; get; }
        private Matrix4x4 StartMarkMS { set; get; }
        private float fCurrentLerp { set; get; }
        private object? pLocalSyncBinding { set; get; }
        private object? pEventMap { set; get; }
        public short sAnimationBindingIndex { set; get; }
        private bool bAtMark { set; get; }
        private bool bAllCharactersInScene { set; get; }
        private bool bAllCharactersAtMarks { set; get; }

        public override uint Signature { set; get; } = 0xd83bea64;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            pClipGenerator = des.ReadClassPointer<hkbGenerator>(br);
            SyncAnimPrefix = des.ReadCString(br);
            bSyncClipIgnoreMarkPlacement = br.ReadBoolean();
            br.Position += 3;
            fGetToMarkTime = br.ReadSingle();
            fMarkErrorThreshold = br.ReadSingle();
            bLeadCharacter = br.ReadBoolean();
            bReorientSupportChar = br.ReadBoolean();
            bApplyMotionFromRoot = br.ReadBoolean();
            br.Position += 1;
            des.ReadEmptyPointer(br);
            br.Position += 8;
            StartMarkWS = des.ReadQSTransform(br);
            EndMarkWS = des.ReadQSTransform(br);
            StartMarkMS = des.ReadQSTransform(br);
            fCurrentLerp = br.ReadSingle();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            sAnimationBindingIndex = br.ReadInt16();
            bAtMark = br.ReadBoolean();
            bAllCharactersInScene = br.ReadBoolean();
            bAllCharactersAtMarks = br.ReadBoolean();
            br.Position += 3;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            s.WriteClassPointer(bw, pClipGenerator);
            s.WriteCString(bw, SyncAnimPrefix);
            bw.WriteBoolean(bSyncClipIgnoreMarkPlacement);
            bw.Position += 3;
            bw.WriteSingle(fGetToMarkTime);
            bw.WriteSingle(fMarkErrorThreshold);
            bw.WriteBoolean(bLeadCharacter);
            bw.WriteBoolean(bReorientSupportChar);
            bw.WriteBoolean(bApplyMotionFromRoot);
            bw.Position += 1;
            s.WriteVoidPointer(bw);
            bw.Position += 8;
            s.WriteQSTransform(bw, StartMarkWS);
            s.WriteQSTransform(bw, EndMarkWS);
            s.WriteQSTransform(bw, StartMarkMS);
            bw.WriteSingle(fCurrentLerp);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            bw.WriteInt16(sAnimationBindingIndex);
            bw.WriteBoolean(bAtMark);
            bw.WriteBoolean(bAllCharactersInScene);
            bw.WriteBoolean(bAllCharactersAtMarks);
            bw.Position += 3;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pClipGenerator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(pClipGenerator));
            SyncAnimPrefix = xd.ReadString(xe, nameof(SyncAnimPrefix));
            bSyncClipIgnoreMarkPlacement = xd.ReadBoolean(xe, nameof(bSyncClipIgnoreMarkPlacement));
            fGetToMarkTime = xd.ReadSingle(xe, nameof(fGetToMarkTime));
            fMarkErrorThreshold = xd.ReadSingle(xe, nameof(fMarkErrorThreshold));
            bLeadCharacter = xd.ReadBoolean(xe, nameof(bLeadCharacter));
            bReorientSupportChar = xd.ReadBoolean(xe, nameof(bReorientSupportChar));
            bApplyMotionFromRoot = xd.ReadBoolean(xe, nameof(bApplyMotionFromRoot));
            sAnimationBindingIndex = xd.ReadInt16(xe, nameof(sAnimationBindingIndex));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(pClipGenerator), pClipGenerator);
            xs.WriteString(xe, nameof(SyncAnimPrefix), SyncAnimPrefix);
            xs.WriteBoolean(xe, nameof(bSyncClipIgnoreMarkPlacement), bSyncClipIgnoreMarkPlacement);
            xs.WriteFloat(xe, nameof(fGetToMarkTime), fGetToMarkTime);
            xs.WriteFloat(xe, nameof(fMarkErrorThreshold), fMarkErrorThreshold);
            xs.WriteBoolean(xe, nameof(bLeadCharacter), bLeadCharacter);
            xs.WriteBoolean(xe, nameof(bReorientSupportChar), bReorientSupportChar);
            xs.WriteBoolean(xe, nameof(bApplyMotionFromRoot), bApplyMotionFromRoot);
            xs.WriteSerializeIgnored(xe, nameof(pSyncScene));
            xs.WriteSerializeIgnored(xe, nameof(StartMarkWS));
            xs.WriteSerializeIgnored(xe, nameof(EndMarkWS));
            xs.WriteSerializeIgnored(xe, nameof(StartMarkMS));
            xs.WriteSerializeIgnored(xe, nameof(fCurrentLerp));
            xs.WriteSerializeIgnored(xe, nameof(pLocalSyncBinding));
            xs.WriteSerializeIgnored(xe, nameof(pEventMap));
            xs.WriteNumber(xe, nameof(sAnimationBindingIndex), sAnimationBindingIndex);
            xs.WriteSerializeIgnored(xe, nameof(bAtMark));
            xs.WriteSerializeIgnored(xe, nameof(bAllCharactersInScene));
            xs.WriteSerializeIgnored(xe, nameof(bAllCharactersAtMarks));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSSynchronizedClipGenerator);
        }

        public bool Equals(BSSynchronizedClipGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((pClipGenerator is null && other.pClipGenerator is null) || (pClipGenerator is not null && other.pClipGenerator is not null && pClipGenerator.Equals((IHavokObject)other.pClipGenerator))) &&
                   (SyncAnimPrefix is null && other.SyncAnimPrefix is null || SyncAnimPrefix == other.SyncAnimPrefix || SyncAnimPrefix is null && other.SyncAnimPrefix == "" || SyncAnimPrefix == "" && other.SyncAnimPrefix is null) &&
                   bSyncClipIgnoreMarkPlacement.Equals(other.bSyncClipIgnoreMarkPlacement) &&
                   fGetToMarkTime.Equals(other.fGetToMarkTime) &&
                   fMarkErrorThreshold.Equals(other.fMarkErrorThreshold) &&
                   bLeadCharacter.Equals(other.bLeadCharacter) &&
                   bReorientSupportChar.Equals(other.bReorientSupportChar) &&
                   bApplyMotionFromRoot.Equals(other.bApplyMotionFromRoot) &&
                   sAnimationBindingIndex.Equals(other.sAnimationBindingIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pClipGenerator);
            hashcode.Add(SyncAnimPrefix);
            hashcode.Add(bSyncClipIgnoreMarkPlacement);
            hashcode.Add(fGetToMarkTime);
            hashcode.Add(fMarkErrorThreshold);
            hashcode.Add(bLeadCharacter);
            hashcode.Add(bReorientSupportChar);
            hashcode.Add(bApplyMotionFromRoot);
            hashcode.Add(sAnimationBindingIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

