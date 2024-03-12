using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Import_Data_From_Excel.Services
{
    public class CsvMappingService : ICsvMappingService
    {


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">SimpleContact</typeparam>
        /// <typeparam name="K">SimpleContactMapping</typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<T> MapCsvToPoco<T, K>(string filePath, bool hasHeaderRecord) where T : new() where K : ClassMap<T>
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaderRecord,
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);


            csv.Context.RegisterClassMap<K>();


            var records = csv.GetRecords<T>().ToList();
            return records;

        }
    }
}