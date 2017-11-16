using System.Text;

namespace UConsole.Commands
{
    [UConsole.Command("HELP", "Display the list of available commands or details about a specific command.", "[command]")]
    public class HelpCommand : ICommand
    {
        private StringBuilder commandList = new StringBuilder();

        public string Execute(params string[] args)
        {
            if (args.Length == 0)
            {
                return DisplayAvailableCommands();
            }
            else
            {
                return DisplayCommandDetails(args[0]);
            }
        }

        private string DisplayAvailableCommands()
        {
            commandList.Length = 0; // clear the command list before rebuilding it
            commandList.Append("<b>Available Commands</b>\n");

            foreach (ConsoleCommand command in ConsoleCommandDatabase.commands)
            {
                commandList.Append(string.Format("    <b>{0}</b> - <color=grey>{1}</color>\n", command.name, command.description));
            }

            commandList.Append("To display details about a specific command, type 'HELP' followed by the command name.");
            return commandList.ToString();
        }

        private string DisplayCommandDetails(string commandName)
        {
            string formatting =
            @"<b>{0} Command</b>
            <b>Description:</b> {1}
            <b>Usage:</b> {2}";

            try
            {
                ConsoleCommand command = ConsoleCommandDatabase.GetCommand(commandName);
                return string.Format(formatting, command.name, command.description, command.usage);
            }
            catch (NoSuchCommandException exception)
            {
                return string.Format("Cannot find help information about {0}. Are you sure it is a valid command?", exception.command);
            }
        }
        
    }
}