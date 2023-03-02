using Backend;
using DataLayer.Backend;
using DataLayer.Frontend;
using System;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var frontEnd = new FrontEndHelp();
            using var ctx = new FoodRescue_DB_Context();
            var adminBack = new AdminBackend(new());

            adminBack.CreateAndSeedDatabas();
            Thread.Sleep(500);
            Console.WriteLine($"Success! ^^ ");
            Thread.Sleep(1000);
            Console.WriteLine($"Launching program...");
            Thread.Sleep(700);
            Console.Clear();

            while (true)
            {
                frontEnd.ShowMenu();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        frontEnd.CreateAndSeedDatabas();
                        break;

                    case "2":

                        frontEnd.ShowCustomers();
                        break;

                    case "3":

                        frontEnd.ShowRestaurants();
                        break;

                    case "4":
                        frontEnd.RemoveCustomer();
                        break;

                    case "5":
                        frontEnd.AddRestaurant();
                        break;

                    case "6":
                        frontEnd.SoldFoodboxesForSpecificRestaurant();
                        break;

                    case "7":
                        frontEnd.AddFoodBox();
                        break;

                    case "8":
                        frontEnd.ShowAllUnsoldFoodboxesByType();
                        break;

                    case "9":
                        frontEnd.PurchaseFoodBox();
                        break;
                    case "10":
                        frontEnd.ShowPurchaseHistory();
                        break;

                    case "X":
                        Environment.Exit(1);
                        break;

                    default:
                        Console.WriteLine("\nDo you wish to exit the program? Type \"X\" to exit the program!");
                        break;
                }
            }
        }
    }
}
