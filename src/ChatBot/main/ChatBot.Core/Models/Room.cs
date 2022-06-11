namespace ChatBot.Core.Models
{
    public sealed class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }  = string.Empty;
        public Guid Code { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
