namespace UAgent
{
    using System;
    using System.IO;
    using System.Reflection;

    public static class GlobalPaths
    {
        public static string CurrDir => Environment.CurrentDirectory;
        public static string AgentsLog => Path.Combine(CurrDir, "Agents.txt");
        public static string AssemblyPath => Assembly.GetExecutingAssembly().Location;
        public static string StartupPath => Path.GetDirectoryName(AssemblyPath);
        public static string GetFileName => Path.GetFileName(AppDomain.CurrentDomain.FriendlyName);
        public static string AppData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string SelfHelper => Path.Combine(AppData, "Self.bat");
    }
}