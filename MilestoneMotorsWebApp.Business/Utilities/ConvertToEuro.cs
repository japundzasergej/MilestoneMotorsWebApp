using System.Globalization;

namespace MilestoneMotorsWebApp.Business.Utilities;

public class ConvertToEuroMethod
{
    public static string ConvertToEuro(double input)
    {
        return string.Format(new CultureInfo("de-De"), "{0:N0} €", input);
    }
}
