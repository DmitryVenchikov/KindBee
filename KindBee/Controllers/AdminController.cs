using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KindBee.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IDataAccess<Customer> customerDAL;
        IDataAccess<Position> positionDAL;
        IDataAccess<Basket> basketDAL;
        IDataAccess<Product> productDAL;

        public AdminController(KindBeeDBContext kindBeeDBContext, ILogger<HomeController> logger)
        {
            _logger = logger;
            customerDAL = new CustomerDAL(kindBeeDBContext);
            positionDAL = new PositionDAL(kindBeeDBContext);
            basketDAL = new BasketDAL(kindBeeDBContext);
            productDAL = new ProductDAL(kindBeeDBContext);
        }
        //защитить
        public IActionResult Init()
        {
            return View(productDAL.Get());
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