using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;


namespace PL
{
    public class NotRangeNumberConverter : IValueConverter
    {


        public Object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
                return String.Empty;
        }

        public Object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
