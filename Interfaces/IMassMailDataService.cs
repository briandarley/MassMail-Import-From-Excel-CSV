namespace Import_Data_From_Excel.Interfaces
{
    public interface IMassMailDataService
    {
        Task<FileDetail> InitializeDataStore(FileDetail data);
        Task AddEmailAddressBatch(FileDetail data);

        Task<UNC.DAL.MassMail.Domain.Models.Campaigns.CampaignModel> GetCampaign(int campaignId);

        Task UpdateCampaign(UNC.DAL.MassMail.Domain.Models.Campaigns.CampaignModel campaign);
    }
}