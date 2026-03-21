namespace RealEstateProj.Data.Interfaces
{
    public interface IPropertyService
    {
        public void AddProperty(Property property);
        public void RemovePropertyById(int id);
        public List<Property> GetPropertysListByRooms(int rooms);
        public Property GetPropertyById(int id);
        IEnumerable<Property> GetAllProperties();
    }
}
