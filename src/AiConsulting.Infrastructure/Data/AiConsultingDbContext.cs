using AiConsulting.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Data;

public class AiConsultingDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public AiConsultingDbContext(DbContextOptions<AiConsultingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Service> Services => Set<Service>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Opportunity> Opportunities => Set<Opportunity>();
    public DbSet<PhaseTransition> PhaseTransitions => Set<PhaseTransition>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<StatusChange> StatusChanges => Set<StatusChange>();
    public DbSet<Deliverable> Deliverables => Set<Deliverable>();
    public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
    public DbSet<ProjectTemplate> ProjectTemplates => Set<ProjectTemplate>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<TrainingWeek> TrainingWeeks => Set<TrainingWeek>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<TopicNote> TopicNotes => Set<TopicNote>();
    public DbSet<ContactRequest> ContactRequests => Set<ContactRequest>();
    public DbSet<TrainingChecklistItem> TrainingChecklistItems => Set<TrainingChecklistItem>();
    public DbSet<NotificationConfig> NotificationConfigs => Set<NotificationConfig>();
    public DbSet<ServiceTranslation> ServiceTranslations => Set<ServiceTranslation>();
    public DbSet<PageVisit> PageVisits => Set<PageVisit>();
    public DbSet<ConsultorAvailability> ConsultorAvailabilities => Set<ConsultorAvailability>();
    public DbSet<BookingSlot> BookingSlots => Set<BookingSlot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AiConsultingDbContext).Assembly);
    }
}
