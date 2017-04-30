using System;
using System.Linq;

namespace Gash
{
    /// <summary>
    /// Resources used in Gash.
    /// </summary>
    public class Resources
    {
        /// <summary>
        /// Text resources.
        /// </summary>
        public class text
        {
            /// <summary>
            /// Displayed when an unknown command is called.
            /// {0} is the command called.
            /// </summary>
            public static string UnknownCommand = "Unknown command {0}.";

            /// <summary>
            /// Displayed when man command is called with unknown command or keyword as a parameter.
            /// {0} is the command or keyword parameter.
            /// </summary>
            public static string UnknownCommandOrKeywordForMan = "Unrecognized command or keyword {0}, cannot display manual page.";

            /// <summary>
            /// Displayed when a command call is missing required parameter.
            /// {0} is the command entered.
            /// </summary>
            public static string MissingParam
            {
                get => String.Format("{0} {1}{2}",
                    GConsole.ColorifyText(1,"Missing required parameter(s) for a command. See"),
                    GConsole.ColorifyText(0,"man {0}"),
                    GConsole.ColorifyText(1,"."));
            }

            /// <summary>
            /// Displayed when there was a generic failure in parsing a required parameter of a command.
            /// {0} is the command entered.
            /// </summary>
            public static string FailureParsingRequiredParam
            {
                get => String.Format("{0} {1}{2}",
                    GConsole.ColorifyText(1,"Failure parsing required parameter. See"),
                    GConsole.ColorifyText(1,"man {0}"),
                    GConsole.ColorifyText(1,"."));
            }

            /// <summary>
            /// Displayed when there was a generic failure in parsing a flags of a command.
            /// {0} are the flags entered.
            /// {1} is the command entered.
            /// </summary>
            public static string FailureParsingFlags {
                get => String.Format("{0} {1}{2}",
                    GConsole.ColorifyText(1,"Failure parsing flags \"{0}\". See"),
                    GConsole.ColorifyText(1,"man {1}"),
                    GConsole.ColorifyText(1,"."));
            }

            /// <summary>
            /// Displayed when an unknown flag(s) were present in a command call.
            /// {0} are a comma seperated flags which were entered.
            /// {1} is the command entered.
            /// </summary>
            public static string UnknownFlags
            {
                get => String.Format("{0} {1}{2}",
                    GConsole.ColorifyText(1,"Unknown flags \"{0}\". See"),
                    GConsole.ColorifyText(1,"man {1}"),
                    GConsole.ColorifyText(1,"."));
            }

            /// <summary>
            /// Pre-implemented "man" command name.
            /// </summary>
            public static string ManCommandName = "man";

            /// <summary>
            /// Intro message for man page of a command.
            /// Out of the box, only pre-implemented commands (man, list) use it.
            /// </summary>
            public static string ManHeaderIntro = "Manual page for command";

            /// <summary>
            /// Header for a name of a command for man page of the command.
            /// Out of the box, only pre-implemented commands (man, list) use it.
            /// </summary>
            public static string ManHeaderName = "NAME";

            /// <summary>
            /// Header for a synopsis of a command for man page of the command.
            /// Out of the box, only pre-implemented commands (man, list) use it.
            /// </summary>
            public static string ManHeaderSynopsis = "SYNOPSIS";

            /// <summary>
            /// Header for a description of a command for man page of the command.
            /// Out of the box, only pre-implemented commands (man, list) use it.
            /// </summary>
            public static string ManHeaderDescription = "DESCRIPTION";


            /// <summary>
            /// Description part of manual page of the man command.
            /// </summary>
            public static string ManMan
            {
                get => String.Format(@"{0}
{1} {2} {3}
{4}",
                    GConsole.ColorifyText(1, "Display a manual page for a command or a game mechanic."),
                    GConsole.ColorifyText(1, "Any text in"),
                    String.Join(", ", 
                        from h in GConsole.Settings.Higlights
                        where h != GConsole.Settings.Higlights[1]
                        select String.Format("{0}", GConsole.ColorifyText(h, h.Foreground.ToString().ToLower()))),
                    GConsole.ColorifyText(1,"colors can be passed as a parameter."),
                    GConsole.ColorifyText(1,"The parameter accepts spaces and therefore it can be multiple words."));
            }

            /// <summary>
            /// Pre-implemented "list" command name. 
            /// </summary>
            public static string ListCommandName = "list";

            /// <summary>
            /// Description part of manual page of the list command.
            /// </summary>
            public static string ListMan
            {
                get => "Display list of currently available commands.";
            }
        }
    }
}
