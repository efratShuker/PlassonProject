using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MachineStatusTracker.Converters
{
    internal class StatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (MachineStatusEnum)value;
            switch (status)
            {
                case MachineStatusEnum.Running:
                    return "./Icons/icons8-gears-50.png";
                case MachineStatusEnum.Idle:
                    return "./Icons/icons8-error-64.png";
                case MachineStatusEnum.Offline:
                    return "./Icons/icons8-offline-48.png";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
