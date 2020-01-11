using System;
using System.Windows.Input;
using Prism.Mvvm;

namespace Xamarin_JuniorProject.Models
{
    public class CategoryViewModel : BindableBase
    {
        public CategoryViewModel(
            int id,
            int userID,
            string category,
            ICommand tappedCommand)
        {
            ID = id;
            UserID = userID;
            Category = category;
            TappedCommand = tappedCommand;
        }

        private int _ID;
        public int ID
        {
            get => _ID;
            set => SetProperty(ref _ID, value);
        }

        private int _UserID;
        public int UserID
        {
            get => _UserID;
            set => SetProperty(ref _UserID, value);
        }

        private string _Category;
        public string Category
        {
            get => _Category;
            set => SetProperty(ref _Category, value);
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set => SetProperty(ref _IsSelected, value);
        }

        private ICommand _TappedCommand;
        public ICommand TappedCommand
        {
            get => _TappedCommand;
            set => SetProperty(ref _TappedCommand, value);
        }
    }
}
