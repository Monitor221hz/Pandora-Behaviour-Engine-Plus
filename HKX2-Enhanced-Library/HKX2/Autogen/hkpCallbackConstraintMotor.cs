using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCallbackConstraintMotor Signatire: 0xafcd79ad size: 72 flags: FLAGS_NONE

    // callbackFunc class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 32 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // callbackType class:  Type.TYPE_ENUM Type.TYPE_UINT32 arrSize: 0 offset: 40 flags: FLAGS_NONE enum: CallbackType
    // userData0 class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // userData1 class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // userData2 class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpCallbackConstraintMotor : hkpLimitedForceConstraintMotor, IEquatable<hkpCallbackConstraintMotor?>
    {
        private object? callbackFunc { set; get; }
        public uint callbackType { set; get; }
        public ulong userData0 { set; get; }
        public ulong userData1 { set; get; }
        public ulong userData2 { set; get; }

        public override uint Signature { set; get; } = 0xafcd79ad;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            des.ReadEmptyPointer(br);
            callbackType = br.ReadUInt32();
            br.Position += 4;
            userData0 = br.ReadUInt64();
            userData1 = br.ReadUInt64();
            userData2 = br.ReadUInt64();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVoidPointer(bw);
            bw.WriteUInt32(callbackType);
            bw.Position += 4;
            bw.WriteUInt64(userData0);
            bw.WriteUInt64(userData1);
            bw.WriteUInt64(userData2);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            callbackType = xd.ReadFlag<CallbackType, uint>(xe, nameof(callbackType));
            userData0 = xd.ReadUInt64(xe, nameof(userData0));
            userData1 = xd.ReadUInt64(xe, nameof(userData1));
            userData2 = xd.ReadUInt64(xe, nameof(userData2));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(callbackFunc));
            xs.WriteEnum<CallbackType, uint>(xe, nameof(callbackType), callbackType);
            xs.WriteNumber(xe, nameof(userData0), userData0);
            xs.WriteNumber(xe, nameof(userData1), userData1);
            xs.WriteNumber(xe, nameof(userData2), userData2);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCallbackConstraintMotor);
        }

        public bool Equals(hkpCallbackConstraintMotor? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   callbackType.Equals(other.callbackType) &&
                   userData0.Equals(other.userData0) &&
                   userData1.Equals(other.userData1) &&
                   userData2.Equals(other.userData2) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(callbackType);
            hashcode.Add(userData0);
            hashcode.Add(userData1);
            hashcode.Add(userData2);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

