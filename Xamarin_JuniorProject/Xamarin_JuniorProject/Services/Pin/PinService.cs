using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.Services.Pin
{
    public class PinService : IPinService
    {
        private IRepositoryService _repositoryService;
        public PinService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public Task AddPinAsync(PinModel pin)
        {
            return _repositoryService.InsertAsync(pin);
        }

        public async Task DeletePinAsync(PinModel pin)
        {
            if (pin != null)
            {
                PinModel Model = await _repositoryService.GetAsync<PinModel>(pin.ID);
                if (Model != null)
                {
                    await _repositoryService.DeleteAsync(Model);
                }
            }
        }

        public Task<PinModel> FindPinModelAsync(Xamarin.Forms.GoogleMaps.Pin pin)
        {
            return _repositoryService.GetAsync<PinModel>(x =>
            x.Latitude == pin.Position.Latitude && x.Longtitude == pin.Position.Longitude && x.UserID == App.CurrentUserId);
        }

        public Task UpdatePinAsync(PinModel pin)
        {
            return _repositoryService.UpdateAsync(pin);
        }

        public async Task<List<PinModel>> GetPinsAsync(int userId)
        {
            var allPins = await _repositoryService.GetAsync<PinModel>();
            return allPins.FindAll(x => x.UserID == userId);
        }
    }
}
