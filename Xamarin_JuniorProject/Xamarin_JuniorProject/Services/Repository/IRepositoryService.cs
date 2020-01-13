using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Xamarin_JuniorProject.Services.Repository
{
    public interface IRepositoryService
    {
        Task<List<T>> GetAsync<T>() where T : new();
        Task<T> GetAsync<T>(int id) where T : new();
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : new();
        Task<int> InsertAsync<T>(T entity) where T : new();
        Task<int> UpdateAsync<T>(T entity) where T : new();
        Task<int> DeleteAsync<T>(T entity) where T : new();
    }
}
