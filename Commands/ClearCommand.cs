using Console.Unity;
using UnityEngine;

namespace Console.Commands
{
    public static class ClearCommand
    {
        public static readonly string name = "CLEAR";
        public static readonly string description = "Clears console screen.";
        public static readonly string usage = "[none]";

        public static string Execute(params string[] args)
        {
            Console.Clear();
            return "";
        }
    }
}