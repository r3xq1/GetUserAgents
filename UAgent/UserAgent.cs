namespace UAgent
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Management;
    using Microsoft.Win32;
     
    // Разработчик: r3xq1 
     
    public static class UserAgent
    {
        /// <summary>
        /// Метод для получения полного пути к браузеру по имени
        /// </summary>
        /// <param name="name">Имя браузера</param>
        /// <returns>Полный путь к браузеру</returns>
        private static string GetBrowserName(string name)
        {
            const string REG = @"Software\Clients\StartMenuInternet", COM = @"\shell\open\command";
            string result = string.Empty;
            RegistryKey Apps;

            // Проверяем админ права
            if (Privilegies.IsAdmin)
            {
                try
                {
                    // Если права есть, сначала проходимся по Локальной машине и собираем
                    using (Apps = Registry.LocalMachine.OpenSubKey(REG, true))
                    {
                        foreach (string subdir in Apps.GetSubKeyNames())
                        {
                            using RegistryKey Path_To_Browsers = Apps?.OpenSubKey(string.Concat(subdir, COM));
                            string value = Path_To_Browsers?.GetValue("")?.ToString().Trim('"') ?? Path_To_Browsers?.GetValue("Path")?.ToString().Trim('"');
                            result = value.Contains(name) ? value : result;
                        }
                    }
                }
                catch { }
            }
            try
            {
                // Если прав нету, собираем из текущего пользователя ( или если есть права, то собираем из неё тоже )
                using (Apps = Registry.CurrentUser.OpenSubKey(REG, true))
                {
                    foreach (string subdir in Apps.GetSubKeyNames())
                    {
                        using RegistryKey Path_To_Browsers = Apps?.OpenSubKey(string.Concat(subdir, COM));
                        string value = Path_To_Browsers?.GetValue("")?.ToString().Trim('"') ?? Path_To_Browsers?.GetValue("")?.ToString().Trim('"');
                        result = value.Contains(name) ? value : result;
                    }
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Метод для получения версии Widows OS
        /// </summary>
        private static string OsVersion
        {
            get 
            {
                string result;
                try
                {
                    result = $"{Environment.OSVersion.Version.Major}.{Environment.OSVersion.Version.Minor}";
                }
                catch
                {
                    result = string.Empty;
                    using var OsSystem = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                    using ManagementObjectCollection ColOs = OsSystem?.Get();
                    foreach (ManagementBaseObject osElement in ColOs)
                    {
                        result = osElement["Version"]?.ToString();
                    }
                    string[] array = result.Split('.');
                    result = $"{array[0]}.{ array[1]}";
                }

                return result;
            }
        }

        /// <summary>
        /// Метод для получения текущей платформы Windows
        /// </summary>
        private static string WinPlatform
        {
            get
            {
                string result = string.Empty;
                try
                {
                    switch (Environment.OSVersion.Platform)
                    {
                        case PlatformID.Win32Windows: result = "Windows NT"; break;
                        case PlatformID.Win32NT: result = "Windows NT"; break;
                        case PlatformID.Win32S: result = "Windows 3.1"; break;
                        case PlatformID.WinCE: result = "Windows CE"; break;
                    }
                }
                catch { }
                return result;
            }
        }

        private static string ArchitectureAgent => Environment.Is64BitOperatingSystem ? "Win64; x64" : "Win32; x86";
        private static string OSBit => Environment.Is64BitOperatingSystem ? "x64" : "x86";

        public static void GetData(string savepath)
        {
            string[] browsers = new string[] { "Chrome", "Firefox", "Opera", "Edge", "Internet Explorer", "Waterfox", "Dragon", "Palemoon", "Seamonkey" };
            try
            {
                using var Agent = new StreamWriter(savepath);
               
                foreach (string names in browsers)
                {
                    string PathToBrowser = GetBrowserName(names), FileVersion = FileVersionInfo.GetVersionInfo(PathToBrowser).FileVersion;
                    string[] array = FileVersion.Split('.'); 
                    string FileVersionShort = $"{array[0]}.{array[1]}", ProdVersion = FileVersionInfo.GetVersionInfo(PathToBrowser).ProductVersion;

                    if (PathToBrowser.Contains("Chrome"))
                    {
                        Agent.WriteLine($"Mozilla/5.0 ({WinPlatform} {OsVersion}; {ArchitectureAgent}) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/{FileVersion} Safari/537.36");
                    }
                    if (PathToBrowser.Contains("Opera"))
                    {
                        Agent.WriteLine($"Mozilla/5.0 ({WinPlatform} {OsVersion}; {ArchitectureAgent}) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/{FileVersion} Safari/537.36 OPR/{ProdVersion}");
                    }
                    if (PathToBrowser.Contains("Edge"))
                    {
                        Agent.WriteLine($"Mozilla/5.0 ({WinPlatform} {OsVersion}; {ArchitectureAgent}) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/{FileVersion} Safari/537.36 Edg/{ProdVersion}");
                    }
                    if (PathToBrowser.Contains("Firefox"))
                    {
                        Agent.WriteLine($"Mozilla/5.0 ({WinPlatform} {OsVersion}; rv:{FileVersion}) Gecko/20100101 Firefox/{FileVersion}");
                    }
                    if (PathToBrowser.Contains("Internet Explorer"))
                    {
                        Agent.WriteLine($"Mozilla/5.0 ({WinPlatform} {OsVersion}; WOW64; Trident/7.0; rv:{FileVersionShort}) like Gecko");
                    }
                    if (PathToBrowser.Contains("Waterfox"))
                    {
                        Agent.WriteLine($"Mozilla/5.0 ({WinPlatform} {OsVersion}; rv:{FileVersionShort}) Gecko/20100101 Firefox/{FileVersionShort} Waterfox/{ProdVersion}");
                    }
                    if (PathToBrowser.Contains("Dragon"))
                    {
                        Agent.WriteLine($"Mozilla/5.0 ({WinPlatform} {OsVersion}; rv:{FileVersionShort}) Gecko/20100101 Firefox/{FileVersionShort} IceDragon/{ProdVersion}");
                    }
                    if (PathToBrowser.Contains("Palemoon"))
                    {
                        Agent.WriteLine($"Mozilla/5.0 ({WinPlatform} {OsVersion}; rv:{FileVersionShort}) Gecko/20100101 Firefox/{FileVersionShort} PaleMoon/{ProdVersion}");
                    }
                    if (PathToBrowser.Contains("Seamonkey"))
                    {
                        Agent.WriteLine($"Mozilla/5.0 ({WinPlatform} {OsVersion}; rv:{FileVersionShort}) Gecko/20100101 Firefox/{FileVersionShort} SeaMonkey/{ProdVersion}");
                    }
                    // Запись всех браузеров и их версии
                    // Agent.WriteLine($"Browser: {PathToBrowser} \r\nFileVersion: {FileVersion} \r\nProductVersion: {ProdVersion}\r\n");
                }
            }
            catch { }
        }
    }
}
