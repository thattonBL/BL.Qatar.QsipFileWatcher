using System.Threading.Channels;

namespace BL.Internal.Messaging.Interfaces
{
    public interface IInMemoryMessageQueue
    {
        ChannelReader<IInternalEvent> Reader { get; }
        ChannelWriter<IInternalEvent> Writer { get; }
    }
}