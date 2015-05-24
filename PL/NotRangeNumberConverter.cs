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


        public object Convert(object value,Type targetType,object parameter,CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
                return String.Empty;
            else
                return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }




    }
}
