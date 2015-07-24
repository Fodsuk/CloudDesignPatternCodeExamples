using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircuitBreakerPattern
{
    public interface ICircuitBreakerState
    {
        bool Open { get; set; }

        DateTime? LastTripped { get; set; }

        Exception LastException { get; set; }

        void Reset();

        void Trip(Exception exception);

        bool OpenForLongerThan(TimeSpan timeSpan);

    }

    public class CircultBreakerState : ICircuitBreakerState
    {
        public bool Open { get; set; }

        public DateTime? LastTripped { get; set; }

        public Exception LastException { get; set; }

        public void Reset()
        {
            Open = false;
        }

        public void Trip(Exception exception)
        {
            LastException = exception;
            LastTripped = DateTime.Now;

            Open = true;
        }

        public bool OpenForLongerThan(TimeSpan timeSpan)
        {
            if (!LastTripped.HasValue) return true;

            return LastTripped.Value.Ticks > timeSpan.Ticks;
        }
    }
}
