using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSenseHandleModifierRange Signatire: 0xfb56b692 size: 32 flags: FLAGS_NONE

    // @eventclass: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // minDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // maxDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // ignoreHandle class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    public partial class hkbSenseHandleModifierRange : IHavokObject, IEquatable<hkbSenseHandleModifierRange?>
    {
        public hkbEventProperty @event{ set; get; } = new();
        public float minDistance { set; get; }
        public float maxDistance { set; get; }
        public bool ignoreHandle { set; get; }

        public virtual uint Signature { set; get; } = 0xfb56b692;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            @event.Read(des, br);
            minDistance = br.ReadSingle();
            maxDistance = br.ReadSingle();
            ignoreHandle = br.ReadBoolean();
            br.Position += 7;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            @event.Write(s, bw);
            bw.WriteSingle(minDistance);
            bw.WriteSingle(maxDistance);
            bw.WriteBoolean(ignoreHandle);
            bw.Position += 7;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            @event= xd.ReadClass<hkbEventProperty>(xe, nameof(@event));
            minDistance = xd.ReadSingle(xe, nameof(minDistance));
            maxDistance = xd.ReadSingle(xe, nameof(maxDistance));
            ignoreHandle = xd.ReadBoolean(xe, nameof(ignoreHandle));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkbEventProperty>(xe, nameof(@event), @event);
            xs.WriteFloat(xe, nameof(minDistance), minDistance);
            xs.WriteFloat(xe, nameof(maxDistance), maxDistance);
            xs.WriteBoolean(xe, nameof(ignoreHandle), ignoreHandle);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSenseHandleModifierRange);
        }

        public bool Equals(hkbSenseHandleModifierRange? other)
        {
            return other is not null &&
                   ((@event is null && other.@event is null) || (@event is not null && other.@event is not null && @event.Equals((IHavokObject)other.@event))) &&
                   minDistance.Equals(other.minDistance) &&
                   maxDistance.Equals(other.maxDistance) &&
                   ignoreHandle.Equals(other.ignoreHandle) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(@event);
            hashcode.Add(minDistance);
            hashcode.Add(maxDistance);
            hashcode.Add(ignoreHandle);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

