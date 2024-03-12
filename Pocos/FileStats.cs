public class FileStats
{
    public int TotalRecords { get; set; }
    public int DuplicateRecords { get; set; }

    public int ActualCount { get; set; }

    public List<string> DuplicateEmails { get; set; }

}