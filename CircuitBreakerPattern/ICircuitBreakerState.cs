using System;

namespace CircuitBreakerPattern
{
    public interface ICircuitBreakerState
    {
        CircuitBreakerCondition Condition { get; set; }

        DateTime? LastTripped { get; set; }

        Exception LastException { get; set; }

        void Reset();

        void Trip(Exception exception);

        bool OpenForLongerThan(TimeSpan timeSpan);

    }
}