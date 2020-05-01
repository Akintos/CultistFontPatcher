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
                if (key.GetValue("SteamPath") == null)
                {
                    return null;
                }
                else
                {
                    string steamPath = key.GetValue("SteamPath").ToString();

                    if (Directory.Exists(steamPath))
                    {
                        return steamPath;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static string[] GetLibraryPaths(string steamPath)
        {
            if (!Directory.Exists(steamPath))
            {
                throw new DirectoryNotFoundException("Steam path " + steamPath + " not found");
            }

            string libraryVdfPath = Path.Combine(steamPath, @"steamapps\libraryfolders.vdf");

            if (!File.Exists(libraryVdfPath))
            {
                throw new FileNotFoundException("Library config file " + steamPath + " not found");
            }

            string libraryVdfData = File.ReadAllText(libraryVdfPath);

            Regex regex = new Regex("\"[0-9]*\"\\s*\"([^\"]*)\"\n");

            MatchCollection mc = regex.Matches(libraryVdfData);

            string[] libraryPaths = new string[1 + mc.Count];

            libraryPaths[0] = Path.Combine(steamPath, @"steamapps\common");

            int i = 1;

            foreach (Match m in mc)
            {
                libraryPaths[i] = Path.Combine(m.Groups[1].Value, @"steamapps\common");
                i++;
            }

            return libraryPaths;
        }
    }
}
