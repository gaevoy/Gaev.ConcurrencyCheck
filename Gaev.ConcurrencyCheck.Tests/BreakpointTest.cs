using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Gaev.ConcurrencyCheck.Tests
{
    [TestFixture]
    public class BreakpointTest
    {
        [Test]
        public void ItShouldPauseThread()
        {
            // Given
            var breakpoint = new Breakpoint();
            breakpoint.Pause();
            int state = 0;

            // When
            Task.Run(() =>
            {
                state++;
                breakpoint.WaitIfPaused();
                state++;
            });
            Thread.Sleep(100);

            // Then
            Assert.AreEqual(1, state);
        }

        [Test]
        public void ItShouldPauseThreadForSpecificTime()
        {
            // Given
            var breakpoint = new Breakpoint().Pause(@for: TimeSpan.FromMilliseconds(200));
            int state = 0;

            // When
            Task.Run(() =>
            {
                state++;
                breakpoint.WaitIfPaused();
                state++;
            });
            Thread.Sleep(100);

            // Then
            Assert.AreEqual(1, state);

            // When
            Thread.Sleep(200);

            // Then
            Assert.AreEqual(2, state);
        }

        [Test]
        public void ItShouldResumeThread()
        {
            // Given
            var breakpoint = new Breakpoint();
            breakpoint.Pause();
            int state = 0;

            // When
            Task.Run(() =>
            {
                state++;
                breakpoint.WaitIfPaused();
                state++;
            });
            Thread.Sleep(100);
            breakpoint.Resume();
            Thread.Sleep(100);

            // Then
            Assert.AreEqual(2, state);
        }

        [Test]
        public void ItShouldWaitForPause()
        {
            // Given
            var breakpoints = new ExecutionContext();
            var p1 = breakpoints["p1"];
            var p2 = breakpoints["p2"];
            p1.Pause();
            p2.Pause();
            int state = 0;

            // When
            Task.Run(() =>
            {
                state++;
                breakpoints["p1"].WaitIfPaused();
                state++;
                breakpoints["p2"].WaitIfPaused();
                state++;
            });
            p1.WaitForPause();

            // Then
            Assert.AreEqual(1, state);

            // When
            p1.Resume();
            p2.WaitForPause();

            // Then
            Assert.AreEqual(2, state);
        }

        [Test]
        public void ItShouldNotWaitUnlessPausedAfterResume()
        {
            // Given
            var breakpoints = new ExecutionContext();
            var p1 = breakpoints["p1"];
            p1.Pause().Resume();
            int state = 0;

            // When
            Task.Run(() =>
            {
                state++;
                breakpoints["p1"].WaitIfPaused();
                state++;
            });
            p1.WaitForPause();
            p1.Resume();
            p1.WaitForPause();
            Thread.Sleep(100);

            // Then
            Assert.AreEqual(2, state);
        }

        [Test]
        public void ItShouldBeResumedByDefault()
        {
            // Given
            var breakpoint = new Breakpoint();
            int state = 0;

            // When
            Task.Run(() =>
            {
                state++;
                breakpoint.WaitIfPaused();
                state++;
            });
            breakpoint.WaitForPause();
            Thread.Sleep(100);

            // Then
            Assert.AreEqual(2, state);
        }
    }
}
