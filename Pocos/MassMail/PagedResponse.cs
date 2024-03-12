namespace Import_Data_From_Excel.Pocos.MassMail
{
    public class PagedResponse<T> where T : class
    {

        public int PageSize { get; set; }
        public int Index { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Entities { get; set; }
    }
}