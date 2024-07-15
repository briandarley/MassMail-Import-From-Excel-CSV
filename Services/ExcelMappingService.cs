using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;

namespace Import_Data_From_Excel.Services
{
    public class ExcelMappingService : IExcelMappingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">SimpleContact</typeparam>
        /// <typeparam name="K">SimpleContactMapping</typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<SimpleContact> MapExcelToPoco(string filePath, bool hasHeaderRecord, string sheetName)
        {
            var list = new List<SimpleContact>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage pck = new ExcelPackage(filePath))
            {
                var worksheets = pck.Workbook.Worksheets;
                var count = worksheets.Count;
                //Name of sheet we're focusing on
                var sheet1 = pck.Workbook.Worksheets[sheetName];

                for (var row = 2; row <= 15000; row++)
                {
                    var record = new SimpleContact();

                    if (sheet1.Cells[row, 1].Value is null)
                    {
                        break;
                    }
                    //record.Id = (string)sheet1.Cells[row, 1].Value;
                    record.DisplayName = (string)sheet1.Cells[row, 1].Value;
                    record.Email = (string)sheet1.Cells[row, 2].Value;
                    //record.Description = (string)sheet1.Cells[row, 4].Value;
                    //record.CreateDate = (DateTime)sheet1.Cells[row, 5].Value;
                    //record.ExpirationDate = (DateTime)sheet1.Cells[row, 6].Value;

                    list.Add(record);

                }


            }
            return list;

        }
    }
}