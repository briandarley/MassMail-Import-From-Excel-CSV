namespace Import_Data_From_Excel.Infrastructure.Attributes
{
    public class ExcelAttribute : Attribute
    {

        public int ColumnNumber { get; }
        public ExcelAttribute(int columnNumber)
        {
            ColumnNumber = columnNumber;
        }


    }
}