using System;
using System.Data;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace UGF.Csv.Runtime
{
    public static class CsvUtility
    {
        public static CsvConfiguration DefaultConfiguration { get; } = new CsvConfiguration(CultureInfo.InvariantCulture);

        public static string ToCsv(DataTable table)
        {
            return ToCsv(table, DefaultConfiguration);
        }

        public static string ToCsv(DataTable table, CsvConfiguration configuration)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            using (var stream = new StringWriter())
            using (var writer = new CsvWriter(stream, configuration))
            {
                ToCsv(writer, table);

                return stream.ToString();
            }
        }

        public static void ToCsv(CsvWriter writer, DataTable table)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (table == null) throw new ArgumentNullException(nameof(table));

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
            return FromCsv(text, DefaultConfiguration);
        }

        public static DataTable FromCsv(string text, CsvConfiguration configuration)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            using (var stream = new StringReader(text))
            using (var reader = new CsvReader(stream, configuration))
            {
                var table = new DataTable();

                FromCsv(reader, table);

                return table;
            }
        }

        public static void FromCsv(CsvReader reader, DataTable table)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (table == null) throw new ArgumentNullException(nameof(table));

            using (var dataReader = new CsvDataReader(reader))
            {
                table.Load(dataReader);
            }
        }

        public static string MergeCsv(string csv, string csvToMerge)
        {
            if (string.IsNullOrEmpty(csv)) throw new ArgumentException("Value cannot be null or empty.", nameof(csv));
            if (string.IsNullOrEmpty(csvToMerge)) throw new ArgumentException("Value cannot be null or empty.", nameof(csvToMerge));

            DataTable table = FromCsv(csv);
            DataTable merge = FromCsv(csvToMerge);

            table.Merge(merge, true, MissingSchemaAction.Add);

            string result = ToCsv(table);

            return result;
        }

        public static DataTable MergeCsv(DataTable table, DataTable tableToMerge)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (tableToMerge == null) throw new ArgumentNullException(nameof(tableToMerge));

            DataTable result = table.Copy();

            result.Merge(tableToMerge, true, MissingSchemaAction.Add);

            return result;
        }
    }
}
