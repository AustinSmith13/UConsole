namespace UConsole.Commands
{
    [UConsole.Command("CLEAR", "Clears console screen.")]
    public class ClearCommand : ICommand
    {
        public string Execute(params string[] args)
        {
            Console.Clear();
            return "";
        }
    }
}