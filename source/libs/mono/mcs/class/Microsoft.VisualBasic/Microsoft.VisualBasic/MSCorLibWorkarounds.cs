namespace Microsoft.VisualBasic
{
    using System;
    using System.Globalization;

    internal sealed class MSCorLibWorkarounds
    {
        private MSCorLibWorkarounds()
        {
        }

        internal static bool DateTimeTryParse(string s, IFormatProvider provider, DateTimeStyles styles, ref DateTime result)
        {
            bool flag;
            try
            {
                result = DateTime.Parse(s, provider, styles);
                flag = true;
            }
            catch (ArgumentNullException)
            {
                flag = false;
            }
            catch (FormatException)
            {
                flag = false;
            }
            catch (OverflowException)
            {
                flag = false;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        internal static bool DoubleTryParse(string s, NumberStyles style, IFormatProvider provider, ref double result)
        {
            bool flag;
            try
            {
                result = double.Parse(s, style, provider);
                flag = true;
            }
            catch (ArgumentNullException)
            {
                flag = false;
            }
            catch (FormatException)
            {
                flag = false;
            }
            catch (OverflowException)
            {
                flag = false;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }
    }
}

