using System;
using System.Collections.Generic;
using System.Text;

namespace Gash.Commands
{
    class Man : ICommand
    {
        private BoolFlag[] Flags = new BoolFlag[0];

        public AutoCompletionResult AutoComplete(string line)
        {
            var result = ParsingHelpers.AutoCompleteCommandBody(line, this);
            if (result.WasSuccessful == AutoCompletionResultType.FailureAlreadyComplete)
            {
                foreach (var command in GConsole.Instance.Commands)
                {
                    var resultSecond = ParsingHelpers.AutoCompleteCommandBody(line.Substring(result.RemainderStartPosition), command);
                    if (resultSecond.WasSuccessful == AutoCompletionResultType.SuccessOneOption)
                    {
                        var finalResult = new AutoCompletionResult();
                        finalResult.WasSuccessful = AutoCompletionResultType.SuccessOneOption;
                        finalResult.Results.Add(line.Substring(0, result.RemainderStartPosition - 1) + " " + resultSecond.Results[0]);
                        return finalResult;
                    }
                }
            }

            return result;
        }

        public bool Available()
        {
            return true;
        }

        public IEnumerable<BoolFlag> GetFlags()
        {
            return Flags;
        }

        public string Name()
        {
            return Resources.text.ManCommandName;
        }

        public ParsingResult Parse(string line)
        {
            string parameter = "";
            var result = ParsingHelpers.ParseSimpleCommandWithOneParameter(line, this, out parameter);
            if (result.Type == ParsingResultType.Success)
            {
                GConsole.Instance.Commands.FindMan(parameter);
            }
            return result;
        }

        public void PrintManPage()
        {
            GConsole.WriteLine(Resources.text.ManHeaderName);
            GConsole.WriteLine("\t{0}", GConsole.HighlightTextAsCommandOrKeyword(Name()));
            GConsole.WriteLine(Resources.text.ManHeaderSynopsis);
            GConsole.WriteLine("\t{0} {1}", 
                GConsole.HighlightTextAsCommandOrKeyword(Name()),
                GConsole.ColorifyText(ConsoleColor.Black, ConsoleColor.Red,"command"));
            GConsole.WriteLine(Resources.text.ManHeaderDescription);

            foreach(var line in Resources.text.ManMan.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                GConsole.WriteLine(line);
            }
        }
    }
}
