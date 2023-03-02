using Backend;
using DataLayer.Backend;
using System;
using System.Linq;
using System.Threading;

namespace DataLayer.Frontend
{
    public class FrontEndHelp
    {
        AdminBackend adminBack = new(new FoodRescue_DB_Context());
        RestaurantBackend restaurantBack = new(new FoodRescue_DB_Context());
        UserBackend userBack = new(new FoodRescue_DB_Context());

        public void ShowMenu()
        {
            Console.WriteLine("\nChoose option: \n");
            Console.WriteLine("1# Create and seed the database (again)");
            Console.WriteLine("2# Show customers");
            Console.WriteLine("3# Show restaurants");
            Console.WriteLine("4# Remove customer");
            Console.WriteLine("5# Add a restaurant");
            Console.WriteLine("6# Show sold foodboxes");
            Console.WriteLine("7# Add a foodbox");
            Console.WriteLine("8# Show unsold/available foodboxes");
            Console.WriteLine("9# Purchase foodbox");
            Console.WriteLine("10# Show purchase history for customer");

        }

        public void CreateAndSeedDatabas()
        {
            Console.Clear();

            adminBack.CreateAndSeedDatabas();
        }

        public void ShowCustomers()
        {
            Console.Clear();
            foreach (var e in adminBack.CheckUsers())
            {
                Console.WriteLine($"{e.FirstName}\t{e.LastName}\t{e.Email}");
            }
        }

        public void RemoveCustomer()
        {
            Console.Clear();

            ShowCustomers();

            Console.WriteLine("\nType the customers username (Email) to remove");
            string emailChoice = Console.ReadLine();

            var isUserRemoved = adminBack.RemoveCustomer(emailChoice);
            Console.WriteLine(isUserRemoved
                ? $"\nUser {emailChoice} is removed"
                : $"\nEmail '{emailChoice}' was incorrect. Try again");
        }

        public void ShowRestaurants()
        {
            Console.Clear();

            foreach (var o in adminBack.CheckRestaurants())
            {
                Console.WriteLine($" ID: #{o.RestaurantId}\t\t{o.RestaurantName}\t\t{o.Address}");
            }
        }

        public void AddRestaurant()
        {
            Console.Clear();

            Console.WriteLine("Type restaurant name: ");

            string restaurantName = Console.ReadLine();
            string newRestaurantName = EmptyStringValidation(restaurantName);

            Console.WriteLine("\nType Restaurant full Address: ");

            string restaurantAddress = Console.ReadLine();
            string newrestaurantAddress = EmptyStringValidation(restaurantAddress);

            adminBack.AddRestaurant(newRestaurantName, newrestaurantAddress);

            Console.WriteLine("\nRestaurant added");
        }

        public void SoldFoodboxesForSpecificRestaurant()
        {
            Console.Clear();

            ShowRestaurants();

            Console.WriteLine("\n\tType the restaurant ID for the restaurant that you want to see: \n");

            bool loop = true;
            while (loop)
            {
                if (!Int32.TryParse(Console.ReadLine(), out int restaurantId))
                {
                    Console.WriteLine($"You need to choose/enter an ID. Try again.");
                    loop = true;
                }

                else if (!restaurantBack.GetCorrectRestaurantId(restaurantId))
                {
                    Console.WriteLine($"restaurantId: {restaurantId} is not a accepted value. Try again.");
                    loop = true;
                }

                else if (restaurantBack.SoldFoodboxesForSpecificRestaurant(restaurantId).Count == 0)
                {
                    Console.WriteLine("\nThere are no sold foodboxes for this restaurant");
                    loop = false;
                }
                else
                {
                    foreach (var e in restaurantBack.SoldFoodboxesForSpecificRestaurant(restaurantId))
                    {
                        Console.WriteLine($"\nSold foodboxes: \nFoodname: {e.FoodName}\t Foodtype: {e.Type}\t Price: {e.Price}");
                    }
                    loop = false;
                }
            }
        }

        public void AddFoodBox()
        {
            Console.Clear();
            ShowRestaurants();
            Console.WriteLine("\n\tChoose your Restaurant by restaurant ID: ");

            bool loop = true;

            while (loop)
            {
                //Handles if null is given on Restaruant ID.
                if (!Int32.TryParse(Console.ReadLine(), out int restaurantId))
                {
                    if (restaurantId == 0)
                    {
                        Console.WriteLine($"You need to choose/enter an ID. Try again.");
                    }
                }

                if (restaurantBack.GetCorrectRestaurantId(restaurantId))
                {
                    Console.WriteLine("\nChoose a FoodName: ");

                    string foodName = Console.ReadLine();
                    string newFoodName = EmptyStringValidation(foodName);

                    Console.WriteLine("Choose one of the follow foodtypes : 'Kött' or 'Fisk' or 'Vego': ");

                    string foodType = Console.ReadLine();
                    string notEmptyFoodType = EmptyStringValidation(foodType);
                    string correctFoodType = CheckCorrectFoodtype(notEmptyFoodType);

                    Console.WriteLine("Choose the selling price: ");

                    //Handles if null is given on Price.
                    bool error = true;
                    while (error)
                    {
                        if (!Decimal.TryParse(Console.ReadLine(), out decimal foodPrice))
                        {
                            EmptyStringMessage();
                        }
                        else
                        {
                            restaurantBack.AddFoodBox(restaurantId, newFoodName, correctFoodType, Convert.ToDecimal(foodPrice));
                            error = false;
                        }
                    }

                    Console.WriteLine("\nThanks for adding a foodbox. Here are your available foodboxes: ");
                    UnsoldFoodboxesForSpecificRestaurant(restaurantId);
                    loop = false;
                }
                else
                {
                    Console.WriteLine("\nChoose a correct restaurant ID");
                }
            }
        }

