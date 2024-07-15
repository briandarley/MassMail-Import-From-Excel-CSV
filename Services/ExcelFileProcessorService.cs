using Import_Data_From_Excel.Services;

public class ExcelFileProcessorService : IExcelFileProcessorService
{
    public List<SimpleContact> GetFileData(string filePath, string sheetName)

    {
        var excelMappingService = new ExcelMappingService();

        var emailRecipients = excelMappingService.MapExcelToPoco(filePath, true, sheetName);

        return emailRecipients.Where(c => c.Email != null).ToList();
    }

    public FileStats GetFileStats(string filePath, string sheetName)
    {
        var excelMappingService = new ExcelMappingService();//<SimpleContact, SimpleContactMapping>()

        var emailRecipients = excelMappingService.MapExcelToPoco(filePath, true, sheetName);

        return new FileStats
        {
            TotalRecords = emailRecipients.Count,
            DuplicateRecords = emailRecipients
                        .GroupBy(c => c.Email, (key, g) =>
                                new { Email = key, Count = g.Count(), Records = g })
                                .Where(g => g.Count > 1)
                                .SelectMany(g => g.Records.Skip(1))
                                .Count(),
            DuplicateEmails = emailRecipients.GroupBy(x => x.Email).Where(g => g.Count() > 1).Select(y => y.Key).ToList(),
            ActualCount = emailRecipients
                        .GroupBy(c => c.Email, (key, g) => g.First()).Count()
        };

    }
}