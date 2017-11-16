using UnityEngine;

namespace UConsole.Commands
{
    [UConsole.Command("QUIT", "Quits the application.")]
    public class ExitCommand : ICommand
    {
        public string Execute(params string[] args)
        {
            if (args.Length == 0)
                Application.Quit();

            return "";
        }
    }
}