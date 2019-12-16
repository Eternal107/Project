using System;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        IRepositoryService Repository;

        public bool IsAuthorized { get; }

        public async Task<bool> Register(User user)
        {
            var result = false;
            try
            {
                await Repository.Insert(user);
                result = true;
            }
            catch(SQLite.SQLiteException)
            {
                
            }
            return result;
        }

        public async Task<bool> Login(string login, string password)
        {
            var users = await Repository.Get<User>(x => x.Login == login && x.Password == password);

            if (users != null)
                return true;

             return false;
        }

        public AuthorizationService(RepositoryService repositoryService)
        {
            Repository = repositoryService;
        }



    }
}
