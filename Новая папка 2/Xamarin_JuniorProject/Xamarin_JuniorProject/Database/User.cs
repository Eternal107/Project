using System;
using SQLite;

namespace Xamarin_JuniorProject.Database
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
