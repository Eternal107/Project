using System;
namespace Xamarin_JuniorProject.Services.Authorization
{
    
        public interface IAuthorizationService
        {
            bool Login(string userName, string password);
            bool Register(UserRegistrationModel user);
            bool IsAuthorized { get; }
        }
    
}
