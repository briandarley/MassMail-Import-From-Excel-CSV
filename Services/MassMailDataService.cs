using System.Text;
using Import_Data_From_Excel.Interfaces;
using Import_Data_From_Excel.Pocos.MassMail;
using UNC.DAL.MassMail.Domain.Models.Campaigns;

namespace Import_Data_From_Excel.Services
{
    public class MassMailDataService : IMassMailDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MassMailDataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<FileDetail> InitializeDataStore(FileDetail data)
        {
            var client = _httpClientFactory.CreateClient("UAT_MASSMAIL_DATA");
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("FileUpload", content);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            var fileDetail = await System.Text.Json.JsonSerializer.DeserializeAsync<FileDetail>(responseStream, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return fileDetail;
        }

        public async Task AddEmailAddressBatch(FileDetail data)
        {
            foreach (var item in data.EmailAddresses)
            {
                item.FileDetailsId = data.Id;

            }
            var client = _httpClientFactory.CreateClient("UAT_MASSMAIL_DATA");
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(data.EmailAddresses), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("FileUpload/emailaddresses/batch", content);
            response.EnsureSuccessStatusCode();
        }

        

     

        public async Task<UNC.DAL.MassMail.Domain.Models.Campaigns.CampaignModel> GetCampaign(int campaignId)
        {
            var client = _httpClientFactory.CreateClient("UAT_MASSMAIL_DATA");
            var response = client.GetAsync($"Campaigns?Id={campaignId}");
            var responseStream = response.Result.Content.ReadAsStreamAsync();
            var pagedResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<PagedResponse<UNC.DAL.MassMail.Domain.Models.Campaigns.CampaignModel>>(responseStream.Result, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return pagedResponse.Entities.FirstOrDefault();


        }

        public async Task UpdateCampaign(CampaignModel campaign)
        {

            var client = _httpClientFactory.CreateClient("UAT_MASSMAIL_DATA");
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(campaign), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"Campaigns/{campaign.Id}", content);
            response.EnsureSuccessStatusCode();
            
        }
    }
}