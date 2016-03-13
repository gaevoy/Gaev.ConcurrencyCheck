using System.Collections.Concurrent;

namespace Gaev.ConcurrencyCheck
{
    public class ExecutionContext
    {
        readonly ConcurrentDictionary<string, Breakpoint> breakpoints = new ConcurrentDictionary<string, Breakpoint>();
        public Breakpoint Breakpoint(string name) => breakpoints.GetOrAdd(name, _ => new Breakpoint());
        public Breakpoint this[string breakpointName] => Breakpoint(breakpointName);
    }
}