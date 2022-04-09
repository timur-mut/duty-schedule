using ApiModels = DutySchedule.Api.Models;
using DataModels = DutySchedule.Data.Models;

namespace DutySchedule.Api.Infrastructure.ModelExtensions;

public static class DutyQueueExtension
{
    public static ApiModels.DutyQueue ToApiDutyQueue(this DataModels.DutyQueue duty) => new()
    {
        Id = duty.Id,
        Title = duty.Title,
        ParticipantNames = duty.ParticipantNames
    };
}