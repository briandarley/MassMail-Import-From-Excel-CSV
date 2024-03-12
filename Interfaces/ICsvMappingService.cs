using CsvHelper.Configuration;

public interface ICsvMappingService
{
    List<T> MapCsvToPoco<T, K>(string filePath, bool hasHeaderRecord) where T : new() where K : ClassMap<T>;
}