using SQLite;

namespace FoodDeliveryApplication.Models
{
    public class UserDataModel
    {
        [PrimaryKey,AutoIncrement]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
