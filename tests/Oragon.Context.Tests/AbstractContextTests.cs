using Moq;
using Oragon.Context.Tests.Schema;
using Oragon.Context.Tests.Schema.Targets;
using Oragon.Contexts;
using System;
using Xunit;

namespace Oragon.Context.Tests
{

    public class AbstractContextTests
    {
        private Oragon.Spring.Context.IApplicationContext GetContext(string caseName) => new Oragon.Spring.Context.Support.XmlApplicationContext($"assembly://Oragon.Context.Tests/Oragon.Context.Tests/{nameof(AbstractContextTests)}.{caseName}.xml");

        [Fact]
        public void IsolationTest()
        {
            using (var springContext = this.GetContext("Case1"))
            {
                var service = springContext.GetObject<IContextTargetService>("Service");
                service.Test(dp1 =>
                {

                    dp1.Add(0);
                    Assert.Equal(0, dp1.Sum());
                    dp1.Add(5);

                    service.Test(dp2 =>
                    {
                        dp2.Add(0);
                        Assert.Equal(0, dp2.Sum());

                        dp2.Add(5);
                        dp2.Add(2);
                        Assert.Equal(7, dp2.Sum());
                    });

                    dp1.Add(2);
                    Assert.Equal(7, dp1.Sum());
                });

            }
        }

        [Fact]
        public void ServiceTest()
        {
            using (var springContext = this.GetContext("Case1"))
            {
                var service = springContext.GetObject<IContextTargetService>("Service");
                service.FakeDataProcess = service.FakeDataProcess;
            }
        }

        [Fact]
        public void ContextAccess1()
        {
            using (var context = this.GetContext("Case1"))
            {
                var springContext = context.GetObject<IContextTargetService>("Service");
                springContext.Test(dp1 =>
                {
                    dp1.Add(2);
                    dp1.Add(5);
                    dp1.Subtract(7);
                    Assert.Equal(0, dp1.Sum());
                });
            }
        }


        [Fact]
        public void ContextAccess2()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {

                using (var springContext = this.GetContext("Case1"))
                {
                    var service = springContext.GetObject<IContextTargetService>("Service");
                    service.Test(dp1 =>
                    {
                        Assert.Equal("123456", dp1.MyContextKey);
                        dp1.MyContextKey = "aaa";
                        dp1.Sum();
                    });
                }

            });

        }



        [Fact]
        public void CascadeContext()
        {
#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
            using (var springContext = this.GetContext("Case1"))
            {
                var advice = springContext.GetObject<FakeAroundAdvice>("FakeAroundAdvice");
                var service = springContext.GetObject<IContextTargetService>("Service");

                Assert.Equal(0, advice.GetContextStack().Count);

                service.Test(a =>
                {
                    Assert.Equal(1, advice.GetContextStack().Count);
                    service.Test(b =>
                    {
                        Assert.Equal(2, advice.GetContextStack().Count);
                    });
                    Assert.Equal(1, advice.GetContextStack().Count);
                    GC.Collect();

                });
                Assert.Equal(0, advice.GetContextStack().Count);
                

            }
#pragma warning restore xUnit2013 // Do not use equality check to check for collection size.
        }

    }
}
