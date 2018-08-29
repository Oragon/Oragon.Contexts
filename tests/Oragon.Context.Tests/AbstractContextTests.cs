using Oragon.Context.Tests.Schema.Targets;
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
            using (var context = this.GetContext("Case1"))
            {
                var service = context.GetObject<IContextTargetService>("Service");
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
            using (var context = this.GetContext("Case1"))
            {
                var service = context.GetObject<IContextTargetService>("Service");
                service.FakeDataProcess = service.FakeDataProcess;
            }
        }

        [Fact]
        public void ContextAccess1()
        {
            using (var context = this.GetContext("Case1"))
            {
                var service = context.GetObject<IContextTargetService>("Service");
                service.Test(dp1 =>
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
            Assert.Throws<InvalidOperationException>(() => {

                using (var context = this.GetContext("Case1"))
                {
                    var service = context.GetObject<IContextTargetService>("Service");
                    service.Test(dp1 =>
                    {
                        Assert.Equal("123456", dp1.MyContextKey);
                        dp1.MyContextKey = "aaa";
                        dp1.Sum();
                    });
                }

            });

            
        }


    }
}
