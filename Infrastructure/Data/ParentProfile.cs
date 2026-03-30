using Infrastructure.Identity;

namespace Infrastructure.Data;

public class ParentProfile
{
    public Guid Id { get; set; }
    public string UserId { get; set; } =string.Empty;
    public ApplicationUser User { get; set; } = new();

    public ICollection<ChildProfile> Children { get; set; } = new List<ChildProfile>();
}
