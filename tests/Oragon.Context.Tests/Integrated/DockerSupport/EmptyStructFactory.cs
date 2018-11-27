using Docker.DotNet.Models;

namespace Oragon.Context.Tests.Integrated.DockerSupport
{
    public class EmptyStructFactory
    {
        public static EmptyStruct BuildEmpty()
        {
            return new EmptyStruct();

        }

    }
}