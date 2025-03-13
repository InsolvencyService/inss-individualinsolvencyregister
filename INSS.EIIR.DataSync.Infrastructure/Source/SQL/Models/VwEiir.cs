using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL.Models;

[Keyless]
public partial class VwEiir
{
    [Column("caseNo")]
    public int? CaseNo { get; set; }

    [Column("indivNo")]
    public int? IndivNo { get; set; }

    [Column("individualForenames")]
    [StringLength(8000)]
    [Unicode(false)]
    public string IndividualForenames { get; set; } = null!;

    [Column("individualSurname")]
    [StringLength(8000)]
    [Unicode(false)]
    public string IndividualSurname { get; set; } = null!;

    [Column("individualTitle")]
    [StringLength(8000)]
    [Unicode(false)]
    public string IndividualTitle { get; set; } = null!;

    [Column("individualGender")]
    [StringLength(8000)]
    [Unicode(false)]
    public string IndividualGender { get; set; } = null!;

    [Column("individualDOB")]
    [StringLength(30)]
    [Unicode(false)]
    public string IndividualDob { get; set; } = null!;

    [Column("individualOccupation")]
    [StringLength(8000)]
    [Unicode(false)]
    public string IndividualOccupation { get; set; } = null!;

    [Column("individualTown")]
    [StringLength(8000)]
    [Unicode(false)]
    public string IndividualTown { get; set; } = null!;

    [Column("individualAddress")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? IndividualAddress { get; set; }

    [Column("individualPostcode")]
    [StringLength(8000)]
    [Unicode(false)]
    public string IndividualPostcode { get; set; } = null!;

    [Column("individualAddressWithheld")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? IndividualAddressWithheld { get; set; }

    [Column("individualAlias")]
    [StringLength(8000)]
    [Unicode(false)]
    public string IndividualAlias { get; set; } = null!;

    [Column("deceasedDate")]
    [StringLength(8000)]
    [Unicode(false)]
    public DateTime? DeceasedDate { get; set; }

    [Column("caseName")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? CaseName { get; set; }

    [Column("courtName")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? CourtName { get; set; }

    [Column("courtNumber")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? CourtNumber { get; set; }

    [Column("caseYear")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? CaseYear { get; set; }

    [Column("insolvencyType")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyType { get; set; }

    [Column("notificationDate")]
    public DateTime? NotificationDate { get; set; }

    [Column("insolvencyDate")]
    public DateTime? InsolvencyDate { get; set; }

    [Column("caseStatus")]
    [StringLength(68)]
    [Unicode(false)]
    public string? CaseStatus { get; set; }

    [Column("annulDate")]
    [StringLength(10)]
    [Unicode(false)]
    public string? AnnulDate { get; set; }

    [Column("annulReason")]
    [StringLength(44)]
    [Unicode(false)]
    public string? AnnulReason { get; set; }

    [Column("caseDescription")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? CaseDescription { get; set; }

    [Column("tradingNames")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? TradingNames { get; set; }

    [Column("hasRestrictions")]
    public int HasRestrictions { get; set; }

    [Column("restrictionsType")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? RestrictionsType { get; set; }

    [Column("restrictionsStartDate")]
    public DateOnly? RestrictionsStartDate { get; set; }

    [Column("restrictionsEndDate")]
    public DateOnly? RestrictionsEndDate { get; set; }

    [Column("hasaPrevInterimRestrictionsOrder")]
    public int HasaPrevInterimRestrictionsOrder { get; set; }

    [Column("prevInterimRestrictionsOrderStartDate")]
    public DateOnly? PrevInterimRestrictionsOrderStartDate { get; set; }

    [Column("prevInterimRestrictionsOrderEndDate")]
    public DateOnly? PrevInterimRestrictionsOrderEndDate { get; set; }

    [Column("insolvencyPractitionerName")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyPractitionerName { get; set; }

    [Column("insolvencyPractitionerFirmName")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyPractitionerFirmName { get; set; }

    [Column("insolvencyPractitionerAddress")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyPractitionerAddress { get; set; }

    [Column("insolvencyPractitionerPostcode")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyPractitionerPostcode { get; set; }

    [Column("insolvencyPractitionerTelephone")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyPractitionerTelephone { get; set; }

    [Column("insolvencyServiceOffice")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyServiceOffice { get; set; }

    [Column("insolvencyServiceContact")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyServiceContact { get; set; }

    [Column("insolvencyServiceAddress")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyServiceAddress { get; set; }

    [Column("insolvencyServicePostcode")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyServicePostcode { get; set; }

    [Column("insolvencyServicePhone")]
    [StringLength(8000)]
    [Unicode(false)]
    public string? InsolvencyServicePhone { get; set; }

    [Column("dateOfPreviousOrder")]
    public DateOnly? DateOfPreviousOrder { get; set; }
}
