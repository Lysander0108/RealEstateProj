namespace RealEstateProj.Data.Interfaces
{
    public interface IPropertyService
    {
        // CRUD

        Task AddProperty(Property property);

        Task RemovePropertyById(int id);

        public Task<List<Property>> GetAllPropertiesAsync();

        Task UpdatePropertyAsync(int id, Action<Property> update);

        Task<Property?> GetPropertyByIdAsync(int id);


        // THE filter!!!!! 
        public  Task<List<Property>> FilterAsync(Property property, int? minPrice, int? maxPrice);
        
        // Filters

        List<Property> GetPropertiesByRooms(int rooms);

        List<Property> GetAllForRent();

        List<Property> GetAllForSale();
    }
}
