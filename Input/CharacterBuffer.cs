using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gash.Input
{
    internal class CharacterBuffer
    {
        private List<char> Buffer = new List<char>();
        private int CursorPosition;

        private ConsoleAccess ConsoleAccess = null;

        private int InputYPos
        {
            get => Console.WindowTop + Console.WindowHeight - 1;
        }

        public bool ProcessInput(ConsoleKeyInfo key)
        {
            bool processed = false;
            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (Buffer.Count > 0)
                    {
                        Buffer.RemoveAt(Buffer.Count - 1);
                        CursorPosition--;
                    }
                    processed = true;
                    break;
                case ConsoleKey.LeftArrow:
                    if (CursorPosition > 0) CursorPosition--;
                    processed = true;
                    break;
                case ConsoleKey.RightArrow:
                    if (CursorPosition < Buffer.Count) CursorPosition++;
                    processed = true;
                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.DownArrow:
                case ConsoleKey.Enter:
                    processed = false;
                    break;
                default:
                    if (CursorPosition >= Buffer.Count)
                    {
                        Buffer.Add(key.KeyChar);
                    }
                    else
                    {
                        Buffer[CursorPosition] = key.KeyChar;
                    }
                    CursorPosition++;
                    processed = true;
                    break;

            }
            return processed;
        }

        internal void Start()
        {
            ConsoleAccess = GConsole.Instance.RequestAccess();
            ConsoleAccess.SetCursorPosition(0, InputYPos);
        }

        public void StartLine()
        {
            CursorPosition = 0;
            Buffer.Clear();
        }

        public void ReadyForInput()
        {
            ConsoleAccess.Lock();
            ConsoleAccess.Unlock();
        }

        public void OverwriteCurrentLine(string line)
        {
            ConsoleAccess.Lock();
            ClearCurrentLine();
            Buffer = new List<char>(line.ToArray());
            CursorPosition = Buffer.Count;
            Console.Write(Characters);
            ConsoleAccess.Unlock();
        }

        public bool RestoreChracters()
        {
            if (Characters.Length > 0)
            {
                Console.Write(Characters);
                CursorPosition = Characters.Length;

                return true;
            }

            return false;
        }

        public void PostprocessApply()
        {
            ConsoleAccess.Lock();
            ClearCurrentLine();
            ConsoleAccess.Write(Characters);
            ConsoleAccess.SetCursorPosition(CursorPosition, InputYPos);
            ConsoleAccess.Unlock();
        }

        public void ClearCurrentLine()
        {
            int width = Console.CursorLeft + 1;
            ConsoleAccess.SetCursorPosition(0, InputYPos);
            Console.Write(new String(' ', width));
            ConsoleAccess.SetCursorPosition(0, InputYPos);
        }

        public string Characters
        {
            get
            {
                return new string(Buffer.ToArray());
            }
        }
    }
}
