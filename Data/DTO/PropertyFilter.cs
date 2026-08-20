namespace RealEstateProj.Data.DTO;

public class PropertyFilter
{
    public string? Title { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public double? Rooms { get; set; }
    public double? Size { get; set; }
    public Status? Status { get; set; }
    public string? Description { get; set; }
    
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
}
