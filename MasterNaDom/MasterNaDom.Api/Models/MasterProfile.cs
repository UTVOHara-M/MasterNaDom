namespace MasterNaDom.Api.Models;

public class MasterProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal HourlyRate { get; set; }
    public string City { get; set; } = "Москва";
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}