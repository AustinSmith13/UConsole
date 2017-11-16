
using UnityEngine;
using System.Collections;
using System;


namespace UConsole
{
    /// <summary>
    /// Handles logging events.
    /// </summary>
    public static class Console
    {

        #region Properties

        public static string Logs { get { return log; } private set { log = value; } }

        #endregion

        public static event Action<string> onLogged;

        public static void HandleLog(string message, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    Console.Log("<color=red><b>Error: </b></color>" + message);
                    break;
                case LogType.Exception:
                    Console.Log("<color=orange><b>Exception: </b></color>" + message);
                    break;
                case LogType.Warning:
                    Console.Log("<color=yellow><b>Warning: </b></color>" + message);
                    break;
                case LogType.Assert:
                    Console.Log("<color=0000FF><b>Assert: </b></color>" + message);
                    break;
                case LogType.Log:
                    Console.Log("<color=gray>" + message + "</color>");
                    break;
            }     
        }

        private static string log = "";

        /// <summary>
        /// Displays a message to console in red.
        /// </summary>
        public static void LogError(string message)
        {
            Log("<color=red><b>Error: </b></color>" + message);
        }

        public static void LogOk(string message)
        {
            Log("[ <color=green>OK</color> ] " + message);
        }

        public static void LogFailed(string message)
        {
            Log("[ <color=red>FAILED</color> ] " + message);
        }

        /// <summary>
        /// Displays a message to console.
        /// </summary>
        public static void Log(string message)
        {
            log += message + "\n";
            ClipLog();
            onLogged(log);
        }

        public static void Clear()
        {
            log = string.Empty;
            onLogged(log);
        }

        private static void ClipLog()
        {
            if (log.Length < 12000)
                return;
            string[] lines = log.Split('\n');
            string[] newLines = new string[lines.Length - 2];
            for (int i = 0; i < newLines.Length; ++i)
            {
                newLines[i] = lines[i + 1];
            }
            log = "";
            foreach (string line in newLines)
                log += line + '\n';
            if (log.Length > 12000)
                ClipLog();
        }
    }
}