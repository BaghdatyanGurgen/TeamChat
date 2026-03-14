using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChat.API.Extensions;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Company;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController(ICompanyService companyService) : BaseController
{
    private readonly ICompanyService _companyService = companyService;

    [Authorize]
    [HttpPost("create-company")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
    {
        var result = await _companyService.CreateCompanyAsync(CurrentUserId, request);

        return result.ToActionResult();
    }

    [HttpPatch("{companyId:int}/set-details")]
    public async Task<IActionResult> SetDetails([FromRoute] int companyId,
                                                [FromForm] SetCompanyDetailsRequest request)
    {
        var userId = CurrentUserId;

        var result = await _companyService.SetCompanyDetailsAsync(companyId, request);

        return result.ToActionResult();
    }

    [HttpPost("{companyId:int}/departments")]
    public async Task<IActionResult> CreateCompanyDepartment([FromRoute] int companyId,
                                                             [FromBody] CreateCompanyDepartmentRequest request)
    {
        var result = await _companyService.CreateCompanyDepartmentAsync(CurrentUserId, companyId, request);

        return result.ToActionResult();
    }

    [HttpPut("{companyId:int}/create-position")]
    public async Task<IActionResult> CreateCompanyPosition([FromRoute] int companyId,
                                                           [FromBody] CreateCompanyPositionRequest request)
    {
        var companyUserResponse = await _companyService.GetCompanyUserByUserIdAsync(CurrentUserId, companyId);
        if (companyUserResponse.Data is null)
            return BadRequest(companyUserResponse.Message);

        var result = await _companyService.CreateCompanyPositionAsync(companyUserResponse.Data, companyId, request);

        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("my-companies")]
    public async Task<IActionResult> GetMyCompanies()
    {
        var result = await _companyService.GetUserCompaniesAsync(CurrentUserId);

        return result.ToActionResult();
    }

    [HttpPost("join-by-invite")]
    public async Task<IActionResult> JoinByInvite([FromBody] JoinCompanyByInviteRequest request)
    {
        var result = await _companyService.JoinCompanyByInviteAsync(CurrentUserId, request);
       
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("{companyId:int}/me")]
    public async Task<IActionResult> GetCompanyUserInfo([FromRoute] int companyId)
    {
        var result = await _companyService.GetCompanyUserByUserIdAsync(CurrentUserId, companyId);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("{companyId:int}/positions/user")]
    public async Task<IActionResult> GetUserPositions([FromRoute] int companyId)
    {
        var result = await _companyService.GetUserPositionsAsync(CurrentUserId, companyId);
        return result.ToActionResult();
    }
}