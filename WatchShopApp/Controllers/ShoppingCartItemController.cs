using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WatchShop.Infrastructure.Data.Domain;
using WatchShopApp.Core.Contracts;
using WatchShopApp.Data;
using WatchShopApp.Models.ShoppingCart;

namespace WatchShopApp.Controllers
{
    public class ShoppingCartItemController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ShoppingCartItemController(IShoppingCartService shoppingCartService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _shoppingCartService = shoppingCartService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Add(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if(user.Id == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var item = new ShoppingCartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };

            _shoppingCartService.AddToCart(item);
            _context.SaveChanges();

             return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.Name;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var items = _shoppingCartService.GetShoppingCartItems(userId);
            var totalPrice = _shoppingCartService.GetTotalPrice(userId);



            var model = new ShoppingCartVM
            {
                Items = items,
                TotalPrice = totalPrice
            };

            return View(model);
        }

        public async Task<IActionResult> Remove(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var item = new ShoppingCartItem
            {
                UserId = userId,
                ProductId = productId
            };

            _shoppingCartService.RemoveFromCart(item);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if(userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            _shoppingCartService.ClearCart(userId);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}
