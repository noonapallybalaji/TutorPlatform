using Application.AUTH.DTOs;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace API.Services;

public class AuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppDbContext _context;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.Phone,
            FullName = request.FullName
        };

        // Location Mapping
        var location = new Location
        {
            Id = Guid.NewGuid(),
            Address = request.Location.Address,
            City = request.Location.City,
            State = request.Location.State,
            Pincode = request.Location.Pincode,
            Latitude = request.Location.Coordinates.Lat,
            Longitude = request.Location.Coordinates.Lng
        };

        user.Location = location;

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));

        // Assign Role
        if (!await _roleManager.RoleExistsAsync(request.Role))
            throw new Exception("Invalid role");

        await _userManager.AddToRoleAsync(user, request.Role);

        // Parent Specific Logic
        if (request.Role == "Parent" && request.ChildProfile != null)
        {
            var parent = new ParentProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Children = new List<ChildProfile>()
            };

            var child = new ChildProfile
            {
                Id = Guid.NewGuid(),
                Name = request.ChildProfile.Name,
                Class = request.ChildProfile.Class,
                Goals = request.ChildProfile.Goals,
                Subjects = request.ChildProfile.Subjects
                    .Select(s => new ChildSubject
                    {
                        Id = Guid.NewGuid(),
                        Subject = s
                    }).ToList(),
                LearningModes = request.ChildProfile.LearningModes
                    .Select(m => new ChildLearningMode
                    {
                        Id = Guid.NewGuid(),
                        Mode = m
                    }).ToList()
            };

            parent.Children.Add(child);

            await _context.ParentProfiles.AddAsync(parent);
        }

        await _context.SaveChangesAsync();

        return "User registered successfully";
    }
}
