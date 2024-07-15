public interface IExcelMappingService
{
    List<SimpleContact> MapExcelToPoco(string filePath, bool hasHeaderRecord, string sheetName);
}