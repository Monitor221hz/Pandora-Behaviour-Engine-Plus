using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBehaviorGraphData Signatire: 0x95aca5d size: 128 flags: FLAGS_NONE

    // attributeDefaults class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // variableInfos class: hkbVariableInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // characterPropertyInfos class: hkbVariableInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // eventInfos class: hkbEventInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // wordMinVariableValues class: hkbVariableValue Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // wordMaxVariableValues class: hkbVariableValue Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // variableInitialValues class: hkbVariableValueSet Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // stringData class: hkbBehaviorGraphStringData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    public partial class hkbBehaviorGraphData : hkReferencedObject, IEquatable<hkbBehaviorGraphData?>
    {
        public IList<float> attributeDefaults { set; get; } = Array.Empty<float>();
        public IList<hkbVariableInfo> variableInfos { set; get; } = Array.Empty<hkbVariableInfo>();
        public IList<hkbVariableInfo> characterPropertyInfos { set; get; } = Array.Empty<hkbVariableInfo>();
        public IList<hkbEventInfo> eventInfos { set; get; } = Array.Empty<hkbEventInfo>();
        public IList<hkbVariableValue> wordMinVariableValues { set; get; } = Array.Empty<hkbVariableValue>();
        public IList<hkbVariableValue> wordMaxVariableValues { set; get; } = Array.Empty<hkbVariableValue>();
        public hkbVariableValueSet? variableInitialValues { set; get; }
        public hkbBehaviorGraphStringData? stringData { set; get; }

        public override uint Signature { set; get; } = 0x95aca5d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            attributeDefaults = des.ReadSingleArray(br);
            variableInfos = des.ReadClassArray<hkbVariableInfo>(br);
            characterPropertyInfos = des.ReadClassArray<hkbVariableInfo>(br);
            eventInfos = des.ReadClassArray<hkbEventInfo>(br);
            wordMinVariableValues = des.ReadClassArray<hkbVariableValue>(br);
            wordMaxVariableValues = des.ReadClassArray<hkbVariableValue>(br);
            variableInitialValues = des.ReadClassPointer<hkbVariableValueSet>(br);
            stringData = des.ReadClassPointer<hkbBehaviorGraphStringData>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteSingleArray(bw, attributeDefaults);
            s.WriteClassArray(bw, variableInfos);
            s.WriteClassArray(bw, characterPropertyInfos);
            s.WriteClassArray(bw, eventInfos);
            s.WriteClassArray(bw, wordMinVariableValues);
            s.WriteClassArray(bw, wordMaxVariableValues);
            s.WriteClassPointer(bw, variableInitialValues);
            s.WriteClassPointer(bw, stringData);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            attributeDefaults = xd.ReadSingleArray(xe, nameof(attributeDefaults));
            variableInfos = xd.ReadClassArray<hkbVariableInfo>(xe, nameof(variableInfos));
            characterPropertyInfos = xd.ReadClassArray<hkbVariableInfo>(xe, nameof(characterPropertyInfos));
            eventInfos = xd.ReadClassArray<hkbEventInfo>(xe, nameof(eventInfos));
            wordMinVariableValues = xd.ReadClassArray<hkbVariableValue>(xe, nameof(wordMinVariableValues));
            wordMaxVariableValues = xd.ReadClassArray<hkbVariableValue>(xe, nameof(wordMaxVariableValues));
            variableInitialValues = xd.ReadClassPointer<hkbVariableValueSet>(this, xe, nameof(variableInitialValues));
            stringData = xd.ReadClassPointer<hkbBehaviorGraphStringData>(this, xe, nameof(stringData));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloatArray(xe, nameof(attributeDefaults), attributeDefaults);
            xs.WriteClassArray(xe, nameof(variableInfos), variableInfos);
            xs.WriteClassArray(xe, nameof(characterPropertyInfos), characterPropertyInfos);
            xs.WriteClassArray(xe, nameof(eventInfos), eventInfos);
            xs.WriteClassArray(xe, nameof(wordMinVariableValues), wordMinVariableValues);
            xs.WriteClassArray(xe, nameof(wordMaxVariableValues), wordMaxVariableValues);
            xs.WriteClassPointer(xe, nameof(variableInitialValues), variableInitialValues);
            xs.WriteClassPointer(xe, nameof(stringData), stringData);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBehaviorGraphData);
        }

        public bool Equals(hkbBehaviorGraphData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   attributeDefaults.SequenceEqual(other.attributeDefaults) &&
                   variableInfos.SequenceEqual(other.variableInfos) &&
                   characterPropertyInfos.SequenceEqual(other.characterPropertyInfos) &&
                   eventInfos.SequenceEqual(other.eventInfos) &&
                   wordMinVariableValues.SequenceEqual(other.wordMinVariableValues) &&
                   wordMaxVariableValues.SequenceEqual(other.wordMaxVariableValues) &&
                   ((variableInitialValues is null && other.variableInitialValues is null) || (variableInitialValues is not null && other.variableInitialValues is not null && variableInitialValues.Equals((IHavokObject)other.variableInitialValues))) &&
                   ((stringData is null && other.stringData is null) || (stringData is not null && other.stringData is not null && stringData.Equals((IHavokObject)other.stringData))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(attributeDefaults.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(variableInfos.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(characterPropertyInfos.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(eventInfos.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(wordMinVariableValues.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(wordMaxVariableValues.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(variableInitialValues);
            hashcode.Add(stringData);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

