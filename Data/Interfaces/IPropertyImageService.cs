namespace RealEstateProj.Data.Interfaces
{
    public interface IPropertyImageService
    {
        void Add(PropertyImage pi);
        void Delete(int imageID);
        List<PropertyImage> GetByPropertyId(int propertyId);

        PropertyImage GetById(int imageID);
    }
}
