using Oragon.Contexts.NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Context.Tests.Integrated.AppSym.Services
{
    public class TestService : ITestService
    {
        [NHContext("OragonSamples", true)]
        public void Test()
        { 
            
        }
    }
}
