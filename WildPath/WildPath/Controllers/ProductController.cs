using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using WildPath.EfModels;
using WildPath.Models;
using WildPath.ViewModels;
using WildPath.Repositories;
using WildPath.ViewModels;

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
            ViewBag.message = HttpContext.Session.GetString("Cart");

            ProductRepo productRepo = new ProductRepo(_wpdb);

            return View(productRepo.GetAll());
        }




        public IActionResult ShopAll(string searchString, int? pageNumber)
        {
            ViewData["currentFilter"] = searchString;

            IQueryable<Item> itemsQuery = _wpdb.Items;

            if (!string.IsNullOrEmpty(searchString))
            {
                itemsQuery = itemsQuery.Where(p => p.ItemName.Contains(searchString) ||
                                            p.Supplier.Contains(searchString) ||
                                            p.Category.Contains(searchString) ||
                                            p.Weight.ToString().Contains(searchString) ||
                                            p.Size.Contains(searchString) ||
                                            p.Colour.Contains(searchString));
            }

            int pageSize = 4;
            int pageIndex = pageNumber ?? 1;
            var count = itemsQuery.Count();
            var items = itemsQuery.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            // Convert Items to ProductVM
            var productVMList = items.Select(item => new ProductVM
            {
                Item = item,
                ImageStore = _wpdb.ImageStores.FirstOrDefault(i => i.ImageId.ToString() == item.ItemImageId)
            }).ToList();

            var paginatedProductVMs = new PaginatedList<ProductVM>(productVMList, count, pageIndex, pageSize);

            return View(paginatedProductVMs);
        }


        public IActionResult ShoppingCart()
        {
            List<CartItemVM> cartItems = new List<CartItemVM>();

            string cartSession = HttpContext.Session.GetString("Cart");

            if (cartSession != null)
            {
                List<CartItem> sessionCartItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(cartSession);
                ProductRepo productRepo = new ProductRepo(_wpdb);
                cartItems = productRepo.GetCartVM(sessionCartItems);
            }

            return View(cartItems);
        }

        public JsonResult GetCartState()
        {
            var cart = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(cart))
            {
                var cartItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(cart);
                return Json(cartItems);
            }
            return Json(new List<CartItem>());
        }


        public ActionResult Details(int id)
        {
            var item = _wpdb.Items.Find(id);
            var viewModel = new ProductVM
            {
                Item = item,
                ImageStore = _wpdb.ImageStores.FirstOrDefault(i => i.ImageId.ToString() == item.ItemImageId)
            };

            return View(viewModel);
        }


        public IActionResult Create()
        {
            Item item = new Item();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int imageId = -1;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string contentType = imageFile.ContentType;
                        if (contentType == "image/png" || contentType == "image/jpeg" || contentType == "image/jpg")
                        {
                            byte[] imageData;
                            using (var memoryStream = new MemoryStream())
                            {
                                await imageFile.CopyToAsync(memoryStream);
                                imageData = memoryStream.ToArray();
                            }

                            var image = new ImageStore
                            {
                                FileName = Path.GetFileNameWithoutExtension(imageFile.FileName),
                                Image = imageData
                            };

                            _wpdb.ImageStores.Add(image);
                            await _wpdb.SaveChangesAsync();

                            imageId = image.ImageId;
                        }
                        else
                        {
                            ModelState.AddModelError("ItemImageId", "Please upload a PNG, JPG, or JPEG file.");
                            return View(item);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ItemImageId", "Please select an image to upload.");
                        return View(item);
                    }

                    item.ItemImageId = imageId.ToString();

                    _wpdb.Items.Add(item);
                    await _wpdb.SaveChangesAsync();

                    return RedirectToAction("Index", new { message = "Product created successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ItemImageId", "An error occurred while saving the image: " + ex.Message);
                    return View(item);
                }
            }
            return View(item);
        }

        public IActionResult Edit(int id)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);
            var item = productRepo.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            var viewModel = new ProductVM
            {
                Item = item,
                ImageStore = _wpdb.ImageStores.FirstOrDefault(i => i.ImageId.ToString() == item.ItemImageId)
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Item item, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string contentType = imageFile.ContentType;
                        if (contentType == "image/png" || contentType == "image/jpeg" || contentType == "image/jpg")
                        {
                            byte[] imageData;
                            using (var memoryStream = new MemoryStream())
                            {
                                await imageFile.CopyToAsync(memoryStream);
                                imageData = memoryStream.ToArray();
                            }

                            var image = new ImageStore
                            {
                                FileName = Path.GetFileNameWithoutExtension(imageFile.FileName),
                                Image = imageData
                            };

                            _wpdb.ImageStores.Add(image);
                            await _wpdb.SaveChangesAsync();

                            item.ItemImageId = image.ImageId.ToString();
                        }
                        else
                        {
                            ModelState.AddModelError("ItemImageId", "Please upload a PNG, JPG, or JPEG file.");
                            return View(item);
                        }
                    }

                    _wpdb.Items.Update(item);
                    await _wpdb.SaveChangesAsync();

                    return RedirectToAction("Index", new { message = "Product updated successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ItemImageId", "An error occurred while saving the image: " + ex.Message);
                    return View(item);
                }
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
                                FileName = Path.GetFileNameWithoutExtension(uploadModel.ImageFile.FileName),
                                Image = imageData
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
