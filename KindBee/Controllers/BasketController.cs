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

namespace KindBee.Controllers
{
    public class BasketController : Controller
    {
        private readonly ILogger<BasketController> _logger;

        static IDataAccess<Basket> basketDAL;
        static IDataAccess<Customer> customerDAL;
        static IDataAccess<Position> positionDAL;
        static IDataAccess<Product> productDAL;
        static KindBeeDBContext _kindBeeDBContext;

        public BasketController(ILogger<BasketController> logger, KindBeeDBContext db)
        {
            _logger = logger;
            _kindBeeDBContext = db;
            basketDAL = new BasketDAL(_kindBeeDBContext);
            customerDAL = new CustomerDAL(_kindBeeDBContext);
            positionDAL = new PositionDAL(_kindBeeDBContext);
            productDAL = new ProductDAL(_kindBeeDBContext);
        }

        [Authorize(Roles = "customer")]
        [HttpGet(Name = "Init")]
        public IActionResult Init()
        {
            int id;
            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out id))
            {
                var customer = customerDAL.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    return View(customer.Basket);
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


        [HttpPost]
        [Authorize(Roles = "customer")]
        public int DeleteOneProductFromBasket(int id)
        {
            int userId;

            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out userId))
            {
                //customerDAL = new CustomerDAL(_kindBeeDBContext);
                var customer = customerDAL.Get(userId);
                //var customer = _kindBeeDBContext.Customers.Find(userId);
                if (customer != null) //если такой клиент существует
                {
                    //var product = _kindBeeDBContext.Products.Find(id);
                    var product = customer.Basket.Positions.First(t => t.ProductId == id).Product;
                    if (customer.Basket.Positions.Where(t => t.Product.Id == id).Count() <= 0)//если такой продукт не существует в корзине клиента
                    {
                        //по идее невозможный случай
                        return StatusCodes.Status200OK;
                    }
                    else
                    {
                        var position = customer.Basket.Positions.Where(t => t.Product.Id == id).
                            First();
                        position.Quantity--;
                        //добавляем на склад
                        position.Product.Quantity++;
                        try
                        {
                            //_kindBeeDBContext.SaveChanges();
                            customerDAL.context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            var tr = ex;
                        }

                        if (position.Quantity == 0)
                        {
                            positionDAL.Delete(position.Id);
                        }
                    }
                     return StatusCodes.Status200OK;
                }
                return StatusCodes.Status203NonAuthoritative;
            }
            return StatusCodes.Status203NonAuthoritative;
        }


        [HttpPost]
        [Authorize(Roles = "customer")]
        public int AddOneProductInBasket(int id)
        {
            int userId;

            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out userId))
            {
                // var customer = context.Customers.Include(t => t.Basket).First(i=>i.Id==userId);
                var customer = customerDAL.Get(userId);

                if (customer != null) //если такой клиент существует
                {
                    var product = productDAL.Get(id);
                    if (product.Quantity == 0)
                    {
                        return StatusCodes.Status204NoContent;
                    }
                    if (customer.Basket.Positions.Where(t => t.Product.Id == id).Count() == 0)//если такой продукт не существует в корзине клиента
                    {
                        var position = new Position { Product = product, Quantity = 1, Basket = customer.Basket };
                        customer.Basket.Positions.Add(position);
                        //_kindBeeDBContext.Positions.Add(position);
                    }
                    else
                    {
                        customer.Basket.Positions.Where(t => t.Product.Id == id).First().Quantity++;
                    }

                    //вычитаем из остатка склада
                    product.Quantity--;
                    productDAL.context.SaveChanges();
                    customerDAL.context.SaveChanges();

                    return StatusCodes.Status200OK;
                }

                return StatusCodes.Status203NonAuthoritative;
            }
            return StatusCodes.Status203NonAuthoritative;
        }

        [HttpPost]
        public int DeleteAllPositions()
        {
            int id;
            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out id))
            {
                var customer = customerDAL.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    foreach (var t in customer.Basket.Positions.ToList())
                    {
                        t.Product.Quantity += t.Quantity;
                        customer.Basket.Positions.Remove(t);
                    }
                    _kindBeeDBContext.SaveChanges();
                    return StatusCodes.Status200OK;
                }
                return StatusCodes.Status203NonAuthoritative;
            }
            return StatusCodes.Status203NonAuthoritative;

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

        [HttpPost]
        public int DeletePosition(int id)
        {
            int userId;

            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out userId))
            {
                var customer = customerDAL.Get(userId);
                if (customer != null) //если такой клиент существует
                {
                    var position = customer.Basket.Positions.Where(t => t.Product.Id == id).ToList().First();
                    position.Product.Quantity += position.Quantity;
                    customer.Basket.Positions.Remove(position);
                    _kindBeeDBContext.SaveChanges();
                    return StatusCodes.Status200OK;
                }
                return StatusCodes.Status203NonAuthoritative;
            }
            return StatusCodes.Status400BadRequest;
        }
    }
}