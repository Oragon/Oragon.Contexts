using Oragon.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Context.Tests.Schema
{
    public class FakeAroundAdvice : AbstractContextAroundAdvice<FakeContext, FakeAttribute>
    {
        

        protected override string ContextStackListKey => "Oragon.Context.Tests.Schema.FakeAroundAdvice#ContextStackListKey";

        protected override Func<FakeAttribute, bool> AttributeQueryFilter => null;

        protected override object Invoke(global::AopAlliance.Intercept.IMethodInvocation invocation, IEnumerable<FakeAttribute> contextAttributes)
        {
            using (FakeContext context = new FakeContext(contextAttributes.First(), this.ContextStack))
            {
                return invocation.Proceed();
            }
        }

        public Stack<AbstractContext<FakeAttribute>> GetContextStack() => this.ContextStack;
    }
}
