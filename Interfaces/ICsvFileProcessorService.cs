using CsvHelper.Configuration;
using Import_Data_From_Excel.Pocos;

namespace Import_Data_From_Excel.Interfaces
{
    public interface ICsvFileProcessorService
    {
        FileStats GetFileStats<T, K>(string filePath) where T : BaseEmailContact, new() where K : ClassMap<T>;
        List<T> GetFileData<T, K>(string filePath) where T : BaseEmailContact, new() where K : ClassMap<T>;
    }
}