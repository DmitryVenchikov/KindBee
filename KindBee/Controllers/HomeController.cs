﻿using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KindBee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IDataAccess<Customer> customerDAL;
        IDataAccess<Position> positionDAL;
        IDataAccess<Basket> basketDAL;
        IDataAccess<Product> productDAL;
        KindBeeDBContext _kindBeeDBContext;
        public HomeController(KindBeeDBContext kindBeeDBContext, ILogger<HomeController> logger)
        {
            _logger = logger;
            customerDAL = new CustomerDAL(kindBeeDBContext);
            positionDAL = new PositionDAL(kindBeeDBContext);
            basketDAL = new BasketDAL(kindBeeDBContext);
            productDAL = new ProductDAL(kindBeeDBContext);
            _kindBeeDBContext=kindBeeDBContext;
        }
        [Authorize(Roles = "customer")]
        public IActionResult Index()
        {

            var productsOnMain = new ProductsOnMain { };
            return View(productDAL.Get());
        }
        //проверить на коллизии запросов к базе
        [HttpPost]
        [Authorize(Roles ="customer")]
        public IActionResult AddProductInBasket(PurchasedProductVM purchasedProductVM)
        {
            int id;
           
            if(int.TryParse(HttpContext.User.Claims.ToList().First().Value,out id))
            {
                var customer = customerDAL.Get(id);
                if (customer != null) //если такой клиент существует
                {
                    var product = productDAL.Get(id);
                    if (customer.Basket.Positions.Where(t=>t.Product.Id == purchasedProductVM.Id).Count()==0)//если такой продукт существует в корзине клиента
                    {
                        var position = new Position { Product = product, Quantity = purchasedProductVM.Quantity };
                        customer.Basket.Positions.Add(position);
                    }
                    else
                    {
                        customer.Basket.Positions.Where(t => t.Product.Id == purchasedProductVM.Id).
                            First().Quantity += purchasedProductVM.Quantity;
                    }
                    //вычитаем из остатка склада
                    product.Quantity -= purchasedProductVM.Quantity;

                    _kindBeeDBContext.SaveChanges();

                    return RedirectToAction("Index","Home");
                }
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        public IActionResult DeleteOneProductFromBasket(int id)
        {
            int userId;

            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out userId))
            {
                var customer = customerDAL.Get(userId);
                if (customer != null) //если такой клиент существует
                {
                    var product = productDAL.Get(id);
                    if (customer.Basket.Positions.Where(t => t.Product.Id == id).Count() == 0)//если такой продукт существует в корзине клиента
                    {
                        //по идее невозможный случай
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        customer.Basket.Positions.Where(t => t.Product.Id == id).
                            First().Quantity--;
                    }
                    //вычитаем из остатка склада
                    product.Quantity++;

                    _kindBeeDBContext.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Error", "Home");
        }


        [HttpPost]
        [Authorize(Roles = "customer")]
        public IActionResult AddOneProductInBasket(int id)
        {
            int userId;

            if (int.TryParse(HttpContext.User.Claims.ToList().First().Value, out userId))
            {
                var customer = customerDAL.Get(userId);
                if (customer != null) //если такой клиент существует
                {
                    var product = productDAL.Get(id);
                    if (customer.Basket.Positions.Where(t => t.Product.Id == id).Count() == 0)//если такой продукт существует в корзине клиента
                    {
                        var position = new Position { Product = product, Quantity = 1 };
                        customer.Basket.Positions.Add(position);
                    }
                    else
                    {
                        customer.Basket.Positions.Where(t => t.Product.Id == id).
                            First().Quantity ++;
                    }
                    //вычитаем из остатка склада
                    product.Quantity --;

                    _kindBeeDBContext.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Error", "Home");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}