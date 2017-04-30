using System;
using System.Collections.Generic;

namespace Gash.Commands
{
    internal class Man : ICommand
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

                foreach (var keyword in GConsole.Instance.Keywords)
                {
                    var resultSecond = ParsingHelpers.AutoCompleteString(line.Substring(result.RemainderStartPosition-1), keyword.Name);
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
                if(GConsole.Instance.Commands.FindMan(parameter) == false)
                {
                    if (GConsole.Instance.Keywords.FindMan(parameter) == false)
                    {
                        GConsole.WriteLine(GConsole.ColorifyText(1, 
                            String.Format(Resources.text.UnknownCommandOrKeywordForMan, parameter)));
                    }
                   
                }
            }
            return result;
        }

        public void PrintManPage()
        {
            GConsole.WriteLine(-1.0f, "{0} {1}",
                GConsole.ColorifyText(1, Resources.text.ManHeaderIntro),
                GConsole.ColorifyText(0, Name()));
            GConsole.WriteLine(-1.0f, GConsole.ColorifyText(1, Resources.text.ManHeaderName));
            GConsole.WriteLine(-1.0f, "\t{0}", GConsole.ColorifyText(1, Name()));
            GConsole.WriteLine(-1.0f, GConsole.ColorifyText(1, Resources.text.ManHeaderSynopsis));
            GConsole.WriteLine(-1.0f, "\t{0} {1}",
                GConsole.ColorifyText(0, Name()),
                GConsole.ColorifyText(ConsoleColor.Black, GConsole.Settings.Higlights[0].Foreground, "command"));
            GConsole.WriteLine(-1.0f, GConsole.ColorifyText(1, Resources.text.ManHeaderDescription));

            foreach(var line in Resources.text.ManMan.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                GConsole.WriteLine(-1.0f, line);
            }
            GConsole.WriteLine(-1.0f, " ");
        }
    }
}
