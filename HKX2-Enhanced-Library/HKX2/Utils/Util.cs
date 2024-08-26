using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HKX2E
{
    public static class Util
    {
        public static IHavokObject ReadHKX(Stream stream)
        {
            var des = new PackFileDeserializer();
            var br = new BinaryReaderEx(stream);

            return des.Deserialize(br);
        }

        public static IHavokObject ReadHKX(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            return ReadHKX(stream);
        }

        public static IHavokObject ReadHKX(byte[] bytes)
        {
            return ReadHKX(new MemoryStream(bytes));
        }

        public static void WriteHKX(IHavokObject root, HKXHeader header, Stream stream)
        {
            var s = new PackFileSerializer();
            var bw = new BinaryWriterEx(stream);

            s.Serialize(root, bw, header);
        }

        public static void WriteHKX(IHavokObject root, HKXHeader header, string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            WriteHKX(root, header, stream);
        }

        public static byte[] WriteHKX(IHavokObject root, HKXHeader header)
        {
            var ms = new MemoryStream();
            WriteHKX(root, header, ms);
            return ms.ToArray();
        }

        public static IHavokObject ReadXml(Stream stream, HKXHeader header)
        {
            var xd = new HavokXmlDeserializer();
            return xd.Deserialize(stream, header);
        }

        public static IHavokObject ReadXml(string filePath, HKXHeader header)
        {
            using FileStream stream = File.OpenRead(filePath);
            return ReadXml(stream, header);
        }

        public static void WriteXml(IHavokObject root, HKXHeader header, Stream stream)
        {
            var s = new HavokXmlSerializer();
            s.Serialize(root, header, stream);
        }

        public static void WriteXml(IHavokObject root, HKXHeader header, string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            WriteXml(root, header, stream);
        }

        public static bool ScrambledEquals<T>(this IEnumerable<T?>? list1, IEnumerable<T?>? list2) where T : notnull
        {
            if (list1 is null && list2 is null) return true;
            if (list1 is null || list2 is null) return false;
            if (!list1.Any() && !list2.Any()) return true;
            if (list1.Count() != list2.Count()) return false;

            //var firstNotSecond = list1.Except(list2);
            //var secondNotFirst = list2.Except(list1);

            //return !firstNotSecond.Any() && !secondNotFirst.Any();

            var nullCounter = 0;
            var cnt = new Dictionary<T, int>();
            foreach (var s in list1)
            {
                if (s is null)
                {
                    nullCounter++;
                }
                else if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (var s in list2)
            {
                if (s is null)
                {
                    nullCounter--;
                }
                else if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return nullCounter == 0 && cnt.Values.All(c => c == 0);
        }
    }
}
