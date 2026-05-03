namespace RealEstateProj.Data.Interfaces
{
    public interface IPropertyService
    {
        void AddProperty(Property property);
        void RemovePropertyById(int id);
        void UpdatePropertyBySmth(int id, Action<Property> update);
        List<Property> GetPropertysListByRooms(int rooms);
        Property GetPropertyById(int id);
        List<Property> GetAllProperties();
        List<Property> GetAllForSale();
        List<Property> GetAllForRent();
    }
}
