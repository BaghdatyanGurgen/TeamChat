using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Application.DTOs.Company;
using TeamChat.Domain.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using TeamChat.Application.Abstraction.Services;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController(ICompanyService companyService) : ControllerBase
{
    private readonly ICompanyService _companyService = companyService;

    [Authorize]
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var directorGuidId))
            throw new UserNotFoundException();

        var result = await _companyService.CreateCompanyAsync(directorGuidId, request);
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPatch("{companyId:int}/set-details")]
    public async Task<IActionResult> SetDetails([FromRoute] int companyId, 
                                                [FromForm] SetCompanyDetailsRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out _))
            throw new UserNotFoundException();

        var result = await _companyService.SetCompanyDetailsAsync(companyId, request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPut("{companyId:int}/create-department")]
    public async Task<IActionResult> CreateCompanyDepartment([FromRoute] int companyId,
                                                             [FromBody] CreateCompanyDepartmentRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var directorGuidId))
            throw new UserNotFoundException();

        var result = await _companyService.CreateCompanyDepartmentAsync(directorGuidId, companyId, request);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        
        return Ok(result);
    }

    [HttpPut("{companyId:int}/create-possition")]
    public async Task<IActionResult> CreateCompanyPosition([FromRoute] int companyId,
                                                           [FromBody] CreateCompanyPositionRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var directorGuidId))
            throw new UserNotFoundException();

        var companyUserResponse = await _companyService.GetCompanyUserByUserIdAsync(directorGuidId, companyId);
        if (companyUserResponse.Data is null)
            return BadRequest(companyUserResponse.Message);

        var result = await _companyService.CreateCompanyPositionAsync(companyUserResponse.Data, companyId, request);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        
        return Ok(result);
    }
}