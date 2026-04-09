namespace Genesis_Core_Api.Models.dto
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public UserRole Role { get; set; }
    }
}
