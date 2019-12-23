namespace UGF.Csv.Runtime
{
    public class CsvElement
    {
        public string Value { get; set; }

        public CsvElement(string value = "")
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
