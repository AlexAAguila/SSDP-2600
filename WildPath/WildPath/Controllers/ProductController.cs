using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SendGrid.Helpers.Mail;
using WildPath.EfModels;
using WildPath.Models;
using WildPath.Repositories;

namespace WildPath.Controllers
{
    public class ProductController : Controller
    {
        private readonly WildPathDbContext _wpdb;

        public ProductController(WildPathDbContext wpdb)
        {
            _wpdb = wpdb;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            ViewBag.message = HttpContext.Session.GetString("SessionExample");

            ProductRepo productRepo = new ProductRepo(_wpdb);

            return View(productRepo.GetAll());
        }

        public IActionResult ShopAll(string searchString, int? pageNumber)
        {
            ViewData["currentFilter"] = searchString;

            IQueryable<Item> items = _wpdb.Items;

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(p => p.ItemName.Contains(searchString) ||
                                        p.Supplier.Contains(searchString) ||
                                        p.Category.Contains(searchString) ||
                                        p.Weight.ToString().Contains(searchString) ||
                                        p.Size.Contains(searchString) ||
                                        p.Colour.Contains(searchString));
            }

            int pageSize = 4;
            int pageIndex = pageNumber ?? 1;
            var count = items.Count();
            var paginatedItems = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var paginatedItemsModel = new PaginatedList<Item>(paginatedItems, count, pageIndex, pageSize);

            return View(paginatedItemsModel);
        }




        //public IActionResult Category(string category)
        //{
        //    ViewData["Category"] = category;

        //    ProductRepo productRepo = new ProductRepo(_wpdb);
        //    var products = productRepo.GetByCategory(category);

        //    return View(products);
        //}

        public IActionResult Details(int id)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);
            return View(productRepo.GetById(id));
        }

        public IActionResult Create()
        {
            Item item = new Item();

            return View(item);
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                ProductRepo productRepo = new ProductRepo(_wpdb);

                string addMessage = productRepo.Add(item);

                return RedirectToAction("Index", new { message = addMessage });
            }

            return View(item);
        }
        public IActionResult Edit(int id)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);

            return View(productRepo.GetById(id));
        }

        [HttpPost]
        public IActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                ProductRepo productRepo = new ProductRepo(_wpdb);

                string repoMessage = productRepo.Update(item);

                return RedirectToAction("Index", new { message = repoMessage });
            }

            return View(item);
        }
        public IActionResult Delete(int id)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);

            return View(productRepo.GetById(id));
        }

        [HttpPost]
        public IActionResult Delete(Item item)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);

            string repoMessage = productRepo.Delete(item.PkItemId);

            return RedirectToAction("Index", new { message = repoMessage });
        }

        public IActionResult Images() 
        { 
            IEnumerable<ImageStore> images = _wpdb.ImageStores;
            return View(images); 
        }
        public IActionResult SaveImage() 
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> SaveImage(UploadModel uploadModel)
        {
            if (ModelState.IsValid)
            {
                if (uploadModel.ImageFile != null && uploadModel.ImageFile.Length > 0)
                {
                    string contentType = uploadModel.ImageFile.ContentType; 
                    if (contentType == "image/png" || contentType == "image/jpeg" || contentType == "image/jpg") 
                    { 
                        try 
                        { 
                            byte[] imageData; 
                            using (var memoryStream = new MemoryStream()) 
                            { 
                                await uploadModel.ImageFile.CopyToAsync(memoryStream); 
                                imageData = memoryStream.ToArray(); 
                            } 
                            var image = new ImageStore 
                            { 
                                FileName = Path.GetFileNameWithoutExtension(uploadModel.ImageFile.FileName), Image = imageData 
                            }; 
                            _wpdb.ImageStores.Add(image); await _wpdb.SaveChangesAsync(); 
                            
                            return RedirectToAction("Index", "Images"); 
                        } 
                        catch (Exception ex) 
                        { 
                            ModelState.AddModelError("imageUpload", "An error occured uploading your image." + " Please try again."); 
                            System.Diagnostics.Debug.WriteLine(ex.Message); 
                        } 
                    }
                    else
                    {
                        ModelState.AddModelError("imageUpload", "Please upload a PNG, " + "JPG, or JPEG file.");
                    }
                }
                else 
                { 
                    ModelState.AddModelError("imageUpload", "Please select an " + " image to upload."); 
                }
            }
            return View(uploadModel);
        }
    }
}
    