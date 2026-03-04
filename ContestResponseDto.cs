namespace ContestSystem.Response
{
    public class ContestResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AccessLevel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Prize { get; set; }
    }
}
