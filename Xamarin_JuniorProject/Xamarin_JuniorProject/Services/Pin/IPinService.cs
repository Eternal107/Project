using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Services.Pin
{
    public interface IPinService
    {    
        Task AddPinAsync(PinModel pin);
        Task DeletePinAsync(PinModel pin);
        Task UpdatePinAsync(PinModel pin);
        Task<PinModel> FindPinModelAsync(Xamarin.Forms.GoogleMaps.Pin pin);
        Task<List<PinModel>> GetPinsAsync(int userId);
    }

}
