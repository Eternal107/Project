using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Services.Pin
{
    public interface IPinService
    {
        Task AddPin(PinModel pin);

        Task DeletePin(PinModel pin);

        Task<List<PinModel>> GetPins(int userId);
    }

}
