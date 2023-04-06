using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using FoodDeliveryApplication.Models;
using FoodDeliveryApplication.Repos.Interface;
using SQLite;

namespace FoodDeliveryApplication.Repos
{
    [ExcludeFromCodeCoverage]
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        private SQLiteAsyncConnection db;

        public GenericRepository()
        {
            var connection = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LocalDB.db3");
            db = new SQLiteAsyncConnection(connection);
            db.CreateTableAsync<T>().Wait();
        }

        public async Task<List<T>> Get()
        {
            return await db.Table<T>().ToListAsync();
        }

        public async Task<int> Insert(T entity)
        {
            return await db.InsertAsync(entity);
        }

        public async Task<int> DeleteCart(FoodCartModel entity)
        {
            var x = await db.FindAsync<FoodCartModel>(entity.FoodProductId);
            return await db.DeleteAsync(x);
        }

        public async Task<int> DeleteUser(UserDataModel entity)
        {
            var x = await db.FindAsync<UserDataModel>(entity.UserId);
            return await db.DeleteAsync(x);
        }
    }
}
