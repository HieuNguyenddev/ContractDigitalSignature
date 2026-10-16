using ContractDigitalSignature.Modules.Roles.Common.DTOs;
using ContractDigitalSignature.Modules.Roles.DTOs;
using ContractDigitalSignature.Modules.Roles.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractDigitalSignature.Modules.Roles.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roles;

    public RolesController(IRoleService roles)
    {
        _roles = roles;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<RoleResponse>>> GetList(
        string? search,
        int page = 1,
        int pageSize = RoleService.DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _roles.SearchAsync(search, page, pageSize, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoleResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _roles.GetByIdAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<RoleResponse>> Create(RoleRequest request, CancellationToken cancellationToken)
    {
        var created = await _roles.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RoleResponse>> Update(
        Guid id,
        RoleRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await _roles.UpdateAsync(id, request, cancellationToken));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _roles.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
