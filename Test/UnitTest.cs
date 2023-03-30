using Heritage.IO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test;

public class UnitTest
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("", 0)]
    [TestCase(" ", 1)]
    [TestCase("日本国,東京,127767944 \r\nアメリカ合衆国, ワシントン, 300007997 \r\n", 2)]
    [TestCase("日本国,東京, \r\n", 1)]
    [TestCase("\"日本 \r\n国\",\"\"\"東京\"\"\",\"127,767,944\"", 1)]
    [TestCase("\"日本 \r\n国\",\"\"\"東京\"\"\",\"127,767,944\" \r\n", 1)]
    [TestCase("\"日本 \r\n国\",\"\"\"東京\"\"\",\"127,767,944\" \r\n\r\n", 2)]
    [TestCase("\"日本 \r\n国\",\"\"\"東京\"\"\",\"127,767,944\" \r\n\n", 2)]
    [TestCase("\"aaa\",\"bbb\",\"ccc\" \r\nzzz, yyy, xxx", 2)]
    [TestCase("\"aaa\",\"b \r\nbb\",\"ccc\" \r\nzzz, yyy, xxx", 2)]
    [TestCase("\"aaa\",\"b\"\"bb\",\"ccc\"", 1)]
    public void CSV_レコードの行数テスト(string text, int count)
    {
        var data = CsvParser.ParseFromText(text);

        Assert.That(count, Is.EqualTo(data.Count()));
    }

    [TestCase("", 0)]
    [TestCase(" ", 1)]
    [TestCase("日本国,東京,127767944 ", 3)]
    [TestCase("日本国,東京, ", 3)]
    [TestCase("\"日本 \r\n国\",\"\"\"東京\"\"\",\"127,767,944\"", 3)]
    [TestCase("\"aaa\",\"bbb\",\"ccc\"", 3)]
    [TestCase("\"aaa\",\"b \r\nbb\",\"ccc\"", 3)]
    [TestCase("\"\",\"b\"\"bb\",\"\"", 3)]
    public void CSV_行のフィールド数テスト(string text, int count)
    {
        var expect = CsvParser.ParseFieldsFromText(text);

        Assert.That(expect.Count(), Is.EqualTo(count));
    }

    [TestCase("", null)]
    [TestCase(" ", new string[] { " " })]
    [TestCase("日本国,東京,127767944 ", new string[] { "日本国", "東京", "127767944 " })]
    [TestCase("日本国,東京, ", new string[] { "日本国", "東京", " " })]
    [TestCase("日本国,東京,", new string[] { "日本国", "東京", "" })]
    [TestCase("\"日本 \r\n国\",\"\"\"東京\"\"\",\"127,767,944\"", new string[] { "日本 \r\n国", "\"東京\"", "127,767,944" })]
    [TestCase("\"aaa\",\"bbb\",\"ccc\"", new string[] { "aaa", "bbb", "ccc" })]
    [TestCase("\"aaa\",\"b \r\nbb\",\"ccc\"", new string[] { "aaa", "b \r\nbb", "ccc" })]
    [TestCase("\"\",\"b\"\"bb\",\"\"", new string[] { "", "b\"bb", "" })]
    [TestCase("AAA, \"BBB\"", new string[] { "AAA", "BBB" })]
    public void CSV_行のフィールドデータの一致テスト(string text, IEnumerable<string> actual)
    {
        var data = CsvParser.ParseFromText(text);
        var expect = data.FirstOrDefault();

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase("sample.csv", 12)]
    public void Csv_ファイルの読み込みテスト(string fileName, int count)
    {
        var path = System.AppDomain.CurrentDomain.BaseDirectory;
        var data = CsvParser.ParseFromFile(System.IO.Path.Combine(path, fileName));

        Assert.That(data?.Count() ?? 0, Is.EqualTo(count));
    }

    [TestCase(new int[] { 1, 2, 3, 4}, "1, 2, 3, 4")]
    public void Extension_Join(int[] data, string actual)
    {
        var textData = data.Select(p => p.ToString());
        var expect = textData.Join(", ", hasNewLine: false);

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase("testdata", "\"testdata\"")]
    [TestCase("a \"b\" c", "\"a \"\"b\"\" c\"")]
    public void Extension_Excape(string value, string actual)
    {
        var expect = value.Escape();

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase(100, "", "100")]
    [TestCase(20000, "#,0", "20,000")]
    public void Extension_ToText(int value, string format, string actual)
    {
        var expect = value.ToText(format);

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase(20000, "#,0", "", "20,000")]
    [TestCase(-20000, "#,0", "x", "x")]
    public void Extension_ToText(int value, string format, string replace, string actual)
    {
        var expect = value.ToText((val) => val > 0, replace:replace, format: format);

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase(null, "", "null", "null")]
    [TestCase(20000, "#,0", "", "20,000")]
    [TestCase(-20000, "#,0", "x", "x")]
    public void Extension_ToText(int? value, string format, string replace, string actual)
    {
        var expect = value.ToText((val) => val > 0, replace: replace, format: format);

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase(100.001, "", "100.001")]
    [TestCase(20000.12, "#,0.0000", "20,000.1200")]
    public void Extension_ToText(double value, string format, string actual)
    {
        var expect = value.ToText(format);

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase(20000.12, "#,0.0000", "", "20,000.1200")]
    [TestCase(-20000.12, "#,0.0000", "xx", "xx")]

    public void Extension_ToText(double value, string format, string replace, string actual)
    {
        var expect = value.ToText((val) => val > 0, replace: replace, format: format);

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase(null, "", "null", "null")]
    [TestCase(20000.12, "#,0.0000", "", "20,000.1200")]
    [TestCase(-20000.12, "#,0.0000", "xx", "xx")]
    public void Extension_ToText(double? value, string format, string replace, string actual)
    {
        var expect = value.ToText((val) => val > 0, replace: replace, format: format);

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase(100.123456, 3, "100.123")]
    [TestCase(200.123456, 4, "200.1235")]
    public void Extension_ToText(double value, int decimals, string actual)
    {
        var expect = value.ToText(decimals);

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase(100.012345, 1, "low", "low")]
    [TestCase(200.123456, 4, "low", "200.1235")]
    public void Extension_ToText(double value, int decimals, string replace, string actual)
    {
        var expect = value.ToText((val) => val > 100.1, decimals, replace:replace);

        Assert.That(expect, Is.EqualTo(actual));
    }

    [TestCase(null, 1, "null", "null")]
    [TestCase(100.012345, 1, "low", "low")]
    [TestCase(200.123456, 4, "low", "200.1235")]
    public void Extension_ToText(double? value, int decimals, string replace, string actual)
    {
        var expect = value.ToText((val) => val > 100.1, decimals, replace: replace);

        Assert.That(expect, Is.EqualTo(actual));
    }
}
