using System.Collections.Generic;

namespace Console
{
    /// <summary>
    /// Keeps track of input history and also allows you
    /// to navigate input history.
    /// </summary>
    public class ConsoleHistory
    {
        private List<string> inputHistory = new List<string>();
        private int maxcapacity = 20;
        private int index = 0;

        public string up()
        {
            if (inputHistory.Count == 0)
                return "";

            if (index < inputHistory.Count - 1)
                index++;
            return inputHistory[index];
        }

        public string down()
        {
            if (inputHistory.Count == 0)
                return "";

            if (index > 0)
                index--;
            return inputHistory[index];
        }

        public void Reset()
        {
            index = 0;
        }

        public void AddInput(string input)
        {
            if (inputHistory.Count > 0)
                if (input.Equals(inputHistory[0]))
                    return;

            if (inputHistory.Count > maxcapacity - 1)
                inputHistory.RemoveAt(maxcapacity - 1);

            inputHistory.Insert(0, input);
        }
    }
}