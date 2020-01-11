using System;
using System.Windows.Input;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Extentions
{
    public static class MadelsExtension
    {
        public static CategoryViewModel ToViewModel(
            this CategoryModel model,
            ICommand tappedCommand)
        {
            return new CategoryViewModel(
                model.ID,
                model.UserID,
                model.Category,
                tappedCommand);
        }
    }
}
