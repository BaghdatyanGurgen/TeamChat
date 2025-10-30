using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IPositionRepository
{
    void AddAsync(Position position);
}