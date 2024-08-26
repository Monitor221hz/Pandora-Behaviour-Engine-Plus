using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // BSLookAtModifierBoneData Signatire: 0x29efee59 size: 64 flags: FLAGS_NONE

    // index class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // fwdAxisLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // limitAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // onGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // offGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // enabled class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    // currentFwdAxisLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSLookAtModifierBoneData : IHavokObject, IEquatable<BSLookAtModifierBoneData?>
    {
        public short index { set; get; }
        public Vector4 fwdAxisLS { set; get; }
        public float limitAngleDegrees { set; get; }
        public float onGain { set; get; }
        public float offGain { set; get; }
        public bool enabled { set; get; }
        private Vector4 currentFwdAxisLS { set; get; }

        public virtual uint Signature { set; get; } = 0x29efee59;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            index = br.ReadInt16();
            br.Position += 14;
            fwdAxisLS = br.ReadVector4();
            limitAngleDegrees = br.ReadSingle();
            onGain = br.ReadSingle();
            offGain = br.ReadSingle();
            enabled = br.ReadBoolean();
            br.Position += 3;
            currentFwdAxisLS = br.ReadVector4();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt16(index);
            bw.Position += 14;
            bw.WriteVector4(fwdAxisLS);
            bw.WriteSingle(limitAngleDegrees);
            bw.WriteSingle(onGain);
            bw.WriteSingle(offGain);
            bw.WriteBoolean(enabled);
            bw.Position += 3;
            bw.WriteVector4(currentFwdAxisLS);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            index = xd.ReadInt16(xe, nameof(index));
            fwdAxisLS = xd.ReadVector4(xe, nameof(fwdAxisLS));
            limitAngleDegrees = xd.ReadSingle(xe, nameof(limitAngleDegrees));
            onGain = xd.ReadSingle(xe, nameof(onGain));
            offGain = xd.ReadSingle(xe, nameof(offGain));
            enabled = xd.ReadBoolean(xe, nameof(enabled));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(index), index);
            xs.WriteVector4(xe, nameof(fwdAxisLS), fwdAxisLS);
            xs.WriteFloat(xe, nameof(limitAngleDegrees), limitAngleDegrees);
            xs.WriteFloat(xe, nameof(onGain), onGain);
            xs.WriteFloat(xe, nameof(offGain), offGain);
            xs.WriteBoolean(xe, nameof(enabled), enabled);
            xs.WriteSerializeIgnored(xe, nameof(currentFwdAxisLS));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSLookAtModifierBoneData);
        }

        public bool Equals(BSLookAtModifierBoneData? other)
        {
            return other is not null &&
                   index.Equals(other.index) &&
                   fwdAxisLS.Equals(other.fwdAxisLS) &&
                   limitAngleDegrees.Equals(other.limitAngleDegrees) &&
                   onGain.Equals(other.onGain) &&
                   offGain.Equals(other.offGain) &&
                   enabled.Equals(other.enabled) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(index);
            hashcode.Add(fwdAxisLS);
            hashcode.Add(limitAngleDegrees);
            hashcode.Add(onGain);
            hashcode.Add(offGain);
            hashcode.Add(enabled);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

