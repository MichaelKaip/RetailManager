using System.Configuration;

namespace RMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        // Reading configuration information from the App.Config
        public double GetTaxRate() 
        {

            var taxRateText = ConfigurationManager.AppSettings["taxRate"];

            var isValidTaxRate = double.TryParse(taxRateText, out var output);

            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The tax rat is not set properly in App.Config");
            }

            return output;
        }
    }
}
