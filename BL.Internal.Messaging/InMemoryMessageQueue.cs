using BL.Internal.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BL.Internal.Messaging;

public sealed class InMemoryMessageQueue : IInMemoryMessageQueue
{
    private readonly Channel<IInternalEvent> _channel = Channel.CreateUnbounded<IInternalEvent>();

    public ChannelReader<IInternalEvent> Reader => _channel.Reader;

    public ChannelWriter<IInternalEvent> Writer => _channel.Writer;
}
