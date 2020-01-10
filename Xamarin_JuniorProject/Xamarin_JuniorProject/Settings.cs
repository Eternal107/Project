
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Xamarin_JuniorProject
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static int SavedUserId
        {
            get { return AppSettings.GetValueOrDefault(nameof(SavedUserId), -1); }
            set { AppSettings.AddOrUpdateValue(nameof(SavedUserId), value); }
        }
    }
}
