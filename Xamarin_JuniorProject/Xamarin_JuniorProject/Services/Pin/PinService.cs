using System;
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

            public async Task AddPin(PinModel pin)
            {
                await _repositoryService.Insert(pin);
            }

            public async Task DeletePin(PinModel pin)
            {
            PinModel Model = await _repositoryService.Get<PinModel>(x=>x.ID==pin.ID);
            if (pin != null)
            {
                await _repositoryService.Delete(pin);
            }
            }

            public async Task<List<PinModel>> GetPins(int userId)
            {
                var allPins = await _repositoryService.Get<PinModel>();
                return allPins.FindAll(x => x.UserID == userId);
            }
        }
    
}
