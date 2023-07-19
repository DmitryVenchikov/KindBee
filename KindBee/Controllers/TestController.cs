using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KindBee.Controllers
{
    public class TestController : Controller
    {
        private readonly ILogger<CustomerController> _logger;

        static IDataAccess<Customer> dal;
        static IDataAccess<Basket> basketDAL;


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public AccountController(KindBeeDBContext kindBeeDBContext, ILogger<CustomerController> logger)
        //{
        //    _logger = logger;
        //    dal = new CustomerDAL(kindBeeDBContext);
        //    basketDAL = new BasketDAL(kindBeeDBContext);
        //}

        public TestController(ILogger<CustomerController> logger)
        {
            _logger = logger;
            dal = new CustomerDAL(KindBeeDBContext.GetContext());
            basketDAL = new BasketDAL(KindBeeDBContext.GetContext());
        }

        string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }
        [HttpGet]
        public IActionResult Test()
        {
            return View();
        }
    }
}