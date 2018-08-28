using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Context.Tests.Schema.Targets
{
    public interface IContextTargetService
    {
        void Test(Action<FakeDataProcess> action);

        FakeDataProcess FakeDataProcess { get; set; }
    }


    public class ContextTargetService : IContextTargetService
    {
        public FakeDataProcess FakeDataProcess { get; set; }

        [Fake(ContextKey = "123456")]
        public void Test(Action<FakeDataProcess> action) => action(this.FakeDataProcess);
    }
}
