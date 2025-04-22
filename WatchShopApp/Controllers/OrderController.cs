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


        public async Task<IActionResult> CreateOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cartItems = _shoppingCartService.GetShoppingCartItems(userId);

            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Index", "ShoppingCart");
            }

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

                var product = _context.Products.SingleOrDefault(x => x.Id == item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                    _context.Products.Update(product);
                }
            }

            
            _shoppingCartService.ClearCart(userId);
            _context.SaveChanges();

            return RedirectToAction("OrderConfirmation"); 
        }

        public IActionResult OrderConfirmation()
        {
            return View(); 
        }

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
                TotalPrice = o.Quantity * o.Price * (1 - o.Discount / 100)
            }).ToList();

            return View(vm);
        }

        public IActionResult RemoveOrder(int orderId)
        {
            var result = _orderService.RemoveById(orderId);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("MyOrders");
        }

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

            product.Quantity -= bindingModel.Quantity;
            _context.Update(product);
            await _context.SaveChangesAsync();
            bool success = _orderService.CreateOrderFromCart(user.Id);
            if (!success)
            {
                return RedirectToAction("Denied", "Order");
            }

            return RedirectToAction("MyOrders");
        }

        public IActionResult Denied()
        {
            return View(); 
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            List<OrderIndexVM> orders = _orderService.Orders()
                .Select(x => new OrderIndexVM
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate.ToString("dd-MMM-yyyy hh:mm", CultureInfo.InvariantCulture),
                    UserId = x.UserId,
                    User = x.User.UserName,
                    ProductId = x.ProductId,
                    Product = x.Product.ProductName,
                    Picture = x.Product.Picture,
                    //Description = x.Product.Description,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    Discount = x.Discount,
                    TotalPrice = x.TotalPrice,

                }).ToList();
            return View(orders);


        }
    }
}



