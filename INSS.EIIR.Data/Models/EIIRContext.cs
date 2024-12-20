using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SearchModels;
using Microsoft.EntityFrameworkCore;

namespace INSS.EIIR.Data.Models;

public partial class EIIRContext : DbContext
{
    private readonly string? _connectionString;

    public EIIRContext()
    { }

    public EIIRContext(string? connectionString)
    {
        _connectionString = connectionString; 
    }

    public EIIRContext(DbContextOptions<EIIRContext> options)
        : base(options)
    { }

    public virtual DbSet<AnonCaseName> AnonCaseNames { get; set; } = null!;
    public virtual DbSet<CasesCsv> CasesCsvs { get; set; } = null!;
    public virtual DbSet<CiCase> CiCases { get; set; } = null!;
    public virtual DbSet<CiCaseDesc> CiCaseDescs { get; set; } = null!;
    public virtual DbSet<CiCourt> CiCourts { get; set; } = null!;
    public virtual DbSet<CiIndivDischarge> CiIndivDischarges { get; set; } = null!;
    public virtual DbSet<CiIndividual> CiIndividuals { get; set; } = null!;
    public virtual DbSet<CiIp> CiIps { get; set; } = null!;
    public virtual DbSet<CiIpAddress> CiIpAddresses { get; set; } = null!;
    public virtual DbSet<CiIpAppt> CiIpAppts { get; set; } = null!;
    public virtual DbSet<CiIpAuthorisingBody> CiIpAuthorisingBodies { get; set; } = null!;
    public virtual DbSet<CiIvaCase> CiIvaCases { get; set; } = null!;
    public virtual DbSet<CiOffice> CiOffices { get; set; } = null!;
    public virtual DbSet<CiOfficeCourt> CiOfficeCourts { get; set; } = null!;
    public virtual DbSet<CiOtherName> CiOtherNames { get; set; } = null!;
    public virtual DbSet<CiSelection> CiSelections { get; set; } = null!;
    public virtual DbSet<CiSelectionDecode> CiSelectionDecodes { get; set; } = null!;
    public virtual DbSet<CiTrade> CiTrades { get; set; } = null!;
    public virtual DbSet<CiCaseFeedback> CiCaseFeedback { get; set; } = null!;
    public virtual DbSet<EiirSnapshotTable> EiirSnapshotTables { get; set; } = null!;
    public virtual DbSet<EiirSnapshotTablepreviousDay> EiirSnapshotTablepreviousDays { get; set; } = null!;
    public virtual DbSet<ExtractAvailability> ExtractAvailabilities { get; set; } = null!;
    public virtual DbSet<FeedBack> FeedBacks { get; set; } = null!;
    public virtual DbSet<IpsearchVisit> IpsearchVisits { get; set; } = null!;
    public virtual DbSet<Officialreceiver> Officialreceivers { get; set; } = null!;
    public virtual DbSet<SecretariatAccount> SecretariatAccounts { get; set; } = null!;
    public virtual DbSet<SubjectBro> SubjectBros { get; set; } = null!;
    public virtual DbSet<SubjectDro> SubjectDros { get; set; } = null!;
    public virtual DbSet<SubjectIbro> SubjectIbros { get; set; } = null!;
    public virtual DbSet<SubscriberAccount> SubscriberAccounts { get; set; } = null!;
    public virtual DbSet<SubscriberApplication> SubscriberApplications { get; set; } = null!;
    public virtual DbSet<SubscriberContact> SubscriberContacts { get; set; } = null!;
    public virtual DbSet<SubscriberDownload> SubscriberDownloads { get; set; } = null!;
    public virtual DbSet<TmpAnonName> TmpAnonNames { get; set; } = null!;
    public virtual DbSet<TmpCasesCsv> TmpCasesCsvs { get; set; } = null!;
    public virtual DbSet<Visit> Visits { get; set; } = null!;
    public virtual DbSet<VisitsArchived> VisitsArchiveds { get; set; } = null!;
    public virtual DbSet<WebMessage> WebMessages { get; set; } = null!;

    public virtual DbSet<SearchResult> SearchResults { get; set; } = null!;
    public virtual DbSet<CaseResult> CaseResults { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && !string.IsNullOrWhiteSpace(_connectionString))
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<CaseResult>(
                    eb =>
                    {
                        eb.HasKey(m => new {m.caseNo, m.indivNo});
                    });

        modelBuilder.Entity<Trading>(
          eb =>
          {
              eb.HasNoKey();
          });
        modelBuilder.Entity<TradingDetails>(
         eb =>
         {
             eb.HasNoKey();
         });
        modelBuilder.Entity<AnonCaseName>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("Anon_Case_Names");

            entity.HasIndex(e => e.Casereference, "idx_caseref")
                .IsUnique();

            entity.Property(e => e.Casename)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("casename");

