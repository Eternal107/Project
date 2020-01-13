using Prism.Mvvm;
using System.IO;
using System.Windows.Input;

namespace Xamarin_JuniorProject.Models
{
    public class PinViewViewModel:BindableBase
    {
        public PinViewViewModel(
            int id,
            string name,
            string description,
            double latitude,
            double longitude,
            string imagePath,
            ICommand command)
        {
            ID = id;
            Name = name;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            ImagePath = imagePath;
            TappedCommand = command;
            if (File.Exists(imagePath))
                IsImageVisible = true;
        }

        private int _iD;
        public int ID
        {
            get => _iD;
            set => SetProperty(ref _iD, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }


        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }

        private bool _isImageVisible;
        public bool IsImageVisible
        {
            get => _isImageVisible;
            set => SetProperty(ref _isImageVisible, value);
        }

        private ICommand _TappedCommand;
        public ICommand TappedCommand
        {
            get => _TappedCommand;
            set => SetProperty(ref _TappedCommand, value);
        }
    }
}
