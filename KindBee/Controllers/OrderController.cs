using Castle.Core.Resource;
using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendLib;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace KindBee.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        static IDataAccess<Order> orderDAL;

        static IDataAccess<Basket> basketDAL;
        static IDataAccess<Customer> customerDAL;
        static IDataAccess<Position> positionDAL;
        static IDataAccess<Product> productDAL;
        static KindBeeDBContext _kindBeeDBContext;
        private IConfiguration _configuration;
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public OrderController(ILogger<OrderController> logger, KindBeeDBContext dBContext, IConfiguration configuration)
        {
            _logger = logger;
            _kindBeeDBContext = dBContext;
            orderDAL = new OrderDAL(_kindBeeDBContext);
            basketDAL = new BasketDAL(_kindBeeDBContext);
            customerDAL = new CustomerDAL(_kindBeeDBContext);
            positionDAL = new PositionDAL(_kindBeeDBContext);
            productDAL = new ProductDAL(_kindBeeDBContext);
            _configuration = configuration;
        }
        [Authorize(Roles = "customer, admin")]
        public async Task<IActionResult> Init()
        {
            int id;
            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out id))
            {
                var customer = customerDAL.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    var positions = customer.Basket.Positions;

                    if (customer.Basket.Positions.Count != 0)
                    {
                        var order = new Order() { Customer = customer, DateOfRegistration = DateTime.Now };

                        order.Status = Status.NEW;
                        customer.Orders.Add(order);
                        customerDAL.context.SaveChanges();
                        order = customer.Orders.ToList().Last();
                      
                        //отправляем данные о заказе и сохраняем их
                        StringBuilder body = new StringBuilder();
                        body.AppendLine(
                        $"<h1>Новый заказ от клиента:</h1>" +
                        $"<h5>Id заказа {order.Id}</h5>" +
                        $"<h2>Имя клиента: {customer.Name}</h2>" +
                        $"<h2>Фамилия клиента: {customer.Lastname}</h2>");
                        if (!string.IsNullOrWhiteSpace(customer.Middlename))
                        {
                            body.AppendLine($"<h2>Отчество клиента: {customer.Middlename}</h2>");
                        }
                        body.AppendLine("<br>");
                        decimal totalSum = 0;

                        var idsPositionsList = customer.Basket.Positions.Select(t => t.Id).ToArray();
                        foreach(var positionId in idsPositionsList)
                        {
                            var position = customer.Basket.Positions.First(t => t.Id == positionId);
                            position.Order = order;
                            position.OrderId = order.Id;
                            position.BasketId = null;
                            order.Positions.Add(position);
                            
                            totalSum = (decimal)(totalSum + position.Quantity * position.Product.Price);
                            body.AppendLine(
                            $"<h3>Id продукта в БД: {position.ProductId.ToString()}</h3>" +
                            $"<h3>Название продукта: {position.Product.Name.ToString()}</h3>" +
                            $"<h3>Описание продукта: {position.Product.Description.ToString()}</h3>" +
                            $"<h3>Количество: {position.Quantity.ToString()}</h3>" +
                            $"<h3>Цена позиции: {position.Quantity * position.Product.Price}</h3><br>"
                            );
                            if(position.Quantity==0)
                            {
                                
                            }
                            customerDAL.context.Positions.Update(position);
                        }

                        body.AppendLine($"<h3>Итого: {totalSum}");
                        body.AppendLine($"<br><br><h4>C уважением, администрация сайта</h4>");
                        //удаляем позиции из корзины
                        //customer.Basket.Positions = new List<Position>();
                        customerDAL.context.Orders.Update(order);

                        try
                        {
                            customerDAL.context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            //log it
                            int t = 0;
                        }
                        //добавляем остальные заказы клиента
 
                        //await Sender.SendEmailAsync("venchikovdmitri@mail.ru", "Новый заказ от интернет магазина KindBee", body.ToString());
                        await Sender.SendEmailAsync(_configuration["mail"], "Новый заказ от интернет магазина KindBee", body.ToString());
                    }
                    return View(customer.Orders.ToList());
                }
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Error", "Home");
        }









        [Authorize(Roles = "customer, admin")]

        [HttpGet(Name = "GetAllItems")]
        public IEnumerable<Order> Get()
        {
            return orderDAL.Get();
        }
        [Authorize(Roles = "customer, admin")]

        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int Id)
        {
            Order Order = orderDAL.Get(Id);

            if (Order == null)
            {
                return NotFound();
            }

            return new ObjectResult(Order);
        }
        [Authorize(Roles = "customer, admin")]
        [HttpPost]
        public IActionResult Create([FromBody] Order Order)
        {
            if (Order == null)
            {
                return BadRequest();
            }
            orderDAL.Add(Order);
            return CreatedAtRoute("GetOrder", new { id = Order.Id }, Order);
        }
        [Authorize(Roles = "customer, admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int Id, [FromBody] Order updatedOrder)
        {
            if (updatedOrder == null || updatedOrder.Id != Id)
            {
                return BadRequest();
            }

            var Order = orderDAL.Get(Id);
            if (Order == null)
            {
                return NotFound();
            }

            orderDAL.Update(updatedOrder);
            return RedirectToRoute("GetAllItems");
        }
        [Authorize(Roles = "customer, admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var deletedOrder = orderDAL.Delete(Id);

            if (deletedOrder == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedOrder);
        }
    }
}