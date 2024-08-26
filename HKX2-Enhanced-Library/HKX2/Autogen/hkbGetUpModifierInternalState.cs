using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbGetUpModifierInternalState Signatire: 0xd84cad4a size: 32 flags: FLAGS_NONE

    // timeSinceBegin class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // timeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // initNextModify class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    public partial class hkbGetUpModifierInternalState : hkReferencedObject, IEquatable<hkbGetUpModifierInternalState?>
    {
        public float timeSinceBegin { set; get; }
        public float timeStep { set; get; }
        public bool initNextModify { set; get; }

        public override uint Signature { set; get; } = 0xd84cad4a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            timeSinceBegin = br.ReadSingle();
            timeStep = br.ReadSingle();
            initNextModify = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(timeSinceBegin);
            bw.WriteSingle(timeStep);
            bw.WriteBoolean(initNextModify);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            timeSinceBegin = xd.ReadSingle(xe, nameof(timeSinceBegin));
            timeStep = xd.ReadSingle(xe, nameof(timeStep));
            initNextModify = xd.ReadBoolean(xe, nameof(initNextModify));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(timeSinceBegin), timeSinceBegin);
            xs.WriteFloat(xe, nameof(timeStep), timeStep);
            xs.WriteBoolean(xe, nameof(initNextModify), initNextModify);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbGetUpModifierInternalState);
        }

        public bool Equals(hkbGetUpModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   timeSinceBegin.Equals(other.timeSinceBegin) &&
                   timeStep.Equals(other.timeStep) &&
                   initNextModify.Equals(other.initNextModify) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(timeSinceBegin);
            hashcode.Add(timeStep);
            hashcode.Add(initNextModify);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

