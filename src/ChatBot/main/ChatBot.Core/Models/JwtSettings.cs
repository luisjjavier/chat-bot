namespace ChatBot.Core.Models
{
    public sealed class JwtSettings
    {
        public string SecurityKey { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience  { get; set; } = string.Empty;
        public double ExpiryInDays{ get; set; }
    }
}
