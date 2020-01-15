using System;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        IRepositoryService _repositoryService;

        public AuthorizationService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<bool> RegisterAsync(UserRegistrationModel user)
        {
            var result = false;
            try
            {
                await _repositoryService.SaveOrUpdateAsync(user);
                result = true;
            }
            catch (SQLite.SQLiteException e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public async Task<bool> LoginAsync(string login, string password)
        {
            var result = false;
            try
            {
                var users = await _repositoryService.GetAsync<UserRegistrationModel>
                    (x => x.Login == login && x.Password == password);
                if (users != null)
                {
                    App.CurrentUserId = users.ID;
                    result = true;
                }
            }
            catch(SQLite.SQLiteException e)
            {
                Console.WriteLine(e.Message);
            }
           
            return result;
        }

    }
}
