using Gash;

namespace GashLibrary.Commands
{
    /// <summary>
    /// Utility implementation of IKeyword for simple text only keywords.
    /// Use CreateSimple to create an instance of it.
    /// </summary>
    public class Keyword : IKeyword
    {
        /// <summary>
        /// Creates an instance of Keyword with default highlight.
        /// </summary>
        /// <param name="name">Name of the command.</param>
        /// <param name="manPage">Composite format string of the string to print,
        /// when a man command is called on this keyword.
        /// First parameter is name of the keyword and the second is colored named of the keyword.</param>
        /// <returns></returns>
        public static Keyword CreateSimpleFormatted(string name, string manPage)
        {
            Keyword result = new Keyword();
            result.Highlight = GConsole.Settings.Higlights[0];
            result.RawName = name;
            result.ManFormat = manPage;
            return result;
        }

        private HighlightType Highlight;

        private string ManFormat = "";

        private string RawName = "";
        public string Name
        {
            get => RawName;
        }

        public string ColoredName
        {
            get => GConsole.ColorifyText(Highlight, RawName);
        }

        public void PrintManPage()
        {
            GConsole.WriteLine(-1.0f, ManFormat, RawName, ColoredName);
            GConsole.WriteLine(" ");
        }
    }
}
