namespace Application.AUTH.DTOs;

public class LocationDto
{
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
    public CoordinatesDto Coordinates { get; set; } = new CoordinatesDto();
}