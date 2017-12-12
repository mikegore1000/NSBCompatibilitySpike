using System;
using Payments.Messages.Commands;

namespace ASBClient
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            var asbConnectionString = Environment.GetEnvironmentVariable("NSBCompat_ASBConnectionString");

            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            var topology = transport.UseForwardingTopology();
            var routing = transport.Routing();
            var conventions = endpointConfiguration.Conventions();
            var bridge = routing.ConnectToBridge("payments.bridge.endpoint.asb");

            transport.ConnectionString(asbConnectionString);
            topology.NumberOfEntitiesInBundle(1);

            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));

            bridge.RouteToEndpoint(typeof(RefundPayment), "Payments.Endpoint.Distributor");


            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.SendOnly();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
        }
    }

    public class MessageSender : IWantToRunWhenEndpointStartsAndStops
    {
        public Task Start(IMessageSession session)
        {
            while (true)
            {
                Console.WriteLine("Press a key to send a message");
                Console.ReadKey();
                session.Send(new RefundPayment(Guid.NewGuid(), 100m));
            }
        }

        public Task Stop(IMessageSession session)
        {
            return Task.CompletedTask;
        }
    }
}
