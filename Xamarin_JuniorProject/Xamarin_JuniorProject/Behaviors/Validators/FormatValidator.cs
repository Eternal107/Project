using System.Text.RegularExpressions;

namespace Xamarin_JuniorProject.Behaviors.Validators
{
    public class FormatValidator : IValidator
    {
        public string Message { get; set; } = "Invalid format";
        public string Format { get; set; }

        public bool Check(string value)
        {
            var result = true;
            if (!string.IsNullOrEmpty(value))
            {
                Regex format = new Regex(Format);

                result = format.IsMatch(value);
            }

            return result;
        }
    }
}