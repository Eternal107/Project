namespace Xamarin_JuniorProject.Behaviors.Validators
{
    public interface IValidator
    {
        string Message { get; set; }
        bool Check(string value);
    }
}