namespace EventBus.Messages.Events;

public class IntegrationBaseEvent
{
    public IntegrationBaseEvent()
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
    }

    public IntegrationBaseEvent(Guid id, DateTime createTime)
    {
        Id = id;
        CreationTime = createTime;
    }

    public Guid Id { get; private set; }
    public DateTime CreationTime { get; private set; }
}
