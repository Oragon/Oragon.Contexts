using Oragon.Architecture.LogEngine.ConsoleApp.Services;
using Oragon.Spring.Context.Support;
using System;

namespace Oragon.Architecture.LogEngine.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new XmlApplicationContext(@".\AppContext.xml");

            var service = context.GetObject<ITestService>();

            service.Test();

            Console.WriteLine("Hello World!");
        }
    }
}
