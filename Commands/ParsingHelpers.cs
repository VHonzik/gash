﻿using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gash.Commands
{
    /// <summary>
    /// Collection of static helper functions for parsing and auto-completing.
    /// </summary>
    public class ParsingHelpers
    {
        private static Parser<string> ShortFlag = 
            from trailing in Parse.WhiteSpace.AtLeastOnce()
            from hyphen in Parse.Char('-')
            from flags in Parse.Letter.Many()
            select String.Concat(flags.Select(x => x.ToString().ToLower()));


        private static Parser<IEnumerable<string>> ShortFlagMultMulti = ShortFlag.Many();

        private static Parser<IEnumerable<string>> Flags = ShortFlagMultMulti;

        private static Parser<string> AnyCommandBody = 
            from trailing in Parse.WhiteSpace.Many()
            from body in Parse.LetterOrDigit.AtLeastOnce()
            from ending in Parse.WhiteSpace.Many()
            select new string(body.ToArray());


        private static IResult<string> TryCommandBody(string input, string name)
        {
            var parser = from trailing in Parse.WhiteSpace.Many()
                         from body in Parse.String(name)
                         select new string(body.ToArray()).ToLower();
            return parser.TryParse(input);
        }

        private static IResult<string> TryTextParam(string input)
        {
            var words = from text in Parse.LetterOrDigit.Or(Parse.WhiteSpace).Many()
                        select new string(text.ToArray());

            var stringParameter = from trailing in Parse.WhiteSpace.AtLeastOnce()
                                  from body in words
                                  from ending in Parse.WhiteSpace.Many()
                                  select new string(body.ToArray());

            return stringParameter.TryParse(input);
        }

        internal static IResult<string> TryAnyCommandBody(string input)
        {
            return AnyCommandBody.TryParse(input);
        }

        private static IResult<IEnumerable<string>> TryFlags(string input)
        {
            return Flags.TryParse(input);
        }

        private static ParsingResult ParseSimpleCommandBody(string line, ICommand command, out string remainder)
        {
            var result = new ParsingResult();
            var bodyResult = TryCommandBody(line, command.Name());
            remainder = "";

            if (bodyResult.WasSuccessful == false)
            {
                result.Type = ParsingResultType.WrongCommand;
                return result;
            }

            remainder = line.Substring(bodyResult.Remainder.Position).TrimEnd();

            if (bodyResult.Remainder.AtEnd || remainder.Length <= 0)
            {
                result.Type = ParsingResultType.SuccessReachedEnd;
                return result;
            }
            
            result.Type = ParsingResultType.Success;
            return result;
        }

        /// <summary>
        /// Parse a simple one-word command with no parameters or flags. 
        /// </summary>
        /// <param name="line">Input line.</param>
        /// <param name="command">Command to parse.</param>
        /// <returns>Result of the parsing. Possible result types are Success, WrongCommand. See ParsingResult.</returns>
        public static ParsingResult ParseSimpleCommand(string line, ICommand command)
        {
            string remainder = "";
            var result = ParseSimpleCommandBody(line, command, out remainder);

            // ParseSimpleCommandBody informs when it reaches the end of line, for simple command this is still success
            if (result.Type == ParsingResultType.SuccessReachedEnd)
            {
                result.Type = ParsingResultType.Success;
            }

            return result;
        }

        /// <summary>
        /// Parse a simple command with one parameter and no flags.
        /// </summary>
        /// <param name="line">Input line.</param>
        /// <param name="command">Command to parse.</param>
        /// <param name="parameter">Out paremeter parsed from input.</param>
        /// <returns>Result of the parsing. 
        /// Possible result types are Success, WrongCommand, MissingParam and ParsingFailure. 
        /// See ParsingResult.</returns>
        public static ParsingResult ParseSimpleCommandWithOneParameter(string line, ICommand command, out string parameter)
        {
            parameter = "";
            string remainder = "";

            var result = ParseSimpleCommandBody(line, command, out remainder);

            if (result.Type == ParsingResultType.SuccessReachedEnd)
            {
                result.Type = ParsingResultType.MissingParam;
                GConsole.WriteLine(-1.0f, Resources.text.MissingParam, command.Name());
                return result;
            }

            if(result.Type != ParsingResultType.Success)
            {
                return result;
            }

            var paramResult = TryTextParam(remainder);
            if (paramResult.WasSuccessful == false || paramResult.Remainder.AtEnd == false)
            {
                GConsole.WriteLine(Resources.text.FailureParsingRequiredParam, command.Name());
                result.Type = ParsingResultType.ParsingFailure;
                return result;
            }

            parameter = paramResult.Value;
            result.Type = ParsingResultType.Success;
            return result;
        }

        /// <summary>
        /// Parse a command with parameters and flags.
        /// </summary>
        /// <param name="line">Input line.</param>
        /// <param name="command">Command to parse.</param>
        /// <param name="hasRequiredParams">Whether the command has at least on required parameters.</param>
        /// <returns>Result of the parsing.
        /// Possible result types are Success, WrongCommand, MissingParam and ParsingFailure.
        /// It splits all the parameters by space into ParsingResult.Parameters.
        /// See ParsingResult.</returns>
        public static ParsingResult ParseCommand(string line, ICommand command, bool hasRequiredParams)
        {
            string remainder = "";

            var result = ParseSimpleCommandBody(line, command, out remainder);

            if (result.Type == ParsingResultType.SuccessReachedEnd)
            {
                if(hasRequiredParams == true)
                {
                    result.Type = ParsingResultType.MissingParam;
                    GConsole.WriteLine(-1.0f, Resources.text.MissingParam, command.Name());
                    return result;
                }
                else
                {
                    result.Type = ParsingResultType.Success;
                    return result;
                }
            }

            if (result.Type != ParsingResultType.Success)
            {
                return result;
            }

            var paramResult = TryTextParam(remainder);
            if (paramResult.WasSuccessful == false && hasRequiredParams == true)
            {
                GConsole.WriteLine(Resources.text.FailureParsingRequiredParam, command.Name());
                result.Type = ParsingResultType.ParsingFailure;
                return result;
            }

            string parameters = paramResult.Value.TrimEnd();            
            result.Parameters = parameters.Split(new char[] { ' ', '\t' }).ToList();

            if(paramResult.Remainder.AtEnd == true)
            {
                result.Type = ParsingResultType.Success;
                return result;
            }

            remainder = remainder.Substring(paramResult.Remainder.Position - 1).TrimEnd();

            var flagsResult = ParsingHelpers.TryFlags(remainder);
            if (flagsResult.WasSuccessful == false || flagsResult.Remainder.AtEnd == false)
            {
                GConsole.WriteLine(Resources.text.FailureParsingFlags, remainder, command.Name());
                result.Type = ParsingResultType.ParsingFailure;
                return result;
            }

            var parsedFlags = flagsResult.Value.ToList();

            foreach (var flag in command.GetFlags())
            {
                if (flag.FindInList(ref parsedFlags))
                {
                    result.Flags.Add(flag);
                }
            }

            if (parsedFlags.Count > 0)
            {
                GConsole.WriteLine(Resources.text.UnknownFlags, String.Join(",", parsedFlags), command.Name());
                result.Type = ParsingResultType.ParsingFailure;
                return result;
            }

            result.Type = ParsingResultType.Success;
            return result;
        }

        /// <summary>
        /// Parse a simple command with flags and no paramters.
        /// </summary>
        /// <param name="line">Input line.</param>
        /// <param name="command">Command to parse.</param>
        /// <param name="flags">Out Flags containted in the input line. See BoolFlag.</param>
        /// <returns>Result of the parsing. 
        /// Possible result types are Success, WrongCommand and ParsingFailure. 
        /// See ParsingResult.</returns>
        public static ParsingResult ParseSimpleCommandWithFlags(string line, ICommand command, out IEnumerable<BoolFlag> flags)
        {
            string remainder = "";
            var resultFlags = new List<BoolFlag>();
            flags = resultFlags;

            var result = ParseSimpleCommandBody(line, command, out remainder);

            // Flags are optional
            if (result.Type == ParsingResultType.SuccessReachedEnd)
            {
                result.Type = ParsingResultType.Success;
                return result;
            }

            var flagsResult = ParsingHelpers.TryFlags(remainder);
            if (flagsResult.WasSuccessful == false || flagsResult.Remainder.AtEnd == false)
            {
                GConsole.WriteLine(Resources.text.FailureParsingFlags, remainder, command.Name());
                result.Type = ParsingResultType.ParsingFailure;
                return result;
            }

            var parsedFlags = flagsResult.Value.ToList();

            foreach (var flag in command.GetFlags())
            {
                if(flag.FindInList(ref parsedFlags))
                {
                    resultFlags.Add(flag);
                }
            }

            flags = resultFlags;

            if (parsedFlags.Count > 0)
            {
                GConsole.WriteLine(Resources.text.UnknownFlags, String.Join(",", parsedFlags), command.Name());
                result.Type = ParsingResultType.ParsingFailure;
                return result;
            }

            result.Type = ParsingResultType.Success;
            return result;
        }

        /// <summary>
        /// Attemptes to auto-complete a string based on a list of strings.
        /// </summary>
        /// <param name="input">Input to auto-complete.</param>
        /// <param name="strings">List of potential auto-complete targets.</param>
        /// <returns>Result of the auto-complete. See AutoCompletionResult.</returns>
        public static AutoCompletionResult AutoCompleteStringList(string input, List<string> strings, bool allowSpaces = false)
        {
            AutoCompletionResult result = new AutoCompletionResult();
            var parsingResult = allowSpaces ? TryTextParam(input) : TryAnyCommandBody(input);
            if (parsingResult.WasSuccessful && parsingResult.Value.Length > 0)
            {
                string stringStub = parsingResult.Value;
                if(allowSpaces) stringStub = stringStub.Replace(" ", "");
                if (allowSpaces) stringStub = stringStub.Replace("\t", "");
                foreach (var s in strings)
                {
                    string ss = s;
                    if (allowSpaces) ss = ss.Replace(" ", "");
                    if (allowSpaces) ss = ss.Replace("\t", "");
                    if (ss == stringStub)
                    {
                        result.WasSuccessful = AutoCompletionResultType.FailureAlreadyComplete;
                        result.RemainderStartPosition = parsingResult.Remainder.Position;
                    }
                    else if (ss.StartsWith(stringStub))
                    {
                        result.WasSuccessful = AutoCompletionResultType.SuccessOneOption;
                        result.Results.Add(s);
                    }
                }
            }

            if(result.Results.Count > 1)
            {
                result.WasSuccessful = AutoCompletionResultType.SuccessMultipleOptions;
            }

            return result;
        }

        /// <summary>
        /// Attemptes to auto-complete a string.
        /// </summary>
        /// <param name="input">Input to auto-complete.</param>
        /// <param name="wantedString">Wanted target of the auto-complete.</param>
        /// <returns>Result of the auto-complete. See AutoCompletionResult.</returns>
        public static AutoCompletionResult AutoCompleteString(string input, string wantedString)
        {
            AutoCompletionResult result = new AutoCompletionResult();
            var parsingResult = TryTextParam(input);
            if (parsingResult.WasSuccessful && parsingResult.Value.Length > 0)
            {
                string stringStub = parsingResult.Value;
                if (stringStub == wantedString)
                {
                    result.WasSuccessful = AutoCompletionResultType.FailureAlreadyComplete;
                    result.RemainderStartPosition = parsingResult.Remainder.Position;
                }
                else if (wantedString.StartsWith(stringStub))
                {
                    result.WasSuccessful = AutoCompletionResultType.SuccessOneOption;
                    result.Results.Add(wantedString);
                }
            }

            return result;
        }

        /// <summary>
        /// Auto-complete a command name in an input string.
        /// </summary>
        /// <param name="input">Input to auto-complete.</param>
        /// <param name="command">Command to auto-complete.</param>
        /// <returns>Result of the auto-complete. See AutoCompletionResult.</returns>
        public static AutoCompletionResult AutoCompleteCommandBody(string input, ICommand command)
        {
            AutoCompletionResult result = new AutoCompletionResult();
            var parsingResult = TryAnyCommandBody(input);
            // There must be something to complete to begin with
            if (parsingResult.WasSuccessful && parsingResult.Value.Length > 0)
            {
                string commandStub = parsingResult.Value;
                // Already matching a command, nothing to auto-complete
                if (command.Name() == commandStub)
                {
                    result.WasSuccessful = AutoCompletionResultType.FailureAlreadyComplete;
                    result.Remainder = input.Substring(parsingResult.Remainder.Position);
                    result.RemainderStartPosition = parsingResult.Remainder.Position;

                }
                else if (command.Name().StartsWith(commandStub))
                {
                    result.WasSuccessful = AutoCompletionResultType.SuccessOneOption;
                    result.Results.Add(command.Name());
                }
            }

            return result;
        }
    }
}
