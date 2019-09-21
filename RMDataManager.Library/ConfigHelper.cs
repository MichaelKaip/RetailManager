using System.Configuration;

namespace RMDataManager.Library
{
    public class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            var taxRateText = ConfigurationManager.AppSettings["taxRate"];

            var isValidTaxRate = decimal.TryParse(taxRateText, out var output);

            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The tax rat is not set properly in App.Config");
            }

            return output;
        }
    }
}
