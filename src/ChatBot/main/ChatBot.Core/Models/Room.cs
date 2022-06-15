namespace ChatBot.Core.Models
{   
    /// <summary>
    /// An entity which manage messages
    /// </summary>
    public sealed class Room
    {
        /// <summary>
        /// An unique identifier for rooms
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A room name 
        /// </summary>
        public string Name { get; set; }  = string.Empty;

        /// <summary>
        /// Unique code for a room
        /// </summary>
        public Guid Code { get; set; }

        /// <summary>
        /// A collection of messages sent into a room
        /// </summary>
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
