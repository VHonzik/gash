using System;
using System.Collections.Generic;
using System.Text;

namespace Gash.Output
{
    internal class OutputLine
    {
        public string Line = "";
        public int RealCharLineIndex = -1;
        public int LineIndex = -1;
        public float Speed;
        public int[] ConsolePos;
        public bool SpecialColor = false;
        public int DotCount;

        public ConsoleColor LastCharForeground = ConsoleColor.Gray;
        public ConsoleColor LastCharBackground = ConsoleColor.Black;
    }
}
