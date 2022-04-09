using DutySchedule.Api.Models;
using DutySchedule.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DutySchedule.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DutiesController : ControllerBase
{

    public DutiesController(IDutiesService dutiesService)
    {
        _dutiesService = dutiesService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DutyQueue>>> GetAll()
    {
        return await _dutiesService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DutyQueue>> GetById(int id)
    {
        var dutyQueue = await _dutiesService.GetById(id);

        if (dutyQueue == null)
        {
            return NotFound();
        }

        return dutyQueue;
    }

    [HttpPost]
    public async Task<ActionResult<DutyQueue>> Create(DutyQueue dutyQueue)
    {
        var createdQueue = await _dutiesService.Create(dutyQueue);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdQueue.Id },
            createdQueue);
    }

    [HttpGet("GetTodayDuties")]
    public async Task<ActionResult<List<TodayDutiesItem>>> GetTodayDuties()
    {
        return await _dutiesService.GetTodayDuties();
    }

    [HttpGet("{id}/{daysPeriod}")]
    public async Task<ActionResult<List<DutyScheduleItem>>> GetById(int id, int daysPeriod)
    {
        return await _dutiesService.GetDutyQueueForPeriod(id, daysPeriod);
    }

    [HttpPost("AddParticipant")]
    public async Task<ActionResult<DutyQueue>> AddParticipant(int id, string participantName)
    {
        var added = await _dutiesService.AddParticipant(id, participantName);
        if (added)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<ActionResult<DutyQueue>> Update(DutyQueue dutyQueue)
    {
        var updated = await _dutiesService.Update(dutyQueue);
        if (updated)
        {
            return Ok();
        }

        return BadRequest();
    }

    private readonly IDutiesService _dutiesService;
}