using Microsoft.EntityFrameworkCore;
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
        //public List<Item> GetAllProducts()
        //{
        //    var products = _wpdb.Items.Select(r => new Item
        //    {
        //        ID = r.ID,
        //        ProductName = r.ProductName,
        //        Description = r.Description,
        //        Price = r.Price,
        //        Currency = r.Currency,
        //        ImageName = r.ImageName
        //    }).ToList();

        //    return products;
        //}

        //public Product GetProduct(int ID)
        //{


        //    var product = _db.Products.Where(r => r.ID == ID)
        //                        .FirstOrDefault();

        //    if (product != null)
        //    {
        //        return new Product() { ID = product.ID };
        //    }
        //    return null;
        //}
    }
}
