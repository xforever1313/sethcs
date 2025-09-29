//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Diagnostics;

namespace SethCS
{
    /// <summary>
    /// Class that uses a pattern from C++.
    /// When constructed, it starts a stop watch.
    /// When Disposed, it calculates how much time has passed
    /// since construction.  If the passed time is less than
    /// our tolerance, the OnTimeNotExpired event is triggered, 
    /// otherwise our OnTimeExpired event is triggered.
    /// </summary>
    public class RAIITimer : IDisposable
    {
        // -------- Fields --------

        /// <summary>
        /// Fired if dispose is called BEFORE (or equal to) our tolerance.
        /// Passed in parameter is our elapsed milliseconds.
        /// </summary>
        private Action<long> OnTimeNotExpired;

        /// <summary>
        /// Fired if dispose is called AFTER our tolerance.
        /// Passed in parameter is our elapsed milliseconds.
        /// </summary>
        private Action<long> OnTimeExpired;

        private Stopwatch stopWatch;

        private long toleranceMs;

        // -------- Constructor --------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="toleranceMs">The tolerance in milliseconds.</param>
        /// <param name="onTimeExpired">
        /// Fired if dispose is called AFTER our tolerance. Null for no action.
        /// Passed in parameter is our elapsed milliseconds.
        /// </param>
        /// <param name="onTimeNotExpired">
        /// Fired if dispose is called BEFORE our tolerance. Null for no action.
        /// Passed in parameter is our elapsed milliseconds.
        /// </param>
        public RAIITimer( long toleranceMs, Action<long> onTimeExpired, Action<long> onTimeNotExpired )
        {
            this.toleranceMs = toleranceMs;
            this.OnTimeExpired = onTimeExpired;
            this.OnTimeNotExpired = onTimeNotExpired;
            this.stopWatch = new Stopwatch();
            this.stopWatch.Start();
        }

        // -------- Functions --------

        /// <summary>
        /// Stops the timer and fires which expiration event.
        /// </summary>
        public void Dispose()
        {
            this.stopWatch.Stop();

            if( this.toleranceMs >= this.stopWatch.ElapsedMilliseconds )
            {
                this.OnTimeNotExpired?.Invoke( this.stopWatch.ElapsedMilliseconds );
            }
            else
            {
                this.OnTimeExpired?.Invoke( this.stopWatch.ElapsedMilliseconds );
            }
        }
    }
}

