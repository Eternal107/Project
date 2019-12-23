using System;
namespace Xamarin_JuniorProject.Behaviors
{
    public interface IValidator
    {
        string Message { get; set; }
        bool Check(string value);
    }
}