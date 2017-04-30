using Gash.Commands;
using Gash.Input;
using Gash.Output;
using GashLibrary.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gash
{
    /// <summary>
    /// Main entry point of Gash library.
    /// Use the static functions to interact with the Gash library.
    /// On the background it is a glorified singleton wrapper around .NET Console.
    /// </summary>
    public class GConsole
    {
        private GameLoop Loop = new GameLoop();
        internal OutputManager Output = new OutputManager();
        internal InputManager Input = new InputManager();        

        internal Settings TheSettings = new Settings();

        internal CommandList Commands = new CommandList();
        internal KeywordList Keywords = new KeywordList();

        internal Mutex ConsoleLock = new Mutex();

        private CancellationTokenSource TokenSource = new CancellationTokenSource();
        private CancellationToken CancelToken;
        private Task GameLoopThread = null;

        private static GConsole TheOneAndOnly;

        internal static GConsole Instance
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
        /// Access and change the Gash framework settings.
        /// </summary>
        public static Settings Settings
        {
            get => Instance.TheSettings;
        }

        /// <summary>
        /// Subscribe to the game loop.
        /// </summary>
        /// <param name="looped">An object implementing IGameLooped interface.</param>
        public static void SubscribeLooped(IGameLooped looped)
        {
            Instance.Loop.SubscribeLooped(looped);
        }

        internal ConsoleAccess RequestAccess()
        {
            return new ConsoleAccess(ConsoleLock);
        }

        internal void StartConsole()
        {
            Commands.RegisterCommand(new Man());
            Commands.RegisterCommand(new List());
            Input.Start();
            Output.Start();
            Task inputThread = Task.Factory.StartNew(Input.StartThread, CancellationToken.None,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);
            CancelToken = TokenSource.Token;
            Loop.Token = TokenSource.Token;
            GameLoopThread = Task.Factory.StartNew(Loop.StartThread, CancelToken,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);

            GameLoopThread.Wait();
        }

        /// <summary>
        /// Start the Gash console.
        /// Several threads will be spawned and game loop will start.
        /// This will block the calling thread.
        /// </summary>
        public static void Start()
        {
            Instance.StartConsole();
        }

        /// <summary>
        /// Exit the Gash console.
        /// All threads are stopped.
        /// </summary>
        public static void Exit()
        {
            Instance.TokenSource.Cancel();
        }

        /// <summary>
        /// Register a new command to a list of know commands.
        /// Not registerd commands are invisible to the Gash framework.
        /// </summary>
        /// <param name="command">An object implementing ICommand interface.</param>
        public static void RegisterCommand(ICommand command)
        {
            Instance.Commands.RegisterCommand(command);
        }

        /// <summary>
        /// Register a new keyword to a list of know keywords.
        /// Not registerd keywords are invisible to the Gash framework.
        /// </summary>
        /// <param name="keyword">An object implementing IKeyword interface.</param>
        public static void RegisterKeyword(IKeyword keyword)
        {
            Instance.Keywords.RegisterKeyword(keyword);
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

        public static void Wait(float time)
        { Instance.Output.Wait(time); }

        /// <summary>
        /// Produces a markdown for a colored text.
        /// </summary>
        /// <param name="foreground">Foreground, i.e. font color, of the text.</param>
        /// <param name="background">Background color of the text.</param>
        /// <param name="text">Text to make colored.</param>
        /// <returns>A markdown string usable in GConsole.WriteLine methods.</returns>
        public static string ColorifyText(ConsoleColor foreground, ConsoleColor background, string text)
        {
            return String.Format("${0}{1}{2}$", 
                ((int)foreground).ToString("D2"), 
                ((int)background).ToString("D2"), 
                text);
        }


        /// <summary>
        /// Produces a markdown for a colored text.
        /// </summary>
        /// <param name="type">Highlight instance with color information.</param>
        /// <param name="text">Text to make colored.</param>
        /// <returns>A markdown string usable in GConsole.WriteLine methods.</returns>
        public static string ColorifyText(HighlightType type, string text)
        {
            return String.Format("${0}{1}{2}$",
                ((int)type.Foreground).ToString("D2"),
                ((int)type.Background).ToString("D2"),
                text);
        }

        /// <summary>
        /// Produces a markdown for a colored text.
        /// </summary>
        /// <param name="higlightIndex">Zero-based index of the higlight in GConsole.Settings.Higlights.</param>
        /// <param name="text">Text to make colored.</param>
        /// <returns>A markdown string usable in GConsole.WriteLine methods.</returns>
        public static string ColorifyText(int higlightIndex, string text)
        {
            return String.Format("${0}{1}{2}$",
                ((int)Settings.Higlights[higlightIndex].Foreground).ToString("D2"),
                ((int)Settings.Higlights[higlightIndex].Background).ToString("D2"),
                text);
        }

        /// <summary>
        /// Clear all of the future lines queued for typing in the console.
        /// </summary>
        public static void ClearQueuedOutput()
        {
            Instance.Output.ClearQueuedOutput();
        }

        /// <summary>
        /// Pause typing of new lines.
        /// </summary>
        public static void PauseOutput()
        {
            Instance.Output.PauseOutput(true);
        }

        /// <summary>
        /// Resume typing of new lines.
        /// </summary>
        public static void ResumeOutput()
        {
            Instance.Output.PauseOutput(false);
        }
    }
}
