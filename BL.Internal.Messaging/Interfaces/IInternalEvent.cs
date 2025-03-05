using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Internal.Messaging.Interfaces;

public interface IInternalEvent : INotification
{
    Guid Id { get; init; }
}

public abstract record InternalEvent(Guid Id) : IInternalEvent;
