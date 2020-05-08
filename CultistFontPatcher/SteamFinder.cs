using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CultistFontPatcher
{
    class SteamFinder
    {
        public static string FindSteamPath()
        {
            string registry_key = @"SOFTWARE\Valve\Steam";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key))
            {
                if (key.GetValue("SteamPath") != null)
                { 
                    string steamPath = key.GetValue("SteamPath").ToString();
                    if (Directory.Exists(steamPath))
                        return steamPath;
                }
            }
            return null;
        }

        public static string[] GetLibraryPaths(string steamPath)
        {
            if (!Directory.Exists(steamPath))
                throw new DirectoryNotFoundException("Steam path " + steamPath + " not found");

            string libraryVdfPath = Path.Combine(steamPath, @"steamapps\libraryfolders.vdf");

            if (!File.Exists(libraryVdfPath))
                throw new FileNotFoundException("Library config file " + libraryVdfPath + " not found");

            string libraryVdfData = File.ReadAllText(libraryVdfPath);

            MatchCollection mc = Regex.Matches(libraryVdfData, "\"[0-9]*\"\\s*\"([^\"]*)\"\n");

            string[] libraryPaths = new string[1 + mc.Count];

            libraryPaths[0] = Path.Combine(steamPath, @"steamapps\common");

            for (int i = 0; i < mc.Count; i++)
            {
                var libraryPath = Path.Combine(mc[i].Groups[1].Value, @"steamapps\common");
                if (Directory.Exists(libraryPath))
                    libraryPaths[i+1] = libraryPath;
            }

            return libraryPaths;
        }
    }
}
