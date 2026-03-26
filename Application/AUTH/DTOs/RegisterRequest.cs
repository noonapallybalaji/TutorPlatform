namespace Application.AUTH.DTOs;

public class RegisterRequest
{    
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; } // Parent, Student, Tutor
}