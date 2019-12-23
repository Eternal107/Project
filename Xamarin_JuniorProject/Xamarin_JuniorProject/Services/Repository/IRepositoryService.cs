using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Xamarin_JuniorProject.Services.Repository
{
    public interface IRepositoryService
    {
        Task<List<T>> Get<T>() where T : new();
        Task<T> Get<T>(int id) where T : new();


        Task<T> Get<T>(Expression<Func<T, bool>> predicate) where T : new();

        Task<int> Insert<T>(T entity);
        Task<int> Update<T>(T entity);
        Task<int> Delete<T>(T entity);
    }
}
