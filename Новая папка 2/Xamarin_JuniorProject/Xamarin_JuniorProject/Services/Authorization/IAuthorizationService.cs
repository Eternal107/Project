using System;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Database;


namespace Xamarin_JuniorProject.Services.Authorization
{

    public interface IAuthorizationService
    {

        Task<bool> Login(string userName, string password);
        Task<bool> Register(User user);
        bool IsAuthorized { get; }
    }

}

