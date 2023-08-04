using Azure.Core;
using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace KindBee.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        static IDataAccess<Product> dal;
        static KindBeeDBContext dbContext;
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public ProductController(KindBeeDBContext kindBeeDBContext , ILogger<ProductController> logger)
        //{
        //    _logger = logger;
        //    dbContext = kindBeeDBContext;
        //    dal =  new ProductDAL(kindBeeDBContext);
       
        //}

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
            dbContext = KindBeeDBContext.GetContext();
            dal = new ProductDAL(dbContext);

        }
        [Authorize(Roles = "admin")]
        [HttpGet(Name = "GetAllItems")]
        public IEnumerable<Product> Get()
        {
            return dal.Get();
        }
        [Authorize(Roles = "admin")]
        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult Get(int Id)
        {
            Product Product = dal.Get(Id);

            if (Product == null)
            {
                return NotFound();
            }

            return new ObjectResult(Product);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Create(ProductVM Product)
        {
            var p = new Product() { Name = Product.Name, DateOfManufacture = Product.DateOfManufacture, Description = Product.Description, Price = Product.Price, Quantity = Product.Quantity };

            using (var stream = Product.Image.OpenReadStream())
            {
                byte[] buffer = new byte[stream.Length];
                // считываем данные
                stream.ReadAsync(buffer, 0, buffer.Length);
                p.Image = buffer;
            }
            if (Product == null)
            {
                return BadRequest();
            }
            dal.Add(p);
            return RedirectToAction("Init","Admin");
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            Product product = dal.Get(id);
            var vm = new ProductVM()
            { Id = product.Id, DateOfManufacture = product.DateOfManufacture, Description = product.Description, Name = product.Name, Price = product.Price, Quantity = product.Quantity};

            return View(vm);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Update(ProductVM Product)
        {
            if (Product == null)
            {
                return BadRequest();
            }

            var pr = dal.Get(Product.Id);
            if (pr == null)
            {
                return NotFound();
            }

            var p = new Product() {Id = Product.Id, Name = Product.Name, DateOfManufacture = Product.DateOfManufacture, Description = Product.Description, Price = Product.Price, Quantity = Product.Quantity };

            using (var stream = Product.Image.OpenReadStream())
            {
                byte[] buffer = new byte[stream.Length];
                // считываем данные
                stream.ReadAsync(buffer, 0, buffer.Length);
                p.Image = buffer;
            }

            dal.Update(p);
            return RedirectToAction("Init", "Admin");
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            var deletedProduct = dal.Delete(Id);

            if (deletedProduct == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Init", "Admin");
        }
        [HttpPost]
        public void Test()
        {

            
        }
    }
}