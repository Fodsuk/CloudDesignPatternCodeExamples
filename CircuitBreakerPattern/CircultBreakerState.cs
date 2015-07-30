using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircuitBreakerPattern
{
    public enum CircuitBreakerCondition
    {
        Closed,
        HalfOpen,
        Open,
    }

    public class CircultBreakerState : ICircuitBreakerState
    {
        public CircultBreakerState()
        {
            Reset();
        }

        public CircuitBreakerCondition Condition { get; set; }

        public DateTime? LastTripped { get; set; }

        public Exception LastException { get; set; }

        public void Reset()
        {
            Condition = CircuitBreakerCondition.Closed;
        }

        public void Trip(Exception exception)
        {
            LastException = exception;
            LastTripped = DateTime.Now;

            if (Condition == CircuitBreakerCondition.HalfOpen)
                Condition = CircuitBreakerCondition.Open; //Dam Dog! we're really are down!
            else
                Condition = CircuitBreakerCondition.HalfOpen;
        }

        public bool OpenForLongerThan(TimeSpan timeSpan)
        {
            if (!LastTripped.HasValue) return true;

            return LastTripped.Value.Ticks > timeSpan.Ticks;
        }
    }
}
