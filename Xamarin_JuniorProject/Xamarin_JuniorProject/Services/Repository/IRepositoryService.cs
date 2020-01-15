using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Services.Repository
{
    public interface IRepositoryService
    {
        Task<List<T>> GetAsync<T>() where T : ISQLModel, new();
        Task<T> GetAsync<T>(int id) where T : ISQLModel, new();
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : ISQLModel, new();
        Task<int> SaveOrUpdateAsync<T>(T entity) where T : ISQLModel, new();
        Task<int> DeleteAsync<T>(T entity) where T : ISQLModel, new();
    }
}
