using System;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2E
{
    public static class FlagUtils
    {
        public static TValue ToEnumValue<TEnum, TValue>(this TEnum enumType) where TEnum : Enum where TValue : IBinaryInteger<TValue>
        {
            return (TValue)(IConvertible)enumType;
        }
        public static TValue ToEnumValue<TEnum, TValue>(this string value) where TEnum : Enum where TValue : IBinaryInteger<TValue>
        {
            if (Enum.TryParse(typeof(TEnum), value, out var val))
            {
                return (TValue)val;
            }

            // failed return convert to number
            if (TValue.TryParse(value, null, out var retVal))
            {
                return retVal;
            }

            // check is Hex format
            if (value.StartsWith("0x"))
            {
                value = value[2..];
                TValue.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out var hexVal);
                if (hexVal != null)
                    return hexVal;
            }
            throw new KeyNotFoundException($"{typeof(TEnum)} Enum dose not contain {value} Value");
        }

        public static TEnum ToEnum<TEnum, TValue>(this TValue value) where TEnum : Enum where TValue : IBinaryInteger<TValue>
        {
            return (TEnum)(IConvertible)value;
        }

        public static string ToEnumName<TEnum, TValue>(this TValue value) where TEnum : Enum where TValue : IBinaryInteger<TValue>
        {
            var ret = Enum.GetName(typeof(TEnum), value);
            if (ret is not null)
                return ret;

            return value.ToString() ?? "";
        }

        public static string ToFlagString<TEnum, TValue>(this TValue value) where TEnum : Enum where TValue : IBinaryInteger<TValue>
        {
            var result = new List<string>();
            var enums = (TEnum[])Enum.GetValues(typeof(TEnum));
            for (var i = enums.Length - 1; i >= 0; i--)
            {
                if (enums[i].ToEnumValue<TEnum, TValue>() == TValue.Zero)
                    break;

                if ((enums[i].ToEnumValue<TEnum, TValue>() & value) != enums[i].ToEnumValue<TEnum, TValue>())
                    continue;

                result.Add(enums[i].ToString());
                value &= ~enums[i].ToEnumValue<TEnum, TValue>();
                if (value == TValue.Zero)
                {
                    break;
                }
            }
            if (value > TValue.Zero)
                result.Add(value.ToString()!);
            else if (value == TValue.Zero && result.Count == 0)
                result.Add(value.ToEnumName<TEnum, TValue>());

            return string.Join("|", result);
        }

        public static TValue ToFlagValue<TEnum, TValue>(this string value) where TEnum : Enum where TValue : IBinaryInteger<TValue>
        {
            var splited = value.Split("|", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return splited.ToFlagValue<TEnum, TValue>();
        }

        public static TValue ToFlagValue<TEnum, TValue>(this IEnumerable<string> value) where TEnum : Enum where TValue : IBinaryInteger<TValue>
        {
            var result = TValue.Zero;
            foreach (var item in value)
            {
                result |= item.ToEnumValue<TEnum, TValue>();
            }
            return result;
        }
    }
}
