using Havamal.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Havamal.Converters
{
    public class CountryImageConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valAsString = (string)value;
            return CountryCodeToImageHelper.Get(valAsString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valAsString = (string)value;
            return CountryCodeToImageHelper.Revert(valAsString);
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
