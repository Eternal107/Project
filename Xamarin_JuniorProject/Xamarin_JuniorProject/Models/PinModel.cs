using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Xamarin.Forms.GoogleMaps;

namespace Xamarin_JuniorProject.Models
{
    [Table(nameof(PinModel))]
    public class PinModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public bool IsFavorite { get; set; }
        public string ImagePath { get; set; }
        [TextBlob("CategoriesBlob")]
        public List<CategoryModel> Categories { get; set; }
    }
}
