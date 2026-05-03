namespace RealEstateProj.Data.Interfaces
{
    public interface IPropertyService
    {
        void AddProperty(Property property);
        void RemovePropertyById(int id);
        List<Property> GetPropertysListByRooms(int rooms);
        Property GetPropertyById(int id);
        List<Property> GetAllProperties();
        List<Property> GetAllForSale();
        List<Property> GetAllForRent();
    }
}
