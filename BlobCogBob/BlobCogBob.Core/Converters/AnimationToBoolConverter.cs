using System;
using System.Globalization;
using Xamarin.Forms;
namespace BlobCogBob.Core
{
    public class AnimationToBoolConverter : IValueConverter
    {
        public AnimationToBoolConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool input))
                return false;

            return input == false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
