namespace DutySchedule.Api.Models;

public class DutyQueue
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<string> ParticipantNames { get; set; } = null!;
}