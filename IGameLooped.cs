using System;
using System.Collections.Generic;
using System.Text;

namespace Gash
{
    /// <summary>
    /// Interface for objects updated by the game loop.
    /// </summary>
    public interface IGameLooped
    {
        /// <summary>
        /// Fixed time-step loop trigger.
        /// </summary>
        /// <param name="deltaTime">Time in seconds since the last update.</param>
        void Update(float deltaTime);
    }
}
