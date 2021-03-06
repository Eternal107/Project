﻿using System.Threading.Tasks;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> LoginAsync(string userName, string password);
        Task<bool> RegisterAsync(UserRegistrationModel user);
    }
}

