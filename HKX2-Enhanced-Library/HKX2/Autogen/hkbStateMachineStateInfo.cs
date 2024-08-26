using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachineStateInfo Signatire: 0xed7f9d0 size: 120 flags: FLAGS_NONE

    // listeners class: hkbStateListener Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // enterNotifyEvents class: hkbStateMachineEventPropertyArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // exitNotifyEvents class: hkbStateMachineEventPropertyArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // transitions class: hkbStateMachineTransitionInfoArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // generator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // stateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // probability class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // enable class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkbStateMachineStateInfo : hkbBindable, IEquatable<hkbStateMachineStateInfo?>
    {
        public static hkbStateMachineStateInfo GetDefault() => new()
        {
            probability = 1.0f, 
            enable = true
        };
        public void SetDefault()
        {
            probability = 1.0f;
            enable = true; 
        }
        public IList<hkbStateListener> listeners { set; get; } = Array.Empty<hkbStateListener>();
        public hkbStateMachineEventPropertyArray? enterNotifyEvents { set; get; }
        public hkbStateMachineEventPropertyArray? exitNotifyEvents { set; get; }
        public hkbStateMachineTransitionInfoArray? transitions { set; get; }
        public hkbGenerator? generator { set; get; }
        public string name { set; get; } = "";
        public int stateId { set; get; }
        public float probability { set; get; }
        public bool enable { set; get; }

        public override uint Signature { set; get; } = 0xed7f9d0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            listeners = des.ReadClassPointerArray<hkbStateListener>(br);
            enterNotifyEvents = des.ReadClassPointer<hkbStateMachineEventPropertyArray>(br);
            exitNotifyEvents = des.ReadClassPointer<hkbStateMachineEventPropertyArray>(br);
            transitions = des.ReadClassPointer<hkbStateMachineTransitionInfoArray>(br);
            generator = des.ReadClassPointer<hkbGenerator>(br);
            name = des.ReadStringPointer(br);
            stateId = br.ReadInt32();
            probability = br.ReadSingle();
            enable = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, listeners);
            s.WriteClassPointer(bw, enterNotifyEvents);
            s.WriteClassPointer(bw, exitNotifyEvents);
            s.WriteClassPointer(bw, transitions);
            s.WriteClassPointer(bw, generator);
            s.WriteStringPointer(bw, name);
            bw.WriteInt32(stateId);
            bw.WriteSingle(probability);
            bw.WriteBoolean(enable);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            listeners = xd.ReadClassPointerArray<hkbStateListener>(this, xe, nameof(listeners));
            enterNotifyEvents = xd.ReadClassPointer<hkbStateMachineEventPropertyArray>(this, xe, nameof(enterNotifyEvents));
            exitNotifyEvents = xd.ReadClassPointer<hkbStateMachineEventPropertyArray>(this, xe, nameof(exitNotifyEvents));
            transitions = xd.ReadClassPointer<hkbStateMachineTransitionInfoArray>(this, xe, nameof(transitions));
            generator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(generator));
            name = xd.ReadString(xe, nameof(name));
            stateId = xd.ReadInt32(xe, nameof(stateId));
            probability = xd.ReadSingle(xe, nameof(probability));
            enable = xd.ReadBoolean(xe, nameof(enable));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(listeners), listeners!);
            xs.WriteClassPointer(xe, nameof(enterNotifyEvents), enterNotifyEvents);
            xs.WriteClassPointer(xe, nameof(exitNotifyEvents), exitNotifyEvents);
            xs.WriteClassPointer(xe, nameof(transitions), transitions);
            xs.WriteClassPointer(xe, nameof(generator), generator);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteNumber(xe, nameof(stateId), stateId);
            xs.WriteFloat(xe, nameof(probability), probability);
            xs.WriteBoolean(xe, nameof(enable), enable);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachineStateInfo);
        }

        public bool Equals(hkbStateMachineStateInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   listeners.SequenceEqual(other.listeners) &&
                   ((enterNotifyEvents is null && other.enterNotifyEvents is null) || (enterNotifyEvents is not null && other.enterNotifyEvents is not null && enterNotifyEvents.Equals((IHavokObject)other.enterNotifyEvents))) &&
                   ((exitNotifyEvents is null && other.exitNotifyEvents is null) || (exitNotifyEvents is not null && other.exitNotifyEvents is not null && exitNotifyEvents.Equals((IHavokObject)other.exitNotifyEvents))) &&
                   ((transitions is null && other.transitions is null) || (transitions is not null && other.transitions is not null && transitions.Equals((IHavokObject)other.transitions))) &&
                   ((generator is null && other.generator is null) || (generator is not null && other.generator is not null && generator.Equals((IHavokObject)other.generator))) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   stateId.Equals(other.stateId) &&
                   probability.Equals(other.probability) &&
                   enable.Equals(other.enable) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(listeners.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(enterNotifyEvents);
            hashcode.Add(exitNotifyEvents);
            hashcode.Add(transitions);
            hashcode.Add(generator);
            hashcode.Add(name);
            hashcode.Add(stateId);
            hashcode.Add(probability);
            hashcode.Add(enable);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

