namespace ChatBot.API.Models
{
    public  sealed class ApiSettings
    {
        public ICollection<string> ClientUrls { get; set; } = new List<string>();
    }
}
