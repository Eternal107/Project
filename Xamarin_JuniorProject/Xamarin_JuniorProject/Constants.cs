using System;
namespace Xamarin_JuniorProject
{
    public static class Constants
    {

        public const string GOOGLE_MAPS_API_KEY = "AIzaSyBy9lLjLfNfe4HHLpFayq-0HPQMk2Sk16U";

        public const string DATA_BASE_PATH = "DataBase.3db";

        public const string EmailPatternRegex = "^[_A-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$";

        public static class NavigationParameters
        {
            public const string DeletePin = "DeletePin";
            public const string AddPin = "AddPin";
            public const string LoadFromDataBase = "LoadFromDataBase";
            public const string PinSettings = "PinSettings";
            public const string UpdatePin = "UpdatePin";
            public const string SelectedPin = "SelectedPin";
        }

        public static class MessagingCenter
        {
            public const string DeletePin = "DeletePin";
            public const string AddPin = "AddPin";
            public const string ToFirstPage = "ToFirstPage";
        }

    }

}
