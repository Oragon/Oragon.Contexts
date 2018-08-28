using Oragon.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Context.Tests.Schema
{
    public class FakeDataProcess : AbstractDataProcess<FakeContext, FakeAttribute>
    {
        public void Add(int value)
        {
            this.ObjectContext.ContextData.Add(value);
        }

        public void Subtract(int value)
        {
            this.ObjectContext.ContextData.Add(value * -1);
        }

        public int Sum()
        {
            return this.ObjectContext.ContextData.Sum();
        }

        public List<int> Get()
        {
            return this.ObjectContext.ContextData;
        }

        public string MyContextKey
        {
            get
            {
                return this.ObjectContextKey;
            }
            set
            {
                this.ObjectContextKey = value;
            }
        }

        public FakeContext MyContext => this.ObjectContext;

    }
}
