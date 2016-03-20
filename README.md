# csv-dotnet
A simple .NET Library for CSV-File access.

# Usage

## StreamReader Extension Example

Import the CSV library to extend the StreamReader Class.

	use CSV;

Use the StreamReader.ReadRows enumerator to get the rows.

	using (var reader = new StreamReader("logfile.csv", Encoding.Default))
	{
		foreach (var line in reader.ReadRows('\n', ',', 1))
		{
			Console.WriteLine(string.Join("\t", line));
		}
	}