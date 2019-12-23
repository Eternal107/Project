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


        public void ShowError(View view, string message)
        {
            StackLayout layout = view.Parent as StackLayout;
            int viewIndex = layout.Children.IndexOf(view);


            View sibling = layout.Children[viewIndex + 1];
            string siblingStyleId = view.Id.ToString();

            if (sibling.StyleId == siblingStyleId)
            {
                Label errorLabel = sibling as Label;
                errorLabel.Text = message;
                errorLabel.IsVisible = true;


            }
            else
            {
                layout.Children.Insert(viewIndex + 1, new Label
                {
                    Text = message,
                    FontSize = 10,
                    StyleId = view.Id.ToString(),
                    TextColor = Color.Red
                });
            }
           
        }

        public void RemoveError(View view)
        {
            StackLayout layout = view.Parent as StackLayout;
            int viewIndex = layout.Children.IndexOf(view);


            View sibling = layout.Children[viewIndex + 1];
            string siblingStyleId = view.Id.ToString();

            if (sibling.StyleId == siblingStyleId)
            {
                sibling.IsVisible = false;
            }

        }

    }
}
