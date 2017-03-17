using UnityEngine;
using System.Collections;
using Console.Commands;
using System.Collections.Generic;

namespace Console
{
    /// <summary>
    /// Example Unity Console GUI
    /// </summary>
    public class ConsoleGui : MonoBehaviour
    {

        private int height = Screen.height/2;

        private Texture2D box;
        private Texture2D suggestionBox;
        private int offset = -Screen.height / 2;
        private int scrollView = 0;
        private bool showConsole = false;
        private bool showSuggestions = false;
        private string textField = "";
        private string logs;
        private string suggestions;
        private int suggestionHeight;
        private IEnumerable<ConsoleCommand> commands;

        void Awake()
        {
            GenerateTexture();
            Application.logMessageReceived += Console.HandleLog;
            Console.onLogged += OnLogged;
            commands = ConsoleCommandDatabase.commands;

            if (!ConsoleCommandDatabase.CommandExists(HelpCommand.name))
                ConsoleCommandDatabase.RegisterCommand(HelpCommand.name,
                    HelpCommand.description, HelpCommand.usage, HelpCommand.Execute);
            if (!ConsoleCommandDatabase.CommandExists(MapCommand.name))
                ConsoleCommandDatabase.RegisterCommand(MapCommand.name,
                    MapCommand.description, MapCommand.usage, MapCommand.Execute);
            if (!ConsoleCommandDatabase.CommandExists(ExitCommand.name))
                ConsoleCommandDatabase.RegisterCommand(ExitCommand.name,
                    ExitCommand.description, ExitCommand.usage, ExitCommand.Execute);
            if (!ConsoleCommandDatabase.CommandExists(ClearCommand.name))
                ConsoleCommandDatabase.RegisterCommand(ClearCommand.name,
                    ClearCommand.description, ClearCommand.usage, ClearCommand.Execute);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.BackQuote))
            {
                height = Screen.height / 2;
                showConsole = !showConsole;

                if(showConsole == true)
                {
                    StartCoroutine("FadeIn");
                    StopCoroutine("FadeOut");
                }
                else
                {
                    StartCoroutine("FadeOut");
                    StopCoroutine("FadeIn");
                }
            }

