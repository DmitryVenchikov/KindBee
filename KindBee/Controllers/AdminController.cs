
using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using FluentAssertions.Common;
using System.Configuration;

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
        private IConfiguration _configuration;
        //public AdminController(KindBeeDBContext kindBeeDBContext, ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //    customerDAL = new CustomerDAL(kindBeeDBContext);
        //    positionDAL = new PositionDAL(kindBeeDBContext);
        //    basketDAL = new BasketDAL(kindBeeDBContext);
        //    productDAL = new ProductDAL(kindBeeDBContext);
        //    dbContext = kindBeeDBContext;
        //}
        public AdminController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            dbContext = KindBeeDBContext.GetContext();
            customerDAL = new CustomerDAL(dbContext);
            positionDAL = new PositionDAL(dbContext);
            basketDAL = new BasketDAL(dbContext);
            productDAL = new ProductDAL(dbContext);
            _configuration = configuration;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public int ChangeConfiguration(string key1, string value1)
        {
            _configuration[key1] = value1;
            //System.Configuration.Configuration cfg = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //StartupFoldersConfigSection section = (StartupFoldersConfigSection)cfg.Section["StartupFolders"];
            //if (section != null)
            //{
            //    System.Diagnostics.Debug.WriteLine(section.FolderItems[0].FolderType);
            //    System.Diagnostics.Debug.WriteLine(section.FolderItems[0].Path);
            //    section.FolderItems[0].Path = "C:\\Nanook";
            //    cfg.Save(); //устанавливает перенос на новую строку и производит проверку <exename>.vshost.exe.config файла в вашей отладочной папке.
            //}
            return 0;
        }
        //защитить
        [Authorize(Roles = "admin")]
        public IActionResult Init()
        {

                var t = dbContext.Products.AsNoTracking<Product>().ToList();

                return View(t);
            
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}