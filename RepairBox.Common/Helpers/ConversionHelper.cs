using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.Common.Helpers
{
    public class ConversionHelper
    {
        public static int ConvertToInt32(string value)
        {
            Int32.TryParse(value, out int output);
            return output;
        }
        
        public static decimal ConvertToDecimal(string value)
        {
            decimal.TryParse(value, out decimal output);
            return output;
        }

        public static long ConvertToInt64(string value)
        {
            Int64.TryParse(value, out long output);
            return output;
        }

        public static string DateTimeFormatting(DateTime value)
        {
            return value.ToString("dd-MM-yyyy HH:mm:ss");
        }
    }
}
