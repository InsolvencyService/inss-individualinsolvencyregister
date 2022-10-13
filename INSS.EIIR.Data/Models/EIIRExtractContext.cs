using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.Data.Models
{
    public class EIIRExtractContext : DbContext
    {
        public EIIRExtractContext()
        { }

        public EIIRExtractContext(DbContextOptions<EIIRExtractContext> options) : base(options)
        { }

        public virtual DbSet<ExtractAvailabilitySP> ExtractAvailability { get; set; } = null!;
        public virtual DbSet<SubscriberAccount> SubscriberAccounts { get; set; } = null!;
        public virtual DbSet<SubscriberApplication> SubscriberApplications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriberAccount>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("subscriber_account");

                entity.Property(e => e.AccountActive)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("account_active")
                    .IsFixedLength();

                entity.Property(e => e.AuthorisedBy)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("authorised_by");

                entity.Property(e => e.AuthorisedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("authorised_date");

                entity.Property(e => e.AuthorisedIpaddress)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("authorised_ipaddress");

                entity.Property(e => e.OrganisationName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("organisation_name");

                entity.Property(e => e.SubscribedFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("subscribed_from");

                entity.Property(e => e.SubscribedTo)
                    .HasColumnType("datetime")
                    .HasColumnName("subscribed_to");

                entity.Property(e => e.SubscriberId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("subscriber_id");

                entity.Property(e => e.SubscriberLogin)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("subscriber_login");

                entity.Property(e => e.SubscriberPassword)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("subscriber_password");
            });

            modelBuilder.Entity<SubscriberApplication>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("subscriber_application");

                entity.Property(e => e.ApplicationApproved)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("application_approved")
                    .IsFixedLength();

                entity.Property(e => e.ApplicationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("application_date");

                entity.Property(e => e.ApplicationIpaddress)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("application_ipaddress");

                entity.Property(e => e.ApplicationViewed)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("application_viewed")
                    .IsFixedLength();

                entity.Property(e => e.ApplicationViewedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("application_viewed_by");

                entity.Property(e => e.ApplicationViewedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("application_viewed_date");

                entity.Property(e => e.ApprovedIpaddress)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("approved_ipaddress")
                    .IsFixedLength();

                entity.Property(e => e.ContactAddress1)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("contact_address1");

                entity.Property(e => e.ContactAddress2)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("contact_address2");

                entity.Property(e => e.ContactCity)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("contact_city");

                entity.Property(e => e.ContactCountry)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("contact_country");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("contact_email");

                entity.Property(e => e.ContactForename)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("contact_forename");

                entity.Property(e => e.ContactPostcode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("contact_postcode");

                entity.Property(e => e.ContactSurname)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("contact_surname");

                entity.Property(e => e.ContactTelephone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("contact_telephone");

                entity.Property(e => e.ContactTitle)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("contact_title");

                entity.Property(e => e.OrganisationName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("organisation_name");

                entity.Property(e => e.OrganisationType)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("organisation_type");

                entity.Property(e => e.OrganisationWebsite)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("organisation_website");

                entity.Property(e => e.SubscriberId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("subscriber_id");
            });

            modelBuilder.Entity<ExtractAvailabilitySP>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("extract_availability");

                entity.Property(e => e.ExtractId)
                    .HasColumnType("int")
                    .HasColumnName("extract_id");


                entity.Property(e => e.Currentdate)
                    .HasColumnType("datetime")
                    .HasColumnName("currentdate");

                entity.Property(e => e.DownloadLink)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("download_link");

                entity.Property(e => e.DownloadZiplink)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("download_ziplink");

                entity.Property(e => e.ExtractBanks).HasColumnName("extract_banks");

                entity.Property(e => e.ExtractCompleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("extract_completed")
                    .IsFixedLength();

                entity.Property(e => e.ExtractDate)
                    .HasColumnType("datetime")
                    .HasColumnName("extract_date");

                entity.Property(e => e.ExtractDros).HasColumnName("extract_dros");

                entity.Property(e => e.ExtractEntries).HasColumnName("extract_entries");

                entity.Property(e => e.ExtractFilename)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("extract_filename");

                entity.Property(e => e.ExtractId).HasColumnName("extract_id");

                entity.Property(e => e.ExtractIvas).HasColumnName("extract_ivas");

                entity.Property(e => e.NewBanks).HasColumnName("new_banks");

                entity.Property(e => e.NewCases).HasColumnName("new_cases");

                entity.Property(e => e.NewDros).HasColumnName("new_dros");

                entity.Property(e => e.NewIvas).HasColumnName("new_ivas");

                entity.Property(e => e.SnapshotCompleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("snapshot_completed")
                    .IsFixedLength();

                entity.Property(e => e.SnapshotDate)
                    .HasColumnType("datetime")
                    .HasColumnName("snapshot_date");
            });
        }
    }
}
