using System;
using SQLite;

namespace Xamarin_JuniorProject.Models
{
    public class CategoryModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Category { get; set; }
    }
}
