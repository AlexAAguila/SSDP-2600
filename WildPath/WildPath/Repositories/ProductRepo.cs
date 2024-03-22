using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Drawing;
using WildPath.EfModels;
using WildPath.ViewModels;
using static NuGet.Packaging.PackagingConstants;
using static WildPath.ViewModels.CartItemVM;

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

        //public IEnumerable<Item> GetByCategory(string category)
        //{
        //    return _wpdb.Items.Where(p => p.Category == category);
        //}

        public Item GetById(int id)
        {
            return _wpdb.Items
                           .FirstOrDefault(p => p.PkItemId == id);
        }

        public List<CartItemVM> GetCartVM(List<CartItem> sessionCartItems)
        {
            if (sessionCartItems == null)
            {
                return new List<CartItemVM>();
            }

            var cartItemIds = sessionCartItems.Select(ci => ci.Id).ToList();

            var cartItems = _wpdb.Items
                .Where(item => cartItemIds.Contains(item.PkItemId))
                .ToList() // Execute the query and continue in memory
                .Select(item => {
                    var sessionCartItem = sessionCartItems.FirstOrDefault(ci => ci.Id == item.PkItemId);
                    return new CartItemVM
                    {
                        ItemId = item.PkItemId,
                        ItemName = item.ItemName,
                        ItemDetails = item.ItemDetails,
                        Price = item.Price,
                        Category = item.Category,
                        Size = item.Size,
                        Colour = item.Colour,
                        Quantity = sessionCartItem != null ? sessionCartItem.Quantity : 1
                    };
                }).ToList();

            return cartItems;
        }


        public string Update(Item entity, IFormFile imageFile)
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

                if (imageFile != null && imageFile.Length > 0)
                {
                    string contentType = imageFile.ContentType;
                    if (contentType == "image/png" || contentType == "image/jpeg" || contentType == "image/jpg")
                    {
                        byte[] imageData;
                        using (var memoryStream = new MemoryStream())
                        {
                            imageFile.CopyTo(memoryStream);
                            imageData = memoryStream.ToArray();
                        }

                        var image = new ImageStore
                        {
                            FileName = Path.GetFileNameWithoutExtension(imageFile.FileName),
                            Image = imageData
                        };

                        _wpdb.ImageStores.Add(image);
                        _wpdb.SaveChanges();

                        item.ItemImageId = image.ImageId.ToString();
                    }
                    else
                    {
                        return "Please upload a PNG, JPG, or JPEG file for the image.";
                    }
                }

                _wpdb.Items.Update(item);
                _wpdb.SaveChanges();

                message = $"{entity.ItemName} updated successfully";
            }
            catch (Exception e)
            {
                message = $"Error updating {entity.ItemName}: {e.Message}";
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
