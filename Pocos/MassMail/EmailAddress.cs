public class EmailAddress
{
    public long Id { get; set; }
    public int FileDetailsId { get; set; }
    public string Email { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public string CreateUser { get; set; } = "System";
    public DateTime ChangeDate { get; set; } = DateTime.Now;
    public string ChangeUser { get; set; } = "System";
}