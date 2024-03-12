using CsvHelper.Configuration;
using Import_Data_From_Excel.Interfaces;
using Import_Data_From_Excel.Pocos;

namespace Import_Data_From_Excel.Services;

public class CsvFileProcessorService : ICsvFileProcessorService
{
    private ICsvMappingService _csvMappingService;

    public CsvFileProcessorService(ICsvMappingService csvMappingService)
    {
        _csvMappingService = csvMappingService;
    }

    public List<T> GetFileData<T, K>(string filePath)
        where T : BaseEmailContact, new()
        where K : ClassMap<T>
    {
        var csvMapping = new CsvMappingService();

        var emailRecipients = csvMapping.MapCsvToPoco<T, K>(filePath, true);

        return emailRecipients.Where(c => c.Email != null).ToList();
    }

    public FileStats GetFileStats<T, K>(string filePath) where T : BaseEmailContact, new() where K : ClassMap<T>
    {
        var csvMapping = new CsvMappingService();//<SimpleContact, SimpleContactMapping>()

        var emailRecipients = csvMapping.MapCsvToPoco<T, K>(filePath, true).Where(c => c.Email != null).ToList();

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
