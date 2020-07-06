namespace PolicyService.Application.Common.Interfaces
{
    public interface IEventPublisher
    {
        void PublishMessage<T>(T message);
    }
}
