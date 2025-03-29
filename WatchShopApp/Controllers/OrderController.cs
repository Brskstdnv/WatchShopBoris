using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Globalization;
using System.Security.Claims;

using WatchShop.Infrastructure.Data.Domain;

using WatchShopApp.Core.Contracts;
using WatchShopApp.Data;
using WatchShopApp.Models.Order;

namespace WatchShopApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {


        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;
        private readonly ApplicationDbContext _context;

        public OrderController(IOrderService orderService, IShoppingCartService shoppingCartService, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IProductService productService)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
            _userManager = userManager;
            _context = context;
            _productService = productService;
        }


        // Метод за създаване на поръчка от количката
        public async Task<IActionResult> CreateOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Вземи всички елементи от количката
            var cartItems = _shoppingCartService.GetShoppingCartItems(userId);

            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Index", "ShoppingCart");
            }

            // Създай поръчка от количката
            foreach (var item in cartItems)
            {
                var order = new Order
                {
                    UserId = userId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Discount = item.Discount,
                    OrderDate = DateTime.Now
                };

                _context.Orders.Add(order);

                // Намаляване на количеството на продукта след поръчката
                var product = _context.Products.SingleOrDefault(x => x.Id == item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                    _context.Products.Update(product);
                }
            }

            // Изчистване на количката след създаване на поръчката
            _shoppingCartService.ClearCart(userId);
            _context.SaveChanges();

            return RedirectToAction("OrderConfirmation"); // Пренасочи към страница за потвърждение на поръчката
        }

        // Метод за потвърждение на поръчката
        public IActionResult OrderConfirmation()
        {
            return View(); // Може да добавите съобщение за потвърждение или подробности за поръчката
        }

        // Метод за преглед на всички поръчки на потребителя
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var orders = _orderService.GetOrdersByUser(userId);

            var vm = orders.Select(o => new OrderIndexVM
            {
                Id = o.Id,
                OrderDate = o.OrderDate.ToString("dd/MM/yyyy"),
                UserId = o.UserId,
                User = o.User.UserName,
                ProductId = o.ProductId,
                Product = o.Product.ProductName,
                Picture = o.Product.Picture,
                Quantity = o.Quantity,
                Price = o.Price,
                Discount = o.Discount,
                TotalPrice = (o.Price - o.Discount) * o.Quantity
            }).ToList();

            return View(vm);
        }

        // Метод за премахване на поръчка
        public IActionResult RemoveOrder(int orderId)
        {
            var result = _orderService.RemoveById(orderId);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("MyOrders");
        }

        // Метод за създаване на поръчка (при директно създаване от продукта)
        public ActionResult Create(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            OrderCreateVM order = new OrderCreateVM()
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                QuantityInStock = product.Quantity,
                Price = product.Price,
                Discount = product.Discount,
                Picture = product.Picture,
            };
            return View(order);
        }

        // Метод за създаване на поръчка чрез формата
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateVM bindingModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var product = _productService.GetProductById(bindingModel.ProductId);
            if (product == null || product.Quantity < bindingModel.Quantity)
            {
                return RedirectToAction("Denied", "Order");
            }

            // Прехвърляне на количката към поръчка
            bool success = _orderService.CreateOrderFromCart(user.Id);
            if (!success)
            {
                return RedirectToAction("Denied", "Order");
            }

            return RedirectToAction("MyOrders");
        }

        // Метод за страница с отказ от поръчка
        public IActionResult Denied()
        {
            return View(); // Може да добавите съобщение за отказана поръчка
        }







        //// Метод за създаване на поръчка от количката
        //public async Task<IActionResult> CreateOrder()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var userId = user.Id;

        //    if (userId == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // Извикай сървиса за създаване на поръчка от количката
        //    var result = _orderService.CreateOrderFromCart(userId);
        //    if (!result)
        //    {
        //        return RedirectToAction("Index", "ShoppingCart");
        //    }

        //    return RedirectToAction("OrderConfirmation"); // Пренасочи към страница за потвърждение на поръчката
        //}

        //// Метод за потвърждение на поръчката
        //public IActionResult OrderConfirmation()
        //{
        //    return View(); // Може да добавите съобщение за потвърждение или подробности за поръчката
        //}

        //// Метод за преглед на всички поръчки на потребителя
        //public async Task<IActionResult> MyOrders()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var userId = user.Id;

        //    if (userId == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    var orders = _orderService.GetOrdersByUser(userId);
        //    return View(orders); // Преглед на поръчките на потребителя
        //}

        //// Метод за премахване на поръчка
        //public IActionResult RemoveOrder(int orderId)
        //{
        //    var result = _orderService.RemoveById(orderId);
        //    if (!result)
        //    {
        //        return NotFound();
        //    }

        //    return RedirectToAction("MyOrders");
        //}
        //public ActionResult Create(int id)
        //{
        //    Product product = _productService.GetProductById(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    OrderCreateVM order = new OrderCreateVM()
        //    {
        //        ProductId = product.Id,
        //        ProductName = product.ProductName,
        //        QuantityInStock = product.Quantity,
        //        Price = product.Price,
        //        Discount = product.Discount,
        //        Picture = product.Picture,
        //    };
        //    return View(order);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(OrderCreateVM bindingModel)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var product = _productService.GetProductById(bindingModel.ProductId);
        //    if (product == null || product.Quantity < bindingModel.Quantity)
        //    {
        //        return RedirectToAction("Denied", "Order");
        //    }

        //    bool success = _orderService.CreateOrderFromCart(user.Id);
        //    if (!success)
        //    {
        //        return RedirectToAction("Denied", "Order");
        //    }

        //    return RedirectToAction("MyOrders");
        //}











        //private readonly IProductService _productService;
        //private readonly IOrderService _orderService;

        //public OrderController(IProductService productService, IOrderService orderService)
        //{
        //    _productService = productService;
        //    _orderService = orderService;
        //}

        //public ActionResult Create(int id)
        //{
        //    Product product = _productService.GetProductById(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    OrderCreateVM order = new OrderCreateVM()
        //    {
        //        ProductId = product.Id,
        //        ProductName = product.ProductName,
        //        QuantityInStock = product.Quantity,
        //        Price = product.Price,
        //        //Description = product.Description,  
        //        Discount = product.Discount,
        //        Picture = product.Picture,
        //    };
        //    return View(order);

        //}

        //// POST: OrderController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(OrderCreateVM bindingModel)
        //{
        //    string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var product = this._productService.GetProductById(bindingModel.ProductId);
        //    if (currentUserId == null || product == null || product.Quantity < bindingModel.Quantity ||
        //        product.Quantity == 0)
        //    {
        //        return RedirectToAction("Denied", "Order");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _orderService.CreateOrderFromCart(bindingModel.ProductId, currentUserId, bindingModel.Quantity);
        //    }
        //    return this.RedirectToAction("Index", "Product");
        //}

        //public ActionResult Denied()
        //{
        //    return View();
        //}

        //[Authorize(Roles = "Administrator")]
        //public ActionResult Index()
        //{
        //    List<OrderIndexVM> orders = _orderService.Orders()
        //        .Select(x => new OrderIndexVM
        //        {
        //            Id = x.Id,
        //            OrderDate = x.OrderDate.ToString("dd-MMM-yyyy hh:mm", CultureInfo.InvariantCulture),
        //            UserId = x.UserId,
        //            User = x.User.UserName,
        //            ProductId = x.ProductId,
        //            Product = x.Product.ProductName,
        //            Picture = x.Product.Picture,
        //            //Description = x.Product.Description,
        //            Quantity = x.Quantity,
        //            Price = x.Price,
        //            Discount = x.Discount,
        //            TotalPrice = x.TotalPrice,

        //        }).ToList();
        //    return View(orders);

        //}
        //public ActionResult MyOrders()
        //{
        //    string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    List<OrderIndexVM> orders = _orderService.GetOrdersByUser(currentUserId)
        //        .Select(x => new OrderIndexVM
        //        {
        //            Id = x.Id,
        //            OrderDate = x.OrderDate.ToString("dd-MMM-yyyy hh:mm", CultureInfo.InvariantCulture),
        //            UserId = x.UserId,
        //            User = x.User.UserName,
        //            ProductId = x.ProductId,
        //            Product = x.Product.ProductName,
        //            Picture = x.Product.Picture,
        //            //Description = x.Product.Description,
        //            Quantity = x.Quantity,
        //            Price = x.Price,
        //            Discount = x.Discount,
        //            TotalPrice = x.TotalPrice,
        //        }).ToList();
        //    return View(orders);
        //}
    }
}



