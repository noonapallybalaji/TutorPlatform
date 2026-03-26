using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }

    // Role (optional if using Identity Roles separately)
    public string? UserType { get; set; } // Parent, Student, Tutor, Admin
}