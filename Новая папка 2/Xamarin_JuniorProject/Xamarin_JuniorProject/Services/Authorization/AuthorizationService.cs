using System;
namespace Xamarin_JuniorProject.Services.Authorization
{
    public class AuthorizationService:IAuthorizationService
    {
        

        bool IsAuthorized => throw new NotImplementedException();

        bool Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        bool Register(UserRegistrationModel user)
        {
            throw new NotImplementedException();
        }
    }
}
