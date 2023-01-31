﻿namespace KindBee.DB.DBModels
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Middlename { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfRegistration { get; set; } = DateTime.Now;
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "user";
        public string? Mail { get; set; }
        public string? PhoneNumber { get; set; }

        public int BasketId { get; set; }
        public Basket? Basket { get; set;}
    }
}