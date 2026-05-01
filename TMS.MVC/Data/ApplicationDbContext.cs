using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Models;
using TMS.MVC.Models.Tickets;

namespace TMS.MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<City> Cities => Set<City>();

        public DbSet<LegalEntity> LegalEntities => Set<LegalEntity>();
        public DbSet<LegalEntityContact> LegalEntityContacts => Set<LegalEntityContact>();
        public DbSet<LegalEntityAddress> LegalEntityAddresses => Set<LegalEntityAddress>();
        public DbSet<LegalEntityBankAccount> LegalEntityBankAccounts => Set<LegalEntityBankAccount>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<PermissionDefinition> PermissionDefinitions => Set<PermissionDefinition>();
        public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        public DbSet<TransportAgreement> TransportAgreements => Set<TransportAgreement>();

        public DbSet<Tractor> Tractors => Set<Tractor>();
        public DbSet<TractorBankAccount> TractorBankAccounts => Set<TractorBankAccount>();
        public DbSet<TractorContact> TractorContacts => Set<TractorContact>();
        public DbSet<TractorAddress> TractorAddresses => Set<TractorAddress>();


        public DbSet<DriverProfile> DriverProfiles => Set<DriverProfile>();
        public DbSet<DriverBankAccount> DriverBankAccounts => Set<DriverBankAccount>();
        public DbSet<DriverContact> DriverContacts => Set<DriverContact>();
        public DbSet<DriverAddress> DriverAddresses => Set<DriverAddress>();


        public DbSet<Havaleh> Havalehs => Set<Havaleh>();
        public DbSet<SubHavaleh> SubHavalehs => Set<SubHavaleh>();
        public DbSet<SubHavalehIntermediateCity> SubHavalehIntermediateCities => Set<SubHavalehIntermediateCity>();

        public DbSet<TractorAssignment> TractorAssignments { get; set; }
        public DbSet<LoadingDocument> LoadingDocuments { get; set; }
        public DbSet<UnloadingDocument> UnloadingDocuments { get; set; }
        public DbSet<LocationTracking> LocationTrackings { get; set; }

        // ========== جدید: چت ==========
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<TicketMessage> TicketMessages => Set<TicketMessage>();
        public DbSet<TicketAttachment> TicketAttachments => Set<TicketAttachment>();
        public DbSet<TicketCategory> TicketCategories => Set<TicketCategory>();
        public DbSet<TicketStatusHistory> TicketStatusHistories => Set<TicketStatusHistory>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<City>()
                .HasIndex(x => new { x.CountryName, x.ProvinceName, x.Name })
                .IsUnique();

            builder.Entity<LegalEntityContact>()
                .HasOne(x => x.LegalEntity)
                .WithMany(x => x.Contacts)
                .HasForeignKey(x => x.LegalEntityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LegalEntityAddress>()
                .HasOne(x => x.LegalEntity)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.LegalEntityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LegalEntityBankAccount>()
                .HasOne(x => x.LegalEntity)
                .WithMany(x => x.BankAccounts)
                .HasForeignKey(x => x.LegalEntityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasIndex(x => x.Name)
                .IsUnique();

            builder.Entity<PermissionDefinition>()
                .HasIndex(x => x.Key)
                .IsUnique();

            builder.Entity<UserPermission>()
                .HasIndex(x => new { x.UserId, x.PermissionDefinitionId })
                .IsUnique();

            builder.Entity<RolePermission>()
                .HasIndex(x => new { x.RoleName, x.PermissionDefinitionId })
                .IsUnique();

            builder.Entity<TransportAgreement>()
                .HasIndex(x => x.Title);

            builder.Entity<Tractor>()
                .HasIndex(x => x.PolicePlateNumber)
                .IsUnique();

            builder.Entity<TractorBankAccount>()
                .HasOne(x => x.Tractor)
                .WithMany(x => x.BankAccounts)
                .HasForeignKey(x => x.TractorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TractorContact>()
                .HasOne(x => x.Tractor)
                .WithMany(x => x.Contacts)
                .HasForeignKey(x => x.TractorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TractorAddress>()
                .HasOne(x => x.Tractor)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.TractorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DriverProfile>()
            .HasIndex(x => x.ApplicationUserId)
            .IsUnique();

            builder.Entity<DriverBankAccount>()
                .HasOne(x => x.DriverProfile)
                .WithMany(x => x.BankAccounts)
                .HasForeignKey(x => x.DriverProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DriverContact>()
                .HasOne(x => x.DriverProfile)
                .WithMany(x => x.Contacts)
                .HasForeignKey(x => x.DriverProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DriverAddress>()
                .HasOne(x => x.DriverProfile)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.DriverProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Havaleh>()
                .HasIndex(x => x.HavalehNumber)
                .IsUnique();

            builder.Entity<Havaleh>()
                .HasOne(x => x.TransportContractorLegalEntity)
                .WithMany()
                .HasForeignKey(x => x.TransportContractorLegalEntityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Havaleh>()
                .HasOne(x => x.GoodsOwnerLegalEntity)
                .WithMany()
                .HasForeignKey(x => x.GoodsOwnerLegalEntityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Havaleh>()
                .HasOne(x => x.OriginCity)
                .WithMany()
                .HasForeignKey(x => x.OriginCityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Havaleh>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SubHavaleh>()
                .HasOne(x => x.Havaleh)
                .WithMany(x => x.SubHavalehs)
                .HasForeignKey(x => x.HavalehId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SubHavaleh>()
                .HasOne(x => x.DestinationCity)
                .WithMany()
                .HasForeignKey(x => x.DestinationCityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SubHavalehIntermediateCity>()
                .HasOne(x => x.SubHavaleh)
                .WithMany(x => x.IntermediateCities)
                .HasForeignKey(x => x.SubHavalehId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SubHavalehIntermediateCity>()
                .HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TractorAssignment>()
                .HasOne(a => a.SubHavaleh)
                .WithMany(s => s.TractorAssignments)
                .HasForeignKey(a => a.SubHavalehId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TractorAssignment>()
                .HasOne(a => a.Tractor)
                .WithMany()
                .HasForeignKey(a => a.TractorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TractorAssignment>()
                .HasOne(a => a.DriverProfile)
                .WithMany()
                .HasForeignKey(a => a.DriverProfileId)
                .OnDelete(DeleteBehavior.SetNull);

            // تنظیمات LoadingDocument
            builder.Entity<LoadingDocument>()
                .HasOne(d => d.TractorAssignment)
                .WithMany(t => t.LoadingDocuments)
                .HasForeignKey(d => d.TractorAssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // تنظیمات UnloadingDocument
            builder.Entity<UnloadingDocument>()
                .HasOne(d => d.TractorAssignment)
                .WithMany(t => t.UnloadingDocuments)
                .HasForeignKey(d => d.TractorAssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // تنظیمات LocationTracking
            builder.Entity<LocationTracking>()
                .HasOne(l => l.TractorAssignment)
                .WithMany()
                .HasForeignKey(l => l.TractorAssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LocationTracking>()
                .HasIndex(l => new { l.TractorAssignmentId, l.Timestamp });

            // ========== جدید: تنظیمات ChatMessage ==========
            builder.Entity<ChatMessage>()
                .HasOne(c => c.TractorAssignment)
                .WithMany(t => t.ChatMessages)
                .HasForeignKey(c => c.TractorAssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ChatMessage>()
                .HasIndex(c => new { c.TractorAssignmentId, c.SentDate });

            builder.Entity<ChatMessage>()
                .HasIndex(c => new { c.TractorAssignmentId, c.IsRead });

            builder.Entity<Ticket>()
                .HasIndex(x => x.Code)
                .IsUnique();

            builder.Entity<Ticket>()
                .HasIndex(x => new { x.Status, x.Priority, x.CreatedAt });

            builder.Entity<Ticket>()
                .HasIndex(x => x.CreatedByUserId);

            builder.Entity<TicketMessage>()
                .HasIndex(x => new { x.TicketId, x.CreatedAt });

            builder.Entity<TicketAttachment>()
                .HasIndex(x => x.TicketMessageId);

            builder.Entity<TicketCategory>()
                .HasIndex(x => new { x.IsActive, x.SortOrder });
        }
    }
}