namespace TeamChat.Application.DTOs.Team;

public record TeamResponse(int Id, string Name, int CompanyId)
{
    public TeamResponse(Domain.Entities.Team team) : this(team.Id, team.Name, team.CompanyId)
    {
    }
}
