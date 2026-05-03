namespace RealEstateProj.Data
{
    public class Property
    {
         
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string City { get; set; }
        public required string Address { get; set; }
        public int Price { get; set; }
        public double Rooms { get; set; }
        public double Size { get; set; }
        public Status? Status { get; set; }
        public  string? Description { get; set; }
        public List<PropertyImage>? Images { get; set; } = new();
    }

    public enum Status
    {
        Sale,
        Rent,
        Bought
    }
}
