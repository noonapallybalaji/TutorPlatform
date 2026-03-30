namespace Infrastructure.Data;

public class ChildProfile
{
    public Guid Id { get; set; }

    public Guid ParentProfileId { get; set; }
    public ParentProfile ParentProfile { get; set; } = new();

    public string Name { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public string Goals { get; set; } = string.Empty;

    public ICollection<ChildSubject> Subjects { get; set; } = new List<ChildSubject>();
    public ICollection<ChildLearningMode> LearningModes { get; set; } = new List<ChildLearningMode>();
}

public class ChildSubject
{
    public Guid Id { get; set; }
    public Guid ChildProfileId { get; set; }
    public string Subject { get; set; } = string.Empty ; // e.g., Math, Science, etc.
}

public class ChildLearningMode
{
    public Guid Id { get; set; }
    public Guid ChildProfileId { get; set; }
    public string Mode { get; set; } = string.Empty; // Online / Home Tutor
}

