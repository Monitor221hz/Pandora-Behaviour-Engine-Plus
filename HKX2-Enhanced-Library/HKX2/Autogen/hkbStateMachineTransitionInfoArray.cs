using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachineTransitionInfoArray Signatire: 0xe397b11e size: 32 flags: FLAGS_NONE

    // transitions class: hkbStateMachineTransitionInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbStateMachineTransitionInfoArray : hkReferencedObject, IEquatable<hkbStateMachineTransitionInfoArray?>
    {
        public static hkbStateMachineTransitionInfoArray GetDefault() => new()
        {
            transitions = new List<hkbStateMachineTransitionInfo>(), 
        };
		public void SetDefault()
		{
            transitions = new List<hkbStateMachineTransitionInfo>();
		}
		public IList<hkbStateMachineTransitionInfo> transitions { set; get; } = Array.Empty<hkbStateMachineTransitionInfo>();

        public override uint Signature { set; get; } = 0xe397b11e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            transitions = des.ReadClassArray<hkbStateMachineTransitionInfo>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, transitions);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            transitions = xd.ReadClassArray<hkbStateMachineTransitionInfo>(xe, nameof(transitions));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(transitions), transitions);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachineTransitionInfoArray);
        }

        public bool Equals(hkbStateMachineTransitionInfoArray? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   transitions.SequenceEqual(other.transitions) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(transitions.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

