using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Core.Models
{
    public  class Message
    {
        public int Id { get; set; }
        public string RawMessage { get; set; } = string.Empty;
        public DateTimeOffset SentTime  { get; set; }

        public string FromUser { get; set; } = string.Empty;

        [ForeignKey("ChatRoomId")]
        public int RoomId { get; set; }

        public virtual Room? Room { get; set; }
    }
}
