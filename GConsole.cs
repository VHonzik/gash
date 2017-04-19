using System;
using System.Threading;
using System.Threading.Tasks;
using Gash.Output;
using Gash.Input;
using Gash.Commands;

namespace Gash
{
    /// <summary>
    /// Main entry point - singleton - of Gash library. 
    /// It's a glorified singleton wrapper around .NET Console.
    /// </summary>
    public class GConsole
    {
        private GameLoop Loop = new GameLoop();
        internal OutputManager Output = new OutputManager();
        internal InputManager Input = new InputManager();        

        /// <summary>
        /// Gash settings
        /// </summary>
        public Settings Settings = new Settings();

        /// <summary>
        /// Commands list used to register commands.
        /// </summary>
        public CommandList Commands = new CommandList();

        internal Mutex ConsoleLock = new Mutex();

        private static GConsole TheOneAndOnly;
        /// <summary>
        /// Singleton accessor
        /// </summary>
        public static GConsole Instance
        {
            get
            {
                if (TheOneAndOnly == null)
                {
                    TheOneAndOnly = new GConsole();
                }

                return TheOneAndOnly;
            }
        }

        private GConsole()
        {
            Loop.SubscribeLooped(Output);
        }

        /// <summary>
        /// Subscribe to the game loop.
        /// </summary>
        /// <param name="looped">An object implementing IGameLooped interface.</param>
        public void SubscribeLooped(IGameLooped looped)
        {
            Loop.SubscribeLooped(looped);
        }

        internal ConsoleAccess RequestAccess()
        {
            return new ConsoleAccess(ConsoleLock);
        }

        /// <summary>
        /// Start the Gash console.
        /// Several threads will be spawned and game loop will start.
        /// This will block the calling thread.
        /// </summary>
        public void StartConsole()
        {
            Commands.RegisterCommand(new Man());
            Input.Start();
            Output.Start();
            Task inputThread = Task.Factory.StartNew(Input.StartThread, CancellationToken.None,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);
            Task gameLoopThread = Task.Factory.StartNew(Loop.StartThread, CancellationToken.None,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);

            gameLoopThread.Wait();
        }

        /// <summary>
        /// Short-hand for GConsole.Instance.Start.
        /// </summary>
        public static void Start()
        {
            Instance.StartConsole();
        }

        /// <summary>
        /// Type a line to a console with default speed.
        /// </summary>
        /// <param name="line">Line to write to the console.</param>
        public static void WriteLine(string line) { Instance.Output.WriteLine(line); }

        /// <summary>
        /// Type a line to a console.
        /// </summary>
        /// <param name="speed">Number of seconds per character. Negative number types the line instantly.</param>
        /// <param name="line">Line to write to the console.</param>
        public static void WriteLine(float speed, string line) { Instance.Output.WriteLine(line, speed); }

        /// <summary>
        /// Type a formatted line to a console with default speed.
        /// </summary>
        /// <param name="format">Composite format string of line to type. Same syntax as String.Format.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void WriteLine(string format, params object[] args)
        { Instance.Output.WriteLine(String.Format(format, args)); }

        /// <summary>
        /// Type a formatted line to a console.
        /// </summary>
        /// <param name="speed">Number of seconds per character. Negative number types the line instantly.</param>
        /// <param name="format">Composite format string of line to type. Same syntax as String.Format.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void WriteLine(float speed, string format, params object[] args)
        { Instance.Output.WriteLine(String.Format(format, args), speed); }

        /// <summary>
        /// Returns markdown for text colored as passed, which Gash framework understands when used in WriteLine.
        /// </summary>
        /// <param name="foreground">Foreground, i.e. font color, of the text.</param>
        /// <param name="background">Background color of the text.</param>
        /// <param name="text">Text to make colored.</param>
        /// <returns></returns>
        public static string ColorifyText(ConsoleColor foreground, ConsoleColor background, string text)
        {
            return String.Format("${0}{1}{2}$", 
                ((int)foreground).ToString("D2"), 
                ((int)background).ToString("D2"), 
                text);
        }

        /// <summary>
        /// Highlights a text using "Settings.CommandsAndKeywordsHighlightColor"s
        /// </summary>
        /// <param name="text">Text to highlight.</param>
        /// <returns></returns>
        public static string HighlightTextAsCommandOrKeyword(string text)
        {
            return ColorifyText(Instance.Settings.CommandsAndKeywordsHighlightColorForeground,
                Instance.Settings.CommandsAndKeywordsHighlightColorBackground,
                text);
        }
    }
}
