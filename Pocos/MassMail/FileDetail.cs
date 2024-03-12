public class FileDetail
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string Title { get; set; } //Allow the user to provide a title for the file, this will allow the user to pick from a selection of files previously uploaded
    public string Notes { get; set; }
    public bool Archived { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public string CreateUser { get; set; } = "System";
    public DateTime ChangeDate { get; set; } = DateTime.Now;
    public string ChangeUser { get; set; } = "System";
    public int TotalEmailAddresses { get; set; }
    public int TotalCampaigns { get; set; }

    public virtual List<EmailAddress> EmailAddresses { get; set; }
}