            if(showConsole)
                InputParser();
        }

        void OnGUI()
        {
            if (!showConsole && offset <= -height)
                return;

            Texture2D defaultGUIBackgroundTexture = GUI.skin.box.normal.background;
            Texture2D defaultGUIFieldTexture = GUI.skin.textField.normal.background;

            Event e = Event.current;
            if (e.keyCode == KeyCode.Return && textField.Length != 0)
            {
                Submit(textField);
                textField = "";
            }

            GUI.depth = 100;
            GUI.skin.textField.normal.background = box;
            GUI.SetNextControlName("cmd_field"); 
            textField = GUI.TextField(new Rect(0, height + offset, Screen.width, 30), textField);
            GUI.skin.textField.normal.background = defaultGUIFieldTexture;

            GUI.skin.box.normal.background = box;
            GUI.skin.box.alignment = TextAnchor.UpperLeft;
            GUI.Box(new Rect(0,offset, Screen.width, height), logs);

            if (showSuggestions && GUI.GetNameOfFocusedControl() == "cmd_field")
            {
                GUI.skin.box.normal.background = suggestionBox;
                GUI.Box(new Rect(0, height + 30 + offset, Screen.width, suggestionHeight), suggestions);
            }
            GUI.skin.box.normal.background = defaultGUIBackgroundTexture;
        }

        void GenerateTexture()
        {
            box = new Texture2D(8, 8);
            suggestionBox = new Texture2D(8, 8);

            for(int x = 0; x < 8; ++x)
            {
                for(int y = 0; y < 8; ++y)
                {
                    if (y == 0)
                        box.SetPixel(x, y, Color.grey);
                    else
                        box.SetPixel(x, y, new Color(0,0,0,0.9f));

                    suggestionBox.SetPixel(x, y, new Color(0.1f, 0.1f, 0.1f));
                }
            }

            box.Apply();
            suggestionBox.Apply();
        }

        public void Submit(string message)
        {
            if (message.Length == 0)
                return;

            string[] tokens = message.Split(' ');
            string command = tokens[0];
            string[] args = new string[tokens.Length - 1];
            for (int i = 0; i < args.Length; ++i)
            {
                args[i] = tokens[i + 1];
            }

            message = Sanitize(message);

            Console.Log(message);
            string msg = ConsoleCommandDatabase.ExecuteCommand(command, args);
            if (msg != null && msg.Length != 0)
                Console.Log(msg);
        }

        public string Sanitize(string message)
        {
            string newMessage = string.Empty;
            for(int i = 0; i < message.Length; i++)
            {
                if (message[i] == '>' || message[i] == '<')
                    continue;

                newMessage += message[i];
            }

            return newMessage;
        }

        public void OnLogged(string log)
        {
            List<string> messages = new List<string>();
            string message = string.Empty;

            int h = height / 15;

            for(int i = 0; i < log.Length; ++i)
            {
                message += log[i];
                if(log[i] == '\n')
                {
                    messages.Add(message);
                    message = string.Empty;
                }
            }

            if(messages.Count > h)
            {
                log = string.Empty;

                for(int i = (messages.Count - h + scrollView); i < messages.Count; ++i)
                {
                    log += messages[i];
                }
            }

            logs = log;
        }

        #region Effects

        IEnumerator FadeOut()
        {
            while(offset > -height-10)
            {
                //offset -= (int) Mathf.Log10(width + offset) * 20;
                //offset -= (int) Mathf.InverseLerp(offset, width, offset);
                offset -= height/20;
                yield return null;
            }
            //Debug.Log("I made it!");
            offset = -height;
        }

        IEnumerator FadeIn()
        {
            while(offset < 0)
            {
                offset -= (int)Mathf.Lerp(offset, height, offset) / 5;
                //Debug.Log(offset);
                yield return null;
            }
            offset = 0;
        }

        #endregion

        #region Suggestion Panel

        private void InputParser()
        {
            List<string> cmdNames = new List<string>();
            string[] tokens = textField.Split(' ');
            string inputCommand = tokens[0].ToLower();

            foreach (ConsoleCommand command in commands)
            {
                string name = command.name;
                string helpName = "<color=#00ffffff>";
                bool flag = true;
                int charct = 0;

                // Shouldnt display if there is no input
                if (textField.Length == 0)
                    break;

                for (int i = 0; i < inputCommand.Length; ++i)
                {
                    if (inputCommand.Length == 0 | name.Length <= i)
                    {
                        flag = false;
                        break;
                    }

                    if (name[i] != inputCommand[i])
                    {
                        flag = false;
                        break;
                    }
                    charct++;
                    helpName += name[i];
                }
                helpName += "</color>";
                for (int i = charct; i < name.Length; ++i)
                {
                    helpName += name[i];
                }

                if (flag)
                    cmdNames.Add(helpName + ' ' +
                        "<color=#808080ff>" +
                        command.usage +
                        " " + command.description +
                        "</color>");
            }

            if (cmdNames.Count > 0)
            {
                string message = "";
                foreach (string line in cmdNames)
                    message += line + "\n";

                suggestions = message;
                ShowHelpPanel();
                ResizeHelpPanel(cmdNames.Count);
            }
            else { HideHelpPanel(); }

        }

        private void ResizeHelpPanel(int size)
        {
            suggestionHeight = size * 20;
            if (size == 1)
                suggestionHeight = 30;
        }

        private void ShowHelpPanel()
        {
            showSuggestions = true;
        }

        private void HideHelpPanel()
        {
            showSuggestions = false;
        }
        #endregion
    }
}