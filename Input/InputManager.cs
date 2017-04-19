using System;
using System.Collections.Generic;
using Gash.Commands;
using System.Text;

namespace Gash.Input
{
    internal class InputManager
    {
        private CharacterBuffer Buffer = new CharacterBuffer();
        private CommandHistory History = new CommandHistory();
        private AutoComplete AutoComplete = new AutoComplete();

        public void StartThread()
        {
            do
            {
                Buffer.StartLine();
                History.StartLine();

                var key = new ConsoleKeyInfo();

                do
                {
                    key = Console.ReadKey(true);

                    if (AutoComplete.ProcessInput(key))
                    {
                        var result = AutoComplete.TryComplete(Buffer.Characters);
                        if (result.WasSuccessful == AutoCompletionResultType.SuccessOneOption)
                        {
                            Buffer.OverwriteCurrentLine(result.Results[0]);
                        }
                    }
                    else if (History.ProcessInput(key))
                    {
                        if (History.Used == false)
                        {
                            History.AddCommand(Buffer.Characters);
                            History.Used = true;
                        }

                        if (History.ValidIndex)
                        {
                            Buffer.OverwriteCurrentLine(History.HistoryString);
                        }
                    }
                    else
                    {
                        Buffer.ProcessInput(key);
                    }
                       
                    Buffer.PostprocessApply();

                } while (key.Key != ConsoleKey.Enter);

                string command = Buffer.Characters;
                Buffer.ClearLine(Buffer.InputYPos);
                if (History.Used == true) History.ClearUsedCommand();
                History.AddCommand(command);
                GConsole.Instance.Commands.ParseLine(command);

            } while (true);
        }

        public void Start()
        {
            Buffer.Start();            
        }

        public void ReadyForInput()
        {
            Buffer.ReadyForInput();
        }
    }
}
