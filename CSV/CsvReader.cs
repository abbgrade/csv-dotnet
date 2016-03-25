using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSV
{
    public class CsvReader : IDisposable
    {
        private StreamReader Reader;
        private char RowDelimeter;
        private char ColumnDelimeter;
        private int HeaderRowCount;

        public string[] Header { get; private set; }

        public CsvReader(string path, Encoding encoding, char rowDelimeter, char columnDelimeter, int headerRowCount)
        {
            RowDelimeter = rowDelimeter;
            ColumnDelimeter = columnDelimeter;
            HeaderRowCount = headerRowCount;

            Reader = new StreamReader(path, encoding);
            if (headerRowCount > 0)
            {
                Header = Reader.ReadLine(rowDelimeter).Split(columnDelimeter);
                HeaderRowCount--;
            }
        }

        public CsvReader(StreamReader reader, char rowDelimeter, char columnDelimeter, int headerRowCount)
        {
            Reader = reader;
            RowDelimeter = rowDelimeter;
            ColumnDelimeter = columnDelimeter;
            HeaderRowCount = headerRowCount;
            
            if (headerRowCount > 0)
            {
                Header = Reader.ReadLine(rowDelimeter).Split(columnDelimeter);
                HeaderRowCount--;
            }
        }

        public void Dispose()
        {
            Reader.Close();
        }

        public IEnumerable<string[]> GetRows()
        {
            return Reader.ReadRows(RowDelimeter, ColumnDelimeter, HeaderRowCount);
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
                if (skipRowCount > 0)
                {
                    skipRowCount--;
                    continue;
                }

                yield return line.Split(columnDelimeter);
            }
        }
    }
}