using NUnit.Framework;

namespace UGF.Csv.Runtime.Tests
{
    public class TestCsvDocument
    {
        [Test]
        public void ColumnAndRowCount()
        {
            var document = new CsvDocument();

            Assert.AreEqual(0, document.ColumnCount);
            Assert.AreEqual(0, document.RowCount);

            document.AddColumn();

            Assert.AreEqual(1, document.ColumnCount);
            Assert.AreEqual(0, document.RowCount);

            document.AddRow();

            Assert.AreEqual(1, document.ColumnCount);
            Assert.AreEqual(1, document.RowCount);

            document = new CsvDocument(10, 10);

            Assert.AreEqual(10, document.ColumnCount);
            Assert.AreEqual(10, document.RowCount);
        }

        [Test]
        public void AddColumn()
        {
            var document = new CsvDocument(5, 5);

            document.GetElement(4, 4).Value = "4";
            document.AddColumn();

            Assert.AreEqual(6, document.ColumnCount);
            Assert.AreEqual(5, document.RowCount);
            Assert.AreEqual("4", document.GetElement(4, 4).Value);
        }

        [Test]
        public void AddRow()
        {
            var document = new CsvDocument(5, 5);

            document.GetElement(4, 4).Value = "4";
            document.AddRow();

            Assert.AreEqual(5, document.ColumnCount);
            Assert.AreEqual(6, document.RowCount);
            Assert.AreEqual("4", document.GetElement(4, 4).Value);
        }

        [Test]
        public void InsertColumn()
        {
            var document = new CsvDocument(10, 10);

            document.GetElement(4, 4).Value = "4";
            document.InsertColumn(4);

            Assert.AreEqual(11, document.ColumnCount);
            Assert.AreEqual(10, document.RowCount);

            CsvElement element = document.GetElement(5, 4);

            Assert.AreEqual("4", element.Value);
        }

        [Test]
        public void InsertRow()
        {
            var document = new CsvDocument(10, 10);

            document.GetElement(4, 4).Value = "4";
            document.InsertRow(4);

            Assert.AreEqual(10, document.ColumnCount);
            Assert.AreEqual(11, document.RowCount);

            CsvElement element = document.GetElement(4, 5);

            Assert.AreEqual("4", element.Value);
        }

        [Test]
        public void RemoveColumnAt()
        {
            var document = new CsvDocument(5, 5);

            document.GetElement(2, 2).Value = "2";
            document.GetElement(3, 3).Value = "3";
            document.RemoveColumnAt(2);

            bool result = document.TryGetElement("2", out CsvElement element);

            Assert.False(result);
            Assert.Null(element);
            Assert.AreEqual(4, document.ColumnCount);
            Assert.AreEqual(5, document.RowCount);
            Assert.AreEqual("3", document.GetElement(2, 3).Value);
        }

        [Test]
        public void RemoveRowAt()
        {
            var document = new CsvDocument(5, 5);

            document.GetElement(2, 2).Value = "2";
            document.GetElement(3, 3).Value = "3";
            document.RemoveRowAt(2);

            bool result = document.TryGetElement("2", out CsvElement element);

            Assert.False(result);
            Assert.Null(element);
            Assert.AreEqual(5, document.ColumnCount);
            Assert.AreEqual(4, document.RowCount);
            Assert.AreEqual("3", document.GetElement(3, 2).Value);
        }
    }
}
