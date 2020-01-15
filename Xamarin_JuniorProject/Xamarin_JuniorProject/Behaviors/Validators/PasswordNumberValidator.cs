using System;
using System.Linq;

namespace Xamarin_JuniorProject.Behaviors.Validators
{
    public class PasswordNumberValidator : IValidator
    {

        public string Message { get; set; } = "Password has to contain at least one digit";
        public string Format { get; set; }

        public bool Check(string value)
        {
            var result = true;
            if (!string.IsNullOrEmpty(value))
            {
                result = value.Any(x => char.IsDigit(x));
            }

            return result;
        }
    }
}

