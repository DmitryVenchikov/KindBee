using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KindBee.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        IDataAccess<Order> dal;
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public OrderController(IDataAccess<Order> dal, ILogger<OrderController> logger)
        {
            _logger = logger;
            dal = dal;
        }

        [HttpGet(Name = "GetAllItems")]
        public IEnumerable<Order> Get()
        {
            return dal.Get();
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int Id)
        {
            Order Order = dal.Get(Id);

            if (Order == null)
            {
                return NotFound();
            }

            return new ObjectResult(Order);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Order Order)
        {
            if (Order == null)
            {
                return BadRequest();
            }
            dal.Add(Order);
            return CreatedAtRoute("GetOrder", new { id = Order.Id }, Order);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int Id, [FromBody] Order updatedOrder)
        {
            if (updatedOrder == null || updatedOrder.Id != Id)
            {
                return BadRequest();
            }

            var Order = dal.Get(Id);
            if (Order == null)
            {
                return NotFound();
            }

            dal.Update(updatedOrder);
            return RedirectToRoute("GetAllItems");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var deletedOrder = dal.Delete(Id);

            if (deletedOrder == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedOrder);
        }
    }
}