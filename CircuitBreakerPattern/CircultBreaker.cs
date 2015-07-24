using System;

namespace CircuitBreakerPattern
{
    public class CircultBreaker
    {
        /*
         To Consider:
         *  Retry
         *  State per action (some open, some closed)  
         */

        private readonly ICircuitBreakerState _store;

        public CircultBreaker(ICircuitBreakerState store)
        {
            _store = store;
        }

        public void ExecuteAction(Action action)
        {
            if (_store.Open)
            {
                //Should we give it another go?
                if (_store.OpenForLongerThan(new TimeSpan(0, 10, 2)))
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        _store.Trip(e);

                        throw;
                    }
                }
            }
            else
            {

                try
                {
                    action();
                }
                catch (Exception e)
                {
                    _store.Trip(e);

                    throw;
                }
            }
        }

    }
}