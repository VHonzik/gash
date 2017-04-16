using System;
using System.Collections.Generic;
using System.Text;

namespace Gash.Input
{
    internal class CommandHistory
    {
        private int index;
        private List<string> history = new List<string>();

        public bool Used { get; set; }

        public void StartLine()
        {
            index = history.Count;
            Used = false;
        }

        public bool ProcessInput(ConsoleKeyInfo key)
        {
            bool processed = false;
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (history.Count > 0 && index >= 1) index--;
                    processed = true;
                    break;
                case ConsoleKey.DownArrow:
                    if (history.Count > 0 && index < history.Count - 1) index++;
                    processed = true;
                    break;
                default:
                    processed = false;
                    break;
            }
            return processed;
        }

        public bool ValidIndex
        {
            get
            {
                return (index >= 0 && index < history.Count);
            }
        }

        public string HistoryString
        {
            get
            {
                return history[index];
            }
        }

        public void AddCommand(string command)
        {
            history.Add(command);
        }

        public void ClearUsedCommand()
        {
            history.RemoveAt(history.Count - 1);
        }
    }
}
