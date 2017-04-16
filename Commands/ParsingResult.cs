using System;
using System.Collections.Generic;
using System.Text;

namespace Gash.Commands
{
    /// <summary>
    /// Result of a command parsing player input line.
    /// </summary>
    public enum ParsingResultType
    {
        /// <summary>
        /// Parsing the input line was succesful and matches the command
        /// </summary>
        Success,
        /// <summary>
        /// The parsing was succesful and reached the end of the input line.
        /// </summary>
        SuccessReachedEnd,
        /// <summary>
        /// The input line does not correspond to the command
        /// </summary>
        WrongCommand,
        /// <summary>
        /// A mandatory parameter missing in the input line for this command.
        /// </summary>
        MissingParam,
        /// <summary>
        /// Generic parsing failure when things go bad.
        /// </summary>
        ParsingFailure
    };


    /// <summary>
    /// Result of parsing a player's input line.
    /// </summary>
    public struct ParsingResult
    {
        /// <summary>
        /// An error, or lack of one, produced by parsing of the input line.
        /// </summary>
        public ParsingResultType Type;
    }
}
