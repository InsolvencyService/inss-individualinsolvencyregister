using System;
using System.Collections.Generic;
using INSS.EIIR.DataSync.Infrastructure.Source.SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL.Context;

public partial class ExternalInssContext : DbContext
{
    public ExternalInssContext()
    {
    }

    public ExternalInssContext(DbContextOptions<ExternalInssContext> options)
        : base(options)
    {
    }

    public virtual DbSet<VwEiir> VwEiirs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VwEiir>(entity =>
        {
            entity.ToView("vw_eiir", schema:"ext");

            entity.Property(e => e.AnnulDate).IsFixedLength();
            entity.Property(e => e.CaseDescription).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.CaseName).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.CaseYear).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.CourtName).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.CourtNumber).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.DeceasedDate).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.IndividualAddress).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.IndividualAddressWithheld).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.IndividualAlias).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.IndividualDob).IsFixedLength();
            entity.Property(e => e.IndividualForenames).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.IndividualGender).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.IndividualOccupation).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.IndividualPostcode).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.IndividualSurname).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.IndividualTitle).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.IndividualTown).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyPractitionerAddress).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyPractitionerFirmName).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyPractitionerName).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyPractitionerPostcode).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyPractitionerTelephone).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyServiceAddress).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyServiceContact).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyServiceOffice).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyServicePhone).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyServicePostcode).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.InsolvencyType).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.RestrictionsType).UseCollation("Latin1_General_100_BIN2_UTF8");
            entity.Property(e => e.TradingNames).UseCollation("Latin1_General_100_BIN2_UTF8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
