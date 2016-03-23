# csv-dotnet
[![Build Status](https://travis-ci.org/abbgrade/blob_types.svg?branch=master)](https://travis-ci.org/abbgrade/blob_types)

A simple .NET Library for CSV-File access.
It is inspired by the Python csv module.

## Changelog 

- [x] CSV Reader
- [ ] CSV Writer

## Usage

### StreamReader Extension Example

Import the CSV library to extend the StreamReader class.

	use CSV;

Use the StreamReader.ReadRows enumerator to get the rows.

	using (var reader = new StreamReader("logfile.csv", Encoding.Default))
	{
		foreach (var line in reader.ReadRows('\n', ',', 1))
		{
			Console.WriteLine(string.Join("\t", line));
		}
	}

### CsvReader Example

Import the CSV library to get access to the CsvReader class.

	use CSV;

Use the CsvReader to get stateful access to the CSV file.

    using (var reader = new CsvReader("logfile.csv", Encoding.Default, '\n', ',', 1))
    {
        Console.WriteLine(string.Join("\t", reader.Header));

        foreach(var row in reader.Rows)
            Console.WriteLine(string.Join("\t", row));
    }