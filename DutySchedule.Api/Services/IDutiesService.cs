using DutySchedule.Api.Models;

namespace DutySchedule.Api.Services;

public interface IDutiesService
{
    Task<DutyQueue?> GetById(int id);
    Task<List<DutyQueue>> GetAll();
    Task<DutyQueue> Create(DutyQueue dutyQueue);
    Task<List<DutyScheduleItem>> GetDutyQueueForPeriod(int id, int daysPeriod);
    Task<bool> AddParticipant(int dutyQueueId, string participantName);
    Task<List<TodayDutiesItem>> GetTodayDuties();
    Task<bool> Update(DutyQueue dutyQueue);
}