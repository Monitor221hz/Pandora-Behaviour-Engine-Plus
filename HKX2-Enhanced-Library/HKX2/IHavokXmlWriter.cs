using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E;
public interface IHavokXmlWriter
{
	XElement WriteBoolean(XElement xe, string paramName, bool value);
	XElement WriteBooleanArray(XElement xe, string paramName, IList<bool> value);
	void WriteClass<T>(XElement xe, string paramName, T value) where T : IHavokObject;
	void WriteClassArray<T>(XElement xe, string paramName, IList<T> values) where T : IHavokObject;
	void WriteClassPointer<T>(XElement xe, string paramName, T? value) where T : IHavokObject;
	void WriteClassPointerArray<T>(XElement xe, string paramName, IList<T?> values) where T : IHavokObject;
	XElement WriteEnum<TEnum, TValue>(XElement xe, string paramName, TValue value)
		where TEnum : Enum
		where TValue : IBinaryInteger<TValue>;
	XElement WriteFlag<TEnum, TValue>(XElement xe, string paramName, TValue value)
		where TEnum : Enum
		where TValue : IBinaryInteger<TValue>;
	XElement WriteFloat<T>(XElement xe, string paramName, T value) where T : IFloatingPoint<T>;
	XElement WriteFloatArray<T>(XElement xe, string paramName, IList<T> value) where T : IFloatingPoint<T>;
	XElement WriteMatrix3(XElement xe, string paramName, Matrix4x4 value);
	XElement WriteMatrix3Array(XElement xe, string paramName, IList<Matrix4x4> value);
	XElement WriteMatrix4(XElement xe, string paramName, Matrix4x4 value);
	XElement WriteMatrix4Array(XElement xe, string paramName, IList<Matrix4x4> value);
	XElement WriteNumber<T>(XElement xe, string paramName, T value) where T : IBinaryInteger<T>;
	XElement WriteNumberArray<T>(XElement xe, string paramName, IList<T> value) where T : IBinaryInteger<T>;
	XElement WriteObject(XElement xe, string paramName, XElement value);
	XElement WriteQSTransform(XElement xe, string paramName, Matrix4x4 value);
	XElement WriteQSTransformArray(XElement xe, string paramName, IList<Matrix4x4> value);
	XElement WriteQuaternion(XElement xe, string paramName, Quaternion value);
	XElement WriteQuaternionArray(XElement xe, string paramName, IList<Quaternion> value);
	XElement WriteRotation(XElement xe, string paramName, Matrix4x4 value);
	XElement WriteRotationArray(XElement xe, string paramName, IList<Matrix4x4> value);
	void WriteSerializeIgnored(XElement xe, string prop);
	XElement WriteString(XElement xe, string paramName, string? value);
	XElement WriteStringArray(XElement xe, string paramName, IList<string> values);
	XElement WriteTransform(XElement xe, string paramName, Matrix4x4 value);
	XElement WriteTransformArray(XElement xe, string paramName, IList<Matrix4x4> value);
	XElement WriteVector4(XElement xe, string paramName, Vector4 value);
	XElement WriteVector4Array(XElement xe, string paramName, IList<Vector4> value);
}