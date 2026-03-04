namespace ContestSystem.Entity
{
    public class UserAnswer
    {
        public Guid Id { get; set; }
        public Guid ContestParticipantId { get; set; }
        public ContestParticipant ContestParticipant { get; set; }

        public Guid QuestionId { get; set; }

        // store selected options as JSON (for multi-select)
        public string SelectedOptionIdsJson { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
