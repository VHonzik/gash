using System;
using System.Collections.Generic;
using Gash.Commands;
using System.Linq;
using System.Text;

namespace Gash.Input
{
    class AutoComplete
    {
        public bool ProcessInput(ConsoleKeyInfo key)
        {
            bool processed = false;
            switch (key.Key)
            {
                case ConsoleKey.Tab:
                    processed = true;
                    break;
                default:
                    processed = false;
                    break;
            }
            return processed;
        }

        public AutoCompletionResult TryComplete(string line)
        {
            List<AutoCompletionResult> results = new List<AutoCompletionResult>();

            foreach (var command in GConsole.Instance.Commands)
            {
                results.Add(command.AutoComplete(line));
            }

            int successCount = results.Count(x => x.WasSuccessful == AutoCompletionResultType.SuccessOneOption);
            int successMultipleCount = results.Count(x => x.WasSuccessful == AutoCompletionResultType.SuccessMultipleOptions);
            if (successCount == 1 && successMultipleCount == 0)
            {
                return results.Find(x => x.WasSuccessful == AutoCompletionResultType.SuccessOneOption);
            }
            else if (successMultipleCount > 0 || successCount > 1)
            {
                AutoCompletionResult result = new AutoCompletionResult();
                result.WasSuccessful = AutoCompletionResultType.SuccessMultipleOptions;

                foreach (var successResults in results.FindAll(
                    x => x.WasSuccessful == AutoCompletionResultType.SuccessOneOption
                    || x.WasSuccessful == AutoCompletionResultType.SuccessMultipleOptions))
                {
                    result.Results.AddRange(successResults.Results);
                }
                return result;

            }
            else
            {
                return new AutoCompletionResult();
            }
        }
    }
}
