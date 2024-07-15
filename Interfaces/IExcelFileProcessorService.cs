public interface IExcelFileProcessorService
{
    List<SimpleContact> GetFileData(string filePath, string sheetName);
    FileStats GetFileStats(string filePath, string sheetName);
}