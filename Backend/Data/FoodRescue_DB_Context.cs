using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;


namespace Backend
{
    public partial class FoodRescue_DB_Context : DbContext
    {

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<FoodBox> FoodBoxes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .LogTo(m => Debug.WriteLine(m), LogLevel.Information)
                    .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;database=FoodRescuePartTwo");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Customer>()
           .HasIndex(u => u.Email)
           .IsUnique();

        }

        public void Seed()
        {


            // Customers

            var customers = new Customer[]
            {

            new() { FirstName = "Poya", LastName = "efternamn", Email = "Poya@gmail.com", Password = "Pass123!", BlockedForOrders = false },
            new() { FirstName = "Johan", LastName = "efternamn", Email = "Johan@gmail.com", Password = "Pass123!", BlockedForOrders = false },
            new() { FirstName = "Kim", LastName = "efternamn", Email = "Kim@gmail.com", Password = "Pass123!", BlockedForOrders = false },
            new() { FirstName = "Pia", LastName = "efternamn", Email = "Pia@gmail.com", Password = "Pass123!", BlockedForOrders = false }

            };

            AddRange(customers);

            // Restaurants

            var restaurants = new Restaurant[]
            {
                new() { RestaurantName = "Johans Burgare", Address = "Falunstaden 1" },
                new() { RestaurantName = "Kims Grill", Address = "Lerumvägen1", },
                new() { RestaurantName = "Pia Italia", Address = "Lerumvägen 2"},
            };

            AddRange(restaurants);

            //Foodboxes

            var foodboxes = new FoodBox[]
            {
                new() { Restaurant = restaurants[0], FoodName = "Ostburgare", Type = "Kött", Price = 40.00m},
                new() { Restaurant = restaurants[0], FoodName = "Baconburgare", Type = "Kött", Price = 50.00m,},
                new() { Restaurant = restaurants[0], FoodName = "Sallad", Type = "Vego", Price = 65.00m,},
                new() { Restaurant = restaurants[0], FoodName = "Gårdagensburgare", Type = "Kött", Price = 30.00m,},
                new() { Restaurant = restaurants[1], FoodName = "Schnitzel Tallrik", Type = "Kött", Price = 49.00m },
                new() { Restaurant = restaurants[1], FoodName = "Shish Kebab", Type = "Kött", Price = 79.00m, },
                new() { Restaurant = restaurants[1], FoodName = "Grillad Lax", Type = "Fisk", Price = 75.00m, },
                new() { Restaurant = restaurants[1], FoodName = "Mix Tallrik", Type = "Kött", Price = 90.00m, },
                new() { Restaurant = restaurants[2], FoodName = "Pasta Carbonara", Type = "Kött", Price = 45.00m, },
                new() { Restaurant = restaurants[2], FoodName = "Lasagne Vego", Type = "Vego", Price = 69.00m, },
                new() { Restaurant = restaurants[2], FoodName = "Pasta la playa", Type = "Fisk", Price = 99.00m, },
                new() { Restaurant = restaurants[2], FoodName = "Gårdagens Lasagne", Type = "Kött", Price = 40.00m, },
                new() { Restaurant = restaurants[2], FoodName = "Lezagne la italia", Type = "Kött", Price = 79.00m,},
                new() { Restaurant = restaurants[2], FoodName = "Lezagne del italia", Type = "Kött", Price = 80.00m,},
                new() { Restaurant = restaurants[1], FoodName = "Schinitzel burgare", Type = "Kött", Price = 20.00m,},


             };
            AddRange(foodboxes);

            // Orders
            var orders = new Order[]
            {
                new() { Customer = customers[0], Foodbox = new[]  {foodboxes[0] }, OrderDate = DateTime.Parse("2021-10-10") },
                new() { Customer = customers[0], Foodbox = new[]  {foodboxes[1]},OrderDate = DateTime.Parse("2021-10-19") },
                new() { Customer = customers[1], Foodbox = new[] {foodboxes[2], foodboxes[4] }, OrderDate = DateTime.Parse("2021-10-11") },
                new() { Customer = customers[2], Foodbox = new[] {foodboxes[9], foodboxes[8] }, OrderDate = DateTime.Parse("2021-10-14") },
                new() { Customer = customers[2], Foodbox = new[] {foodboxes [7], foodboxes[13] }, OrderDate = DateTime.Parse("2021-10-19") },
                new() { Customer = customers[1], Foodbox = new[] {foodboxes[14] }, OrderDate = DateTime.Parse("2021-08-11") },

                // när vi  gör en metod för att köpa en foodbox, då skapas en order och definerar vilken foodbox du köper. Går att använda samma "Foodbox = new[] {foodboxes[0] } " - Collection av foodboxes där
            };

            AddRange(orders);
            SaveChanges();
        }
    }
}