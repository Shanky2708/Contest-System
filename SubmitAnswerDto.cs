namespace ContestSystem.Dto
{
    public class SubmitContestDto
    {
        public Guid ContestId { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }

    public class AnswerDto
    {
        public Guid QuestionId { get; set; }
        public List<Guid> SelectedOptionIds { get; set; }
    }
}