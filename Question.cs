using DocumentFormat.OpenXml.Office2013.Excel;
using ContestSystem.Enum;

namespace ContestSystem.Entity
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid ContestId { get; set; }
        public Contest Contest { get; set; }
        public string Text { get; set; }
        public Enum.QuestionType Type { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Option> Options { get; set; }
    }
}
