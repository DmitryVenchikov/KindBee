using Azure;
using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace KindBee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static IDataAccess<Customer> customerDAL;
        static IDataAccess<Position> positionDAL;
        static IDataAccess<Basket> basketDAL;
        static IDataAccess<Product> productDAL;
        static  KindBeeDBContext _kindBeeDBContext;
        //public HomeController(KindBeeDBContext kindBeeDBContext, ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //    customerDAL = new CustomerDAL(kindBeeDBContext);
        //    positionDAL = new PositionDAL(kindBeeDBContext);
        //    basketDAL = new BasketDAL(kindBeeDBContext);
        //    productDAL = new ProductDAL(kindBeeDBContext);
        //    _kindBeeDBContext = kindBeeDBContext;
        //}


        public HomeController( ILogger<HomeController> logger)
        {
            _logger = logger;
            _kindBeeDBContext =  KindBeeDBContext.GetContext();
            customerDAL = new CustomerDAL(_kindBeeDBContext);
            positionDAL = new PositionDAL(_kindBeeDBContext);
            basketDAL = new BasketDAL(_kindBeeDBContext);
            productDAL = new ProductDAL(_kindBeeDBContext);
          
        }
        [Authorize(Roles = "customer")]
        public IActionResult Index()
        {
            int userId;
            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out userId))
            {
                var customer = customerDAL.Get(userId);
                if (customer != null) //если такой клиент существует
                {
                    var productsOnMain = new List<ProductOnMain>();

                    foreach (var product in productDAL.Get())
                    {
                        var pos = customer.Basket.Positions.Where(t => t.Product.Id == product.Id).FirstOrDefault();
                        int? quantityInBasket = pos==null?0:pos.Quantity;
                        productsOnMain.Add(new ProductOnMain { QuantityInBasket = (int)quantityInBasket, Product = product });
                    }
                    return View(productsOnMain);
                }
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Error", "Home");
        }
        //проверить на коллизии запросов к базе
        [HttpPost]
        //[Authorize(Roles = "customer")]
        public int AddProductInBasket(PurchasedProductVM purchasedProductVM)
        {
            int id;

            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out id))
            {
                var customer = customerDAL.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    var product = productDAL.Get(id);
                    if (customer.Basket.Positions.Where(t => t.Product.Id == purchasedProductVM.Id).Count() == 0)//если такой продукт существует в корзине клиента
                    {
                        var position = new Position { Product = product, Quantity = purchasedProductVM.Quantity };
                        customer.Basket.Positions.Add(position);
                    }
                    else
                    {
                        customer.Basket.Positions.Where(t => t.Product.Id == purchasedProductVM.Id).
                            First().Quantity += purchasedProductVM.Quantity;
                    }
                    //вычитаем из остатка склада
                    product.Quantity -= purchasedProductVM.Quantity;

                    _kindBeeDBContext.SaveChanges();

                    return StatusCodes.Status200OK;
                }
                return StatusCodes.Status203NonAuthoritative;
            }
            return StatusCodes.Status203NonAuthoritative;
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