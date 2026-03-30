namespace Application.AUTH.DTOs;

public class ChildProfileDto
{
    public string Name { get; set; }= string.Empty;
    public string Class { get; set; } = string.Empty;
    public List<string> Subjects { get; set; } = new List<string>();
    public string Goals { get; set; } = string.Empty;
    public List<string> LearningModes { get; set; } = new List<string>();
}