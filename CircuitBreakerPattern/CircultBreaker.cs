using System;

namespace CircuitBreakerPattern
{
    public class CircultBreaker
    {
        private readonly ICircuitBreakerState _state;
        private TimeSpan _resetBreakerTime = new TimeSpan(0 , 1, 0);

        public CircultBreaker(ICircuitBreakerState state)
        {
            _state = state;
        }

        public ICircuitBreakerState State {get { return _state; }}

        public TimeSpan ResetBreakerTime { get { return _resetBreakerTime; } set { _resetBreakerTime = value; }}

        public void ExecuteAction(Action action)
        {
            switch (_state.Condition)
            {
                case CircuitBreakerCondition.Closed:
                case CircuitBreakerCondition.HalfOpen:
                    AttemptAction(action);
                    break;

                case CircuitBreakerCondition.Open:
                    ConsiderRetry(action);
                    break;
            }
        }

        private void ConsiderRetry(Action action)
        {
            if (_state.OpenForLongerThan(ResetBreakerTime))
            {
                AttemptAction(action);

                _state.Reset(); //working again!
            }
        }

        private void AttemptAction(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                _state.Trip(e);

                throw;
            }
        }
    }
}