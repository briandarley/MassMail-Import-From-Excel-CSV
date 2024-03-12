using Import_Data_From_Excel.Interfaces;

namespace Import_Data_From_Excel.WorkTasks
{
    public class WorkerTask : IWorkerTask
    {
        private readonly ICsvFileProcessorService _csvFileProcessorService;
        private readonly IMassMailDataService _massMailDataService;

        // private readonly IPidUpdateService _pidUpdateService;
        // private readonly IMailProvisionDataStoreService _mailProvisionDataStoreService;


        public WorkerTask(ICsvFileProcessorService csvFileProcessorService, IMassMailDataService massMailDataService)
        {
            _csvFileProcessorService = csvFileProcessorService;
            _massMailDataService = massMailDataService;
        }
        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Your background task logic here
                Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");
                //await Task.Delay(1000, stoppingToken); // Wait for 1 second


                const string filePath = @"C:\Users\Brian\Downloads\REQ0405055_Degree_Candidates_2242 - to create listserv (1).csv";
                const int campaignId = 3391;
                const string notes = "SCTASK0471429";

                var campaign = await _massMailDataService.GetCampaign(campaignId);
                var stats = _csvFileProcessorService.GetFileStats<SimpleContact, SimpleContactMapping>(filePath);
                var data = _csvFileProcessorService.GetFileData<SimpleContact, SimpleContactMapping>(filePath);

                Console.WriteLine($"Total Records: {stats.TotalRecords}");
                Console.WriteLine($"Duplicate Records: {stats.DuplicateRecords}");
                Console.WriteLine($"Actual Count: {stats.ActualCount}");
                // foreach (var email in stats.DuplicateEmails)
                // {
                //     Console.WriteLine(email);
                // }

                var cd = new CampaignFileDetails
                {
                    FilePath = filePath,
                    Subject = campaign.Subject,
                    Notes = notes,
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


                break;
                //await Task.Delay(1000, stoppingToken); // Wait for 1 second
            }
        }

    }
}