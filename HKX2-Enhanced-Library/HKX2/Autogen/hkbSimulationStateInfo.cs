using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSimulationStateInfo Signatire: 0xa40822b4 size: 24 flags: FLAGS_NONE

    // simulationState class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: SimulationState
    public partial class hkbSimulationStateInfo : hkReferencedObject, IEquatable<hkbSimulationStateInfo?>
    {
        public byte simulationState { set; get; }

        public override uint Signature { set; get; } = 0xa40822b4;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            simulationState = br.ReadByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(simulationState);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            simulationState = xd.ReadFlag<SimulationState, byte>(xe, nameof(simulationState));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<SimulationState, byte>(xe, nameof(simulationState), simulationState);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSimulationStateInfo);
        }

        public bool Equals(hkbSimulationStateInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   simulationState.Equals(other.simulationState) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(simulationState);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

