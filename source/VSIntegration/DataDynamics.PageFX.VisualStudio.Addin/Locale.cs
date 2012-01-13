using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace DataDynamics.PageFX.VisualStudio
{
    internal static class Locale
    {
        public static string GetString(string locale, string name)
        {
            try
            {
                //If you would like to move the command to a different menu, change the word "Tools" to the 
                //  English version of the menu. This code will take the culture, append on the name of the menu
                //  then add the command to that menu. You can find a list of all the top-level menus in the file
                //  CommandBar.resx.
                string resourceName;
                var resourceManager = new ResourceManager("DataDynamics.PageFX.VisualStudio.Addin.CommandBar", Assembly.GetExecutingAssembly());
                var cultureInfo = new CultureInfo(locale);

                if (cultureInfo.TwoLetterISOLanguageName == "zh")
                {
                    System.Globalization.CultureInfo parentCultureInfo = cultureInfo.Parent;
                    resourceName = String.Concat(parentCultureInfo.Name, name);
                }
                else
                {
                    resourceName = String.Concat(cultureInfo.TwoLetterISOLanguageName, name);
                }
                return resourceManager.GetString(resourceName);
            }
            catch
            {
                //We tried to find a localized version of the word Tools, but one was not found.
                //  Default to the en-US word, which may work for the current culture.
                return name;
            }
        }
    }
}