﻿
using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KindBee.DB.DAL
{
    public class CustomerDAL : IDataAccess<Customer>
    {
        public  KindBeeDBContext context { get; set; }
        public CustomerDAL(KindBeeDBContext kindBeeDBContext) {
            context = kindBeeDBContext;
        }
        public void Add(Customer item)
        {
            context.Customers.Add(item);
            context.SaveChanges();
        }

        public Customer Delete(int id)
        {
            var t = Get(id);
            if (t != null)
            {
                context.Customers.Remove(t);
                context.SaveChanges();
            }
            return t;
        }

        public IEnumerable<Customer> Get()
        {
            return context.Customers.ToList();
        }

        public Customer Get(int id)
        {
            return context.Customers.Find(id);
        }

        public void Update(Customer item)
        {
            context.Customers.Update(item);
            context.SaveChanges();
        }
    }
}
