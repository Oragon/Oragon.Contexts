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
            

            //this.DatabaseIntegratedTestInternal(dbTechnology); return;

            string buildTag = Environment.GetEnvironmentVariable("BUILD_TAG") ?? "jenkins-oragon-oragon-github-Oragon.Contexts-LOCAL-1";

            Oragon.Spring.Context.Support.XmlApplicationContext context = new Oragon.Spring.Context.Support.XmlApplicationContext("assembly://Oragon.Context.Tests/Oragon.Context.Tests.Integrated/DatabaseIntegrationTests.docker.xml");

            Skip.IfNot(context.ContainsObject($"{dbTechnology}.CreateContainerParameters"), "Has no configuration about " + dbTechnology);


            TimeSpan dockerDefaultTimeout = context.GetObject<TimeSpan>("Docker.DefaultTimeout");

            int getLogsRetryCount = context.GetObject<int>($"{dbTechnology}.GetLogsRetryCount");
            string textTofound = context.GetObject<string>($"{dbTechnology}.ExpectedText");
            TimeSpan getLogsWaitTime = context.GetObject<TimeSpan>($"{dbTechnology}.GetLogsWaitTime");
            CreateContainerParameters createContainerParameters = context.GetObject<CreateContainerParameters>($"{dbTechnology}.CreateContainerParameters");
            createContainerParameters.HostConfig.PublishAllPorts = !isUnderContainer;

            ContainerStartParameters containerStartParameters = context.GetObject<ContainerStartParameters>($"{dbTechnology}.ContainerStartParameters");
            ContainerLogsParameters containerLogsParameters = context.GetObject<ContainerLogsParameters>($"{dbTechnology}.ContainerLogsParameters");

            using (DockerClient docker = new DockerClientConfiguration(this.GetEndpoint()).CreateClient())
            {
                //testing connectivity
                docker.DefaultTimeout = dockerDefaultTimeout;

                using (ContainerManager container = new ContainerManager(docker))
                {
                    using (NetworkManager network = new NetworkManager(docker))
                    {

                        network.Create(buildTag);

                        container.Create(createContainerParameters);

                        if (container.Start(containerStartParameters))
                        {

                            network.Connect(container);

                            container.WaitUntilTextFoundInLog(containerLogsParameters, textTofound, 10, getLogsWaitTime);

                            ContainerInspectResponse containerInfo = container.Inspect();

                            string portKey = createContainerParameters.ExposedPorts.Keys.Single();
                            string dbPort;
                            string dbHostname;
                            if (!isUnderContainer)
                            {
                                dbPort = containerInfo.NetworkSettings.Ports[portKey].Single().HostPort;
                                dbHostname = "127.0.0.1";
                            }
                            else
                            {
                                ContainerManager jenkinsTestContainer = ContainerManager.GetCurrent(docker);
                                if (jenkinsTestContainer == null)
                                {
                                    throw new InvalidOperationException("ContainerManager.GetCurrent result nothing");
                                }

                                network.Connect(jenkinsTestContainer);

                                dbPort = portKey.Split('/', StringSplitOptions.RemoveEmptyEntries).First();
                                dbHostname = containerInfo.Name;
                            }

                            this.DatabaseIntegratedTestInternal(dbTechnology, dbHostname, dbPort);

                            network.Disconnect(container);

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

            //Code First
            var sfb = context.GetObject<Oragon.Contexts.NHibernate.FluentNHibernateSessionFactoryBuilder>("SessionFactoryBuilder");

            EventHandler<NHibernate.Cfg.Configuration> onExposeConfiguration = (sender, config) =>
            {
                var update = new NHibernate.Tool.hbm2ddl.SchemaUpdate(config);
                update.Execute(true, true);

            };
            sfb.OnExposeConfiguration += onExposeConfiguration;
            sfb.BuildSessionFactory();
            sfb.OnExposeConfiguration -= onExposeConfiguration;


            context.GetObject<AppSym.Services.ITestService>().Test();

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
