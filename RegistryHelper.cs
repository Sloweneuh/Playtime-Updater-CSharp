using Microsoft.Win32;
using System;
using System.Net.NetworkInformation;

namespace PlaytimeUpdater
{
    public static class RegistryHelper
    {
        public static int ReadPlaytimeFromRegistry(string registryPath)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("CollapseLauncher_Playtime");
                        if (value is string strValue)
                        {
                            return int.Parse(strValue, System.Globalization.NumberStyles.HexNumber);
                        }
                        else if (value is int intValue)
                        {
                            return intValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading registry: {ex.Message}");
            }
            return 0;
        }

        public static void SavePlaytimeToRegistry(string registryPath, int value)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath))
                {
                    key.SetValue("CollapseLauncher_Playtime", value, RegistryValueKind.DWord);
                    Console.WriteLine($"Successfully saved playtime to registry: {value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving playtime to registry: {ex.Message}");
            }
        }

        public static string GetRegistryPlaytimeDisplay(string registryPath)
        {
            int playtime = ReadPlaytimeFromRegistry(registryPath); //in seconds
            int hours = playtime / 3600;
            int minutes = (playtime % 3600) / 60;
            return $"Registry : {hours}h {minutes}m";
        }

        public static bool CheckRegistryEntry(string path)
        {
            //check if playtime is saved in registry
            RegistryKey Key = Registry.CurrentUser.OpenSubKey(path);
            if (Key.GetValue("CollapseLauncher_Playtime") == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}