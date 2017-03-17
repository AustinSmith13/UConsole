using Console.Unity;
using System;
using UnityEngine;

namespace Console.Commands
{
    /// <summary>
    /// Deperecated
    public static class MapCommand
    {
        public static readonly string name = "MAP";
        public static readonly string description = "Loads the specified map.";
        public static readonly string usage = "[map]";

        private static string[] levels;

        public static string Execute(params string[] args)
        {
            levels = Game.Levels;

            if (args.Length == 0)
                return DisplayMapDetails();

            // does the level exist?
            foreach(string level in levels)
            {
                if (level == args[0])
                    return LoadMap(args[0]);
            }

            return "No such map";
        }

        private static string DisplayMapDetails()
        {
            string output = string.Empty;

            output += "<b>Map List</b>\n";

            foreach(string level in levels)
            {
                output += "     -" + level + '\n';
            }

            return output;
        }

        //[Obsolete("Do not use! This will change to loading XML style Creo maps.")]
        private static string LoadMap(string map)
        {

            //Application.LoadLevel(map);
            UnityEngine.SceneManagement.SceneManager.LoadScene(map);
            return "Loading " + map + ".Creo";
        }
    }
}