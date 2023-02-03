using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KindBee.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        IDataAccess<Product> dal;

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

        [HttpPost]
        public IActionResult Create([FromBody] Product Product)
        {
            if (Product == null)
            {
                return BadRequest();
            }
            dal.Add(Product);
            return CreatedAtRoute("GetProduct", new { id = Product.Id }, Product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int Id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null || updatedProduct.Id != Id)
            {
                return BadRequest();
            }

            var Product = dal.Get(Id);
            if (Product == null)
            {
                return NotFound();
            }

            dal.Update(updatedProduct);
            return RedirectToRoute("GetAllItems");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var deletedProduct = dal.Delete(Id);

            if (deletedProduct == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedProduct);
        }
    }
}