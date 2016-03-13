using System;
using System.Threading;

namespace Gaev.ConcurrencyCheck
{
    public class Breakpoint
    {
        readonly AutoResetEvent isPaused = new AutoResetEvent(true);
        readonly AutoResetEvent isPauseRiched = new AutoResetEvent(true);
        TimeSpan waitIfPausedTimeout = TimeSpan.FromSeconds(10);
        TimeSpan timeout = TimeSpan.FromSeconds(10);

        public Breakpoint Pause(TimeSpan? @for = null)
        {
            if (@for != null)
                waitIfPausedTimeout = @for.Value;
            isPauseRiched.Reset();
            isPaused.Reset();

            return this;
        }
        public Breakpoint Resume()
        {
            isPauseRiched.Set();
            isPaused.Set();

            return this;
        }

        public void WaitIfPaused()
        {
            isPauseRiched.Set();
            if (!isPaused.WaitOne(waitIfPausedTimeout))
                Resume();
        }

        public void WaitForPause()
        {
            isPauseRiched.WaitOne(timeout);
        }
    }
}
