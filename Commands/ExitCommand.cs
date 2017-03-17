using Console.Unity;
using UnityEngine;

namespace Console.Commands
{
    public static class ExitCommand
    {
        public static readonly string name = "QUIT";
        public static readonly string description = "Quits the application.";
        public static readonly string usage = "[none]";

        public static string Execute(params string[] args)
        {
            if (args.Length == 0)
                Application.Quit();

            return "";
        }
    }
}