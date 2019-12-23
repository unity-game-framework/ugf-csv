using System;
using System.Data;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace UGF.Csv.Runtime
{
    public static class CsvUtility
    {
        public static string ToCsv(DataTable table)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            var configuration = new Configuration
            {
                HasHeaderRecord = false
            };

            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, configuration))
            {
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

            var configuration = new Configuration
            {
                HasHeaderRecord = false
            };

            using (var reader = new StringReader(text))
            using (var csv = new CsvReader(reader, configuration))
            using (var dataReader = new CsvDataReader(csv))
            {
                var table = new DataTable();

                table.Load(dataReader);

                return table;
            }
        }
    }
}
