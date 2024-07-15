using Import_Data_From_Excel.Interfaces;

namespace Import_Data_From_Excel.WorkTasks
{
    public class WorkerTask : IWorkerTask
    {
        private readonly ICsvFileProcessorService _csvFileProcessorService;
        private readonly IExcelFileProcessorService _excelFileProcessor;
        private readonly IMassMailDataService _massMailDataService;

        // private readonly IPidUpdateService _pidUpdateService;
        // private readonly IMailProvisionDataStoreService _mailProvisionDataStoreService;


        public WorkerTask(
            ICsvFileProcessorService csvFileProcessorService,
            IExcelFileProcessorService excelMappingService,
        IMassMailDataService massMailDataService)
        {
            _csvFileProcessorService = csvFileProcessorService;
            _excelFileProcessor = excelMappingService;
            _massMailDataService = massMailDataService;
        }
        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const string folderName = "2024-07-15";
            const string filePrefix = "07-15-2024";
            while (!stoppingToken.IsCancellationRequested)
            {
                // Your background task logic here
                Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");
                //await Task.Delay(1000, stoppingToken); // Wait for 1 second
                var files = new List<dynamic>()
                {
                    new
                    {
                        FileName= $"{filePrefix} Denied.csv",
                        MassMailId = 3657,
                        Notes="Denied.csv"
                    },
                    new
                    {
                        FileName= $"{filePrefix} No Action.csv",
                        MassMailId = 3658,
                        Notes="No Action.csv"
                    },
                    new
                    {
                        FileName= $"{filePrefix} Void.csv",
                        MassMailId = 3659,
                        Notes="Void.csv"
                    }

                };


                foreach (dynamic file in files)
                {

                    
                    var filePath = $"F:\\Temp\\MassMail\\{folderName}\\{file.FileName}";
                    var campaign = await _massMailDataService.GetCampaign(file.MassMailId);
                    var stats = _csvFileProcessorService.GetFileStats<SimpleContact, SimpleContactMapping>(filePath);
                    var data = _csvFileProcessorService.GetFileData<SimpleContact, SimpleContactMapping>(filePath);
                    
                    Console.WriteLine($"Total Records: {stats.TotalRecords}");
                    Console.WriteLine($"Duplicate Records: {stats.DuplicateRecords}");
                    Console.WriteLine($"Actual Count: {stats.ActualCount}");
                  

                    var cd = new CampaignFileDetails
                    {
                        FilePath = filePath,
                        Subject = campaign.Subject,
                        Notes = file.Notes,
                        CampaignId = campaign.Id,
                        Recipients = stats.ActualCount

                    };

                    var fileName = System.IO.Path.GetFileName(filePath);
                    var fileDetails = await _massMailDataService.InitializeDataStore(new FileDetail
                    {
                        FileName = fileName,
                        Title = cd.Subject,
                        Notes = cd.Notes,
                    });

                    fileDetails.EmailAddresses = data.Select(x => new EmailAddress
                    {
                        Email = x.Email,
                    }).ToList();


                    await _massMailDataService.AddEmailAddressBatch(fileDetails);


                    campaign.FileDetailsId = fileDetails.Id;

                    await _massMailDataService.UpdateCampaign(campaign);
                }





                break;
                //await Task.Delay(1000, stoppingToken); // Wait for 1 second
            }
        }

    }
}