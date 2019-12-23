using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin_JuniorProject.Behaviors
{
    public class PasswordsEqualityValidator : IValidator
    {
        public string Message { get; set; } = "Passwords are not equal";
        public Entry confirmPassword { get; set; }

        public bool Check(string value)
        {
            return value == confirmPassword.Text ;
        }
    }
}
