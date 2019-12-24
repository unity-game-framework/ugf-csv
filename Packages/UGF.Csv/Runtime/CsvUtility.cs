using System;
using System.Data;
using System.IO;
using CsvHelper;

namespace UGF.Csv.Runtime
{
    public static class CsvUtility
    {
        public static string ToCsv(DataTable table)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer))
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    DataColumn column = table.Columns[i];

                    csv.WriteField(column.ColumnName);
                }

                csv.NextRecord();

                for (int r = 0; r < table.Rows.Count; r++)
                {
                    DataRow row = table.Rows[r];

                    for (int c = 0; c < table.Columns.Count; c++)
                    {
                        object value = row[c];

                        csv.WriteField(value);
                    }

                    csv.NextRecord();
                }

                return writer.ToString();
            }
        }

        public static DataTable FromCsv(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or empty.", nameof(text));

            using (var reader = new StringReader(text))
            using (var csv = new CsvReader(reader))
            using (var dataReader = new CsvDataReader(csv))
            {
                var table = new DataTable();

                table.Load(dataReader);

                return table;
            }
        }
    }
}
