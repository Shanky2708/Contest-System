namespace ContestSystem.Dto
{
    public class CreateContestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int AccessLevel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Prize { get; set; }
        public List<CreateQuestionDto> Questions { get; set; }
    }

    public class CreateQuestionDto
    {
        public string Text { get; set; }
        public int Type { get; set; }
        public List<CreateOptionDto> Options { get; set; }
    }

    public class CreateOptionDto
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
