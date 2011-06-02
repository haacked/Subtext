using System;
using System.IO;
using System.Text;

namespace TimeZoneGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var writer = new StreamWriter("TimeZones.txt", append: false, encoding: Encoding.UTF8))
            {
                foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
                {
                    writer.WriteLine(tz.ToSerializedString());
                }
            }
        }
    }
}
