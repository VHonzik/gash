using System;
using System.Collections.Generic;
using System.Text;

namespace Gash.Commands
{
    class List : ICommand
    {
        private BoolFlag[] Flags = new BoolFlag[0];

        public AutoCompletionResult AutoComplete(string line)
        {
            return ParsingHelpers.AutoCompleteCommandBody(line, this);
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
            return Resources.text.ListCommandName;
        }

        public ParsingResult Parse(string line)
        {
            var result = ParsingHelpers.ParseSimpleCommand(line, this);

            if (result.Type == ParsingResultType.Success)
            {
                foreach (var command in GConsole.Instance.Commands)
                {
                    if (command.Available() == true)
                    {
                        GConsole.WriteLine(-1.0f, "\t{0}",
                            GConsole.HighlightTextAsCommandOrKeyword(command.Name()));
                    }
                }
            }

            return result;
        }

        public void PrintManPage()
        {
            GConsole.WriteLine(-1.0f, Resources.text.ManHeaderName);
            GConsole.WriteLine(-1.0f, "\t{0}", GConsole.HighlightTextAsCommandOrKeyword(Name()));
            GConsole.WriteLine(Resources.text.ManHeaderSynopsis);
            GConsole.WriteLine(-1.0f, "\t{0}",
                GConsole.HighlightTextAsCommandOrKeyword(Name()));
            GConsole.WriteLine(-1.0f, Resources.text.ManHeaderDescription);

            GConsole.WriteLine(-1.0f, Resources.text.ListMan);
        }
    }
}
