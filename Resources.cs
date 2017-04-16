using System;
using System.Collections.Generic;
using System.Text;

namespace Gash
{
    public class Resources
    {
        public class text
        {
            public static string UnknownCommand = "Unknown command {0}.";
            public static string UnknownCommandForMan = "Unrecognized command {0}, cannot display manual page.";
            public static string MissingParam
            {
                get => String.Format("Missing required parameter for a command. See {0}.",
                    GConsole.HighlightTextAsCommandOrKeyword("man {0}"));
            }

            public static string FailureParsingRequiredParam
            {
                get => String.Format("Failure parsing required parameter. See {0}.",
                    GConsole.HighlightTextAsCommandOrKeyword("man {0}"));
            }

            public static string FailureParsingFlags {
                get => String.Format("Failure parsing flags \"{{0}\". See {0}.",
                GConsole.HighlightTextAsCommandOrKeyword("man {1}"));
            }

            public static string UnknownFlags
            {
                get => String.Format("Unknown flags {{0}. See {0}.",
                    GConsole.HighlightTextAsCommandOrKeyword("man {1}"));
            }
            public static string ManCommandName = "man";
            public static string ManHeaderName = "NAME";
            public static string ManHeaderSynopsis = "SYNOPSIS";
            public static string ManHeaderDescription = "DESCRIPTION";
            public static string ManMan
            {
                get => String.Format(@"Display a manual page for a command or a game mechanic.
Any text in {0} color can be passed as a parameter. 
The parameter accepts spaces and therefore it can be multiple words.",
                    GConsole.HighlightTextAsCommandOrKeyword("cyan"));
            }
                
        }
    }
}
