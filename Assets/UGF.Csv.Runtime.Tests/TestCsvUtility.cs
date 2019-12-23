using System.Data;
using NUnit.Framework;
using UnityEngine;

namespace UGF.Csv.Runtime.Tests
{
    public class TestCsvUtility
    {
        [Test]
        public void ToCsv()
        {
            var table = new DataTable();

            for (int i = 0; i < 4; i++)
            {
                table.Columns.Add(new DataColumn());
            }

            for (int r = 0; r < 2; r++)
            {
                DataRow row = table.NewRow();

                row[0] = "0";
                row[1] = "1";
                row[2] = " 2";
                row[3] = "\"3\"";

                table.Rows.Add(row);
            }

            string csv = CsvUtility.ToCsv(table);
            string expected = Resources.Load<TextAsset>("table0").text;

            Assert.NotNull(csv);
            Assert.AreEqual(expected, csv);
        }

        [Test]
        public void FromCsv()
        {
            string csv = Resources.Load<TextAsset>("table1").text;
            DataTable table = CsvUtility.FromCsv(csv);

            Assert.AreEqual(4, table.Columns.Count);
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual("0", table.Rows[0][0]);
            Assert.AreEqual("1", table.Rows[0][1]);
            Assert.AreEqual(" 2", table.Rows[0][2]);
            Assert.AreEqual("\"3\"", table.Rows[0][3]);
            Assert.AreEqual("0", table.Rows[1][0]);
            Assert.AreEqual("1", table.Rows[1][1]);
            Assert.AreEqual(" 2", table.Rows[1][2]);
            Assert.AreEqual("\"3\"", table.Rows[1][3]);
        }
    }
}
