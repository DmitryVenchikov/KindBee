using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KindBee.Controllers
{
    public class BasketController : Controller
    {
        private readonly ILogger<BasketController> _logger;

        IDataAccess<Basket> dal;

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public BasketController(KindBeeDBContext kindBeeDBContext, ILogger<BasketController> logger)
        {
            _logger = logger;
            dal = new BasketDAL(kindBeeDBContext);
        }

        [HttpGet(Name = "GetAllItems")]
        public IEnumerable<Basket> Get()
        {
            return dal.Get();
        }

        [HttpGet("{id}", Name = "GetBasket")]
        public IActionResult Get(int Id)
        {
            Basket Basket = dal.Get(Id);

            if (Basket == null)
            {
                return NotFound();
            }

            return new ObjectResult(Basket);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Basket Basket)
        {
            if (Basket == null)
            {
                return BadRequest();
            }
            dal.Add(Basket);
            return CreatedAtRoute("GetBasket", new { id = Basket.Id }, Basket);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int Id, [FromBody] Basket updatedBasket)
        {
            if (updatedBasket == null || updatedBasket.Id != Id)
            {
                return BadRequest();
            }

            var Basket = dal.Get(Id);
            if (Basket == null)
            {
                return NotFound();
            }

            dal.Update(updatedBasket);
            return RedirectToRoute("GetAllItems");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var deletedBasket = dal.Delete(Id);

            if (deletedBasket == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedBasket);
        }
    }
}