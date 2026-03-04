using ContestSystem.Entity;
using Microsoft.VisualStudio.Services.UserAccountMapping;

namespace ContestSystem.Entit
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ContestParticipant> ContestParticipants { get; set; }
    }
}