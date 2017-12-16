using System;

namespace Payments.Messages.Events
{
    public interface IRefundCompleted
    {
        decimal Amount { get; }
        Guid PaymentReference { get; }
    }
}