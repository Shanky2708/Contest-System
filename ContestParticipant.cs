using ContestSystem.Entit;

namespace ContestSystem.Entity
{
    public class ContestParticipant
    {
        public Guid Id { get; set; }
        public Guid ContestId { get; set; }
        public Contest Contest { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public int Score { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
