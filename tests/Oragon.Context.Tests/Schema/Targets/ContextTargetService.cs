using Oragon.Contexts.ExceptionHandling;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Oragon.Context.Tests.Schema.Targets
{
    public interface IContextTargetService
    {
        void Test(Action<FakeDataProcess> action);

        Task Test1Async(IContextTargetService service);

        Task Test2Async();

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
        public async Task Test1Async(IContextTargetService service)
        {
            Assert.Equal(0, this.FakeDataProcess.Sum());
            this.FakeDataProcess.Add(0);
            Assert.Equal(0, this.FakeDataProcess.Sum());
            this.FakeDataProcess.Add(5);
            Assert.Equal(5, this.FakeDataProcess.Sum());


            await service.Test2Async();

            this.FakeDataProcess.Add(2);
            Assert.Equal(7, this.FakeDataProcess.Sum());
        }


        [Fake(ContextKey = "123456")]
        public async Task Test2Async()
        {
            Assert.Equal(0, this.FakeDataProcess.Sum());
            await Task.Delay(100);
            Assert.Equal(0, this.FakeDataProcess.Sum());
            this.FakeDataProcess.Add(5);
            this.FakeDataProcess.Add(2);

            Assert.Equal(7, this.FakeDataProcess.Sum());

            await Task.Delay(100);

            Assert.Equal(7, this.FakeDataProcess.Sum());
        }

        [Fake(ContextKey = "123456")]
        public void Test(Action<FakeDataProcess> action) => action?.Invoke(this.FakeDataProcess);
    }
}
