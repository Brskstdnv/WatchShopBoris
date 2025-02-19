using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WatchShopApp.Core.Contracts;
using WatchShopApp.Core.Service;
using WatchShopApp.Models.Manufacturer;
using WatchShopApp.Models.Product;

namespace WatchShopApp.Controllers
{
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
        public ActionResult Index(string searchStringCategoryName, string searchStringManufactureName)
        {
            List<ProductIndexVM> products = _productService.GetProducts(searchStringCategoryName, searchStringManufactureName)
            .Select(product => new ProductIndexVM
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ManufactureId = product.ManufacturerdId,
                ManufactureName = product.Manufacturer.Name,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.CategoryName,
                PictureUrl = product.Picture,
                Quantity = product.Quantity,
                Price = product.Price,
                Discount = product.Discount

            }).ToList();
            return this.View(products);
        }

        // GET: ProductCotntroller/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            return View();
        }

        // POST: ProductCotntroller/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductCotntroller/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductCotntroller/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
