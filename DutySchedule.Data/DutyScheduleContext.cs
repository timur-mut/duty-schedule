using System.Text.Json;
using DutySchedule.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DutySchedule.Data;

public class DutyScheduleContext : DbContext
{
    public DutyScheduleContext(DbContextOptions<DutyScheduleContext> options) : base(options) { }
    
    public DbSet<DutyQueue> DutyQueues { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var splitStringConverter = new ValueConverter<List<string>, string>(
            v => string.Join(";", v), 
            v => v.Split(new[] { ';' }).ToList());

        modelBuilder.Entity<DutyQueue>()
            .Property(e => e.ParticipantNames)
            .HasConversion(splitStringConverter);
    }
}