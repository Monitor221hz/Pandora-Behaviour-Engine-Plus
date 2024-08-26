using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbHandIkControlsModifierHand Signatire: 0x9c72e9e3 size: 112 flags: FLAGS_NONE

    // controlData class: hkbHandIkControlData Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // handIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // enable class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    public partial class hkbHandIkControlsModifierHand : IHavokObject, IEquatable<hkbHandIkControlsModifierHand?>
    {
        public hkbHandIkControlData controlData { set; get; } = new();
        public int handIndex { set; get; }
        public bool enable { set; get; }

        public virtual uint Signature { set; get; } = 0x9c72e9e3;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            controlData.Read(des, br);
            handIndex = br.ReadInt32();
            enable = br.ReadBoolean();
            br.Position += 11;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            controlData.Write(s, bw);
            bw.WriteInt32(handIndex);
            bw.WriteBoolean(enable);
            bw.Position += 11;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            controlData = xd.ReadClass<hkbHandIkControlData>(xe, nameof(controlData));
            handIndex = xd.ReadInt32(xe, nameof(handIndex));
            enable = xd.ReadBoolean(xe, nameof(enable));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkbHandIkControlData>(xe, nameof(controlData), controlData);
            xs.WriteNumber(xe, nameof(handIndex), handIndex);
            xs.WriteBoolean(xe, nameof(enable), enable);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbHandIkControlsModifierHand);
        }

        public bool Equals(hkbHandIkControlsModifierHand? other)
        {
            return other is not null &&
                   ((controlData is null && other.controlData is null) || (controlData is not null && other.controlData is not null && controlData.Equals((IHavokObject)other.controlData))) &&
                   handIndex.Equals(other.handIndex) &&
                   enable.Equals(other.enable) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(controlData);
            hashcode.Add(handIndex);
            hashcode.Add(enable);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

