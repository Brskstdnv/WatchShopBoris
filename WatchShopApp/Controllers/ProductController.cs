using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchShop.Infrastructure.Data.Domain;
using WatchShopApp.Core.Contracts;
using WatchShopApp.Core.Service;
using WatchShopApp.Models.Manufacturer;
using WatchShopApp.Models.Product;

namespace WatchShopApp.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class ProductController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private IManufacturerService _manufacturerService;

        public ProductController(IProductService productService, ICategoryService categoryService, IManufacturerService manufacturerService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
        }




        // GET: ProductCotntroller
        [AllowAnonymous]
        public ActionResult Index(string searchStringCategoryName, string searchStringManufactureName, string searchStringProductName)
        {
            List<ProductIndexVM> products = _productService.GetProducts(searchStringCategoryName, searchStringManufactureName, searchStringProductName)
            .Select(product => new ProductIndexVM
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ManufactureId = product.ManufacturerdId,
                ManufactureName = product.Manufacturer.Name,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.CategoryName,
                PictureUrl = product.Picture,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                Discount = product.Discount

            }).ToList();
            return this.View(products);
        }

        // GET: ProductCotntroller/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            Product item = _productService.GetProductById(id);
            if(item == null)
            {
                return NotFound();
            }
            ProductDetailsVM product = new ProductDetailsVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                ManufactureId = item.ManufacturerdId,
                ManufactureName = item.Manufacturer.Name,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                PictureUrl = item.Picture,
                Description = item.Description,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount
            };
            return View(product);
        }

        // GET: ProductCotntroller/Create
        public ActionResult Create()
        {
            var product = new ProductCreateVM();
            product.Manufacturers = _manufacturerService.GetManufacturer()
                .Select(x => new ManufactrerPairVM()
                {
                    Id = x.Id,
                    Name = x.Name

                }).ToList();

            product.Categories = _categoryService.GetCategories()
                .Select(x => new Models.Category.CategoryPairVM()
                {
                    Id = x.Id,
                    Name = x.CategoryName
                }).ToList();

            return View(product);
        }

        // POST: ProductCotntroller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, [FromForm] ProductCreateVM product)
        {
            var createdId = _productService.Create(product.ProductName, product.ManufacturerId, product.CategoryId, product.PictureUrl, product.Description, product.Quantity, product.Price, product.Discount);
            if (createdId)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: ProductCotntroller/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            ProductEditVM updatedProduct = new ProductEditVM()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ManufacturerId = product.ManufacturerdId,
                // Manufactrers = product.Manufacturer,
                CategoryId = product.CategoryId,
                // Categories = product.Category,
                PictureUrl = product.Picture,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                Discount = product.Discount
            };

            updatedProduct.Manufactrers = _manufacturerService.GetManufacturer()
                .Select(x => new ManufactrerPairVM()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            updatedProduct.Categories = _categoryService.GetCategories()
                .Select(x => new Models.Category.CategoryPairVM()
                {
                    Id = x.Id,
                    Name = x.CategoryName
                }).ToList();

            return View(updatedProduct);
        }

        // POST: ProductCotntroller/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEditVM product)
        {
           if(ModelState.IsValid)
            {
                var updated = _productService.Update(id, product.ProductName, product.ManufacturerId, product.CategoryId, product.PictureUrl, product.Description, product.Quantity, product.Price, product.Discount);
                if (updated)
                {
                    return this.RedirectToAction("Index");
                }
            }
            return View(product);
        }

        // GET: ProductCotntroller/Delete/5
        public ActionResult Delete(int id)
        {
            Product item = _productService.GetProductById(id);
            if (item == null)
            {
                return NotFound();
            }

            ProductDeleteVM product = new ProductDeleteVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                ManufactureId = item.ManufacturerdId,
                ManufactureName = item.Manufacturer.Name,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                PictureUrl = item.Picture,
                Description = item.Description,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount
            };

            return View(product);
        }

        // POST: ProductCotntroller/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var delete = _productService.RemoveById(id);

            if (delete)
            {
                return RedirectToAction("Success");

            }
            else
            {
                return View();
            }


        }
        public IActionResult Success()
        {
            return View();
        }
    }
}
