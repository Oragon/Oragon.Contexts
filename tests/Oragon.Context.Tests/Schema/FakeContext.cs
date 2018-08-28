using Oragon.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Context.Tests.Schema
{
    public class FakeContext : AbstractContext<FakeAttribute>
    {
        public FakeContext(FakeAttribute contextAttribute, Stack<AbstractContext<FakeAttribute>> contextStack) : base(contextAttribute, contextStack)
        {

        }

        public List<int> ContextData { get; } = new List<int>();

        public FakeAttribute GetContextAttribute() => this.ContextAttribute;

        public string GetContextKey() => this.ContextKey;

        public Stack<AbstractContext<FakeAttribute>> GetContextStack() => this.ContextStack;

    }
}
