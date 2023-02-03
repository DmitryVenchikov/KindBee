using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KindBee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IDataAccess<Customer> customerDAL;
        IDataAccess<Position> positionDAL;
        IDataAccess<Basket> basketDAL;
        IDataAccess<Product> productDAL;

        public HomeController(KindBeeDBContext kindBeeDBContext, ILogger<HomeController> logger)
        {
            _logger = logger;
            customerDAL = new CustomerDAL(kindBeeDBContext);
            positionDAL = new PositionDAL(kindBeeDBContext);
            basketDAL = new BasketDAL(kindBeeDBContext);
            productDAL = new ProductDAL(kindBeeDBContext);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View(productDAL.Get());
        }
        [HttpPost]
        public IActionResult AddProductInBasket(PurchasedProductVM purchasedProductVM)
        {
            int id;
            var position = new Position { Product = productDAL.Get(purchasedProductVM.Id), Quantity = purchasedProductVM.Quantity };
            if(int.TryParse(HttpContext.User.Claims.ToList().First().Value,out id))
            {
                var customer = customerDAL.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    customer.Basket.Positions.Add(position);
                    return RedirectToAction("Index","Home");
                }
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Error", "Home");

        }
        [HttpPost]
        public IActionResult DeleteProductFromBasket(int productId)
        {
            int id;
            
            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out id))
            {
                var customer = customerDAL.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    var position = customer.Basket.Positions.Where(t => t.Product.Id == productId).First();
                    customer.Basket.Positions.Remove(position);
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Error", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}