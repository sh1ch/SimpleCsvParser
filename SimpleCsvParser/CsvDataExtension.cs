using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Heritage.IO;

/// <summary>
/// <see cref="CsvFormatExtension"/> クラスは、データを様々な <see cref="string"/> 形式に変換するためのクラスです。
/// </summary>
public static class CsvFormatExtension
{
    public static string Join(this IEnumerable<string> data, bool hasNewLine = true)
    {
        var text = string.Join(", ", data);

        if (hasNewLine)
        {
            text += Environment.NewLine;
        }

        return text;
    }

    public static string Escape(this string value, char escapeChar = '"')
    {
        if (string.IsNullOrEmpty(value))
        {
            return $"{escapeChar}{escapeChar}";
        }

        string temp = value;

        if (escapeChar == '"') 
        {
            temp = value.Replace($"{escapeChar}", $"{escapeChar}{escapeChar}");
        }

        return $"{escapeChar}{temp}{escapeChar}";
    }

    public static string ToText(this int? value, Func<int?, bool> predicate, string replace = "", string format = "")
    {
        if (value == null) return replace;
        if (!predicate(value)) return replace;

        return ToText((int)value, format);
    }

    public static string ToText(this int? value, string replace = "", string format = "")
    {
        if (value == null) return replace;

        return ToText((int)value, format);
    }

    public static string ToText(this int value, Func<int, bool> predicate, string replace = "", string format = "")
    {
        if (!predicate(value)) return replace;

        return ToText(value, format);
    }

    public static string ToText(this int value, string format = "")
    {
        return value.ToString(format);
    }

    public static string ToText(this double? value, Func<double?, bool> predicate, string replace = "", string format = "")
    {
        if (value == null) return replace;
        if (!predicate(value)) return replace;

        return ToText((double)value, format);
    }

    public static string ToText(this double? value, string replace = "", string format = "")
    {
        if (value == null) return replace;

        return ToText((double)value, format);
    }

    public static string ToText(this double value, Func<double, bool> predicate, string replace = "", string format = "")
    {
        if (!predicate(value)) return replace;

        return ToText(value, format);
    }

    public static string ToText(this double value, string format = "")
    {
        return value.ToString(format);
    }

    public static string ToText(this double? value, Func<double?, bool> predicate, int decimals, string replace = "", MidpointRounding rounding = MidpointRounding.AwayFromZero)
    {
        if (value == null) return replace;
        if (!predicate(value)) return replace;

        return ToText((double)value, decimals, rounding);
    }

    public static string ToText(this double? value, int decimals, string replace = "", MidpointRounding rounding = MidpointRounding.AwayFromZero)
    {
        if (value == null) return replace;

        return ToText((double)value, decimals, rounding);
    }

    public static string ToText(this double value, Func<double?, bool> predicate, int decimals, string replace = "", MidpointRounding rounding = MidpointRounding.AwayFromZero)
    {
        if (!predicate(value)) return replace;

        return ToText(value, decimals, rounding);
    }

    public static string ToText(this double value, int decimals, MidpointRounding rounding = MidpointRounding.AwayFromZero)
    {
        var d = new string('0', decimals);
        var temp = Math.Round(value, decimals, rounding);

        return string.Format("{0:0." + d + "}", temp);
    }
}
