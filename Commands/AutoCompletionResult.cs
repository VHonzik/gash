using System.Collections.Generic;

namespace Gash.Commands
{
    /// <summary>
    /// Result type of auto-complete operations.
    /// </summary>
    public enum AutoCompletionResultType
    {
        /// <summary>
        /// The input can be auto-completed and there is only on choice.
        /// For the result see the first element of AutoCompletionResult.Results.
        /// </summary>
        SuccessOneOption,
        /// <summary>
        /// The input can be auto-completed but there are multiple options.
        /// For the results see the elements of AutoCompletionResult.Results.
        /// </summary>
        SuccessMultipleOptions,
        /// <summary>
        /// The input already matches the provided option(s) for auto-complete.
        /// </summary>
        FailureAlreadyComplete,
        /// <summary>
        /// Generic failure of auto-complete operation.
        /// </summary>
        Failure
    };

    /// <summary>
    /// Result of auto-complete operation.
    /// </summary>
    public class AutoCompletionResult
    {
        /// <summary>
        /// Type of success/failure of auto-complete operation.
        /// See AutoCompletionResultType enum.
        /// </summary>
        public AutoCompletionResultType WasSuccessful = AutoCompletionResultType.Failure;
        /// <summary>
        /// Possible candidate(s) for successful auto-complete operation.
        /// </summary>
        public List<string> Results = new List<string>();
        /// <summary>
        /// Remainder of the input after the auto-completed part.
        /// </summary>
        public string Remainder;
        /// <summary>
        /// Zero-based index of the start of the remainder in the input string.
        /// </summary>
        public int RemainderStartPosition;
    };
}
