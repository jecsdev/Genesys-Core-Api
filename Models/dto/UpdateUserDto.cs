using Genesis_Core_Api.Models;

public class UpdateUserDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
}