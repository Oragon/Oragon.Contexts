using Oragon.Contexts.ExceptionHandling;
using System;
using System.Collections.Generic;
using Xunit;


namespace Oragon.Context.Tests
{
    public class LogContextTests
    {

        [Fact]
        public void LogContextConstructorInitializersTest()
        {
            LogContext logContext = new LogContext();
            Dictionary<string, object> logContextTags = logContext.GetDictionary();

            Assert.True(logContextTags.ContainsKey("LogContextID"));

            Assert.NotEqual(Guid.Parse((string)logContextTags["LogContextID"]), Guid.Empty);

        }

        [Fact]
        public void LogContextEnlistTest()
        {
            using (LogContext parentLogContext = new LogContext(true))
            {
                Assert.Null(parentLogContext.Parent);

                using (LogContext childLogContext = new LogContext(true))
                {
                    Assert.NotNull(childLogContext.Parent);
                    Assert.Same(childLogContext.Parent, parentLogContext);
                }
            }

        }

        [Fact]
        public void LogContextAddAndRemoveTagsTest()
        {
            string tagKey = "teste1";
            string tagValue = DateTime.Now.Ticks.ToString();

            using (LogContext logContext = new LogContext(true))
            {
                Assert.False(logContext.GetDictionary().ContainsKey(tagKey));

                logContext.SetValue(tagKey, tagValue);

                Assert.True(logContext.GetDictionary().ContainsKey(tagKey));

                Assert.Equal(logContext.GetDictionary()[tagKey], tagValue);

                //removing unknow tag does not result in exception
                logContext.Remove(DateTime.Now.Ticks.ToString());

                
                logContext.Remove(tagKey);

                Assert.False(logContext.GetDictionary().ContainsKey(tagKey));
            }

        }

        [Fact]
        public void LogContextCurrentTest()
        {
            using (LogContext otherContext = LogContext.Current)
            {
                Assert.Same(LogContext.Current, otherContext);
            }

            using (LogContext parentLogContext = new LogContext(true))
            {
                Assert.Same(LogContext.Current , parentLogContext);

                using (LogContext childLogContext = new LogContext(true))
                {
                    Assert.Same(LogContext.Current, childLogContext);
                }
            }

        }

        [Fact]
        public void LogContextStandaloneTest()
        {

            using (LogContext parentLogContext = new LogContext(false))
            {
                Assert.NotSame(LogContext.Current, parentLogContext);

                using (LogContext childLogContext = new LogContext(false))
                {
                    Assert.NotSame(LogContext.Current, childLogContext);
                }
            }

        }

    }
}
