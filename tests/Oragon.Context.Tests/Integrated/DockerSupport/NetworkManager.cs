using Docker.DotNet;
using Docker.DotNet.Models;
using System;

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
        }

        public void Connect(ContainerManager container)
        {
            this.Docker.Networks.ConnectNetworkAsync(this.CreateNetworkResponse.ID, new NetworkConnectParameters() { Container = container.CreateResponse.ID }).GetAwaiter().GetResult();
        }

        public void Disconnect(ContainerManager container)
        {
            this.Docker.Networks.DisconnectNetworkAsync(this.CreateNetworkResponse.ID, new NetworkDisconnectParameters() { Container = container.CreateResponse.ID, Force = true }).GetAwaiter().GetResult();
        }



        public void Dispose()
        {
            if (this.CreateNetworkResponse != null)
            {
                this.Docker.Networks.DeleteNetworkAsync(this.CreateNetworkResponse.ID).GetAwaiter().GetResult();
            }
        }




    }
}
