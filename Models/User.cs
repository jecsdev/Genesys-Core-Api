namespace Genesis_Core_Api.Models
{
    public enum UserRole
    {
        Administrator,
        Accountant,
        Reader
    }

    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;  
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.Reader;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
    }
}
