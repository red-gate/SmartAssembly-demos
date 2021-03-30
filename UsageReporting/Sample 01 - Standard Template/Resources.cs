using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace SmartAssembly.SmartExceptionsCore
{
    internal class Resources
    {
        public static Bitmap GetBitmap(string key)
        {
            try
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SmartAssembly.SmartUsageWithUI.Resources." + key + ".png");
                return (stream == null) ? null : new Bitmap(stream);
            }
            catch
            {
                return null;
            }
        }

        public static Icon GetIcon(string key)
        {
            try
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SmartAssembly.SmartUsageWithUI.Resources." + key + ".ico");
                return (stream == null) ? null : new Icon(stream);
            }
            catch
            {
                return null;
            }
        }
    }
}