// See https://aka.ms/new-console-template for more information
using KindBee.DB;
using KindBee.DB.DAL;


var c = KindBeeDBContext.GetContext();
var productsDAL = new ProductDAL(c);

var product = productsDAL.Get().First();
product.Name = "new 1";
productsDAL.Update(product);
product = productsDAL.Get(product.Id);

Console.WriteLine(product.Name);


var c2 = KindBeeDBContext.GetContext();
var productsDAL2 = new ProductDAL(c2);

var product2 = productsDAL2.Get().First();
product2.Name = "new 2";
productsDAL2.Update(product2);
product = productsDAL2.Get(product2.Id);

Console.WriteLine(product2.Name);
