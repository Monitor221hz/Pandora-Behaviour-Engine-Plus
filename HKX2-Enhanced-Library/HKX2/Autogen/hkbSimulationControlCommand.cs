using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSimulationControlCommand Signatire: 0x2a241367 size: 24 flags: FLAGS_NONE

    // command class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: SimulationControlCommand
    public partial class hkbSimulationControlCommand : hkReferencedObject, IEquatable<hkbSimulationControlCommand?>
    {
        public byte command { set; get; }

        public override uint Signature { set; get; } = 0x2a241367;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            command = br.ReadByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(command);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            command = xd.ReadFlag<SimulationControlCommand, byte>(xe, nameof(command));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<SimulationControlCommand, byte>(xe, nameof(command), command);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSimulationControlCommand);
        }

        public bool Equals(hkbSimulationControlCommand? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   command.Equals(other.command) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(command);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

