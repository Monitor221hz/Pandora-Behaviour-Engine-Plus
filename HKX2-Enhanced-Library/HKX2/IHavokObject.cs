using System.Xml.Linq;

namespace HKX2E
{
    public interface IHavokObject
    {
        public uint Signature { set; get; }

        public void Read(PackFileDeserializer des, BinaryReaderEx br);

        public void Write(PackFileSerializer s, BinaryWriterEx bw);
        public void WriteXml(IHavokXmlWriter xs, XElement xe);
        public void ReadXml(IHavokXmlReader xd, XElement xe);
    }
}