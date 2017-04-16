using System;
using System.Collections.Generic;
using System.Text;

namespace Gash.Commands
{
    public class BoolFlag
    {
        public string Name { get; private set; }
        public bool Value { get; private set; }

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
