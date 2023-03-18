using Castle.Core.Resource;
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
                var customer = _kindBeeDBContext.Customers.Find(id);
                if (customer != null) //если такой клиент существует
                {
                    var positions = customer.Basket.Positions;
                    var listOfOrders = new List<Order>();
                
                    if (customer.Basket.Positions.Count != 0)
                    {
                        var order = new Order() { Customer = customer, DateOfRegistration = DateTime.Now };
                      
                        order.Status = Status.NEW;
                        
                        _kindBeeDBContext.SaveChanges();

                        var orderId = dal.Get().ToList().Last().Id;

                        //отправляем данные о заказе и сохраняем их
                        StringBuilder body = new StringBuilder();
                        body.AppendLine(
                        $"<h1>Новый заказ от клиента:</h1>" +
                        $"<h5>Id заказа {orderId}</h5>" +
                        $"<h2>Имя клиента: {customer.Name}</h2>" +
                        $"<h2>Фамилия клиента: {customer.Lastname}</h2>");
                        if (!string.IsNullOrWhiteSpace(customer.Middlename))
                        {
                            body.AppendLine($"<h2>Отчество клиента: {customer.Middlename}</h2>");
                        }
                        body.AppendLine("<br>");
                        decimal totalSum = 0;
                        


                        for (int i =0; i< customer.Basket.Positions.Count; i++)
                        {
                            var p = customer.Basket.Positions.ToArray()[i];
                       
                            
                            p.Order = order;
                            order.Positions.Add(p);
                            totalSum = (decimal)(totalSum + p.Quantity * p.Product.Price);
                            body.AppendLine(
                            $"<h3>Id продукта в БД: {p.ProductId.ToString()}</h3>" +
                            $"<h3>Название продукта: {p.Product.Name.ToString()}</h3>" +
                            $"<h3>Описание продукта: {p.Product.Description.ToString()}</h3>" +
                            $"<h3>Количество: {p.Quantity.ToString()}</h3>" +
                            $"<h3>Цена позиции: {p.Quantity * p.Product.Price}</h3><br>"
                            );
                        }

                           
                        body.AppendLine($"<h3>Итого: {totalSum}");
                        body.AppendLine($"<br><br><h4>C уважением, администрация сайта</h4>");
                        //customer.Basket.Positions.Clear();

                        _kindBeeDBContext.Orders.Add(order);
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
                        listOfOrders.Add(order);
                        //добавляем остальные заказы клиента

                        int grt = 0;
                        //await Sender.SendEmailAsync("venchikovdmitri@mail.ru", "Новый заказ от интернет магазина KindBee", body.ToString());
                    }
                    return View(customer.Orders.ToList());
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