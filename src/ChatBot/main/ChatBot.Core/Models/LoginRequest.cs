namespace ChatBot.Core.Models
{
    /// <summary>
    /// Login request entity
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Represents a user name for login
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Represents a user name for login
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
