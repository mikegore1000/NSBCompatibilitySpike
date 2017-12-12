using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Persistence;

namespace Payments.Bridge
{
    class Program
    {
        static async Task Main()
        {
            var dbConnectionString = Environment.GetEnvironmentVariable("NSBCompat_DBConnectionString");
            var asbConnectionString = Environment.GetEnvironmentVariable("NSBCompat_ASBConnectionString");

            var bridgeConfiguration = NServiceBus.Bridge.Bridge
                .Between<MsmqTransport>(
                    endpointName: "payments.bridge.endpoint.msmq",
                    customization: transportExtensions =>
                    {
                        transportExtensions.Transactions(TransportTransactionMode.ReceiveOnly);
                    })
                .And<AzureServiceBusTransport>(
                    endpointName: "payments.bridge.endpoint.asb",
                    customization: transportExtensions =>
                    {
                        transportExtensions.ConnectionString(asbConnectionString);
                        transportExtensions.Transactions(TransportTransactionMode.ReceiveOnly);
                        var topology = transportExtensions.UseForwardingTopology();
                        topology.NumberOfEntitiesInBundle(1);
                    });


            bridgeConfiguration.UseSubscriptionPersistece<NHibernatePersistence>((configuration, extensions) => configuration
                .UsePersistence<NHibernatePersistence>().ConnectionString(dbConnectionString));
            bridgeConfiguration.AutoCreateQueues();

            var bridge = bridgeConfiguration.Create();
            await bridge.Start();

            Console.ReadLine();
        }
    }
}
