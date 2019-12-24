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

            using (var stream = new StringWriter())
            using (var writer = new CsvWriter(stream))
            {
                ToCsv(writer, table);

                return stream.ToString();
            }
        }

        public static void ToCsv(CsvWriter writer, DataTable table)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                DataColumn column = table.Columns[i];

                writer.WriteField(column.ColumnName);
            }

            writer.NextRecord();

            for (int r = 0; r < table.Rows.Count; r++)
            {
                DataRow row = table.Rows[r];

                for (int c = 0; c < table.Columns.Count; c++)
                {
                    object value = row[c];

                    writer.WriteField(value);
                }

                writer.NextRecord();
            }
        }

        public static DataTable FromCsv(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or empty.", nameof(text));

            using (var stream = new StringReader(text))
            using (var reader = new CsvReader(stream))
            {
                var table = new DataTable();

                FromCsv(reader, table);

                return table;
            }
        }

        public static void FromCsv(CsvReader reader, DataTable table)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            using (var dataReader = new CsvDataReader(reader))
            {
                table.Load(dataReader);
            }
        }
    }
}
