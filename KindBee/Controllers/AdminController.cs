
using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace KindBee.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static IDataAccess<Customer> customerDAL;
        static IDataAccess<Position> positionDAL;
        static IDataAccess<Basket> basketDAL;
        static IDataAccess<Product> productDAL;
        static KindBeeDBContext dbContext;
        //public AdminController(KindBeeDBContext kindBeeDBContext, ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //    customerDAL = new CustomerDAL(kindBeeDBContext);
        //    positionDAL = new PositionDAL(kindBeeDBContext);
        //    basketDAL = new BasketDAL(kindBeeDBContext);
        //    productDAL = new ProductDAL(kindBeeDBContext);
        //    dbContext = kindBeeDBContext;
        //}
        public AdminController(ILogger<HomeController> logger)
        {
            _logger = logger;
            dbContext = KindBeeDBContext.GetContext();
            customerDAL = new CustomerDAL(dbContext);
            positionDAL = new PositionDAL(dbContext);
            basketDAL = new BasketDAL(dbContext);
            productDAL = new ProductDAL(dbContext);
          
        }
        //защитить
        public IActionResult Init()
        {

                var t = dbContext.Products.AsNoTracking<Product>().ToList();

                return View(t);
            
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