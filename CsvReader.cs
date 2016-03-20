
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSV
{
    public class CsvReader : IDisposable
    {
        StreamReader Reader;
        char RowDelimeter;
        char ColumnDelimeter;
        int SkipRowCount;

        public string[] Header { get; private set; }

        public IEnumerable<string[]> Rows
        {
            get
            {
                Reader.BaseStream.Position = 0;
                return Reader.ReadRows(RowDelimeter, ColumnDelimeter, SkipRowCount);
            }
        }
        public CsvReader(string path, Encoding encoding, char rowDelimeter, char columnDelimeter, int skipRowCount)
        {
            RowDelimeter = rowDelimeter;
            ColumnDelimeter = columnDelimeter;
            SkipRowCount = skipRowCount;

            Reader = new StreamReader(path, encoding);
            if (skipRowCount > 0)
                Header = Reader.ReadLine(rowDelimeter).Split(columnDelimeter);
        }
        public void Dispose()
        {
            Reader.Close();
        }
    }

    public static class ExtensionsForTextReader
    {
        public static string ReadLine(this TextReader reader, char delimeter)
        {
            List<char> chars = new List<char>();
            while (reader.Peek() >= 0)
            {
                char c = (char)reader.Read();

                if (c == delimeter)
                    return new String(chars.ToArray());

                chars.Add(c);
            }
            return null;
        }
        public static IEnumerable<string> ReadLines(this TextReader reader, char delimeter)
        {
            List<char> chars = new List<char>();
            while (reader.Peek() >= 0)
            {
                char c = (char)reader.Read();

                if (c == delimeter)
                {
                    yield return new String(chars.ToArray());
                    chars.Clear();
                    continue;
                }

                chars.Add(c);
            }
        }

        public static IEnumerable<string[]> ReadRows(this TextReader reader, char rowDelimeter, char columnDelimeter, int skipRowCount = 0)
        {
            foreach (var line in reader.ReadLines(rowDelimeter))
            {
                if (skipRowCount-- > 0) continue;
                yield return line.Split(columnDelimeter);
                continue;
            }
        }
    }
}
