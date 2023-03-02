using Backend;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Backend
{
    public class RestaurantBackend
    {
        private readonly FoodRescue_DB_Context _context;

        public RestaurantBackend(FoodRescue_DB_Context context)
        {
            _context = context;
        }

        public List<FoodBox> SoldFoodboxesForSpecificRestaurant(int restaurantId)
        {
            var listOFoodBoxes = _context.FoodBoxes
                .Where(o => o.Order.OrderId != null)
                .Where(e => e.Restaurant.RestaurantId == restaurantId).ToList();

            return listOFoodBoxes;
        }

        public void AddFoodBox(int restaurantId, string foodName, string type, decimal price)
        {
            var newFoodbox = new FoodBox() { Restaurant = _context.Restaurants.Find(restaurantId), FoodName = foodName, Type = type, Price = price };

            _context.FoodBoxes.Add(newFoodbox);
            _context.SaveChanges();
        }

        public List<FoodBox> UnsoldFoodboxesForSpecificRestaurant(int restaurantId)
        {
            var query = _context.FoodBoxes
                .Where(o => o.Order.OrderId == null)
                .Where(e => e.Restaurant.RestaurantId == restaurantId).ToList();

            return query;
        }

        public bool GetCorrectRestaurantId(int restaurantId)
        {
            foreach (var restaurant in _context.Restaurants)
            {
                if (restaurant.RestaurantId == restaurantId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool GetCorrectFoodBoxId(int foodBoxId)
        {
            foreach (var foodBox in _context.FoodBoxes)
            {
                if (foodBox.FoodBoxId == foodBoxId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
