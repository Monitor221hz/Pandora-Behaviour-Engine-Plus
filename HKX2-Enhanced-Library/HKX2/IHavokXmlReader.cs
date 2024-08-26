using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E;
public interface IHavokXmlReader
{
	bool ReadBoolean(XElement element, string name);
	IList<bool> ReadBooleanArray(XElement element, string name);
	bool[] ReadBooleanCStyleArray(XElement element, string name, short length);
	byte ReadByte(XElement element, string name);
	IList<byte> ReadByteArray(XElement element, string name);
	byte[] ReadByteCStyleArray(XElement element, string name, short length);
	T ReadClass<T>(XElement element, string name) where T : IHavokObject, new();
	IList<T> ReadClassArray<T>(XElement element, string name) where T : IHavokObject, new();
	T[] ReadClassCStyleArray<T>(XElement element, string name, short length) where T : IHavokObject, new();
	T? ReadClassPointer<T>(IHavokObject owner, XElement element, string name) where T : IHavokObject, new();
	IList<T> ReadClassPointerArray<T>(IHavokObject owner, XElement element, string name) where T : IHavokObject, new();
	T?[] ReadClassPointerCStyleArray<T>(IHavokObject owner, XElement element, string name, short length) where T : IHavokObject, new();
	TValue ReadEnum<TEnum, TValue>(XElement element, string name)
		where TEnum : Enum
		where TValue : IBinaryInteger<TValue>;
	TValue ReadFlag<TEnum, TValue>(XElement element, string name)
		where TEnum : Enum
		where TValue : IBinaryInteger<TValue>;
	Half ReadHalf(XElement element, string name);
	IList<Half> ReadHalfArray(XElement element, string name);
	Half[] ReadHalfCStyleArray(XElement element, string name, short length);
	short ReadInt16(XElement element, string name);
	IList<short> ReadInt16Array(XElement element, string name);
	short[] ReadInt16CStyleArray(XElement element, string name, short length);
	int ReadInt32(XElement element, string name);
	IList<int> ReadInt32Array(XElement element, string name);
	int[] ReadInt32CStyleArray(XElement element, string name, short length);
	long ReadInt64(XElement element, string name);
	IList<long> ReadInt64Array(XElement element, string name);
	long[] ReadInt64CStyleArray(XElement element, string name, short length);
	Matrix4x4 ReadMatrix3(XElement element, string name);
	IList<Matrix4x4> ReadMatrix3Array(XElement element, string name);
	Matrix4x4[] ReadMatrix3CStyleArray(XElement element, string name, short length);
	Matrix4x4 ReadMatrix4(XElement element, string name);
	IList<Matrix4x4> ReadMatrix4Array(XElement element, string name);
	Matrix4x4[] ReadMatrix4CStyleArray(XElement element, string name, short length);
	Matrix4x4 ReadQSTransform(XElement element, string name);
	IList<Matrix4x4> ReadQSTransformArray(XElement element, string name);
	Matrix4x4[] ReadQSTransformCStyleArray(XElement element, string name, short length);
	Quaternion ReadQuaternion(XElement element, string name);
	IList<Quaternion> ReadQuaternionArray(XElement element, string name);
	Quaternion[] ReadQuaternionCStyleArray(XElement element, string name, short length);
	Matrix4x4 ReadRotation(XElement element, string name);
	IList<Matrix4x4> ReadRotationArray(XElement element, string name);
	Matrix4x4[] ReadRotationCStyleArray(XElement element, string name, short length);
	sbyte ReadSByte(XElement element, string name);
	IList<sbyte> ReadSByteArray(XElement element, string name);
	sbyte[] ReadSByteCStyleArray(XElement element, string name, short length);
	float ReadSingle(XElement element, string name);
	IList<float> ReadSingleArray(XElement element, string name);
	float[] ReadSingleCStyleArray(XElement element, string name, short length);
	string ReadString(XElement element, string name);
	IList<string> ReadStringArray(XElement element, string name);
	Matrix4x4 ReadTransform(XElement element, string name);
	IList<Matrix4x4> ReadTransformArray(XElement element, string name);
	Matrix4x4[] ReadTransformCStyleArray(XElement element, string name, short length);
	ushort ReadUInt16(XElement element, string name);
	IList<ushort> ReadUInt16Array(XElement element, string name);
	ushort[] ReadUInt16CStyleArray(XElement element, string name, short length);
	uint ReadUInt32(XElement element, string name);
	IList<uint> ReadUInt32Array(XElement element, string name);
	uint[] ReadUInt32CStyleArray(XElement element, string name, short length);
	ulong ReadUInt64(XElement element, string name);
	IList<ulong> ReadUInt64Array(XElement element, string name);
	ulong[] ReadUInt64CStyleArray(XElement element, string name, short length);
	Vector4 ReadVector4(XElement element, string name);
	IList<Vector4> ReadVector4Array(XElement element, string name);
	Vector4[] ReadVector4CStyleArray(XElement element, string name, short length);
}