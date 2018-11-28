using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;

namespace Oragon.Context.Tests.Integrated.DockerSupport
{
    public class NetworkManager : IDisposable
    {
        public NetworkManager(DockerClient docker)
        {
            this.Docker = docker;

        }

        public DockerClient Docker { get; }
        public NetworksCreateResponse CreateNetworkResponse { get; private set; }

        public void Create(string name)
        {
            this.CreateNetworkResponse = this.Docker.Networks.CreateNetworkAsync(new NetworksCreateParameters(new NetworkCreate() { Driver = "bridge", CheckDuplicate = false }) { Name = name }).GetAwaiter().GetResult();

            Console.WriteLine($"Network {name} created as {this.CreateNetworkResponse.ID}");
        }

        public void Connect(ContainerManager container, string alias)
        {
            string name = container.Inspect().Name;

            this.Docker.Networks.ConnectNetworkAsync(
                this.CreateNetworkResponse.ID,
                new NetworkConnectParameters()
                {
                    Container = container.CreateResponse.ID,
                    EndpointConfig = new EndpointSettings()
                    {
                        Aliases = new List<string>() {
                            alias
                        }
                    }
                }).GetAwaiter().GetResult();

            //this.ConnectedContainers.Insert(0, container);

            Console.WriteLine($"Container {container.CreateResponse.ID} was connected to {this.CreateNetworkResponse.ID}");
        }

        //private List<ContainerManager> ConnectedContainers { get; set; } = new List<ContainerManager>();

        public void Disconnect(ContainerManager container)
        {
            this.Docker.Networks.DisconnectNetworkAsync(this.CreateNetworkResponse.ID, new NetworkDisconnectParameters() { Container = container.CreateResponse.ID, Force = true }).GetAwaiter().GetResult();

            //this.ConnectedContainers.Remove(container);

            Console.WriteLine($"Container {container.CreateResponse.ID} was disconected from {this.CreateNetworkResponse.ID}");
        }



        public void Dispose()
        {
            Console.WriteLine($"Disposing Network {this.CreateNetworkResponse.ID}");

            if (this.CreateNetworkResponse != null)
            {

                //while (this.ConnectedContainers.Any())
                //{
                //    this.Disconnect(this.ConnectedContainers[0]);
                //}

                Console.WriteLine($"Removing Network {this.CreateNetworkResponse.ID}");

                this.Docker.Networks.DeleteNetworkAsync(this.CreateNetworkResponse.ID).GetAwaiter().GetResult();

                Console.WriteLine($"Network Removed {this.CreateNetworkResponse.ID}");
            }

            Console.WriteLine($"Network Disposed {this.CreateNetworkResponse.ID}");
        }




    }
}
