namespace ChatBot.Core.Models
{
    /// <summary>
    /// JWT Settings 
    /// </summary>
    public sealed class JwtSettings
    {
        /// <summary>
        /// Unique security key for token
        /// </summary>
        public string SecurityKey { get; set; } = string.Empty;

        /// <summary>
        /// A valid issuer
        /// </summary>
        public string ValidIssuer { get; set; } = string.Empty;

        /// <summary>
        /// A valid audience
        /// </summary>
        public string ValidAudience  { get; set; } = string.Empty;

        /// <summary>
        /// Indicates in day when a token expires
        /// </summary>
        public double ExpiryInDays{ get; set; }
    }
}
