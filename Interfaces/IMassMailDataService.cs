namespace Import_Data_From_Excel.Interfaces
{
    public interface IMassMailDataService
    {
        Task<FileDetail> InitializeDataStore(FileDetail data);
        Task AddEmailAddressBatch(FileDetail data);

        Task<Campaign> GetCampaign(int campaignId);
    }
}