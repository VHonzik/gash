using System.Collections.Generic;

namespace Gash.Commands
{
    internal class List : ICommand
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
                GConsole.WriteLine(-1.0f, "{0} {1} {2}",
                    GConsole.ColorifyText(1,"Currently available commands follow. You can use"),
                    GConsole.ColorifyText(0,Resources.text.ManCommandName),
                    GConsole.ColorifyText(1,"to learn more about them."));

                foreach (var command in GConsole.Instance.Commands)
                {
                    if (command.Available() == true)
                    {
                        GConsole.WriteLine(-1.0f, "\t{0}",
                            GConsole.ColorifyText(0,command.Name()));
                    }
                }
            }

            return result;
        }

        public void PrintManPage()
        {
            GConsole.WriteLine(-1.0f, "{0} {1}",
                GConsole.ColorifyText(1,Resources.text.ManHeaderIntro),
                GConsole.ColorifyText(1,Name()));
            GConsole.WriteLine(-1.0f, GConsole.ColorifyText(1,Resources.text.ManHeaderName));
            GConsole.WriteLine(-1.0f, "\t{0}", GConsole.ColorifyText(0,Name()));
            GConsole.WriteLine(-1.0f, GConsole.ColorifyText(1,Resources.text.ManHeaderSynopsis));
            GConsole.WriteLine(-1.0f, "\t{0}",
                GConsole.ColorifyText(0,Name()));
            GConsole.WriteLine(-1.0f, GConsole.ColorifyText(1,Resources.text.ManHeaderDescription));

            GConsole.WriteLine(-1.0f, GConsole.ColorifyText(1,Resources.text.ListMan));
            GConsole.WriteLine(-1.0f, " ");
        }
    }
}
