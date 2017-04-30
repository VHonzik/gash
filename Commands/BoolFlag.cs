using System.Collections.Generic;

namespace Gash.Commands
{
    /// <summary>
    /// A simple bool flag which turns on/off a behaviour of a command.
    /// E.g. "-doX".
    /// </summary>
    public class BoolFlag
    {
        /// <summary>
        /// A name of the flag.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The current value of the flag.
        /// If the flag wasn't specified it has the default value,
        /// if it was specified it has a opposite value of the default.
        /// </summary>
        public bool Value { get; private set; }

        /// <summary>
        /// Create a bool flag with a name and default value.
        /// </summary>
        /// <param name="name">Name of the flag.</param>
        /// <param name="defaultValue">Default value of the underlying bool.</param>
        public BoolFlag(string name, bool defaultValue = false)
        {
            Name = name;
            Value = defaultValue;
        }

        internal bool FindInList(ref List<string> flags)
        {
            var removedSome = flags.RemoveAll(x => x == Name) > 0;
            if (removedSome == true) Value = !Value;
            return removedSome;
        }
    }
}
