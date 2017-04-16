using System;
using System.Collections.Generic;
using System.Text;

namespace Gash.Input
{
    internal class InputManager
    {
        private CharacterBuffer Buffer = new CharacterBuffer();

        public void StartThread()
        {
            do
            {
                Buffer.StartLine();

                var key = new ConsoleKeyInfo();

                do
                {
                    key = Console.ReadKey(true);
                    Buffer.ProcessInput(key);
                    Buffer.PostprocessApply();

                } while (key.Key != ConsoleKey.Enter);

            } while (true);
        }

        public void Start()
        {
            Buffer.Start();            
        }
    }
}
