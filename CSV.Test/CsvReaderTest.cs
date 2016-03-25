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
                foreach (var row in reader.ReadRows())
                    index++;

                Assert.AreEqual(10698 - 1, index);
            }
        }

        [TestMethod]
        public void TestCsvReader()
        {
            string[][] refTable;
            string tempFilePath;
            InitTestData(out refTable, out tempFilePath);

            // test csv file
            using (var reader = new CsvReader(tempFilePath, Encoding.UTF8, '\n', ',', 1))
            {
                TestCsvReader(refTable, reader);
            }
        }

        [TestMethod]
        public void TestStreamReader()
        {
            string[][] refTable;
            string tempFilePath;
            InitTestData(out refTable, out tempFilePath);

            // test csv file
            using (var reader = new StreamReader(tempFilePath, Encoding.UTF8))
            {
                TestStreamReader(refTable, reader, '\n', ',', 1);
            }
        }

        #region Helper

        private static void TestCsvReader(string[][] refTable, CsvReader reader)
        {
            int rowIndex = 0;
            TestHeader(refTable, reader, rowIndex);

            foreach (var row in reader.ReadRows())
            {
                rowIndex++;

                Assert.AreEqual(refTable[rowIndex].Length, row.Length);

                for (int colIndex = 0; colIndex < refTable[rowIndex][colIndex].Length; colIndex++)
                {
                    Assert.AreEqual(refTable[rowIndex][colIndex], row[colIndex]);
                }
            }
        }

        private static void TestHeader(string[][] refTable, CsvReader reader, int rowIndex)
        {
            Assert.AreEqual(refTable[rowIndex].Length, reader.Header.Length);

            for (int colIndex = 0; colIndex < refTable[rowIndex][colIndex].Length; colIndex++)
            {
                Assert.AreEqual(refTable[rowIndex][colIndex], reader.Header[colIndex]);
            }
        }

        private static void TestStreamReader(string[][] refTable, StreamReader reader, char rowDelimeter, char columnDelimeter, int headerRowCount)
        {
            int rowIndex = 0;

            foreach (var row in reader.ReadRows(rowDelimeter, columnDelimeter, headerRowCount))
            {
                rowIndex++;

                Assert.AreEqual(refTable[rowIndex].Length, row.Length);

                for (int colIndex = 0; colIndex < refTable[rowIndex][colIndex].Length; colIndex++)
                {
                    Assert.AreEqual(refTable[rowIndex][colIndex], row[colIndex]);
                }
            }
        }

        private static void InitTestData(out string[][] refTable, out string tempFilePath)
        {
            refTable = new string[][] {
                new string[] { "A", "B" },
                new string[] { "1", "2" },
                new string[] { "3", "4" },
                new string[] { "5", "6" }
            };

            // write test file
            tempFilePath = Path.GetTempFileName();
            using (var file = File.CreateText(tempFilePath))
            {
                foreach (var row in refTable)
                {
                    file.Write(string.Join(",", row) + "\n");
                }
            }
        }

        #endregion Helper
    }
}