using Docker.DotNet;
using Docker.DotNet.Models;
using FluentAssertions;
using Oragon.Context.Tests.Integrated.AppSym.Domain;
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
        [InlineData("oracle")]
        [InlineData("postgresql")]
        [InlineData("mysql")]
        [InlineData("sqlite")]
        [InlineData("sqlserver")]
        public void DatabaseIntegratedTest(string dbTechnology)
        {
            bool isUnderContainer = (System.Environment.OSVersion.Platform == PlatformID.Unix);

            string buildTag = Environment.GetEnvironmentVariable("BUILD_TAG") ?? "jenkins-oragon-oragon-github-Oragon.Contexts-LOCAL-1";

            Oragon.Spring.Context.Support.XmlApplicationContext context = new Oragon.Spring.Context.Support.XmlApplicationContext("assembly://Oragon.Context.Tests/Oragon.Context.Tests.Integrated/DatabaseIntegrationTests.docker.xml");

            Skip.IfNot(context.ContainsObject($"{dbTechnology}.CreateContainerParameters"), "Has no configuration about " + dbTechnology);

            TimeSpan dockerDefaultTimeout = context.GetObject<TimeSpan>($"{dbTechnology}.DefaultTimeout");

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

                            network.Connect(container, dbTechnology);

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

                                network.Connect(jenkinsTestContainer, "jenkins_worker");

                                dbPort = portKey.Split('/', StringSplitOptions.RemoveEmptyEntries).First();

                                dbHostname = dbTechnology;
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

        public virtual void DatabaseIntegratedTestInternal(string dbTechnology, string dbHostname, string dbPort)
        {
            Oragon.Spring.Context.Support.XmlApplicationContext context = new Oragon.Spring.Context.Support.XmlApplicationContext(
                $"assembly://Oragon.Context.Tests/Oragon.Context.Tests.Integrated.AppSym.Config/custom.db.{dbTechnology}.xml",
                $"assembly://Oragon.Context.Tests/Oragon.Context.Tests.Integrated.AppSym.Config/general.xml"
                );


            if (context.ContainsObject("ConnectionString"))
            {
                Configuration.StaticConfigurationResolver constr = context.GetObject<Oragon.Configuration.StaticConfigurationResolver>("ConnectionString");

                Console.WriteLine("Setting Up Connectionstring for Database Container");

                Console.WriteLine($"   FROM: {constr.Configuration}");

                //Replacing Configuration In Memory
                constr.Configuration = constr.Configuration
                    .Replace("db_hostname", dbHostname)
                    .Replace("db_port", dbPort);

                Console.WriteLine($"   TO: {constr.Configuration}");
            }

            //Console.WriteLine("Sleep 10 minutes...");
            //System.Threading.Thread.Sleep(TimeSpan.FromMinutes(10));


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

            AppSym.Services.ITestService service = context.GetObject<AppSym.Services.ITestService>("TestService");

            service.RetrieveAll().Should().BeEmpty("Expected none");

            // Create1
            service.Create1();

            service.RetrieveAll().Count().Should().Be(4, "Expected 4 objects");

            // Create2
            service.Create2();

            System.Collections.Generic.List<DomainEntity> all = service.RetrieveAll();

            all.Count().Should().Be(9, "Expected 9 objects");

            all.Where(it => it is Student).Count().Should().Be(2, "Expected 2 students");
            all.Where(it => it is Language).Count().Should().Be(3, "Expected 3 languages");
            all.Where(it => it is Classroom).Count().Should().Be(4, "Expected 4 classrooms");

            all.Where(it => it is Language).Cast<Language>().Single(it => it.LanguageId == "PT").Should().NotBeNull("Expected PT language");
            all.Where(it => it is Language).Cast<Language>().Single(it => it.LanguageId == "EN").Should().NotBeNull("Expected EN language");
            all.Where(it => it is Language).Cast<Language>().Single(it => it.LanguageId == "ES").Should().NotBeNull("Expected ES language");

            Student studentLuiz = all.Where(it => it is Student).Cast<Student>().Single(it => it.FullName == "Luis Carlos Faria");
            Student studentTatiana = all.Where(it => it is Student).Cast<Student>().Single(it => it.FullName == "Tatiana");

            Classroom classroom4 = all.Where(it => it is Classroom).Cast<Classroom>().Single(it => it.Name == "Class 4");
            Classroom classroom3 = all.Where(it => it is Classroom).Cast<Classroom>().Single(it => it.Name == "Class 3");

            studentLuiz.Classrooms.Should().Contain(it => it == classroom3, "Student Luiz must be in Class3");

            classroom3.Students.Should().Contain(it => it == studentLuiz, "Student Luiz must be in Class3");


            studentLuiz.FullName = "Luiz Carlos de Azevedo Faria";

            service.Update(studentLuiz);

            all = service.RetrieveAll();

            all.Where(it => it is Student)
                .Cast<Student>()
                .SingleOrDefault(it => it.FullName == studentLuiz.FullName).Should().NotBeNull("Update does not ok");

            all.ForEach(service.Delete);

            all = service.RetrieveAll();

            all.Count().Should().Be(0, "After delete all itens must be deleted");

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
