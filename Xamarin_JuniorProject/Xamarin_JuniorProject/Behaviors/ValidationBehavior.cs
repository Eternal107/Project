using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Xamarin_JuniorProject.Behaviors
{
    public class ValidationBehavior : Behavior<View>
    {

        View _view;
        public string PropertyName { get; set; }
        public ValidationGroupBehavior Group { get; set; }
        public StackLayout stackLayout { get; set; }
        public ObservableCollection<IValidator> Validators { get; set; } = new ObservableCollection<IValidator>();

        public bool Validate()
        {

            bool isValid = true;
            string errorMessage = "";

            foreach (IValidator validator in Validators)
            {
                isValid = validator.Check(_view.GetType()
                                       .GetProperty(PropertyName)
                                       .GetValue(_view) as string);


                if (!isValid)
                {
                    errorMessage = validator.Message;
                    break;
                }
            }
            if (Group != null)
                if (!isValid)
                {
                    Group.ShowError(_view, errorMessage);
                }
                else
                {
                    Group.RemoveError(_view);
                }

            return isValid;
        }

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
            _view = bindable as View;
            _view.PropertyChanged += OnPropertyChanged;


            if (Group != null)
            {
                Group.Add(this);
            }
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);

            _view.PropertyChanged -= OnPropertyChanged;


            if (Group != null)
            {
                Group.Remove(this);
            }
        }



        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PropertyName)
            {
                Validate();

                if (Group != null)
                {
                    Group.Update();
                }
            }

        }
    }
}