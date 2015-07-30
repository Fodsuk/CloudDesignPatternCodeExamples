using System;
using System.Security.Authentication;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CircuitBreakerPattern.Tests
{
    [TestClass]
    public class CircuitBreakerTests
    {
        [TestMethod]
        public void ExecuteAction_HappyCall_BreakerDoesntTrip()
        {
            var state = new CircultBreakerState();

            var breaker = new CircultBreaker(state);

            breaker.ExecuteAction(CallWorkingExternalService);    
            breaker.ExecuteAction(CallWorkingExternalService);    
            breaker.ExecuteAction(CallWorkingExternalService);    
            breaker.ExecuteAction(CallWorkingExternalService);    
            breaker.ExecuteAction(CallWorkingExternalService);    
        }

        [TestMethod]
        public void ExecuteAction_FailedCall_BreakerTripAtStages()
        {
            var breaker = new CircultBreaker(new CircultBreakerState());
            breaker.ResetBreakerTime = new TimeSpan(0, 0, 5);

            Assert.AreEqual(CircuitBreakerCondition.Closed, breaker.State.Condition);

            try
            {
                breaker.ExecuteAction(CallFailedExternalService);
            }
            catch(Exception) { /* Naughty Naughty */ }
            
            Assert.AreEqual(CircuitBreakerCondition.HalfOpen, breaker.State.Condition);

            try
            {
                breaker.ExecuteAction(CallFailedExternalService);
            }
            catch (Exception) { /* Naughty Naughty */ }

            Assert.AreEqual(CircuitBreakerCondition.Open, breaker.State.Condition);



            //Wait the amount of time for the breaker to reset
            Thread.Sleep(breaker.ResetBreakerTime);

            try
            {
                breaker.ExecuteAction(CallWorkingExternalService);
            }
            catch (Exception) { /* Naughty Naughty */ }


            Assert.AreEqual(CircuitBreakerCondition.Closed, breaker.State.Condition);
        }

        

        private void CallWorkingExternalService()
        {
            //Call bbc/football API for the latest Arsenal Scores!
        }

        
        private void CallFailedExternalService()
        {
            throw new InvalidCredentialException();
        }
    }
}
