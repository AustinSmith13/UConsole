
namespace UConsole
{
    public delegate string cmdCallBack(params string[] args);

    public struct ConsoleCommand
    {
        public string name;
        public string description;
        public string usage;
        public cmdCallBack callback;

        public ConsoleCommand(string name, string description, string usage, cmdCallBack callback) : this()
        {
            this.name = name;
            this.description = (string.IsNullOrEmpty(description.Trim()) ? "No description provided" : description);
            this.usage = (string.IsNullOrEmpty(usage.Trim()) ? "[none]" : usage);
            this.callback = callback;
        }
    }
}
