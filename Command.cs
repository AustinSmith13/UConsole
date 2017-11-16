namespace UConsole
{
    public class Command : System.Attribute
    {
        public string CommandName { get { return commandName; } }
        public string Description { get { return description; } }
        public string Usage { get { return usage; } }

        private string commandName;
        private string description;
        private string usage;
        
        public Command(string command, string description, string usage)
        {
            this.commandName = command;
            this.description = description;
            this.usage = usage;
        }

        public Command(string command, string description)
        {
            this.commandName = command;
            this.description = description;
            this.usage = "[none]";
        }

        public Command(string command)
        {
            this.commandName = command;
            this.description = "Description not available.";
            this.usage = "[none]";
        }
    }

    public interface ICommand
    {
        string Execute(string[] args);
    }
}
