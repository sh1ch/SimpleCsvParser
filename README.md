[![.NET Tests](https://github.com/sh1ch/SimpleCsvParser/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/sh1ch/SimpleCsvParser/actions/workflows/dotnet-desktop.yml)
[![CodeFactor](https://www.codefactor.io/repository/github/sh1ch/simplecsvparser/badge)](https://www.codefactor.io/repository/github/sh1ch/simplecsvparser)

# Simple CSV Parser

Simple CSV Parser for C#, Suitable for Unity.

Based on [CSV-Parser](https://github.com/yutokun/CSV-Parser).


# Table of Contents

* [Methods](#Methods)
* [Usage](#Usage)
* [Compliant](#Compliant)
* [License](#License)


## Methods

```cs
using System.IO;

IEnumerable<IEnumerable<string>> ParseFromFile(string filePath, Delimiter delimiter = Delimiter.Comma, Encoding encoding = null);
IEnumerable<IEnumerable<string>> ParseFromText(string text, Delimiter delimiter = Delimiter.Comma);
IEnumerable<string> ParseFieldsFromText(string text, Delimiter delimiter = Delimiter.Comma);
```


## Usage

sample1 code

```cs
var fields = CsvParser.ParseFieldsFromText("aaa,bbb,ccc");

foreach (var field in fields)
{
    Console.WriteLine(field);
}
```

data

```txt
aaa
bbb
ccc
```

**A** CSV record parsed to CSV fields.

***

sample2 code

```cs
var fields = CsvParser.ParseFromText("aaa,bbb,ccc\r\n111,222,333");

foreach (var record in records)
{
    foreach (var field in record)
    {
        Console.WriteLine(field);
    }
}
```

data

```txt
aaa
bbb
ccc
111
222
333
```

CSV record**s** parsed to CSV fields. (sample has records.Count() = 2.)

***

sample3 code

```cs
var records = CsvParser.ParseFromText("aaa, bbb, ccc\r\n111,, 333\r\nAAA, \"BBB\"");

foreach (var record in records)
{
    Console.WriteLine($"record has {record.Count()} fields.");

    foreach (var field in record)
    {
        Console.WriteLine(field);
    }
}
```

data

```txt
record has 3 fields.
aaa
 bbb
 ccc
record has 3 fields.
111

 333
record has 2 fields.
AAA
BBB
```

` BBB`, ` CCC` and ` 333` have **halfwidth space**. but, `"BBB"` has not halfwidth space.

***

sample4 code

```cs
var records = CsvParser.ParseFromText("aaa,bbb,ccc\r\n111,222,\r\n,\"\"\"bbb\",");

foreach (var record in records)
{
    Console.WriteLine($"record has {record.Count()} fields.");

    foreach (var field in record)
    {
        Console.WriteLine(field);
    }
}

```

data

```txt
record has 3 fields.
aaa
bbb
ccc
record has 3 fields.
111
222

record has 3 fields.

"BBB

```

If double-quotes are used to enclose fields, then a double-quote appearing inside a field must be escaped by preceding it with another double quote.

***

sample5 code

```cs
var path = "..."; // System.AppDomain.CurrentDomain.BaseDirectory;
var data = CsvParser.ParseFromFile(System.IO.Path.Combine(path, "sample.csv"));

foreach (var record in records)
{
    foreach (var field in record)
    {
        Console.WriteLine("do something...");
    }
}

```

<img src="https://github.com/sh1ch/SimpleCsvParser/blob/images/txt-sample.png">
<img src="https://github.com/sh1ch/SimpleCsvParser/blob/images/txt-result.png">

This is an example of directly reading CSV file. After reading, operate in the same way as `ParseFromText`.


## Compliant

- [RFC 4180](http://www.ietf.org/rfc/rfc4180.txt).

> ignores CSV Header (Treat as record)


## License

[CC0](https://creativecommons.org/publicdomain/zero/1.0/) (LICENSE)
