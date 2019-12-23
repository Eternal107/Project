using System;
using SQLite;

namespace Xamarin_JuniorProject.Database
{
    public class UserRegistrationModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Unique]
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
