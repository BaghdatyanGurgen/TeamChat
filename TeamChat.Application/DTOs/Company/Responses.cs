using TeamChat.Domain.Entities;

namespace TeamChat.Application.DTOs.Company
{
    public record CompanyResponse (Domain.Entities.Company Company);
    public record SetCompanyDetailsResponse(int Id, string Name, string Description, string? LogoUrl);
    public record CreateCompanyDepartmentResponse(int Id, string Name, string? Description)
    {
        public CreateCompanyDepartmentResponse(Department department)
            : this(department.Id, department.Name, department.Description) { }
    }
    public record CreateCompanyPositionResponse(Position Position);
}