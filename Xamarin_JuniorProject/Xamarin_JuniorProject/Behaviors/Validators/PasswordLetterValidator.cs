using System;
using System.Linq;

namespace Xamarin_JuniorProject.Behaviors.Validators
{
    public class PasswordLetterValidator : IValidator
    {
        public string Message { get; set; } = "Password has to contain at Least one letter";
        public string Format { get; set; }

        public bool Check(string value)
        {
            var result = true;
            if (!string.IsNullOrEmpty(value))
            {
                result = value.Any(x => char.IsLetter(x)); ;
            }

            return result;
        }
    }
}
