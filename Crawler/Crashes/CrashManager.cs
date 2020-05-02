using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Crashes
{
    internal class CrashManager
    {
        private static string fileLocation = @".\crash.dump";

        public static void Handle(Exception ex, object[] extra = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Crash occured!");
            builder.AppendLine(DateTime.Now.ToString());

            AddException(ex, builder, "Outer Exception:");

            Exception innerException = ex.InnerException;
            int count = 1;
            while(innerException != null)
            {
                AddException(innerException, builder, "Inner Exception " + count);
                count++;
                innerException = innerException.InnerException;
            }

            if(extra != null)
            {
                builder.AppendLine("Extra information:");
                foreach(object obj in extra)
                {
                    if (obj == null) continue;
                    builder.AppendLine(obj.ToString());
                }
            }

            File.WriteAllText(fileLocation, builder.ToString());
            Console.WriteLine("Crash! - Dump output to " + fileLocation);
        }

        private static void AddException(Exception ex, StringBuilder builder, string optionalPrepend = null)
        {
            if(optionalPrepend != null)
            {
                builder.AppendLine(optionalPrepend);
            }
            builder.AppendLine(ex.Message);
            builder.AppendLine(ex.StackTrace);
        }
    }
}
