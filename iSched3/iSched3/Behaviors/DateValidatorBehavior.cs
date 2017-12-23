using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iSched3.Behaviors
{
    public class DateValidatorBehavior : Behavior<DatePicker>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(DateValidatorBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(DatePicker bindable)
        {
            bindable.DateSelected += DateValueChanged;
            base.OnAttachedTo(bindable);
        }

        private void DateValueChanged(object sender, DateChangedEventArgs e)
        {
            IsValid = false;
            IsValid = (((DatePicker)sender).Date > DateTime.Now);
            ((DatePicker)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(DatePicker bindable)
        {
            bindable.DateSelected -= DateValueChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}
