using Azure.Core;
using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;

namespace KindBee.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        IDataAccess<Product> dal;
        KindBeeDBContext dbContext;
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ProductController(KindBeeDBContext kindBeeDBContext , ILogger<ProductController> logger)
        {
            _logger = logger;
            dal =  new ProductDAL(kindBeeDBContext);
            dbContext = kindBeeDBContext;
        }

        [HttpGet(Name = "GetAllItems")]
        public IEnumerable<Product> Get()
        {
            return dal.Get();
        }

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
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

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
        [HttpGet]
        public IActionResult Update(int id)
        {
            Product Product = dal.Get(id);
            return View(Product);
        }

        [HttpPost]
        public IActionResult Update(Product updatedProduct)
        {
            if (updatedProduct == null || updatedProduct.Id != updatedProduct.Id)
            {
                return BadRequest();
            }

            var Product = dal.Get(updatedProduct.Id);
            if (Product == null)
            {
                return NotFound();
            }

          
            dbContext.Attach(updatedProduct);
            dal.Update(updatedProduct);
            return RedirectToRoute("GetAllItems");
        }

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