using Microsoft.EntityFrameworkCore;

namespace INSS.EIIR.Data.Models;

public class EIIRExtractContext : DbContext
{
    public EIIRExtractContext()
    { }

    public EIIRExtractContext(DbContextOptions<EIIRExtractContext> options) : base(options)
    { }

    public virtual DbSet<ExtractAvailabilitySP> ExtractAvailability { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
