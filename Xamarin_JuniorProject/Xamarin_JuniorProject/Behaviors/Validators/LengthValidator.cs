using System;
namespace Xamarin_JuniorProject.Behaviors.Validators
{
    public class LengthValidator : IValidator
    {
        public string Message { get; set; } 
        public string Format { get; set; }
        public int MinimalLength { get; set; }

        public bool Check(string value)
        {
            var result = true;
            if (!string.IsNullOrEmpty(value))
            {
                result = value.Length >= MinimalLength ? true : false;
            }

            return result;
        }
    }
}
