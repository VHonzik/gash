namespace GashLibrary.Commands
{
    /// <summary>
    /// Interface which defines a keyword.
    /// </summary>
    public interface IKeyword
    {
        /// <summary>
        /// Name of the keyword.
        /// Can be multiple words.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Highlighted name of the keyword.
        /// You can use GConsole.ColorifyText methods on Name to produce it.
        /// </summary>
        string ColoredName { get; }

        /// <summary>
        /// Triggered when player calls a man command on this keyword.
        /// </summary>
        void PrintManPage();
    }
}
