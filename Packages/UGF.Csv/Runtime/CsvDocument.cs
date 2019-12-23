using System;
using System.Collections.Generic;

namespace UGF.Csv.Runtime
{
    public class CsvDocument
    {
        public int ColumnCount { get { return m_elements.Count; } }
        public int RowCount { get { return m_elements.Count > 0 ? m_elements[0].Count : 0; } }

        private readonly List<List<CsvElement>> m_elements = new List<List<CsvElement>>();

        public CsvDocument()
        {
        }

        public CsvDocument(int columnCount, int rowCount)
        {
            if (columnCount < 0) throw new ArgumentOutOfRangeException(nameof(columnCount));
            if (rowCount < 0) throw new ArgumentOutOfRangeException(nameof(rowCount));

            for (int i = 0; i < columnCount; i++)
            {
                List<CsvElement> column = CreateColumn(rowCount);

                m_elements.Add(column);
            }
        }

        public void AddColumn()
        {
            InsertColumn(ColumnCount);
        }

        public void AddRow()
        {
            InsertRow(RowCount);
        }

        public void InsertColumn(int index)
        {
            if (index < 0 || index > ColumnCount) throw new ArgumentOutOfRangeException(nameof(index));

            List<CsvElement> column = CreateColumn(RowCount);

            m_elements.Insert(index, column);
        }

        public void InsertRow(int index)
        {
            if (index < 0 || index > RowCount) throw new ArgumentOutOfRangeException(nameof(index));

            for (int i = 0; i < m_elements.Count; i++)
            {
                List<CsvElement> column = m_elements[i];

                column.Insert(index, new CsvElement());
            }
        }

        public void RemoveColumnAt(int index)
        {
            if (index < 0 || index >= ColumnCount) throw new ArgumentOutOfRangeException(nameof(index));

            m_elements.RemoveAt(index);
        }

        public void RemoveRowAt(int index)
        {
            if (index < 0 || index >= RowCount) throw new ArgumentOutOfRangeException(nameof(index));

            for (int i = 0; i < m_elements.Count; i++)
            {
                List<CsvElement> column = m_elements[i];

                column.RemoveAt(index);
            }
        }

        public void Clear()
        {
            m_elements.Clear();
        }

        public CsvElement GetElement(string value)
        {
            if (!TryGetElement(value, out CsvElement element))
            {
                throw new ArgumentException($"Element with the specified value not found: '{value}'.");
            }

            return element;
        }

        public bool TryGetElement(string value, out CsvElement element)
        {
            for (int i = 0; i < m_elements.Count; i++)
            {
                List<CsvElement> column = m_elements[i];

                for (int r = 0; r < column.Count; r++)
                {
                    element = column[r];

                    if (element.Value == value)
                    {
                        return true;
                    }
                }
            }

            element = null;
            return false;
        }

        public CsvElement GetElement(string columnName, string rowName)
        {
            if (!TryGetElement(columnName, rowName, out CsvElement element))
            {
                throw new ArgumentException($"Element not found at specified column and row name: [{columnName}, {rowName}].");
            }

            return element;
        }

        public bool TryGetElement(string columnName, string rowName, out CsvElement element)
        {
            if (TryGetColumn(columnName, out List<CsvElement> column) && TryGetRowIndex(rowName, out int rowIndex))
            {
                if (rowIndex < column.Count)
                {
                    element = column[rowIndex];
                    return true;
                }
            }

            element = null;
            return false;
        }

        public CsvElement GetElement(int column, int row)
        {
            if (!TryGetElement(column, row, out CsvElement element))
            {
                throw new ArgumentException($"Element not found at specified column and row index: [{column}, {row}].");
            }

            return element;
        }

        public bool TryGetElement(int column, int index, out CsvElement element)
        {
            if (column < m_elements.Count && index < m_elements[column].Count)
            {
                element = m_elements[column][index];
                return true;
            }

            element = null;
            return false;
        }

        private bool TryGetColumn(string columnName, out List<CsvElement> column)
        {
            for (int i = 0; i < m_elements.Count; i++)
            {
                column = m_elements[i];

                if (column.Count > 0 && column[0].Value == columnName)
                {
                    return true;
                }
            }

            column = null;
            return false;
        }

        private bool TryGetRowIndex(string rowName, out int index)
        {
            if (m_elements.Count > 0)
            {
                List<CsvElement> column = m_elements[0];

                return TryGetElement(column, rowName, out _, out index);
            }

            index = default;
            return false;
        }

        private static bool TryGetElement(IReadOnlyList<CsvElement> elements, string elementValue, out CsvElement element, out int elementIndex)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                element = elements[i];

                if (element.Value == elementValue)
                {
                    elementIndex = i;
                    return true;
                }
            }

            element = null;
            elementIndex = default;
            return false;
        }

        private static List<CsvElement> CreateColumn(int count)
        {
            var column = new List<CsvElement>();

            for (int i = 0; i < count; i++)
            {
                column.Add(new CsvElement());
            }

            return column;
        }
    }
}
