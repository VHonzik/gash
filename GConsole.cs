using System;
using System.Threading;
using Gash.Output;
using System.Threading.Tasks;
using Gash.Input;

namespace Gash
{
    /// <summary>
    /// Main entry point - singleton - of Gash library. 
    /// It's a glorified singleton wrapper around .NET Console.
    /// Your application should:
    ///     1. Initialize, such as register commands, using GConsole.Instance
    ///     2. Call GConsole.Instance.Start method
    /// </summary>
    public class GConsole
    {
        private GameLoop Loop = new GameLoop();
        internal OutputManager Output = new OutputManager();
        internal InputManager Input = new InputManager();

        private static GConsole OneAndOnly;

        public Settings Settings = new Settings();

        internal Mutex ConsoleLock = new Mutex();

        /// <summary>
        /// Singleton accessor
        /// </summary>
        public static GConsole Instance
        {
            get
            {
                if (OneAndOnly == null)
                {
                    OneAndOnly = new GConsole();
                }

                return OneAndOnly;
            }
        }

        private GConsole()
        {
            Loop.SubscribeLooped(Output);
        }

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
        public void Start()
        {
            Input.Start();
            Output.Start();
            Task inputThread = Task.Factory.StartNew(Input.StartThread, CancellationToken.None,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);
            Task gameLoopThread = Task.Factory.StartNew(Loop.StartThread, CancellationToken.None,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);

            gameLoopThread.Wait();
        }

        /// <summary>
        /// Type a line to a console with default speed.
        /// </summary>
        /// <param name="line">Line to write to the console.</param>
        public static void WriteLine(string line) { Instance.Output.WriteLine(line); }

        /// <summary>
        /// Type a formatted line to a console with default speed.
        /// </summary>
        /// <param name="format">Composite format string of line to type. Same syntax as String.Format.</param>
        /// <param name="arg0">The object to format.</param>
        public void WriteLine(string format, object arg0) { Instance.Output.WriteLine(String.Format(format, arg0)); }

        /// <summary>
        /// Type a formatted line to a console with default speed.
        /// </summary>
        /// <param name="format">Composite format string of line to type. Same syntax as String.Format.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void WriteLine(string format, params object[] args)
        { Instance.Output.WriteLine(String.Format(format, args)); }
    }
}
