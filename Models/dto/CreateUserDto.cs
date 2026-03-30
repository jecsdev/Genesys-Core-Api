using Genesis_Core_Api.Models;

public class CreateUserDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; } = UserRole.Accountant;
    public bool IsActive { get; set; } = true;
}