        public void UnsoldFoodboxesForSpecificRestaurant(int restaurantId)
        {
            Console.WriteLine("\nUnsold or available foodboxes: \n");

            foreach (var e in restaurantBack.UnsoldFoodboxesForSpecificRestaurant(restaurantId))
            {
                Console.WriteLine($"Foodname: {e.FoodName}\t Foodtype: {e.Type}\t Price: {e.Price}");
            }
        }

        public void ShowAllUnsoldFoodboxesByType()
        {
            Console.Clear();

            Console.WriteLine("Write the type of food (Kött/Fisk/Vego) you want to see: \n");
            string typeOfFood = Console.ReadLine().ToLower();

            while (
                typeOfFood != "Kött".ToLower() &&
                typeOfFood != "Fisk".ToLower() &&
                typeOfFood != "Vego".ToLower()
                )
            {
                Console.WriteLine("Type of food is incorrect, please try again");
                typeOfFood = Console.ReadLine();
            }

            foreach (var e in userBack.showAllUnsoldFoodboxesByType(typeOfFood))
            {
                Console.WriteLine($"\n Restaurant name: {e.Restaurant.RestaurantName} \t Foodboxes: ID #{e.FoodBoxId} Type: {e.Type} Food name: {e.FoodName} Price: {e.Price}:-");
            }
        }

        public void PurchaseFoodBox()
        {
            ShowAllUnsoldFoodboxesByType();

            int customerId = ReturnCustomerId();
            int foodboxId = ReturnFoodboxId();

            bool orderTry = true;

            while (orderTry)
            {
                try
                {
                    userBack.purchaseFoodBox(customerId, foodboxId);
                    Console.WriteLine("\nFoodbox Purchased!");
                    orderTry = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}, try again");
                    Thread.Sleep(2500);
                    ShowAllUnsoldFoodboxesByType();

                    customerId = ReturnCustomerId();
                    foodboxId = ReturnFoodboxId();
                }
            }
        }

        public void ShowPurchaseHistory()
        {
            Console.Clear();

            ShowCustomers();

            Console.WriteLine("\nChoose the customerID to see purchase history: ");
            int customerId = ReturnCustomerId();

            if (adminBack.PurchaseHistory(customerId).Any(i => i != null))
            {
                foreach (var x in adminBack.PurchaseHistory(customerId))
                {
                    Console.WriteLine($"\nOrderID: {x.OrderId} Date of order: {x.OrderDate}\n");

                    foreach (var fx in x.Foodbox)
                    {
                        Console.WriteLine($"Restaurant name: {fx.Restaurant.RestaurantName} \t Name of Foodbox: {fx.FoodName} Foodtype: {fx.Type} Price: {fx.Price}:-");
                    }
                }
            }

            else
            {
                Console.WriteLine("\nList is empty... go buy something!\n");
            }
        }

        //local methods
        private int ReturnCustomerId()
        {
            int customerId = 0;
            ShowCustomersWithoutClear();
            Console.WriteLine("\nChoose your customer number: ");
            bool loop = true;
            while (loop)
            {
                if (!Int32.TryParse(Console.ReadLine(), out int numericCustomerId))
                {
                    ShowCustomersWithoutClear();
                    Console.WriteLine("\nWrong customerId, try again");
                    customerId = numericCustomerId;
                }
                else if (!userBack.GetCorrectCustomerId(numericCustomerId))
                {
                    ShowCustomersWithoutClear();
                    Console.WriteLine("\nCustomerId does not exist, try again.");
                    customerId = numericCustomerId;

                }
                else
                {
                    loop = false;
                    customerId = numericCustomerId;
                }
            }

            return customerId;
        }
        private int ReturnFoodboxId()
        {
            int foodboxId = 0;
            Console.WriteLine("\nChoose your FoodboxId number: ");
            bool loop = true;
            while (loop)
            {
                if (!Int32.TryParse(Console.ReadLine(), out int numericFoodboxId))
                {
                    Console.WriteLine("\nYou have to enter a empty or non-numerical FoodboxId, try again");
                    foodboxId = numericFoodboxId;
                }
                else if (!restaurantBack.GetCorrectFoodBoxId(numericFoodboxId))
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\nFoodboxId does not exist, try again.");
                    foodboxId = numericFoodboxId;
                }
                else
                {
                    loop = false;
                    foodboxId = numericFoodboxId;
                }
            }

            return foodboxId;
        }
        private void EmptyStringMessage()
        {
            Console.WriteLine($"Value cannot be empty, try again");
        }

        private string EmptyStringValidation(string testValue)
        {
            while (testValue == "")
            {
                Console.WriteLine("You have entered an empty value, please try again");
                testValue = Console.ReadLine();
            }
            return testValue;
        }

        private string CheckCorrectFoodtype(string foodType)
        {
            foodType.ToLower();

            while (foodType != "kött"
                   && foodType != "fisk"
                   && foodType != "vego")
            {
                Console.WriteLine("Wrong foodtype. Please enter a correct FoodType: 'Kött' or 'Fisk' or 'Vego'");
                foodType = Console.ReadLine().ToLower();
            }
            return foodType;
        }
        private void ShowCustomersWithoutClear()
        {
            Console.WriteLine("\n List of Customers: ");
            foreach (var e in adminBack.CheckUsers())
            {
                Console.WriteLine($"\n {e.FirstName}\t{e.LastName}\t{e.Email}\t\t Id: {e.CustomerId}");
            }
        }
    }
}