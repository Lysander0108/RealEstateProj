using System.ComponentModel.DataAnnotations;

namespace RealEstateProj.Data
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public required byte[] ImageData { get; set; }
        [Required] public required string Type { get; set; }
        public int PropertyId { get; set; }
        [Required] public required Property property { get; set; }
    }
}

