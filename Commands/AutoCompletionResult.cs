using System;
using System.Collections.Generic;
using System.Text;

namespace Gash.Commands
{
    public enum AutoCompletionResultType
    {
        SuccessOneOption,
        SuccessMultipleOptions,
        FailureAlreadyComplete,
        Failure
    };

    public class AutoCompletionResult
    {
        public AutoCompletionResultType WasSuccessful = AutoCompletionResultType.Failure;
        public List<string> Results = new List<string>();
        public string Remainder;
        public int RemainderStartPosition;
    };
}
