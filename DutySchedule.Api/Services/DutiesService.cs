using DutySchedule.Api.Infrastructure.ModelExtensions;
using DutySchedule.Api.Models;
using DutySchedule.Data;
using Microsoft.EntityFrameworkCore;
using DataModels = DutySchedule.Data.Models;

namespace DutySchedule.Api.Services;

class DutiesService : IDutiesService
{
    public DutiesService(DutyScheduleContext context)
    {
        _context = context;
    }


    public async Task<DutyQueue?> GetById(int id)
    {
       var dutyQueue = await _context.DutyQueues.SingleOrDefaultAsync(dutyQueue => dutyQueue.Id == id);

       return dutyQueue?.ToApiDutyQueue();
    }


    public async Task<List<DutyQueue>> GetAll()
    {
       return await _context.DutyQueues.Select(queue => queue.ToApiDutyQueue()).ToListAsync();
    }


    public async Task<DutyQueue> Create(DutyQueue dutyQueue)
    {
        var newDutyQueue = new DataModels.DutyQueue()
        {
            Title = dutyQueue.Title,
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            ParticipantNames = dutyQueue.ParticipantNames
        };

        _context.DutyQueues.Add(newDutyQueue);
        await _context.SaveChangesAsync();

        return newDutyQueue.ToApiDutyQueue();
    }


    public async Task<List<TodayDutiesItem>> GetTodayDuties()
    {
        var dutyQueues = await _context.DutyQueues.ToListAsync();

        return dutyQueues.Select(queue => new TodayDutiesItem
            {
                QueueTitle = queue.Title, 
                ParticipantName = GetParticipantName(queue)
            })
            .ToList();
    }


    public async Task<List<DutyScheduleItem>> GetDutyQueueForPeriod(int id, int daysPeriod)
    {
        var result = new List<DutyScheduleItem>();
        var dutyQueue = await _context.DutyQueues.FirstOrDefaultAsync(d => d.Id == id);
        if (dutyQueue == null || dutyQueue.ParticipantNames.Count == 0)
            return result;
        
        for (int i = 0; i < daysPeriod; i++)
        {
            result.Add(new DutyScheduleItem
            {
                Date = DateTime.Today.AddDays(i),
                ParticipantName = GetParticipantName(dutyQueue, i)
            });
        }

        return result;
    }
    

    public async Task<bool> Update(DutyQueue dutyQueue)
    {
        var dutyQueueForUpdate = await _context.DutyQueues.SingleOrDefaultAsync();
        if (dutyQueueForUpdate == null)
        {
            return false;
        }

        dutyQueueForUpdate.Title = dutyQueue.Title;
        dutyQueueForUpdate.StartDate = DateOnly.FromDateTime(DateTime.Now);
        dutyQueueForUpdate.ParticipantNames = dutyQueue.ParticipantNames;

        await _context.SaveChangesAsync();

        return true;
    }


    public async Task<bool> AddParticipant(int dutyQueueId, string participantName)
    {
        var dutyQueue = await _context.DutyQueues.FindAsync(dutyQueueId);
        if (dutyQueue == null)
            return false;

        dutyQueue.ParticipantNames = new List<string>(dutyQueue.ParticipantNames) { participantName };
        await _context.SaveChangesAsync();
        return true;
    }


    private static string GetParticipantName(DataModels.DutyQueue dutyQueue, int offset = 0)
    {
        var daysFromStart = (DateTime.Today - dutyQueue.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays;
        var participantIndex = (int)(daysFromStart + offset) % dutyQueue.ParticipantNames.Count;
        return dutyQueue.ParticipantNames[participantIndex];
    }

    private readonly DutyScheduleContext _context;
}