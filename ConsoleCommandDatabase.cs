using System.Collections.Generic;
using System.Linq;

namespace Console
{
    /// <summary>
    /// Responsible for storing and executing console
    /// commands.
    /// </summary>
    public static class ConsoleCommandDatabase
    {
        public static IEnumerable<ConsoleCommand> commands { get { return database.OrderBy(kv => kv.Key).Select(kv => kv.Value); } }

        private static Dictionary<string, ConsoleCommand> database = new Dictionary<string, ConsoleCommand>();

        public static void RegisterCommand(string command, string description, string usage, cmdCallBack callback)
        {
            database[command.ToLower()] = new ConsoleCommand(command.ToLower(), description, usage, callback);
        }

        /// <summary>
        /// Removes a command from the database.
        /// </summary>
        public static void RemoveCommand(string command)
        {
            try { database.Remove(command); }
            catch { throw new NoSuchCommandException("Command " + command + " not found", command); } 
        }

        public static bool CommandExists(string command)
        {
            return database.ContainsKey(command);
        }

        /// <summary>
        /// Returns a console command. Note that the command must exist or
        /// you will get a NoSuchCommandException.
        /// </summary>
        public static ConsoleCommand GetCommand(string command)
        {
            if(CommandExists(command))
                return database[command];
            throw new NoSuchCommandException("Command " + command +" not found", command);
        }

        /// <summary>
        /// If the command exists, its callback function will
        /// be called.
        /// </summary>
        public static string ExecuteCommand(string command, params string[] args)
        {
            if (CommandExists(command.ToLower()))
            {
                // Execute command
                return database[command.ToLower()].callback(args);
            }
            return "<color=red><b>Unrecognized command</b></color>";
        }
    }
}