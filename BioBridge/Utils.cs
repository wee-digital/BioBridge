using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioBridge
{
    class Utils
    {
        private static readonly CultureInfo culture = new CultureInfo("fr-FR");

        public static string Now()
        {
            return DateTime.Now.ToString(culture);
        }
    }
}
