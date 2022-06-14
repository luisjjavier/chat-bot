namespace ChatBot.Core.Models
{
    public sealed class ClientMessage
    {
        public string ClientUserName { get; set; } = string.Empty;

        public string RoomCode { get; set; } =string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTimeOffset SentOnUtc { get; set; }
    }
}
