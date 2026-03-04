namespace ContestSystem.Dto
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; } // 1=Admin,2=VIP,3=Normal
    }
}
