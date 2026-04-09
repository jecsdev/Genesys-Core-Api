using Genesis_Core_Api.Models;

public class CreateUserDto
{
    public string FullName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; } = UserRole.Reader;
    public bool IsActive { get; set; } = true;
}