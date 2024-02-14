using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using WildPath.EfModels;

namespace WildPath.Repositories
{
    public class ProductRepo
    {
        private readonly WildPathDbContext _wpdb;

        public ProductRepo(WildPathDbContext wpdb)
        {
            _wpdb = wpdb;
        }

        public IEnumerable<Item> GetAll()
        {
            return _wpdb.Items;
        }
        public Item GetById(int id)
        {
            return _wpdb.Items
                           .FirstOrDefault(p => p.PkItemId == id);
        }

        public string Add(Item entity)
        {
            string message = string.Empty;
            try
            {
                _wpdb.Add(entity);
                _wpdb.SaveChanges();
                message = $"{entity.ItemName} saved successfully";
            }
            catch (Exception e)
            {
                message = $" Error saving {entity.ItemName} comment: {e.Message}";
            }
            return message;
        }

        public string Update(Item entity)
        {
            string message = string.Empty;
            try
            {
                Item item = GetById(entity.PkItemId) ?? new Item();
                item.Supplier = entity.Supplier;
                item.ItemName = entity.ItemName;
                item.ItemDetails = entity.ItemDetails;
                item.Price = entity.Price;
                item.Stock = entity.Stock;
                item.Category = entity.Category;
                item.Weight = entity.Weight;
                item.Size = entity.Size;
                item.Colour = entity.Colour;
                _wpdb.SaveChanges();
                message = $"{entity.ItemName} comment updated successfully";
            }
            catch (Exception e)
            {
                message = $" Error updating {entity.ItemName} comment: {e.Message}";
            }
            return message;
        }

        public string Delete(int id)
        {
            string message = string.Empty;
            try
            {
                Item item = GetById(id);
                _wpdb.Remove(item);
                _wpdb.SaveChangesAsync();
                message = $"{item.ItemName} deleted successfully";
            }
            catch (Exception e)
            {
                message = $" Error deleting Item-{id}: {e.Message}";
            }
            return message;
        }
    }
}
