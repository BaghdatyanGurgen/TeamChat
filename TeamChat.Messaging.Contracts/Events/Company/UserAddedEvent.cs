using TeamChat.Messaging.Contracts.Payload;

namespace TeamChat.Messaging.Contracts.Events.Company;

public class UserAddedEvent(UserAddedEventPayload payload) : Event(payload)
{
    public override string EventName => EventNames.Company.UserAdded;
}

public record UserAddedEventPayload(int CompanyId, Guid UserId, int PositionId) : BasePayload();