namespace DutySchedule.Data.Models;

public class DutyQueue
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public List<string> ParticipantNames { get; set; } = null!;
}