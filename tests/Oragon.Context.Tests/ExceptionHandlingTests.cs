using Moq;
using Oragon.Business;
using Oragon.Context.Tests.Schema;
using Oragon.Context.Tests.Schema.Targets;
using Oragon.Contexts.ExceptionHandling;
using Oragon.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Oragon.Context.Tests
{
    public class ExceptionHandlingTests
    {
        private Oragon.Spring.Context.IApplicationContext GetContext(string caseName) => new Oragon.Spring.Context.Support.XmlApplicationContext($"assembly://Oragon.Context.Tests/Oragon.Context.Tests/{nameof(ExceptionHandlingTests)}.{caseName}.xml");

        [Fact]
        public void Log2Times()
        {
            using (var springContext = this.GetContext("Case1"))
            {
                var advice = springContext.GetObject<ExceptionHandlerAroundAdvice>("ExceptionHandlerAroundAdvice");

                var mock = new Mock<ILogger>();
                advice.EnableDebug = true;
                advice.Logger = mock.Object;

                var service = springContext.GetObject<IContextTargetService>("Service");
                service.Test(dp1 =>
                {

                });

                mock.Verify(it => it.Log(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<LogLevel>(data => data == LogLevel.Debug),
                    It.IsAny<IDictionary<string, object>>()
                    ), Times.Exactly(2));
            }
        }

        [Fact]
        public void Log0Times()
        {
            using (var springContext = this.GetContext("Case1"))
            {
                var advice = springContext.GetObject<ExceptionHandlerAroundAdvice>("ExceptionHandlerAroundAdvice");

                var mock = new Mock<ILogger>();
                advice.EnableDebug = false;
                advice.Logger = mock.Object;

                var service = springContext.GetObject<IContextTargetService>("Service");
                service.Test(dp1 =>
                {

                });

                mock.Verify(it => it.Log(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<LogLevel>(data => data == LogLevel.Debug),
                    It.IsAny<IDictionary<string, object>>()
                    ), Times.Never);

                mock.Verify(it => it.Log(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.Is<LogLevel>(data => data == LogLevel.Warn),
                   It.IsAny<IDictionary<string, object>>()
                   ), Times.Never);

                mock.Verify(it => it.Log(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.Is<LogLevel>(data => data == LogLevel.Error),
                   It.IsAny<IDictionary<string, object>>()
                   ), Times.Never);
            }
        }

        [Fact]
        public void LogExceptionAsWarning()
        {
            using (var springContext = this.GetContext("Case1"))
            {
                var advice = springContext.GetObject<ExceptionHandlerAroundAdvice>("ExceptionHandlerAroundAdvice");

                var mock = new Mock<ILogger>();
                advice.EnableDebug = false;
                advice.Logger = mock.Object;
                advice.BusinessExceptionTypes.Add(typeof(NewException));

                var service = springContext.GetObject<IContextTargetService>("Service");
                try
                {
                    service.Test(dp1 =>
                    {
                        throw new NewException();
                    });
                }
                catch (Exception)
                {

                }

                mock.Verify(it => it.Log(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<LogLevel>(data => data == LogLevel.Debug),
                    It.IsAny<IDictionary<string, object>>()
                    ), Times.Never);

                mock.Verify(it => it.Log(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.Is<LogLevel>(data => data == LogLevel.Warn),
                   It.IsAny<IDictionary<string, object>>()
                   ), Times.Exactly(1));

                mock.Verify(it => it.Log(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.Is<LogLevel>(data => data == LogLevel.Error),
                   It.IsAny<IDictionary<string, object>>()
                   ), Times.Never);
            }
        }


        [Fact]
        public void LogExceptionAsError()
        {
            using (var springContext = this.GetContext("Case1"))
            {
                var advice = springContext.GetObject<ExceptionHandlerAroundAdvice>("ExceptionHandlerAroundAdvice");

                var mock = new Mock<ILogger>();
                advice.EnableDebug = false;
                advice.Logger = mock.Object;
                advice.BusinessExceptionTypes.Add(typeof(NewException));

                var service = springContext.GetObject<IContextTargetService>("Service");
                try
                {
                    service.Test(dp1 =>
                    {
                        throw new NullReferenceException();
                    });
                }
                catch (Exception)
                {

                }

                mock.Verify(it => it.Log(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<LogLevel>(data => data == LogLevel.Debug),
                    It.IsAny<IDictionary<string, object>>()
                    ), Times.Never);

                mock.Verify(it => it.Log(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<LogLevel>(data => data == LogLevel.Warn),
                    It.IsAny<IDictionary<string, object>>()
                    ), Times.Never);

                mock.Verify(it => it.Log(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.Is<LogLevel>(data => data == LogLevel.Fatal),
                   It.IsAny<IDictionary<string, object>>()
                   ), Times.Exactly(1));
            }
        }


        [Fact]
        public void CannotUseForThisMethod()
        {
            using (var springContext = this.GetContext("Case1"))
            {
                var advice = springContext.GetObject<ExceptionHandlerAroundAdvice>("ExceptionHandlerAroundAdvice");

                var mock = new Mock<ILogger>();
                advice.EnableDebug = false;
                advice.Logger = mock.Object;
                advice.BusinessExceptionTypes.Add(typeof(NewException));

                var service = springContext.GetObject<IContextTargetService>("Service");

                Assert.Throws<InvalidOperationException>(() =>
                {
                    service.SuppressException2(() =>
                    {

                    });
                });
            }
        }


        [Fact]
        public void SuppressException()
        {
            using (var springContext = this.GetContext("Case1"))
            {
                var advice = springContext.GetObject<ExceptionHandlerAroundAdvice>("ExceptionHandlerAroundAdvice");

                var mock = new Mock<ILogger>();
                advice.EnableDebug = false;
                advice.Logger = mock.Object;
                advice.BusinessExceptionTypes.Add(typeof(NewException));

                var service = springContext.GetObject<IContextTargetService>("Service");
                service.SuppressException(() =>
                {
                    throw new NewException();
                });

                mock.Verify(it => it.Log(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<LogLevel>(data => data == LogLevel.Debug),
                    It.IsAny<IDictionary<string, object>>()
                    ), Times.Never);

                mock.Verify(it => it.Log(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.Is<LogLevel>(data => data == LogLevel.Warn),
                   It.IsAny<IDictionary<string, object>>()
                   ), Times.Never);

                mock.Verify(it => it.Log(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.Is<LogLevel>(data => data == LogLevel.Error),
                   It.IsAny<IDictionary<string, object>>()
                   ), Times.Exactly(1));

                mock.Verify(it => it.Log(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<LogLevel>(data => data == LogLevel.Fatal),
                    It.IsAny<IDictionary<string, object>>()
                    ), Times.Never);
            }
        }

        [Fact]
        public void UseUndefinedException()
        {
            using (var springContext = this.GetContext("Case1"))
            {
                var advice = springContext.GetObject<ExceptionHandlerAroundAdvice>("ExceptionHandlerAroundAdvice");

                var mock = new Mock<ILogger>();
                advice.EnableDebug = false;
                advice.Logger = mock.Object;
                advice.BusinessExceptionTypes.Add(typeof(NewException));

                var service = springContext.GetObject<IContextTargetService>("Service");

                Assert.Throws<UndefinedException>(() =>
                {
                    service.SuppressException(() =>
                    {
                        throw new UndefinedException();
                    });
                });
            }
        }
    }
}
