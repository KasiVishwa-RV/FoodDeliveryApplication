using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApplication.Models;

namespace FoodDeliveryApplication.Repos.Interface
{
    public interface IGenericRepository<T> where T : class, new()
    {
        Task<List<T>> Get();
        Task<int> Insert(T entity);
        Task<int> DeleteCart(FoodCartModel entity);
        Task<int> DeleteUser(UserDataModel entity);
    }
}
