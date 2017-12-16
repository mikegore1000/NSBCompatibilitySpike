using System;
using System.Threading.Tasks;
using NServiceBus;
using Payments.Messages.Events;

namespace Orders.Endpoint.Handlers
{
    public class RefundCompletedHandler : IHandleMessages<IRefundCompleted>
    {
        public Task Handle(IRefundCompleted message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Refund completed for payment {message.PaymentReference}");
            return Task.CompletedTask;
        }
    }
}
