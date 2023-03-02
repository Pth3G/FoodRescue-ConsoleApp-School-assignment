using Backend;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DataLayer.Backend
{
    public class UserBackend
    {
        private readonly FoodRescue_DB_Context _context;

        public UserBackend(FoodRescue_DB_Context Context)
        {
            _context = Context;
        }

        public List<FoodBox> showAllUnsoldFoodboxesByType(string typeOfFood)
        {
            var query = _context.FoodBoxes.Include(e => e.Restaurant)
                .Where(o => o.Order.OrderId == null && o.Type == typeOfFood)
                .OrderBy(o => o.Price);

            return query.ToList();
        }

        public void purchaseFoodBox(int customerId, int foodboxId)
        {
            var foodboxIdNull = _context.FoodBoxes.Find(foodboxId);
            var customerIdNull = _context.Customers.Find(customerId);

            if (foodboxIdNull == null)
            {
                throw new Exception("\nFoodboxId cannot be null");
            }

            if (customerIdNull == null)
            {
                throw new Exception("\nCustomerId cannot be null");
            }

            var newOrder = new Order() { Customer = _context.Customers.Find(customerId), OrderDate = DateTime.Now, Foodbox = new[] { _context.FoodBoxes.Find(foodboxId) } };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();
        }

        public bool GetCorrectCustomerId(int customerId)
        {
            foreach (var customer in _context.Customers)
            {
                if (customer.CustomerId == customerId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
