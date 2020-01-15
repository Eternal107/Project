using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Models;

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

        public async Task<List<T>> GetAsync<T>() where T : ISQLModel, new()
        {
            await db.CreateTableAsync<T>();
            return await db.Table<T>().ToListAsync();
        }

        public async Task<T> GetAsync<T>(int id) where T : ISQLModel, new()
        {
            await db.CreateTableAsync<T>();
            return await db.FindAsync<T>(id);
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : ISQLModel, new()
        {
            await db.CreateTableAsync<T>();
            return await db.FindAsync(predicate);
        }

        public async Task<int> SaveOrUpdateAsync<T>(T entity) where T : ISQLModel, new()
        {
            await db.CreateTableAsync<T>();

            if(entity.ID!=0)
            {
                return await db.UpdateAsync(entity);
            }
            else
            {
                return await db.InsertAsync(entity);
            }
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : ISQLModel, new()
        {
            await db.CreateTableAsync<T>();
            return await db.DeleteAsync(entity);
        }
    }
}
