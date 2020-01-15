using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin_JuniorProject.Behaviors
{
    public class ValidationGroupBehavior : Behavior<View>
    {
        private IList<ValidationBehavior> _validationBehaviors;

        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid),
                                    typeof(bool),
                                    typeof(ValidationGroupBehavior),
                                    false);

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }


        public ValidationGroupBehavior()
        {
            _validationBehaviors = new List<ValidationBehavior>();
        }

        public void Add(ValidationBehavior validationBehavior)
        {
            _validationBehaviors.Add(validationBehavior);
        }

        public void Remove(ValidationBehavior validationBehavior)
        {
            _validationBehaviors.Remove(validationBehavior);
        }

        public void Update()
        {
            bool isValid = true;

            foreach (ValidationBehavior validationItem in _validationBehaviors)
            {
                isValid = isValid && validationItem.Validate();
            }

            IsValid = isValid;
        }


        public void ShowError(Label ErrorMessage, string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.TextColor = Color.Red;
        }

        public void RemoveError(Label ErrorMessage)
        {
            ErrorMessage.TextColor = Color.Transparent;
        }
    }
}
