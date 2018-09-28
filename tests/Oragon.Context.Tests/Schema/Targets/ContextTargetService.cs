using Oragon.Contexts.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Context.Tests.Schema.Targets
{
    public interface IContextTargetService
    {
        void Test(Action<FakeDataProcess> action);

        FakeDataProcess FakeDataProcess { get; set; }


        void SuppressException(Action action);

        string SuppressException2(Action action);
    }


    public class ContextTargetService : IContextTargetService
    {
        public FakeDataProcess FakeDataProcess { get; set; }

        [ExceptionHandling(ExceptionHandlingStrategy.ContinueRunning)]
        public void SuppressException(Action action)
        {
            action.Invoke();
        }

        [ExceptionHandling(ExceptionHandlingStrategy.ContinueRunning)]
        public string SuppressException2(Action action)
        {
            return null;
        }

        [Fake(ContextKey = "123456")]
        public void Test(Action<FakeDataProcess> action) => action(this.FakeDataProcess);
    }
}
