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

        public int InputYPos
        {
            get => GConsole.Instance.Output.ConsolePosition[1];
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
                        Buffer.Add(' ');
                        for(int i=Buffer.Count-2;i>=CursorPosition;i--)
                        {
                            Buffer[i + 1] = Buffer[i]; 
                        }
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

        public void OverwriteCurrentLine(string line)
        {
            ConsoleAccess.Lock();
            ClearLine(InputYPos);
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
            ClearLine(InputYPos);
            ConsoleAccess.Write(Characters);
            ConsoleAccess.SetCursorPosition(CursorPosition, InputYPos);
            ConsoleAccess.Unlock();
        }

        public void ClearLine(int yPos)
        {
            int width = Console.CursorLeft + 1;
            ConsoleAccess.SetCursorPosition(0, yPos);
            Console.Write(new String(' ', width));
            ConsoleAccess.SetCursorPosition(0, yPos);
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
