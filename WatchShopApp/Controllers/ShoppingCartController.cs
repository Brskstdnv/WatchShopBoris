using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;

using WatchShop.Infrastructure.Data.Domain;
using WatchShopApp.Core.Contracts;
using WatchShopApp.Data;
using WatchShopApp.Models.Order;
using WatchShopApp.Models.ShoppingCart;

namespace WatchShopApp.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IProductService productService)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
            _userManager = userManager;
            _context = context;
            _productService = productService;
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
                //Description = product.Description,  
                Discount = product.Discount,
                Picture = product.Picture,
            };
            return View(order);

        }

        // Добавяне на продукт в количката
        //[HttpPost]
        //public async Task<IActionResult> AddToCart(int productId)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var userId = user?.Id;

        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    var cartItem = new ShoppingCartItem
        //    {
        //        UserId = userId,
        //        ProductId = productId,
        //        Quantity = 1,
        //        Price = product.Price,
        //        Discount = product.Discount,
        //        OrderDate = DateTime.Now
        //    };

        //    _shoppingCartService.AddToCart(cartItem);

        //    return RedirectToAction("Index", "ShoppingCart");
        //}

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);

            Console.WriteLine($"productId: {productId}, quantity: {quantity}, product?.Quantity: {product?.Quantity}");

            if (product == null || quantity <= 0 || product.Quantity < quantity)
            {
                return RedirectToAction("Denied", "Order");
            }

            var cartItem = new ShoppingCartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                Price = product.Price,
                Discount = product.Discount,
                OrderDate = DateTime.Now
            };

            _shoppingCartService.AddToCart(cartItem);

            return RedirectToAction("Index", "ShoppingCart");
        }
        public async Task<IActionResult> CreateOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var result = _orderService.CreateOrderFromCart(userId);

            if (result)
            {
                _shoppingCartService.ClearCart(userId);
                return RedirectToAction("OrderConfirmation"); 
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCart");  
            }
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            var items = _context.ShoppingCarts
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .ToList();

            var cartItems = items.Select(x => new ShoppingCartItemVM
            {
                ProductId = x.ProductId,
                Product = x.Product.ProductName,
                Picture = x.Product.Picture,
                Quantity = x.Quantity,
                Price = x.Price,
                Discount = x.Discount
            }).ToList();

            var cartVM = new ShoppingCartVM
            {
                UserId = userId!,
                User = user.UserName!,
                OrderDate = DateTime.Now,
                Items = cartItems
            };

            return View(cartVM);
        }

        [HttpPost]
        public IActionResult RemoveOne(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var item = _context.ShoppingCarts
                .FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity -= 1;
                    _context.Update(item);
                }
                else
                {
                    _context.ShoppingCarts.Remove(item);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveAll(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var item = _context.ShoppingCarts
                .FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);

            if (item != null)
            {
                _context.ShoppingCarts.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            if (userId != null)
            {
                var items = _context.ShoppingCarts.Where(x => x.UserId == userId).ToList();

                if (items.Any())
                {
                    _context.ShoppingCarts.RemoveRange(items);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ProceedToOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = _orderService.CreateOrderFromCart(userId);

            if (result)
            {
                
                var cartItems = _shoppingCartService.GetShoppingCartItems(userId);
                foreach (var item in cartItems)
                {
                    var product = _productService.GetProductById(item.ProductId);
                    if (product != null)
                    {
                        product.Quantity -= item.Quantity;
                        if (product.Quantity < 0)
                            product.Quantity = 0;

                        _context.Products.Update(product);
                    }
                }

                _context.SaveChanges(); 
                _shoppingCartService.ClearCart(userId);

                return RedirectToAction("MyOrders", "Order");
            }

            return RedirectToAction("Index", "ShoppingCart");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                return NotFound();
            }

            if (quantity > product.Quantity)
            {
                quantity = product.Quantity;
            }

            _shoppingCartService.UpdateItemQuantity(userId, productId, quantity);

            return RedirectToAction("Index");
        }
    }
}

