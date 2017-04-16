using System;
using System.Collections.Generic;
using System.Text;

namespace Gash.Commands
{
    /// <summary>
    /// Interface which defines a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Name of the command as used to call it.
        /// It has to be one word, no whitespaces. Lowercase is preferred.
        /// </summary>
        /// <returns>string with the name of the command.</returns>
        string Name();

        /// <summary>
        /// Triggered when player inputs a new line.
        /// Used to determine where the line matches this command.
        /// Use ParsingHelpers static methods for typical use-cases.
        /// </summary>
        /// <param name="line">Line the player entered.</param>
        /// <returns>Result of the parsing. See ParsingResult.</returns>
        ParsingResult Parse(string line);

        /// <summary>
        /// Triggered when player attempts to auto-complete a line.
        /// Use ParsingHelpers static methods for typical use-cases.
        /// </summary>
        /// <param name="line">Current input line.</param>
        /// <returns>Result of the auto-completion. See AutoCompletionResult.</returns>
        AutoCompletionResult AutoComplete(string line);

        /// <summary>
        /// List optional bool type of flags for this command.
        /// </summary>
        /// <returns>List of flags.</returns>
        IEnumerable<BoolFlag> GetFlags();

        /// <summary>
        /// Triggered when player calls man command on this command.
        /// </summary>
        void PrintManPage();

        /// <summary>
        /// Triggered when "list available commands" command is called.
        /// Result of this call can depend on the current game state.
        /// </summary>
        /// <returns>Whether the command is available.</returns>
        bool Available();
    }
}
