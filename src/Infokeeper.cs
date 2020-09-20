using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;


namespace src
{
    public class Infokeeper
    {
        public static string GetEnvironmentMode()
        {
            // The environmentMode should be set from the docker-compose file. 
            return Environment.GetEnvironmentVariable("ENVIRONMENT_MODE");
        }
        public static string GetSettingsFilename()
        {
            // set 'copy always' in the file properties.
            return GetBinDir() + "/settings." + GetEnvironmentMode() + ".json";
        }

        public static string GetBinDir()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
