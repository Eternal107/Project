using SQLite;

namespace Xamarin_JuniorProject.Models
{
    public class UserRegistrationModel : ISQLModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Unique]
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
