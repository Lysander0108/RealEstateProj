namespace RealEstateProj.Data
{
    public class Property
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int Price { get; set; }
        public double Rooms { get; set; }
        public double Size { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
    }

    public enum Status
    {
        ForSale,
        Rented,
        Bought
    }
}
