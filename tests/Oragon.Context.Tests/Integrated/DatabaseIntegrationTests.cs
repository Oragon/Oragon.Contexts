using Docker.DotNet;
using Docker.DotNet.Models;
using Oragon.Context.Tests.Integrated.DockerSupport;
using System;
using System.Linq;
using Xunit;

namespace Oragon.Context.Tests.Integrated
{
    public class DatabaseIntegrationTests
    {

        [SkippableTheory]
        ////[InlineData("db2")]
        ////[InlineData("mysql")]
        ////[InlineData("oracle")]
        ////[InlineData("postgresql")]
        ////[InlineData("sqlite")]
        [InlineData("sqlserver")]
        public void DatabaseIntegratedTest(string dbTechnology)
        {
            bool isUnderContainer = (System.Environment.OSVersion.Platform == PlatformID.Unix);

            string buildTag = Environment.GetEnvironmentVariable("BUILD_TAG") ?? "jenkins-oragon-oragon-github-Oragon.Contexts-LOCAL-1";

            Oragon.Spring.Context.Support.XmlApplicationContext context = new Oragon.Spring.Context.Support.XmlApplicationContext("assembly://Oragon.Context.Tests/Oragon.Context.Tests.Integrated/DatabaseIntegrationTests.docker.xml");

            Skip.IfNot(context.ContainsObject($"{dbTechnology}.CreateContainerParameters"), "Has no configuration about " + dbTechnology);

            TimeSpan dockerDefaultTimeout = context.GetObject<TimeSpan>("Docker.DefaultTimeout");

            int getLogsRetryCount = context.GetObject<int>($"{dbTechnology}.GetLogsRetryCount");

            string textTofound = context.GetObject<string>($"{dbTechnology}.ExpectedText");

            TimeSpan getLogsWaitTime = context.GetObject<TimeSpan>($"{dbTechnology}.GetLogsWaitTime");

            CreateContainerParameters createContainerParameters = context.GetObject<CreateContainerParameters>($"{dbTechnology}.CreateContainerParameters");

            ContainerStartParameters containerStartParameters = context.GetObject<ContainerStartParameters>($"{dbTechnology}.ContainerStartParameters");

            ContainerLogsParameters containerLogsParameters = context.GetObject<ContainerLogsParameters>($"{dbTechnology}.ContainerLogsParameters");

            //Convention - If runnig outside docker, need expose port to perform the test
            createContainerParameters.HostConfig.PublishAllPorts = !isUnderContainer;

            using (DockerClient docker = new DockerClientConfiguration(this.GetEndpoint()).CreateClient())
            {
                //testing connectivity
                docker.DefaultTimeout = dockerDefaultTimeout;

                using (NetworkManager network = new NetworkManager(docker))
                {
                    network.Create(buildTag);

                    using (ContainerManager container = new ContainerManager(docker))
                    {

                        container.Create(createContainerParameters);

                        if (container.Start(containerStartParameters))
                        {

                            network.Connect(container);

                            container.WaitUntilTextFoundInLog(containerLogsParameters, textTofound, 10, getLogsWaitTime);

                            ContainerInspectResponse containerInfo = container.Inspect();

                            string portKey = createContainerParameters.ExposedPorts.Keys.Single();
                            string dbPort, dbHostname;

                            ContainerManager jenkinsTestContainer = null;

                            if (!isUnderContainer)
                            {
                                dbPort = containerInfo.NetworkSettings.Ports[portKey].Single().HostPort;
                                dbHostname = "127.0.0.1";
                            }
                            else
                            {
                                jenkinsTestContainer = ContainerManager.GetCurrent(docker) ?? throw new InvalidOperationException("ContainerManager.GetCurrent result nothing");

                                network.Connect(jenkinsTestContainer);

                                dbPort = portKey.Split('/', StringSplitOptions.RemoveEmptyEntries).First();

                                dbHostname = containerInfo.Name.Substring(1);//Every container has a dash (/) on start.
                            }

                            try
                            {
                                this.DatabaseIntegratedTestInternal(dbTechnology, dbHostname, dbPort);
                            }
                            finally
                            {
                                if (jenkinsTestContainer != null)
                                {
                                    network.Disconnect(jenkinsTestContainer);
                                }

                            }

                        }
                    }
                }
            }
        }

        private void DatabaseIntegratedTestInternal(string dbTechnology, string dbHostname, string dbPort)
        {
            Oragon.Spring.Context.Support.XmlApplicationContext context = new Oragon.Spring.Context.Support.XmlApplicationContext(
                $"assembly://Oragon.Context.Tests/Oragon.Context.Tests.Integrated.AppSym.Config/custom.db.{dbTechnology}.xml",
                $"assembly://Oragon.Context.Tests/Oragon.Context.Tests.Integrated.AppSym.Config/general.xml"
                );

            Configuration.StaticConfigurationResolver constr = context.GetObject<Oragon.Configuration.StaticConfigurationResolver>("ConnectionString");

            Console.WriteLine("Setting Up Connectionstring for Database Container");

            Console.WriteLine($"   FROM: {constr.Configuration}");

            //Replacing Configuration In Memory
            constr.Configuration = constr.Configuration
                .Replace("db_hostname", dbHostname)
                .Replace("db_port", dbPort);

            Console.WriteLine($"   TO: {constr.Configuration}");


            Console.WriteLine("Start database object creation...");
            //Code First
            Contexts.NHibernate.FluentNHibernateSessionFactoryBuilder sfb = context.GetObject<Oragon.Contexts.NHibernate.FluentNHibernateSessionFactoryBuilder>("SessionFactoryBuilder");


            sfb.OnExposeConfiguration = config =>
            {
                Console.WriteLine("Updating schema");

                NHibernate.Tool.hbm2ddl.SchemaUpdate update = new NHibernate.Tool.hbm2ddl.SchemaUpdate(config);
                update.Execute(true, true);

                Console.WriteLine("Updating finished!");
            };

            NHibernate.ISessionFactory sf = sfb.BuildSessionFactory();
            sfb.OnExposeConfiguration = null;

            Console.WriteLine($"NH Statistics ConnectCount {sf.Statistics.ConnectCount}!");

            Console.WriteLine("Objects created on database!");

            Console.WriteLine("Start functional tests ITestService.Test()");

            context.GetObject<AppSym.Services.ITestService>("TestService").Test();

            Console.WriteLine("End of functional tests ITestService.Test()");

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
