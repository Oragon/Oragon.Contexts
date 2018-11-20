using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using Xunit;

namespace Oragon.Context.Tests.Integrated
{
    public class DatabaseIntegrationTests
    {

        //[SkippableTheory]
        ////[InlineData("db2")]
        ////[InlineData("mysql")]
        ////[InlineData("oracle")]
        ////[InlineData("postgresql")]
        ////[InlineData("sqlite")]
        //[InlineData("sqlserver")]
        public void DatabaseIntegratedTest(string dbTechnology)
        {
            Oragon.Spring.Context.Support.XmlApplicationContext context = new Oragon.Spring.Context.Support.XmlApplicationContext("assembly://Oragon.Context.Tests/Oragon.Context.Tests.Integrated/DatabaseIntegrationTests.docker.xml");

            Skip.IfNot(context.ContainsObject($"{dbTechnology}.CreateContainerParameters"), "Has no configuration about " + dbTechnology);

            CreateContainerParameters createContainerParameters = context.GetObject<CreateContainerParameters>($"{dbTechnology}.CreateContainerParameters");

            using (DockerClient docker = new DockerClientConfiguration(this.GetEndpoint()).CreateClient())
            {
                //testing connectivity
                docker.DefaultTimeout = context.GetObject<TimeSpan>("Docker.DefaultTimeout");

                CreateContainerResponse containerId = docker.Containers.CreateContainerAsync(createContainerParameters).GetAwaiter().GetResult();

                bool isOk = false;
                if (docker.Containers.StartContainerAsync(containerId.ID, context.GetObject<ContainerStartParameters>()).GetAwaiter().GetResult())
                {

                    string textTofound = context.GetObject<string>("sqlserver.ExpectedText");

                    int getLogsRetryCount = context.GetObject<int>("GetLogsRetryCount");

                    string logs = null;

                    for (int i = 0; i < getLogsRetryCount; i++)
                    {
                        System.Threading.Thread.Sleep(context.GetObject<TimeSpan>("GetLogsWaitTime"));

                        using (System.IO.Stream logStream = docker.Containers.GetContainerLogsAsync(containerId.ID, context.GetObject<ContainerLogsParameters>()).GetAwaiter().GetResult())
                        {

                            using (System.IO.StreamReader reader = new System.IO.StreamReader(logStream))
                            {
                                logs = reader.ReadToEnd();
                            }

                            if (!string.IsNullOrWhiteSpace(logs))
                            {
                                isOk = logs.Contains(textTofound);
                                if (isOk)
                                {
                                    break;
                                }
                            }
                        }

                    }

                    if (!isOk)
                    {

                        docker.Containers.StopContainerAsync(containerId.ID, new ContainerStopParameters() { WaitBeforeKillSeconds=10 }).GetAwaiter().GetResult();
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
                        docker.Containers.RemoveContainerAsync(containerId.ID, new ContainerRemoveParameters() { Force = true, RemoveVolumes = true }).GetAwaiter().GetResult();

                        Skip.IfNot(true, "TimeOut");

                    }
                    /*Begin Docker Test*/
                    try
                    {
                        this.DatabaseIntegratedTestInternal(dbTechnology);
                    }
                    finally
                    {
                        /*End Docker Test*/
                        docker.Containers.StopContainerAsync(containerId.ID, new ContainerStopParameters() { WaitBeforeKillSeconds = 10 }).GetAwaiter().GetResult();
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
                        docker.Containers.RemoveContainerAsync(containerId.ID, new ContainerRemoveParameters() { Force = true, RemoveVolumes = true }).GetAwaiter().GetResult();
                    }

                }

            }

        }



        private void DatabaseIntegratedTestInternal(string dbTechnology)
        { 
        




        }


        private Uri GetEndpoint()
        {
            if (System.Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return new Uri("unix:///var/run/docker.sock");
            }
            else if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                //return new Uri("npipe://./pipe/docker");
                return new Uri("tcp://localhost:2375");
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
