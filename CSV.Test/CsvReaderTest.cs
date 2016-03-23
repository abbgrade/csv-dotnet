using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace CSV.Test
{
    [TestClass]
    public class CsvReaderTest
    {
        [TestMethod]
        public void TestExternalFile()
        {
            using (var reader = new CsvReader(
                    @"..\..\TestData\Test.csv",
                    Encoding.Default,
                    '\n',
                    ',',
                    0
                ))
            {
                int index = 0;
                foreach (var row in reader.GetRows())
                    index++;

                Assert.AreEqual(10698 - 1, index);
            }
        }

        [TestMethod]
        public void TestCsvReader()
        {
            var refTable = new string[][] {
                new string[] { "A", "B" },
                new string[] { "1", "2" },
                new string[] { "3", "4" },
                new string[] { "5", "6" }
            };

            // write test file
            var tempFilePath = Path.GetTempFileName();
            using (var file = File.CreateText(tempFilePath))
            {
                foreach (var row in refTable)
                {
                    file.Write(string.Join(",", row) + "\n");
                }
            }

            // test csv file
            using (var reader = new CsvReader(tempFilePath, Encoding.UTF8, '\n', ',', 1))
            {
                int rowIndex = 0;

                Assert.AreEqual(refTable[rowIndex].Length, reader.Header.Length);

                for (int colIndex = 0; colIndex < refTable[rowIndex][colIndex].Length; colIndex++)
                {
                    Assert.AreEqual(refTable[rowIndex][colIndex], reader.Header[colIndex]);
                }

                foreach (var row in reader.GetRows())
                {
                    rowIndex++;

                    Assert.AreEqual(refTable[rowIndex].Length, row.Length);

                    for (int colIndex = 0; colIndex < refTable[rowIndex][colIndex].Length; colIndex++)
                    {
                        Assert.AreEqual(refTable[rowIndex][colIndex], row[colIndex]);
                    }
                }
            }
        }
    }
}