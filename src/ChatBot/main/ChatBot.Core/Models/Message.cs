using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Core.Models
{
    /// <summary>
    /// A message entity
    /// </summary>
    public  class Message
    {
        /// <summary>
        /// An unique identification for a message
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A raw message
        /// </summary>
        public string RawMessage { get; set; } = string.Empty;

        /// <summary>
        /// A sent time 
        /// </summary>
        public DateTimeOffset SentTime  { get; set; }

        /// <summary>
        /// Who sent this a message
        /// </summary>
        public string FromUser { get; set; } = string.Empty;

        [ForeignKey("ChatRoomId")]
        public int RoomId { get; set; }

        public virtual Room? Room { get; set; }
    }
}
