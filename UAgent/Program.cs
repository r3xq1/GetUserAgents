namespace UAgent
{
    using System;

    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            Console.Title = "UserAgent Grabber by r3xq1";
            Console.WriteLine("Инициализация...");
            UserAgent.GetData(GlobalPaths.AgentsLog);
            Console.WriteLine("Закончили сбор Юзерагентов! Посмотрите рядом файл \".txt\"");
            Console.ReadLine();
        }
    }
}