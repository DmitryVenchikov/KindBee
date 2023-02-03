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

        IDataAccess<Basket> basketDAL;
        IDataAccess<Customer> customerDAL;
        IDataAccess<Position> positionDAL;
        IDataAccess<Product> productDAL;
        public BasketController(KindBeeDBContext kindBeeDBContext, ILogger<BasketController> logger)
        {
            _logger = logger;
            basketDAL = new BasketDAL(kindBeeDBContext);
            customerDAL = new CustomerDAL(kindBeeDBContext);
            positionDAL = new PositionDAL(kindBeeDBContext);
            productDAL = new ProductDAL(kindBeeDBContext);
        }
        public IActionResult Index()
        {
            int id;

            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out id))
            {
                var customer = customerDAL.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    var model = customer.Basket;
                    return View(model);
                }
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Error", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

     
        [HttpGet(Name = "DeleteAllPositions")]
        public IActionResult DeleteAllPositions()
        {
            int id;

            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out id))
            {
                var customer = customerDAL.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    if (customer.Basket.Positions!=null)
                    {
                        foreach (var t in customer.Basket.Positions)
                        {
                            customer.Basket.Positions.Remove(t);
                        }
                    }
                    return View(customer.Basket);
                }
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Error", "Home");
          
        }
        [HttpGet(Name = "GetAllItems")]
        public IEnumerable<Basket> Get()
        {
            return basketDAL.Get();
        }

        [HttpGet("{id}", Name = "GetBasket")]
        public IActionResult Get(int Id)
        {
            Basket Basket = basketDAL.Get(Id);

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
            basketDAL.Add(Basket);
            return CreatedAtRoute("GetBasket", new { id = Basket.Id }, Basket);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int Id, [FromBody] Basket updatedBasket)
        {
            if (updatedBasket == null || updatedBasket.Id != Id)
            {
                return BadRequest();
            }

            var Basket = basketDAL.Get(Id);
            if (Basket == null)
            {
                return NotFound();
            }

            basketDAL.Update(updatedBasket);
            return RedirectToRoute("GetAllItems");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var deletedBasket = basketDAL.Delete(Id);

            if (deletedBasket == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedBasket);
        }
    }
}