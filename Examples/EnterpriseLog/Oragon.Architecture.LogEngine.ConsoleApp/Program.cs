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

            var machineName = System.Environment.MachineName;

            var os = System.Environment.OSVersion.ToString();



            var service = context.GetObject<ITestService>();

            service.Test5Delete();

            service.Test1Create();

            service.Test2Compare();

            service.Test3Change();

            service.Test4Compare();

            service.Test5Delete();

            Console.WriteLine("Hello World!");
        }
    }
}
