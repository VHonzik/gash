using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Gash
{
    internal class GameLoop
    {
        private float SecondsPerFrame
        {
            get => 1.0f / GConsole.Settings.FPS;
        }
        private float MilisecondsPerFrame
        {
            get => SecondsPerFrame * 1000.0f;
        }
        private float FrameTimerElapsed = 0.0f;

        private Stopwatch Timer = new Stopwatch();

        private List<IGameLooped> Looped = new List<IGameLooped>();

        public CancellationToken Token;

        public void SubscribeLooped(IGameLooped looped)
        {
            Looped.Add(looped);
        }

        private void Update(float delta)
        {
            foreach(var looped in Looped)
            {
                looped.Update(delta);
            }
        }

        public void StartThread()
        {
            Timer.Start();

            while (Token.IsCancellationRequested == false)
            {
                Timer.Restart();

                Update(SecondsPerFrame);

                while (FrameTimerElapsed + Timer.ElapsedMilliseconds <= MilisecondsPerFrame) ;
                FrameTimerElapsed = FrameTimerElapsed + Timer.ElapsedMilliseconds - MilisecondsPerFrame;
            }
        }
    }
}
