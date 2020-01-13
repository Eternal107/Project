using Prism.Mvvm;

namespace Xamarin_JuniorProject.Models
{
    public class PinViewModel : BindableBase
    {
        public PinViewModel(
            int id,
            int userID,
            string name,
            string description,
            double latitude,
            double longitude,
            bool isFavorite,
            string imagePath,
            int categoryID)
        {
            ID = id;
            UserID = userID;
            Name = name;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            IsFavorite = isFavorite;
            ImagePath = imagePath;
            CategoryID = categoryID;
        }

        public PinViewModel()
        {
           
        }

        private int _iD;
        public int ID
        {
            get => _iD;
            set => SetProperty(ref _iD, value);
        }

        private int _userID;
        public int UserID
        {
            get => _userID;
            set => SetProperty(ref _userID, value);
        }

        private int _categoryID;
        public int CategoryID
        {
            get => _categoryID;
            set => SetProperty(ref _categoryID, value);
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

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }
    }
}
