using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Mvc;
using SendLib;
using System.Diagnostics;
using System.Text;

namespace KindBee.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        static IDataAccess<Order> dal;

        static IDataAccess<Basket> basketDAL;
        static IDataAccess<Customer> customerDAL;
        static IDataAccess<Position> positionDAL;
        static IDataAccess<Product> productDAL;
        static KindBeeDBContext _kindBeeDBContext;

        public async Task<IActionResult> Init()
        {
            int id;
            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out id))
            {
                var customer = customerDAL.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    var order = new Order() { Customer = customer, DateOfRegistration = DateTime.Now };
                    dal.Add(order);
                    _kindBeeDBContext.SaveChanges();

                    var orderId = dal.Get().ToList().Last().Id;

                    //отправляем данные о заказе и сохраняем их
                    StringBuilder body = new StringBuilder();
                    body.AppendLine($"Новый заказ от клиента: \n" +
                    $"имя клиента {customer.Name}\n" +
                    $"фамилия клиента {customer.Lastname}\n" +
                    $"отчество клиента {customer.Middlename}\n\n");

                    foreach (var p in customer.Basket.Positions)
                    {
                        p.OrderId = orderId;
                  
                        body.AppendLine(
                        " id: " + p.ProductId.ToString() +
                        "\tназвание продукта: " + p.Product.Name.ToString() +
                        "\tописание продукта: " + p.Product.Description.ToString() +
                        "\tколичество" + p.Quantity.ToString()
                        );
                    }
                    var positions = customer.Basket.Positions;
                    customer.Basket = new Basket();
                    //customer.Basket.Positions = new List<Position>();
                    try
                    {
                        _kindBeeDBContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //log it
                        int t = 0;
                    }
                    await Sender.SendEmailAsync("venchikovdmitri@mail.ru", "Новый заказ от интернет магазина KindBee", body.ToString());

                    return View(positions);
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

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
            _kindBeeDBContext = KindBeeDBContext.GetContext();
            dal = new OrderDAL(_kindBeeDBContext);
            basketDAL = new BasketDAL(_kindBeeDBContext);
            customerDAL = new CustomerDAL(_kindBeeDBContext);
            positionDAL = new PositionDAL(_kindBeeDBContext);
            productDAL = new ProductDAL(_kindBeeDBContext);
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