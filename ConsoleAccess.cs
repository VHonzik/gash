using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Gash
{
    internal class ConsoleAccess
    {
        private Mutex ConsoleMutex;

        private ConsoleColor DefBgColor = Console.BackgroundColor;
        private ConsoleColor DefFgColor = Console.ForegroundColor;

        private ConsoleColor BgColor = Console.BackgroundColor;
        public ConsoleColor BackgroundColor
        {
            get => BgColor;
            set {
                BgColor = value;
                Console.BackgroundColor = BgColor;
            }
        }

        private ConsoleColor FgColor = Console.ForegroundColor;
        public ConsoleColor ForegroundColor
        {
            get => FgColor;
            set
            {
                FgColor = value;
                Console.ForegroundColor = FgColor;
            }
        }

        private int[] ConsolePos = new int[2] { 0, 0 };

        public int[] ConsolePosition
        {
            get => ConsolePos;
            set
            {
                ConsolePos = value;
                Console.SetCursorPosition(ConsolePos[0], ConsolePos[1]);
            }
        }

        public void SetCursorPosition(int x, int y)
        {
            ConsolePosition = new int[2] { x, y };
        }

        public ConsoleAccess(Mutex consoleMutex)
        {
            ConsoleMutex = consoleMutex;
        }

        public void Lock()
        {
            ConsoleMutex.WaitOne();
            Console.SetCursorPosition(ConsolePos[0], ConsolePos[1]);
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;
        }

        public void Write(char character)
        {
            Console.Write(character);
        }

        public void Write(string line)
        {
            Console.Write(line);
        }

        public void ResetColor()
        {
            ForegroundColor = DefFgColor;
            BackgroundColor = DefBgColor;
        }

        public void Unlock()
        {
            ConsolePos = new int[2] { Console.CursorLeft, Console.CursorTop };
            ConsoleMutex.ReleaseMutex();
        }
    }
}