            entity.Property(e => e.Casereference)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("casereference");
        });

        modelBuilder.Entity<CasesCsv>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("cases.csv");

            entity.Property(e => e.AddressLine1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address_line_1");

            entity.Property(e => e.AddressLine2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address_line_2");

            entity.Property(e => e.AddressLine3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address_line_3");

            entity.Property(e => e.AddressLine4)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address_line_4");

            entity.Property(e => e.AddressLine5)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address_line_5");

            entity.Property(e => e.AddressType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address_type");

            entity.Property(e => e.AddressWithheldFlag)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address_withheld_flag");

            entity.Property(e => e.AnnulmentDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("annulment_date");

            entity.Property(e => e.AnnulmentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("annulment_type");

            entity.Property(e => e.BatchNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("batch_no");

            entity.Property(e => e.BatchType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("batch_type");

            entity.Property(e => e.CasbankruptcasedetailLastUpdatedDate1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("casbankruptcasedetail_last_updated_date1");

            entity.Property(e => e.CasbktcasedetailLastUpdatedDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("casbktcasedetail_last_updated_date");

            entity.Property(e => e.CascasedetailLastUpdatedDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cascasedetail_last_updated_date");

            entity.Property(e => e.CascasedetailLastUpdatedDate1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cascasedetail_last_updated_date1");

            entity.Property(e => e.CascasehearingLastUpdatedDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cascasehearing_last_updated_date");

            entity.Property(e => e.CascasehearingLastUpdatedDate1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cascasehearing_last_updated_date1");

            entity.Property(e => e.CaseName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("case_name");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("case_no");

            entity.Property(e => e.CaseNo1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("case_no1");

            entity.Property(e => e.CaseStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("case_status");

            entity.Property(e => e.CaseYear)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("case_year");

            entity.Property(e => e.CdscasedatastoreLastUpdatedDate1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cdscasedatastore_last_updated_date1");

            entity.Property(e => e.Court)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("court");

            entity.Property(e => e.CourtNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("court_no");

            entity.Property(e => e.CreatedDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("created_date");

            entity.Property(e => e.CreatedDate1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("created_date1");

            entity.Property(e => e.DateOfBirth)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("date_of_birth");

            entity.Property(e => e.DeceasedDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("deceased_date");

            entity.Property(e => e.DtSubjectAnnulled)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dt_subject_annulled");

            entity.Property(e => e.ExaminerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("examiner_id");

            entity.Property(e => e.FileNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("file_no");

            entity.Property(e => e.FinalReleaseDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("final_release_date");

            entity.Property(e => e.Forenames)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("forenames");

            entity.Property(e => e.GazetteDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gazette_date");

            entity.Property(e => e.HtipDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("htip_date");

            entity.Property(e => e.IndivNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("indiv_no");

            entity.Property(e => e.IndivType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("indiv_type");

            entity.Property(e => e.Initials)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("initials");

            entity.Property(e => e.InsolvencyDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("insolvency_date");

            entity.Property(e => e.InsolvencyType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("insolvency_type");

            entity.Property(e => e.IscisCaseRef)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("iscis_case_ref");

            entity.Property(e => e.IsedCaseId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ised_case_id");

            entity.Property(e => e.IsedIndividualId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ised_individual_id");

            entity.Property(e => e.JobTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("job_title");

            entity.Property(e => e.MembersApptDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("members_appt_date");

            entity.Property(e => e.NiNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ni_number");

            entity.Property(e => e.OfficeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("office_id");

            entity.Property(e => e.OnBancs)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("on_bancs");

            entity.Property(e => e.OnEms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("on_ems");

            entity.Property(e => e.OnLois)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("on_lois");

            entity.Property(e => e.Partnership)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("partnership");

            entity.Property(e => e.PetitionDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("petition_date");

            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.Property(e => e.Postcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("postcode");

            entity.Property(e => e.PreRegisterFlag)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pre_register_flag");

            entity.Property(e => e.PtycourtLastUpdatedDate1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ptycourt_last_updated_date1");

            entity.Property(e => e.PtylocationLastUpdatedDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ptylocation_last_updated_date");

            entity.Property(e => e.PtyofficeLastUpdatedDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ptyoffice_last_updated_date");

            entity.Property(e => e.PtypartylocationLastUpdatedDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ptypartylocation_last_updated_date");

            entity.Property(e => e.PtypersonLastUpdatedDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ptyperson_last_updated_date");

            entity.Property(e => e.RegisterExpiryDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("register_expiry_date");

            entity.Property(e => e.RegisterLastAmendBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("register_last_amend_by");

            entity.Property(e => e.RegisterLastAmendDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("register_last_amend_date");

            entity.Property(e => e.Sex)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sex");

            entity.Property(e => e.StayExpiryDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stay_expiry_date");

            entity.Property(e => e.StayFlag)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stay_flag");

            entity.Property(e => e.SubjAnnulmentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("subj_annulment_type");

            entity.Property(e => e.SummaryCertFlag)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("summary_cert_flag");

            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("surname");

            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.Property(e => e.TradeClassNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("trade_class_no");

            entity.Property(e => e.TradingEndDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("trading_end_date");

            entity.Property(e => e.TradingStartDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("trading_start_date");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<CiCase>(entity =>
        {
            entity.HasKey(e => e.CaseNo);

            entity.ToTable("ci_case");

            entity.HasIndex(e => e.CaseNo, "ci_case_index_1");

            entity.HasIndex(e => e.OfficeId, "ci_case_index_2");

            entity.HasIndex(e => e.PetitionDate, "ci_case_index_3");

            entity.Property(e => e.CaseNo)
                .ValueGeneratedNever()
                .HasColumnName("case_no");

            entity.Property(e => e.AnnulmentDate)
                .HasColumnType("datetime")
                .HasColumnName("annulment_date");

            entity.Property(e => e.AnnulmentType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("annulment_type")
                .IsFixedLength();

            entity.Property(e => e.BatchNo).HasColumnName("batch_no");

            entity.Property(e => e.BatchType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("batch_type")
                .IsFixedLength();

            entity.Property(e => e.CaseName)
                .HasMaxLength(96)
                .IsUnicode(false)
                .HasColumnName("case_name");

            entity.Property(e => e.CaseStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("case_status")
                .IsFixedLength();

            entity.Property(e => e.CaseYear).HasColumnName("case_year");

            entity.Property(e => e.Court)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("court");

            entity.Property(e => e.CourtNo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("court_no");

            entity.Property(e => e.ExaminerId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("examiner_id");

            entity.Property(e => e.FileNo).HasColumnName("file_no");

            entity.Property(e => e.FinalReleaseDate)
                .HasColumnType("datetime")
                .HasColumnName("final_release_date");

            entity.Property(e => e.GazetteDate)
                .HasColumnType("datetime")
                .HasColumnName("gazette_date");

            entity.Property(e => e.HtipDate)
                .HasColumnType("datetime")
                .HasColumnName("htip_date");

            entity.Property(e => e.InsolvencyDate)
                .HasColumnType("datetime")
                .HasColumnName("insolvency_date");

            entity.Property(e => e.InsolvencyType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("insolvency_type")
                .IsFixedLength();

            entity.Property(e => e.IscisCaseRef)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("iscis_case_ref");

            entity.Property(e => e.IsedCaseId).HasColumnName("ised_case_id");

            entity.Property(e => e.MembersApptDate)
                .HasColumnType("datetime")
                .HasColumnName("members_appt_date");

            entity.Property(e => e.OfficeId).HasColumnName("office_id");

            entity.Property(e => e.OnBancs)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("on_bancs")
                .IsFixedLength();

            entity.Property(e => e.OnEms)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("on_ems")
                .HasDefaultValueSql("(' ')")
                .IsFixedLength();

            entity.Property(e => e.OnLois)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("on_lois")
                .IsFixedLength();

            entity.Property(e => e.Partnership)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("partnership")
                .IsFixedLength();

            entity.Property(e => e.PetitionDate)
                .HasColumnType("datetime")
                .HasColumnName("petition_date");

            entity.Property(e => e.PreRegisterFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("pre_register_flag")
                .IsFixedLength();

            entity.Property(e => e.PrimaryTradeClass)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("primary_trade_class")
                .IsFixedLength();

            entity.Property(e => e.SicCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("sic_code")
                .IsFixedLength();

            entity.Property(e => e.SicFreeText)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("sic_free_text");

            entity.Property(e => e.SicSubCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("sic_sub_code");

            entity.Property(e => e.SicYear)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("sic_year");

            entity.Property(e => e.StayExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("stay_expiry_date");

            entity.Property(e => e.StayFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("stay_flag")
                .IsFixedLength();

            entity.Property(e => e.TradeClassNo)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("trade_class_no");

            entity.Property(e => e.TradingEndDate)
                .HasColumnType("datetime")
                .HasColumnName("trading_end_date");

            entity.Property(e => e.TradingStartDate)
                .HasColumnType("datetime")
                .HasColumnName("trading_start_date");

            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<CiCaseDesc>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CaseDescNo, e.CaseDescLineNo });

            entity.ToTable("ci_case_desc");

            entity.Property(e => e.CaseNo).HasColumnName("case_no");

            entity.Property(e => e.CaseDescNo).HasColumnName("case_desc_no");

            entity.Property(e => e.CaseDescLineNo).HasColumnName("case_desc_line_no");

            entity.Property(e => e.CaseDescLine)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("case_desc_line");
        });

        modelBuilder.Entity<CiCourt>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("ci_court");

            entity.HasIndex(e => e.Court, "ci_court_index_2")
                .HasFillFactor(90);

            entity.Property(e => e.Court)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("court");

            entity.Property(e => e.CourtId).HasColumnName("court_id");

            entity.Property(e => e.CourtName)
                .HasMaxLength(48)
                .IsUnicode(false)
                .HasColumnName("court_name");
        });

        modelBuilder.Entity<CiIndivDischarge>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.IndivNo })
                .HasName("PK_ci_indivdisch");

            entity.ToTable("ci_indiv_discharge");

            entity.HasIndex(e => new { e.CaseNo, e.IndivNo }, "ci_indivdisch_index_1")
                .HasFillFactor(90);

            entity.Property(e => e.CaseNo).HasColumnName("case_no");

            entity.Property(e => e.IndivNo).HasColumnName("indiv_no");

            entity.Property(e => e.DischargeDate)
                .HasColumnType("datetime")
                .HasColumnName("discharge_date");

            entity.Property(e => e.DischargeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("discharge_type")
                .IsFixedLength();

            entity.Property(e => e.PreviousOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("previous_order_date");

            entity.Property(e => e.PreviousOrderEndDate)
                .HasColumnType("datetime")
                .HasColumnName("previous_order_end_date");

            entity.Property(e => e.PreviousOrderStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("previous_order_status")
                .IsFixedLength();

            entity.Property(e => e.SuspensionDate)
                .HasColumnType("datetime")
                .HasColumnName("suspension_date");

            entity.Property(e => e.SuspensionEndDate)
                .HasColumnType("datetime")
                .HasColumnName("suspension_end_date");
        });

        modelBuilder.Entity<CiIndividual>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.IndivNo })
                .HasName("PK_ci_indivKEY");

            entity.ToTable("ci_individual");

            entity.HasIndex(e => e.Surname, "ci_individual_index_1")
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.Surname, e.Forenames }, "ci_individual_index_2")
                .HasFillFactor(90);

            entity.Property(e => e.CaseNo).HasColumnName("case_no");

            entity.Property(e => e.IndivNo).HasColumnName("indiv_no");

            entity.Property(e => e.AddressLine1)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_1");

            entity.Property(e => e.AddressLine2)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_2");

            entity.Property(e => e.AddressLine3)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_3");

            entity.Property(e => e.AddressLine4)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_4");

            entity.Property(e => e.AddressLine5)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_5");

            entity.Property(e => e.AddressType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("address_type")
                .IsFixedLength();

            entity.Property(e => e.AddressWithheldFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("address_withheld_flag")
                .IsFixedLength();

            entity.Property(e => e.DateOfBirth)
                .HasColumnType("datetime")
                .HasColumnName("date_of_birth");

            entity.Property(e => e.DeceasedDate)
                .HasColumnType("datetime")
                .HasColumnName("deceased_date");

            entity.Property(e => e.DtSubjectAnnulled)
                .HasColumnType("datetime")
                .HasColumnName("dt_subject_annulled");

            entity.Property(e => e.Forenames)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("forenames");

            entity.Property(e => e.IndivType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("indiv_type")
                .IsFixedLength();

            entity.Property(e => e.Initials)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("initials");

            entity.Property(e => e.IsedIndividualId).HasColumnName("ised_individual_id");

            entity.Property(e => e.JobTitle)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("job_title");

            entity.Property(e => e.NiNumber)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ni_number")
                .IsFixedLength();

            entity.Property(e => e.OnRegisterFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("on_register_flag")
                .IsFixedLength();

            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.Property(e => e.Postcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("postcode");

            entity.Property(e => e.PrimaryOccClass)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("primary_occ_class");

            entity.Property(e => e.RegisterExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("register_expiry_date");

            entity.Property(e => e.RegisterLastAmendBy)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("register_last_amend_by")
                .IsFixedLength();

            entity.Property(e => e.RegisterLastAmendDate)
                .HasColumnType("datetime")
                .HasColumnName("register_last_amend_date");

            entity.Property(e => e.Sex)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("sex")
                .IsFixedLength();

            entity.Property(e => e.SocCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("soc_code")
                .IsFixedLength();

            entity.Property(e => e.SocSubCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("soc_sub_code");

            entity.Property(e => e.SocYear)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("soc_year");

            entity.Property(e => e.SubjAnnulmentType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("subj_annulment_type")
                .IsFixedLength();

            entity.Property(e => e.SummaryCertFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("summary_cert_flag")
                .IsFixedLength();

            entity.Property(e => e.Surname)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("surname");

            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<CiIp>(entity =>
        {
            entity.HasKey(e => e.IpNo);

            entity.ToTable("ci_ip");

            entity.HasIndex(e => e.IpNo, "ci_ip_index_1")
                .HasFillFactor(90);

            entity.Property(e => e.IpNo)
                .ValueGeneratedNever()
                .HasColumnName("ip_no");

            entity.Property(e => e.Authorised)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("authorised")
                .IsFixedLength();

            entity.Property(e => e.DateAuthWithdrawn)
                .HasColumnType("datetime")
                .HasColumnName("date_auth_withdrawn");

            entity.Property(e => e.DateFirstAuth)
                .HasColumnType("datetime")
                .HasColumnName("date_first_auth");

            entity.Property(e => e.Forenames)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("forenames");

            entity.Property(e => e.IncludeOnInternet)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("include_on_internet")
                .HasDefaultValueSql("(' ')")
                .IsFixedLength();

            entity.Property(e => e.Initials)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("initials");

            entity.Property(e => e.IpEmailAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ip_email_address");

            entity.Property(e => e.IsedPractitionerId).HasColumnName("ised_practitioner_id");

            entity.Property(e => e.IsedPractitionerTypeCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("ised_practitioner_type_code")
                .HasDefaultValueSql("('')");

            entity.Property(e => e.LicensingBody)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("licensing_body");

            entity.Property(e => e.NoOfCases).HasColumnName("no_of_cases");

            entity.Property(e => e.OrigVisitDate)
                .HasColumnType("datetime")
                .HasColumnName("orig_visit_date");

            entity.Property(e => e.PlannedVisitDate)
                .HasColumnType("datetime")
                .HasColumnName("planned_visit_date");

            entity.Property(e => e.ReasonWithdrawn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("reason_withdrawn")
                .IsFixedLength();

            entity.Property(e => e.RegisteredAddressLine1)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("registered_address_line_1");

            entity.Property(e => e.RegisteredAddressLine2)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("registered_address_line_2");

            entity.Property(e => e.RegisteredAddressLine3)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("registered_address_line_3");

            entity.Property(e => e.RegisteredAddressLine4)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("registered_address_line_4");

            entity.Property(e => e.RegisteredAddressLine5)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("registered_address_line_5");

            entity.Property(e => e.RegisteredFax)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("registered_fax");

            entity.Property(e => e.RegisteredFirmName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("registered_firm_name");

            entity.Property(e => e.RegisteredPhone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("registered_phone");

            entity.Property(e => e.RegisteredPostCode)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("registered_post_code");

            entity.Property(e => e.RenewalDate)
                .HasColumnType("datetime")
                .HasColumnName("renewal_date");

            entity.Property(e => e.RestrictionDescription)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("restriction_description");

            entity.Property(e => e.RestrictionExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("restriction_expiry_date");

            entity.Property(e => e.RestrictionType)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("restriction_type");

            entity.Property(e => e.Surname)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("surname");

            entity.Property(e => e.Title)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<CiIpAddress>(entity =>
        {
            entity.HasKey(e => new { e.IpNo, e.IpAddressNo });

            entity.ToTable("ci_ip_address");

            entity.HasIndex(e => new { e.IpNo, e.IpAddressNo }, "ci_ipaddress_index_1")
                .HasFillFactor(90);

            entity.Property(e => e.IpNo).HasColumnName("ip_no");

            entity.Property(e => e.IpAddressNo).HasColumnName("ip_address_no");

            entity.Property(e => e.AddressLine1)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_1");

            entity.Property(e => e.AddressLine2)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_2");

            entity.Property(e => e.AddressLine3)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_3");

            entity.Property(e => e.AddressLine4)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_4");

            entity.Property(e => e.AddressLine5)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_5");

            entity.Property(e => e.AddressType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("address_type")
                .IsFixedLength();

            entity.Property(e => e.CountryCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("country_code");

            entity.Property(e => e.CurrentAddress)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("current_address")
                .IsFixedLength();

            entity.Property(e => e.DxExchange)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dx_exchange");

            entity.Property(e => e.DxNumber)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("dx_number");

            entity.Property(e => e.DxOnBancs)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("dx_on_bancs")
                .IsFixedLength();

            entity.Property(e => e.DxSortCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dx_sort_code");

            entity.Property(e => e.FaxNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("fax_no");

            entity.Property(e => e.IpFirmName)
                .HasMaxLength(48)
                .IsUnicode(false)
                .HasColumnName("ip_firm_name");

            entity.Property(e => e.IsedFirmId).HasColumnName("ised_firm_id");

            entity.Property(e => e.IsedFirmLocationId).HasColumnName("ised_firm_location_id");

            entity.Property(e => e.IsedPractitionerFirmLocationId).HasColumnName("ISED_practitionerFirmLocationID");

            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.Property(e => e.Postcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("postcode");

            entity.Property(e => e.RegionCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("region_code");
        });

        modelBuilder.Entity<CiIpAppt>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.IpApptNo });

            entity.ToTable("ci_ip_appt");

            entity.HasIndex(e => new { e.CaseNo, e.IpApptNo }, "ci_ip_appt_index_1")
                .HasFillFactor(90);

            entity.Property(e => e.CaseNo).HasColumnName("case_no");

            entity.Property(e => e.IpApptNo).HasColumnName("ip_appt_no");

            entity.Property(e => e.ApptEndDate)
                .HasColumnType("datetime")
                .HasColumnName("appt_end_date");

            entity.Property(e => e.ApptStartDate)
                .HasColumnType("datetime")
                .HasColumnName("appt_start_date");

            entity.Property(e => e.ChangeDate)
                .HasColumnType("datetime")
                .HasColumnName("change_date");

            entity.Property(e => e.EndReason)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("end_reason")
                .IsFixedLength();

            entity.Property(e => e.IpAddressNo).HasColumnName("ip_address_no");

            entity.Property(e => e.IpApptType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ip_appt_type")
                .IsFixedLength();

            entity.Property(e => e.IpNo).HasColumnName("ip_no");

            entity.Property(e => e.IsedPractitionerAppointmentId).HasColumnName("ised_practitioner_appointment_id");

            entity.Property(e => e.OldIpAddressNo).HasColumnName("old_ip_address_no");
        });

        modelBuilder.Entity<CiIpAuthorisingBody>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("ci_ip_authorising_body");

            entity.Property(e => e.AuthBodyAddressLine1)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("auth_body_address_line_1");

            entity.Property(e => e.AuthBodyAddressLine2)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("auth_body_address_line_2");

            entity.Property(e => e.AuthBodyAddressLine3)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("auth_body_address_line_3");

            entity.Property(e => e.AuthBodyAddressLine4)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("auth_body_address_line_4");

            entity.Property(e => e.AuthBodyAddressLine5)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("auth_body_address_line_5");

            entity.Property(e => e.AuthBodyCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("auth_body_code");

            entity.Property(e => e.AuthBodyFaxNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("auth_body_fax_no");

            entity.Property(e => e.AuthBodyName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("auth_body_name");

            entity.Property(e => e.AuthBodyPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("auth_body_phone");

            entity.Property(e => e.AuthBodyPostcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("auth_body_postcode");

            entity.Property(e => e.AuthBodyWebsite)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("auth_body_website");
        });

        modelBuilder.Entity<CiIvaCase>(entity =>
        {
            entity.HasKey(e => e.CaseNo);

            entity.ToTable("ci_iva_case");

            entity.HasIndex(e => e.CaseNo, "ci_iva_index_1")
                .HasFillFactor(90);

            entity.Property(e => e.CaseNo)
                .ValueGeneratedNever()
                .HasColumnName("case_no");

            entity.Property(e => e.DateFeePaid)
                .HasColumnType("datetime")
                .HasColumnName("date_fee_paid");

            entity.Property(e => e.DateOfCompletion)
                .HasColumnType("datetime")
                .HasColumnName("date_of_completion");

            entity.Property(e => e.DateOfFailure)
                .HasColumnType("datetime")
                .HasColumnName("date_of_failure");

            entity.Property(e => e.DateOfNotification)
                .HasColumnType("datetime")
                .HasColumnName("date_of_notification");

            entity.Property(e => e.DateOfRegistration)
                .HasColumnType("datetime")
                .HasColumnName("date_of_registration");

            entity.Property(e => e.DateOfRevocation)
                .HasColumnType("datetime")
                .HasColumnName("date_of_revocation");

            entity.Property(e => e.DateOfSuspension)
                .HasColumnType("datetime")
                .HasColumnName("date_of_suspension");

            entity.Property(e => e.DateSuspensionLifted)
                .HasColumnType("datetime")
                .HasColumnName("date_suspension_lifted");

            entity.Property(e => e.IvaCourt)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("iva_court");

            entity.Property(e => e.IvaNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("iva_number");
        });

        modelBuilder.Entity<CiOffice>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("ci_office");

            entity.HasIndex(e => e.OfficeId, "ci_office_pkey")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.AddressLine1)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_1");

            entity.Property(e => e.AddressLine2)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_2");

            entity.Property(e => e.AddressLine3)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_3");

            entity.Property(e => e.AddressLine4)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_4");

            entity.Property(e => e.AddressLine5)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_5");

            entity.Property(e => e.DatabaseNo).HasColumnName("database_no");

            entity.Property(e => e.HasCases)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("has_cases")
                .IsFixedLength();

            entity.Property(e => e.OfficeId).HasColumnName("office_id");

            entity.Property(e => e.OfficeName)
                .HasMaxLength(110)
                .IsUnicode(false)
                .HasColumnName("office_name");

            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.Property(e => e.Postcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("postcode");

            entity.Property(e => e.RegisterContact)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("register_contact")
                .IsFixedLength();
        });

        modelBuilder.Entity<CiOfficeCourt>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("ci_office_courts");

            entity.Property(e => e.Court)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("court");

            entity.Property(e => e.CourtName)
                .HasMaxLength(48)
                .IsUnicode(false)
                .HasColumnName("court_name");

            entity.Property(e => e.OfficeCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("office_code")
                .IsFixedLength();

            entity.Property(e => e.OfficeId).HasColumnName("office_id");

            entity.Property(e => e.OfficeName)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("office_name");
        });

        modelBuilder.Entity<CiOtherName>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.IndivNo, e.AliasNo });

            entity.ToTable("ci_other_name");

            entity.HasIndex(e => new { e.Surname, e.Forenames }, "ci_other_name_index_1");

            entity.HasIndex(e => new { e.CaseNo, e.IndivNo }, "ci_other_name_index_2");

            entity.Property(e => e.CaseNo).HasColumnName("case_no");

            entity.Property(e => e.IndivNo).HasColumnName("indiv_no");

            entity.Property(e => e.AliasNo).HasColumnName("alias_no");

            entity.Property(e => e.AliasType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("alias_type")
                .IsFixedLength();

            entity.Property(e => e.Forenames)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("forenames");

            entity.Property(e => e.Initials)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("initials");

            entity.Property(e => e.IsedAliasId).HasColumnName("ised_alias_id");

            entity.Property(e => e.IsedIndividualId).HasColumnName("ised_individual_id");

            entity.Property(e => e.Surname)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("surname");
        });

        modelBuilder.Entity<CiSelection>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("ci_selection");

            entity.Property(e => e.SelectionCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("selection_code");

            entity.Property(e => e.SelectionSubtype).HasColumnName("selection_subtype");

            entity.Property(e => e.SelectionType).HasColumnName("selection_type");

            entity.Property(e => e.SelectionValue)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("selection_value");
        });

        modelBuilder.Entity<CiSelectionDecode>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("ci_selection_decode");

            entity.Property(e => e.SelectionField)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("selection_field");

            entity.Property(e => e.SelectionType).HasColumnName("selection_type");
        });

        modelBuilder.Entity<CiTrade>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.TradingNo });

            entity.ToTable("ci_trade");

            entity.HasIndex(e => e.TradingName, "ci_trade_index_1");

            entity.Property(e => e.CaseNo).HasColumnName("case_no");

            entity.Property(e => e.TradingNo).HasColumnName("trading_no");

            entity.Property(e => e.AddressLine1)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_1");

            entity.Property(e => e.AddressLine2)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_2");

            entity.Property(e => e.AddressLine3)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_3");

            entity.Property(e => e.AddressLine4)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_4");

            entity.Property(e => e.AddressLine5)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("address_line_5");

            entity.Property(e => e.AddressWithheldFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("address_withheld_flag")
                .IsFixedLength();

            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.Property(e => e.Postcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("postcode");

            entity.Property(e => e.TradingName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("trading_name");
        });

        modelBuilder.Entity<CiCaseFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId);

            entity.ToTable("CI_Case_Feedback");            

            entity.HasIndex(e => e.Viewed, "ci_case_feedback_viewed");

            entity.Property(e => e.FeedbackDate).HasColumnName("FeedbackDate");

            entity.Property(e => e.CaseId).HasColumnName("CaseId");

            entity.Property(e => e.Message)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("Message");

            entity.Property(e => e.ReporterFullname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ReporterFullname");

            entity.Property(e => e.ReporterEmailAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ReporterEmailAddress");

            entity.Property(e => e.ReporterOrganisation)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ReporterOrganisation");

            entity.Property(e => e.Viewed).HasColumnName("Viewed");

            entity.Property(e => e.ViewedDate).HasColumnName("ViewedDate").IsRequired(false);

            entity.Property(e => e.InsolvencyType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("InsolvencyType");

            entity.Property(e => e.CaseName)
                .HasMaxLength(96)
                .IsUnicode(false)
                .HasColumnName("CaseName");

            entity.Property(e => e.InsolvencyDate).HasColumnName("InsolvencyDate").IsRequired(false);

            entity.Property(e => e.IndivNo).HasColumnName("IndivNo");

        });

        modelBuilder.Entity<EiirSnapshotTable>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("eiirSnapshotTABLE");

            entity.HasIndex(e => e.CaseNo, "snapshot_index_caseno");

            entity.HasIndex(e => e.DateofOrder, "snapshot_index_orderdate");

            entity.HasIndex(e => e.Surname, "snapshot_index_surname");

            entity.Property(e => e.Address1)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.Address2)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.Address3)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.Address4)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.Address5)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.AnnulmentDateCase)
                .HasColumnType("datetime")
                .HasColumnName("AnnulmentDateCASE");

            entity.Property(e => e.AnnulmentDatePartner)
                .HasColumnType("datetime")
                .HasColumnName("AnnulmentDatePARTNER");

            entity.Property(e => e.AnnulmentTypeCase)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("AnnulmentTypeCASE")
                .IsFixedLength();

            entity.Property(e => e.AnnulmentTypePartner)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("AnnulmentTypePARTNER")
                .IsFixedLength();

            entity.Property(e => e.CaseName)
                .HasMaxLength(96)
                .IsUnicode(false);

            entity.Property(e => e.Court)
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.Property(e => e.DateofBirth).HasColumnType("datetime");

            entity.Property(e => e.DateofOrder).HasColumnType("datetime");

            entity.Property(e => e.Deceased).HasColumnType("datetime");

            entity.Property(e => e.Dronumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DRONumber");

            entity.Property(e => e.FirstName)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Number)
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.Property(e => e.Occupation)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.Property(e => e.PostCode)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Surname)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.Type)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Wflag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("wflag")
                .IsFixedLength();
        });

        modelBuilder.Entity<EiirSnapshotTablepreviousDay>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("eiirSnapshotTABLEPreviousDay");

            entity.Property(e => e.Address1)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.Address2)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.Address3)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.Address4)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.Address5)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.AnnulmentDateCase)
                .HasColumnType("datetime")
                .HasColumnName("AnnulmentDateCASE");

            entity.Property(e => e.AnnulmentDatePartner)
                .HasColumnType("datetime")
                .HasColumnName("AnnulmentDatePARTNER");

            entity.Property(e => e.AnnulmentTypeCase)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("AnnulmentTypeCASE")
                .IsFixedLength();

            entity.Property(e => e.AnnulmentTypePartner)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("AnnulmentTypePARTNER")
                .IsFixedLength();

            entity.Property(e => e.CaseName)
                .HasMaxLength(96)
                .IsUnicode(false);

            entity.Property(e => e.Court)
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.Property(e => e.DateofBirth).HasColumnType("datetime");

            entity.Property(e => e.DateofOrder).HasColumnType("datetime");

            entity.Property(e => e.Deceased).HasColumnType("datetime");

            entity.Property(e => e.Dronumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DRONumber");

            entity.Property(e => e.FirstName)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Number)
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.Property(e => e.Occupation)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.Property(e => e.PostCode)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Surname)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.Type)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Wflag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("wflag")
                .IsFixedLength();
        });

        modelBuilder.Entity<ExtractAvailability>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("extract_availability");

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

            entity.Property(e => e.HideDownloadLink)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("hide_download_link")
                .HasDefaultValueSql("('')");

            entity.Property(e => e.HideDownloadZiplink)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("hide_download_ziplink")
                .HasDefaultValueSql("('')");

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

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("FeedBack");

            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.FeedbackDate)
                .HasColumnType("datetime")
                .HasColumnName("Feedback_Date");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.Message)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.Property(e => e.RemoteAddr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("remote_addr");

            entity.Property(e => e.RemoteHost)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("remote_host");

            entity.Property(e => e.Status).HasColumnName("status");

            entity.Property(e => e.UserAgent)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("User_agent");
        });

        modelBuilder.Entity<IpsearchVisit>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("ipsearchVisits");

            entity.Property(e => e.DateHit)
                .HasColumnType("datetime")
                .HasColumnName("Date_Hit");

            entity.Property(e => e.Host)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.HttpLanguage)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("HTTP_Language");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");

            entity.Property(e => e.LocalAddr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Local_Addr");

            entity.Property(e => e.Referer)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.RemoteAddr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Remote_Addr");

            entity.Property(e => e.RemoteHost)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Remote_Host");

            entity.Property(e => e.SearchType)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("Search_Type");

            entity.Property(e => e.UserAgent)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("User_Agent");
        });

        modelBuilder.Entity<Officialreceiver>(entity =>
        {
            entity.ToTable("officialreceivers");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Address)
                .HasColumnType("ntext")
                .HasColumnName("address");

            entity.Property(e => e.Assistants)
                .HasMaxLength(255)
                .HasColumnName("assistants");

            entity.Property(e => e.Countycourt)
                .HasColumnType("ntext")
                .HasColumnName("countycourt");

            entity.Property(e => e.Courts)
                .HasColumnType("ntext")
                .HasColumnName("courts");

            entity.Property(e => e.Dx)
                .HasMaxLength(50)
                .HasColumnName("dx");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");

            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .HasColumnName("fax");

            entity.Property(e => e.Intrarm)
                .HasMaxLength(50)
                .HasColumnName("intrarm");

            entity.Property(e => e.Map)
                .HasColumnType("ntext")
                .HasColumnName("map");

            entity.Property(e => e.Office)
                .HasMaxLength(255)
                .HasColumnName("office");

            entity.Property(e => e.Orpicture)
                .HasMaxLength(255)
                .HasColumnName("orpicture");

            entity.Property(e => e.Ors)
                .HasMaxLength(255)
                .HasColumnName("ors");

            entity.Property(e => e.Rm)
                .HasMaxLength(255)
                .HasColumnName("rm");

            entity.Property(e => e.Telephone)
                .HasMaxLength(50)
                .HasColumnName("telephone");
        });

        modelBuilder.Entity<SecretariatAccount>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("secretariat_account");

            entity.Property(e => e.AccountActive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("account_active")
                .IsFixedLength();

            entity.Property(e => e.OrganisationName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("organisation_name");

            entity.Property(e => e.SecretariatId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("secretariat_id");

            entity.Property(e => e.SecretariatLogin)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("secretariat_login");

            entity.Property(e => e.SecretariatPassword)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("secretariat_password");
        });

        modelBuilder.Entity<SubjectBro>(entity =>
        {
            entity.HasKey(e => new { e.CaseId, e.SubjRefno, e.RegBNo })
                .IsClustered(false);

            entity.ToTable("subject_bro");

            entity.Property(e => e.CaseId).HasColumnName("case_id");

            entity.Property(e => e.SubjRefno).HasColumnName("subj_refno");

            entity.Property(e => e.RegBNo).HasColumnName("reg_b_no");

            entity.Property(e => e.AppFiledDate)
                .HasColumnType("datetime")
                .HasColumnName("app_filed_date");

            entity.Property(e => e.AtpGrantedDate)
                .HasColumnType("datetime")
                .HasColumnName("atp_granted_date");

            entity.Property(e => e.AtpRefusedDate)
                .HasColumnType("datetime")
                .HasColumnName("atp_refused_date");

            entity.Property(e => e.BroAbandonedDate)
                .HasColumnType("datetime")
                .HasColumnName("bro_abandoned_date");

            entity.Property(e => e.BroAnnulledDate)
                .HasColumnType("datetime")
                .HasColumnName("bro_annulled_date");

            entity.Property(e => e.BroEndDate)
                .HasColumnType("datetime")
                .HasColumnName("bro_end_date");

            entity.Property(e => e.BroHearingDate)
                .HasColumnType("datetime")
                .HasColumnName("bro_hearing_date");

            entity.Property(e => e.BroNoOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("bro_no_order_date");

            entity.Property(e => e.BroOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("bro_order_date");

            entity.Property(e => e.BruAccptDate)
                .HasColumnType("datetime")
                .HasColumnName("bru_accpt_date");

            entity.Property(e => e.BruPropDate)
                .HasColumnType("datetime")
                .HasColumnName("bru_prop_date");

            entity.Property(e => e.BruVariedDate)
                .HasColumnType("datetime")
                .HasColumnName("bru_varied_date");

            entity.Property(e => e.Deselected)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("deselected")
                .IsFixedLength();

            entity.Property(e => e.OrderVariedDate)
                .HasColumnType("datetime")
                .HasColumnName("order_varied_date");

            entity.Property(e => e.ReasonNotGranted)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("reason_not_granted")
                .IsFixedLength();

            entity.Property(e => e.RepResubmittedDate)
                .HasColumnType("datetime")
                .HasColumnName("rep_resubmitted_date");

            entity.Property(e => e.RepSubmittedDate)
                .HasColumnType("datetime")
                .HasColumnName("rep_submitted_date");

            entity.Property(e => e.RepTargetDate)
                .HasColumnType("datetime")
                .HasColumnName("rep_target_date");
        });

        modelBuilder.Entity<SubjectDro>(entity =>
        {
            entity.HasKey(e => new { e.CaseId, e.CidebtorSubjectNo, e.CasdrocaseDetailId });

            entity.ToTable("subject_dro");

            entity.Property(e => e.CaseId).HasColumnName("CaseID");

            entity.Property(e => e.CidebtorSubjectNo).HasColumnName("CIDebtorSubjectNo");

            entity.Property(e => e.CasdrocaseDetailId).HasColumnName("CASDROCaseDetailID");

            entity.Property(e => e.ExperianReference)
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

            entity.Property(e => e.LastUpdatedUser)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.MoratoriumPeriodEndingDate).HasColumnType("datetime");

            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.RevokedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<SubjectIbro>(entity =>
        {
            entity.HasKey(e => new { e.CaseId, e.SubjRefno, e.RegBNo })
                .IsClustered(false);

            entity.ToTable("subject_ibro");

            entity.Property(e => e.CaseId).HasColumnName("case_id");

            entity.Property(e => e.SubjRefno).HasColumnName("subj_refno");

            entity.Property(e => e.RegBNo).HasColumnName("reg_b_no");

            entity.Property(e => e.IbroAppFiledDate)
                .HasColumnType("datetime")
                .HasColumnName("ibro_app_filed_date");

            entity.Property(e => e.IbroDischargeDate)
                .HasColumnType("datetime")
                .HasColumnName("ibro_discharge_date");

            entity.Property(e => e.IbroEndDate)
                .HasColumnType("datetime")
                .HasColumnName("ibro_end_date");

            entity.Property(e => e.IbroHearingDate)
                .HasColumnType("datetime")
                .HasColumnName("ibro_hearing_date");

            entity.Property(e => e.IbroNoOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("ibro_no_order_date");

            entity.Property(e => e.IbroOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("ibro_order_date");
        });

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

        modelBuilder.Entity<SubscriberContact>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("subscriber_contact");

            entity.Property(e => e.EmailAddress)
               .HasMaxLength(60)
               .IsUnicode(false)
               .HasColumnName("email_address");
            
            entity.Property(e => e.CreatedOn)
               .HasColumnType("datetime")
               .HasColumnName("created_on");

            entity.Property(e => e.SubscriberId)
                .ValueGeneratedOnAdd()
                .HasColumnName("subscriber_id");
        });

        modelBuilder.Entity<SubscriberDownload>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("subscriber_downloads");

            entity.Property(e => e.DownloadDate)
                .HasColumnType("datetime")
                .HasColumnName("download_date");

            entity.Property(e => e.DownloadIpaddress)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("download_ipaddress");

            entity.Property(e => e.DownloadWebserver)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("download_webserver");

            entity.Property(e => e.ExtractId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("extract_id");

            entity.Property(e => e.ExtractZipdownload)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("extract_zipdownload");

            entity.Property(e => e.SubscriberId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("subscriber_id");
        });

        modelBuilder.Entity<TmpAnonName>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("TMP_AnonNames");

            entity.Property(e => e.Casename)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("casename");

            entity.Property(e => e.Casereference)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("casereference");
        });

        modelBuilder.Entity<TmpCasesCsv>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("tmp_cases.csv");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("case_no");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.DateHit, "visits_datehit_index")
                .IsClustered();

            entity.Property(e => e.AuthType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Auth_Type");

            entity.Property(e => e.AuthUser)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Auth_User");

            entity.Property(e => e.DateHit)
                .HasColumnType("datetime")
                .HasColumnName("Date_Hit");

            entity.Property(e => e.Host)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.HttpLanguage)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("HTTP_Language");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");

            entity.Property(e => e.LocalAddr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Local_Addr");

            entity.Property(e => e.LogonUser)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Logon_User");

            entity.Property(e => e.Page)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Referer)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.RemoteAddr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Remote_Addr");

            entity.Property(e => e.RemoteHost)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Remote_Host");

            entity.Property(e => e.SearchArea).HasColumnName("Search_Area");

            entity.Property(e => e.SearchType).HasColumnName("Search_Type");

            entity.Property(e => e.UserAgent)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("User_Agent");
        });

        modelBuilder.Entity<VisitsArchived>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("VisitsArchived");

            entity.HasIndex(e => e.DateHit, "archivedvisits_date_index")
                .IsClustered();

            entity.Property(e => e.AuthType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Auth_Type");

            entity.Property(e => e.AuthUser)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Auth_User");

            entity.Property(e => e.DateHit)
                .HasColumnType("datetime")
                .HasColumnName("Date_Hit");

            entity.Property(e => e.Host)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.HttpLanguage)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("HTTP_Language");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");

            entity.Property(e => e.LocalAddr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Local_Addr");

            entity.Property(e => e.LogonUser)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Logon_User");

            entity.Property(e => e.Page)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Referer)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.RemoteAddr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Remote_Addr");

            entity.Property(e => e.RemoteHost)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Remote_Host");

            entity.Property(e => e.SearchArea).HasColumnName("Search_Area");

            entity.Property(e => e.SearchType).HasColumnName("Search_Type");

            entity.Property(e => e.UserAgent)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("User_Agent");
        });

        modelBuilder.Entity<WebMessage>(entity =>
        {
            entity.ToTable("web_messages");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Application)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("application");

            entity.Property(e => e.HideSearch)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("hide_search")
                .IsFixedLength();

            entity.Property(e => e.Message)
                .IsUnicode(false)
                .HasColumnName("message");
        });

        modelBuilder.Entity<SearchResult>(entity =>
        {
            entity.HasNoKey();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
