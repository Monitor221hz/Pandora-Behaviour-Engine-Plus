using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbLookAtModifierInternalState Signatire: 0xa14caba6 size: 48 flags: FLAGS_NONE

    // lookAtLastTargetWS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // lookAtWeight class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // isTargetInsideLimitCone class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    public partial class hkbLookAtModifierInternalState : hkReferencedObject, IEquatable<hkbLookAtModifierInternalState?>
    {
        public Vector4 lookAtLastTargetWS { set; get; }
        public float lookAtWeight { set; get; }
        public bool isTargetInsideLimitCone { set; get; }

        public override uint Signature { set; get; } = 0xa14caba6;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            lookAtLastTargetWS = br.ReadVector4();
            lookAtWeight = br.ReadSingle();
            isTargetInsideLimitCone = br.ReadBoolean();
            br.Position += 11;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(lookAtLastTargetWS);
            bw.WriteSingle(lookAtWeight);
            bw.WriteBoolean(isTargetInsideLimitCone);
            bw.Position += 11;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            lookAtLastTargetWS = xd.ReadVector4(xe, nameof(lookAtLastTargetWS));
            lookAtWeight = xd.ReadSingle(xe, nameof(lookAtWeight));
            isTargetInsideLimitCone = xd.ReadBoolean(xe, nameof(isTargetInsideLimitCone));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(lookAtLastTargetWS), lookAtLastTargetWS);
            xs.WriteFloat(xe, nameof(lookAtWeight), lookAtWeight);
            xs.WriteBoolean(xe, nameof(isTargetInsideLimitCone), isTargetInsideLimitCone);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbLookAtModifierInternalState);
        }

        public bool Equals(hkbLookAtModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   lookAtLastTargetWS.Equals(other.lookAtLastTargetWS) &&
                   lookAtWeight.Equals(other.lookAtWeight) &&
                   isTargetInsideLimitCone.Equals(other.isTargetInsideLimitCone) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(lookAtLastTargetWS);
            hashcode.Add(lookAtWeight);
            hashcode.Add(isTargetInsideLimitCone);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

