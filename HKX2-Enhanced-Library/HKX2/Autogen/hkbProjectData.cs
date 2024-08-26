using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbProjectData Signatire: 0x13a39ba7 size: 48 flags: FLAGS_NONE

    // worldUpWS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // stringData class: hkbProjectStringData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // defaultEventMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 40 flags: FLAGS_NONE enum: EventMode
    public partial class hkbProjectData : hkReferencedObject, IEquatable<hkbProjectData?>
    {
        public Vector4 worldUpWS { set; get; }
        public hkbProjectStringData? stringData { set; get; }
        public sbyte defaultEventMode { set; get; }

        public override uint Signature { set; get; } = 0x13a39ba7;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            worldUpWS = br.ReadVector4();
            stringData = des.ReadClassPointer<hkbProjectStringData>(br);
            defaultEventMode = br.ReadSByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(worldUpWS);
            s.WriteClassPointer(bw, stringData);
            bw.WriteSByte(defaultEventMode);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            worldUpWS = xd.ReadVector4(xe, nameof(worldUpWS));
            stringData = xd.ReadClassPointer<hkbProjectStringData>(this, xe, nameof(stringData));
            defaultEventMode = xd.ReadFlag<EventMode, sbyte>(xe, nameof(defaultEventMode));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(worldUpWS), worldUpWS);
            xs.WriteClassPointer(xe, nameof(stringData), stringData);
            xs.WriteEnum<EventMode, sbyte>(xe, nameof(defaultEventMode), defaultEventMode);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbProjectData);
        }

        public bool Equals(hkbProjectData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   worldUpWS.Equals(other.worldUpWS) &&
                   ((stringData is null && other.stringData is null) || (stringData is not null && other.stringData is not null && stringData.Equals((IHavokObject)other.stringData))) &&
                   defaultEventMode.Equals(other.defaultEventMode) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(worldUpWS);
            hashcode.Add(stringData);
            hashcode.Add(defaultEventMode);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

