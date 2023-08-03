using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KindBee.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<CustomerController> _logger;

        static IDataAccess<Customer> dal;
        static IDataAccess<Basket> basketDAL;


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public AccountController(KindBeeDBContext kindBeeDBContext, ILogger<CustomerController> logger)
        //{
        //    _logger = logger;
        //    dal = new CustomerDAL(kindBeeDBContext);
        //    basketDAL = new BasketDAL(kindBeeDBContext);
        //}

        public AccountController(ILogger<CustomerController> logger)
        {
            _logger = logger;
            dal = new CustomerDAL(KindBeeDBContext.GetContext());
            basketDAL = new BasketDAL(KindBeeDBContext.GetContext());
        }

        string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public bool IsAuthorized()
        {
            int id;
            if(HttpContext.User.Claims == null || HttpContext.User.Claims.Count()==0)
            {
                return false;
            }
            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out id))
            {
                var customer = dal.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        [HttpPost]
        public async Task<IActionResult> Register(CustomerRegisterVM model)
        {
            if(model.Middlename==null)
            {
                model.Middlename = "";
            }
            //if (ModelState.IsValid)
            //{
                var user = dal.Get().FirstOrDefault(u => u.Login == model.Login);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new Customer()
                    {
                        Name = model.Name,
                        Middlename = model.Middlename,
                        Lastname = model.Lastname,
                        PhoneNumber = model.PhoneNumber,
                        Mail = model.Mail,
               
                        DateOfRegistration = DateTime.Now,
                        Login = model.Login,
                        Password = GetHash(model.Password),
                        Role = "customer",
                    };

                    dal.Add(user);
                    user.Id = dal.Get().ToList().Last().Id;
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            //}
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            //var t = HttpContext.User.Claims.ToList().FirstOrDefault().Value;
            //if (t!=null)
            //{
            //    if (int.TryParse(t, out int id))
            //    {
            //        var user = dal.Get(id);
            //        if (user != null)
            //        {
            //            return RedirectToAction("Index", "Home");
            //        }
            //    }
            //}
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(CustomerLoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = dal.Get().ToList().FirstOrDefault(u => u.Login == model.Login && u.Password == GetHash(model.Password));
                if (user != null)
                {
                    await Authenticate(user); // аутентификация
                    
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        private async Task Authenticate(Customer user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
             //   new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
               new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
             //  ,new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name.ToString())
               
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            //var t = HttpContext.User.Claims.ToList().Last().Value;
            //int u =0;
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}