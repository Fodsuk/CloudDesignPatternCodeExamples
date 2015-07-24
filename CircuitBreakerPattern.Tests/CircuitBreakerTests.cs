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
        public void ExecuteAction_FailedCall_BreakerTrip()
        {
            var state = new CircultBreakerState();

            var breaker = new CircultBreaker(state);

            try
            {
                breaker.ExecuteAction(CallFailedExternalService);
            }
            catch(Exception) {}

            Thread.Sleep(3000);

            try
            {
                breaker.ExecuteAction(CallFailedExternalService);
            }
            catch (Exception) { }

            Thread.Sleep(3000);

            try
            {
                breaker.ExecuteAction(CallFailedExternalService);
            } catch (Exception) { }
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
