﻿using System;

namespace PaymentGateway.Domain.Abstractions
{
    public interface IDomainEvent
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}