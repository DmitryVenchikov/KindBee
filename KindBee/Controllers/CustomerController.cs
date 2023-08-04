using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace KindBee.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;

        static IDataAccess<Customer> dal;

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public CustomerController( ILogger<CustomerController> logger, IConfiguration configuration)
        {
            _logger = logger;
            dal = new CustomerDAL(KindBeeDBContext.GetContext());
        }
        [Authorize(Roles = "admin")]
        [HttpGet(Name = "GetAllItems")]
        public IEnumerable<Customer> Get()
        {
            return dal.Get();
        }
        [Authorize(Roles = "admin")]
        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get(int Id)
        {
            Customer Customer = dal.Get(Id);

            if (Customer == null)
            {
                return NotFound();
            }

            return new ObjectResult(Customer);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Create([FromBody] Customer Customer)
        {
            if (Customer == null)
            {
                return BadRequest();
            }
            dal.Add(Customer);
            return RedirectToAction("Index", "Home");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Update(int Id, [FromBody] Customer updatedCustomer)
        {
            if (updatedCustomer == null || updatedCustomer.Id != Id)
            {
                return BadRequest();
            }

            var Customer = dal.Get(Id);
            if (Customer == null)
            {
                return NotFound();
            }

            dal.Update(updatedCustomer);
            return RedirectToRoute("GetAllItems");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int Id)
        {
            var deletedCustomer = dal.Delete(Id);

            if (deletedCustomer == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedCustomer);
        }
    }
}