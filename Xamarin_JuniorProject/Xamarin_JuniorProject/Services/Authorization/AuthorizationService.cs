using System;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        IRepositoryService Repository;



        public async Task<bool> Register(UserRegistrationModel user)
        {
            var result = false;
            try
            {
                await Repository.Insert(user);
                result = true;
            }
            catch (SQLite.SQLiteException)
            {

            }
            return result;
        }

        public async Task<bool> Login(string login, string password)
        {
            var result = false;

            var users = await Repository.Get<UserRegistrationModel>(x => x.Login == login && x.Password == password);


            if (users != null)
            {
                App.CurrentUserId = users.ID;
                result = true;
            }
            return result;
        }

        public AuthorizationService(IRepositoryService repositoryService)
        {
            Repository = repositoryService;
        }



    }
}
