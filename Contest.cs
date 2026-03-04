using ContestSystem.Enum;

namespace ContestSystem.Entity
{
    public class Contest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Prize { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
