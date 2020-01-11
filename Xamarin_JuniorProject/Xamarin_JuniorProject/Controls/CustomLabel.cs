using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin_JuniorProject.Controls
{
    public class CustomLabel : Label
    {
        private TapGestureRecognizer Tap = new TapGestureRecognizer();

        public CustomLabel()
        {
            Tap.Tapped += ChangeColor;

            GestureRecognizers.Add(Tap);
        }

        public static readonly BindableProperty TappedProperty =
        BindableProperty.Create(nameof(Tapped), typeof(ICommand), typeof(CustomLabel), null,propertyChanged:TappedPropertyChanged);

        public ICommand Tapped
        {
            set { SetValue(TappedProperty, value); }
            get { return (ICommand)GetValue(TappedProperty); }
        }

        public static readonly BindableProperty IsChosenProperty =
        BindableProperty.Create(nameof(IsChosen), typeof(bool), typeof(CustomLabel), true);

        public bool IsChosen
        {
            set { SetValue(IsChosenProperty, value); }
            get { return (bool)GetValue(IsChosenProperty); }
        }

        public static readonly BindableProperty StringContainingProperty =
        BindableProperty.Create(nameof(StringContaining), typeof(string), typeof(CustomLabel), null,propertyChanged:StringContainingPropertyChanged);

        public string StringContaining
        {
            set { SetValue(StringContainingProperty, value); }
            get { return (string)GetValue(StringContainingProperty); }
        }

        private static void StringContainingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customLabel = (CustomLabel)bindable;
            if (newValue != null && customLabel.Text!=null)
            {
                if ((newValue as string).Contains(customLabel.Text))
                {
                    customLabel.IsChosen = false;
                    customLabel.BackgroundColor = Color.DarkGray;
                }
            }
        }

        private static void TappedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customLabel = (CustomLabel)bindable;

            if (oldValue != null)
            {
                customLabel.Tap.Tapped -= (o, e) => (oldValue as ICommand)?.Execute(o);
            }
            if (newValue != null)
            {
                customLabel.Tap.Tapped += (o,e)=>(newValue as ICommand)?.Execute(o);
            }
        }

        private void ChangeColor (object o,EventArgs e)
        {
           
                var customLabel = o as CustomLabel;
                if (customLabel.IsChosen)
                {
                    customLabel.BackgroundColor = Color.DarkGray;
                    customLabel.IsChosen = false;
                }
                else
                {
                    customLabel.BackgroundColor = Color.Default;
                    customLabel.IsChosen = true;
                }
        }
    }
}
