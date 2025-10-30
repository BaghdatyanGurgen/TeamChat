using TeamChat.Domain.Entities;

namespace TeamChat.Application.DTOs.Company
{
    public record NullableCompanyResponse(Domain.Entities.Company? Company);
    public record CompanyResponse (Domain.Entities.Company Company);
    public record CompanyUserResponse(CompanyUser CompanyUser);
    public record GetUserCompaniesResponse (IEnumerable<Domain.Entities.Company> Companies);
    public record PositionResponse (Position Position);
}