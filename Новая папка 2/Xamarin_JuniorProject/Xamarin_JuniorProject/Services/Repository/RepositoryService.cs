﻿using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Database;

namespace Xamarin_JuniorProject.Services.Repository
{
    public class RepositoryService : IRepositoryService
    {
        private SQLiteAsyncConnection db;

        public RepositoryService()
        {
            var DataBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.DATA_BASE_PATH);
            db = new SQLiteAsyncConnection(DataBasePath);
            db.CreateTableAsync<User>();
        }

        public async Task<List<T>> Get<T>() where T : new() =>
            await db.Table<T>().ToListAsync();

        public async Task<T> Get<T>(int id) where T : new() =>
             await db.FindAsync<T>(id);

        public async Task<T> Get<T>(Expression<Func<T, bool>> predicate) where T : new() =>
            await db.FindAsync<T>(predicate);

        public async Task<int> Insert<T>(T entity) =>
             await db.InsertAsync(entity);

        public async Task<int> Update<T>(T entity) =>
             await db.UpdateAsync(entity);

        public async Task<int> Delete<T>(T entity) =>
             await db.DeleteAsync(entity);


    }
}
