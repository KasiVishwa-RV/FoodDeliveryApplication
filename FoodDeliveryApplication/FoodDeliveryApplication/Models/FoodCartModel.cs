using SQLite;

namespace FoodDeliveryApplication.Models
{
    public class FoodCartModel
    {
        [PrimaryKey,AutoIncrement]
        public int FoodProductId { get; set; }
        public string FoodProductName { get; set; }
        public string FoodProductDescription { get; set; }
        public int FoodProductPrice { get; set; }
    }
}
