namespace RealEstateProj.Data.DTO;

public class PropertyFilterDTO
{
    public string? City { get; set; }
    public string? FreeSearch { get; set; }
    public List<double?> Rooms { get; set; }
    public int? MaxPrice { get; set; }
    public int? MinPrice { get; set; }
    public Status? FilterStatus { get; set; }
}