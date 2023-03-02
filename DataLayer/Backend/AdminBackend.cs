using Backend;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Backend
{
    public class AdminBackend
    {
        private readonly FoodRescue_DB_Context _context;

        public AdminBackend(FoodRescue_DB_Context context)
        {
            _context = context;
        }

        //TODO: en metod för att skapa om och seeda databasen
        public void CreateAndSeedDatabas()
        {
            //instance of db for seeding so it dosen't crash.
            var obj = new FoodRescue_DB_Context();

            _context.Database.EnsureDeleted();
            Console.WriteLine("Database deleted");

            _context.Database.EnsureCreated();
            Console.WriteLine("Database created");

            obj.Seed();
            Console.WriteLine("Database seeded");
        }

        //TODO: en metod för att se alla användare
        public List<Customer> CheckUsers()
        {
            var query = _context.Customers.ToList();

            return query;
        }

        //TODO: en metod för att kunna ta bort en användare utifrån användarnamn
        public bool RemoveCustomer(string username)
        {
            var getAllUser = _context.Customers.ToList();

            foreach (var user in getAllUser)
            {
                if (user.Email.ToLower() == username.ToLower())
                {
                    _context.Customers.Remove(user);
                    _context.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        //TODO: en metod för att se alla restauranger
        public List<Restaurant> CheckRestaurants()
        {
            var query = _context.Restaurants.ToList();

            return query;
        }

        //TODO: en metod för att kunna lägga till ett nytt restaurang objekts
        public void AddRestaurant(string restaurantName, string adress)
        {
            var newRestaurant = new Restaurant() { RestaurantName = restaurantName, Address = adress };

            _context.Restaurants.Add(newRestaurant);

            _context.SaveChanges();
        }

        // TODO: en query för att lista köphistoriken för en användare VG
        public List<Order> PurchaseHistory(int CustomerId)
        {
            var query = _context.Orders.Include(e => e.Foodbox)
                .ThenInclude(e => e.Restaurant)
               .Where(o => o.OrderId != null)
               .Where(e => e.Customer.CustomerId == CustomerId).ToList();

            return query;
        }
    }
}
