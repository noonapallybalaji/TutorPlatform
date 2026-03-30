using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;    

    public Guid? LocationId { get; set; }
    public Location Location { get; set; } = new Location();

}