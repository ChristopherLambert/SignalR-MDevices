using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCTech.Opstech.Core.Api.Infrastructure.Api.Controllers;
using NCTech.Opstech.Core.Infrastructure.CQRS;

namespace NCTech.Opstech.Oee.Api.Controllers.Equipments.v1;


[ApiVersion(1)]
[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
public class DeviceMessageController : NCApiController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    // private readonly IEquipmentEntityService _equipmentEntityService;

    public DeviceMessageController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher
        /* ,IEquipmentEntityService equipmentEntityService*/)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        // _equipmentEntityService = equipmentEntityService;
    }


    //[HttpGet]
    //[Authorize]
    //[MapToApiVersion(1)]
    //[ProducesResponseType(typeof(List<EquipmentQueryDto>), 200)]
    //[ProducesResponseType(500)]
    //[ProducesResponseType(401)]
    //public async Task<IActionResult> Get()
    //{
    //    var query = new GetAllEquipmentQuery();
    //    var result = await _queryDispatcher.DispatchAsync<GetAllEquipmentQuery, List<EquipmentQueryDto>>(query);
    //    return Ok(result);
    //}

    //[Authorize]
    //[HttpGet("{id}")]
    //[MapToApiVersion(1)]
    //[ProducesResponseType(typeof(EquipmentQueryDto), 200)]
    //[ProducesResponseType(404)]
    //[ProducesResponseType(500)]
    //[ProducesResponseType(401)]
    //public async Task<IActionResult> Get([FromRoute][Required] int id)
    //{
    //    var query = new GetEquipmentByIdQuery { Id = id };
    //    var result = await _queryDispatcher.DispatchAsync<GetEquipmentByIdQuery, EquipmentQueryDto>(query);
    //    return Ok(result);
    //}
}