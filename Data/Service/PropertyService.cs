using Microsoft.EntityFrameworkCore;
using RealEstateProj.Data.Interfaces;

namespace RealEstateProj.Data.Service
{
    public class PropertyService : IPropertyService
    {
        private IDbContextFactory<AppDbContext> _dbContextFactory;
        public PropertyService(IDbContextFactory<AppDbContext> dbContextFactory) 
        {
            _dbContextFactory = dbContextFactory;
        }

        //CRUD
        public async Task AddProperty(Property property)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

        }

        public async Task RemovePropertyById(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var property = await GetPropertyByIdAsync(id);
            context.Properties.Remove(property);
            await context.SaveChangesAsync();

        }

        public async Task<List<Property>> GetAllPropertiesAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Properties.Include(p => p.Images).ToListAsync();
        }

        public async Task UpdatePropertyAsync(int id, Action<Property> update)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var prop = context.Properties.Find(id);

            if (prop is null)
                return;

            update(prop);
            await context.SaveChangesAsync();

        }

        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Properties.Include(p => p.Images).FirstOrDefaultAsync(x => x.Id == id); //null if not found
        }

        //--Filter--

        //Behold! the holy filltering method!
        public async Task<List<Property>> Filter(Property property)
        { 
            using var context = _dbContextFactory.CreateDbContext();
            IQueryable<Property> query = context.Properties.AsQueryable();
            foreach (var prop in property.GetType().GetProperties())
            {
                var value = prop.GetValue(property);
                if (prop.Name != "Price")
                {
                    if (value is not null)
                    {
                        query = query.Where(x => EF.Property<object>(x, prop.Name).Equals(value));//propety<obj> is a generic method from the ef thing that syas that it gets an object , x is the object itsekf and prop.name is the name in string of the property of the obj. 
                    }
                }
            }

            return await query.Include(p => p.Images).ToListAsync();

        }

        public async Task<List<Property>> FilterForPrice(int MinPrice, int MaxPrice)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var query = context.Properties.AsQueryable();
            query = query.Where(x => x.Price >= MinPrice && x.Price <= MaxPrice);
            return await query.Include(p => p.Images).ToListAsync();
        }
        public List<Property> GetPropertiesByRooms(int rooms)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Properties.Where(x => x.Rooms == rooms).ToList();
        }

     
        public List<Property> GetAllForRent()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Properties.Include(p => p.Images).Where(x => x.Status == Status.Rent).ToList();
        }

        public List<Property> GetAllForSale()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Properties.Include(p => p.Images).Where(x => x.Status == Status.Sale).ToList();
        }

      
    }
}
