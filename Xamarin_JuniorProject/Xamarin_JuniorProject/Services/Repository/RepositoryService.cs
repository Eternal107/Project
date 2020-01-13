using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Xamarin_JuniorProject.Services.Repository
{
    public class RepositoryService : IRepositoryService
    {
        private SQLiteAsyncConnection db;

        public RepositoryService()
        {
            var DataBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                            Constants.DATA_BASE_PATH);
            db = new SQLiteAsyncConnection(DataBasePath);
        }

        public async Task<List<T>> GetAsync<T>() where T : new()
        {
            await db.CreateTableAsync<T>();
            return await db.Table<T>().ToListAsync();
        }
        public async Task<T> GetAsync<T>(int id) where T : new()
        {
            await db.CreateTableAsync<T>();
            return await db.FindAsync<T>(id);
        }
        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : new()
        {
            await db.CreateTableAsync<T>();
            return await db.FindAsync(predicate);
        }
        public async Task<int> InsertAsync<T>(T entity) where T : new()
        {
            await db.CreateTableAsync<T>();
            return await db.InsertAsync(entity);
        }
        public async Task<int> UpdateAsync<T>(T entity) where T : new()
        {
            await db.CreateTableAsync<T>();
            return await db.UpdateAsync(entity);
        }
        public async Task<int> DeleteAsync<T>(T entity) where T : new()
        {
            await db.CreateTableAsync<T>();
            return await db.DeleteAsync(entity);
        }
    }
}
