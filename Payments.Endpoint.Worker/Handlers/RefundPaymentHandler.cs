using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Payments.Messages.Commands;

namespace Payments.Endpoint.Worker.Handlers
{
    public class RefundPaymentHandler : IHandleMessages<RefundPayment>
    {
        public Task Handle(RefundPayment message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Refunded {message.Amount} on payment {message.PaymentReference}");
            context.SendLocal(new TestCommand("Hello"));
            return Task.CompletedTask;
        }
    }

    public class TestCommandHandler : IHandleMessages<TestCommand>
    {
        public Task Handle(TestCommand message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Test message = {message.Message}");
            return Task.CompletedTask;
        }
    }
}
