SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE OR ALTER PROCEDURE [dbo].[getEiirExtractXml]
AS
BEGIN
CREATE TABLE #StatusCodes
(
	SelectionCode varchar(255),
	SelectionValue varchar(255)
)
INSERT INTO #StatusCodes
SELECT selection_code, selection_value FROM ci_selection WHERE selection_type = 24 AND selection_subtype = 0 

CREATE TABLE #Cases
(
	DateOrder datetime,
	CaseNo int,
	IndivNo int,
	InsolvencyType varchar(1)
)

INSERT INTO #Cases 
SELECT DISTINCT DateofOrder as DateOrder, CaseNo, IndivNo, Type as InsolvencyType FROM eiirSnapshotTABLE ORDER BY DateofOrder

CREATE TABLE #broDetails(
	case_id int, 
	subj_refno int,
	bro_order_date datetime,
	bro_end_date datetime
) 
INSERT INTO  #broDetails
	SELECT case_id, subj_refno,	bro_order_date,	bro_end_date
	FROM	subject_bro 
	LEFT JOIN #Cases c on case_id = c.CaseNo AND subj_refno = c.IndivNo
	WHERE (bro_order_date IS NOT NULL
	AND    app_filed_date IS NOT NULL
	AND    bro_hearing_date IS NOT NULL
	AND    bro_annulled_date IS NULL
	AND	bro_end_date >= getDate())


CREATE TABLE #bruDetails(
	case_id int, 
	subj_refno int,
	bru_accpt_date datetime,
	bro_end_date datetime
) 

INSERT INTO #bruDetails
	SELECT case_id, subj_refno, bru_accpt_date,	bro_end_date
	FROM	subject_bro 
	LEFT JOIN #Cases c on case_id = c.CaseNo AND subj_refno = c.IndivNo
	WHERE (bro_order_date IS NULL
	AND      bro_annulled_date IS NULL
	AND      bru_accpt_date IS NOT NULL
	AND	 bro_end_date >= getDate())

CREATE TABLE #droDetails(
	CaseID int,
	CIDebtorSubjectNo int,
	MoratoriumPeriodEndingDate datetime,
	RevokedDate datetime
)
INSERT INTO #droDetails
	SELECT CaseID, CIDebtorSubjectNo, MoratoriumPeriodEndingDate, RevokedDate
	FROM   subject_dro 
	LEFT JOIN #Cases c on CaseID = c.CaseNo AND CIDebtorSubjectNo = c.IndivNo

CREATE TABLE #ibroDetails(
	case_id int, 
	subj_refno int,
	ibro_order_date datetime,
	ibro_end_date datetime
) 
INSERT INTO #ibroDetails
	SELECT case_id, subj_refno, ibro_order_date, ibro_end_date
	FROM  subject_ibro 
	LEFT JOIN #Cases c on case_id = c.CaseNo AND subj_refno = c.IndivNo
	WHERE (ibro_order_date IS NOT NULL
	AND      ibro_app_filed_date IS NOT NULL
	AND      ibro_hearing_date IS NOT NULL
	AND      ibro_discharge_date IS NULL
	AND	 (ibro_end_date >= getDate() OR ibro_end_date IS NULL))

CREATE TABLE #discharge (
	case_no int,
	indiv_no int,
	discharge_type varchar(1),
	discharge_date datetime,
	suspension_date datetime,
	suspension_end_date datetime
)

INSERT INTO #discharge
	SELECT case_no, indiv_no, discharge_type, discharge_date, suspension_date, suspension_end_date 
	FROM	ci_indiv_discharge 
	LEFT JOIN #Cases c on case_no = c.CaseNo AND indiv_no = c.IndivNo

CREATE TABLE #prevIBRO (
	case_id int, 
	subj_refno int,
	ibro_order_date datetime,
	ibro_end_date datetime
) 

INSERT INTO #prevIBRO
	SELECT case_id, subj_refno,	ibro_order_date, ibro_end_date
	FROM	subject_ibro 
	LEFT JOIN #Cases c on case_id = c.CaseNo AND subj_refno = c.IndivNo
	AND	(ibro_order_date IS NOT NULL
	AND      ibro_app_filed_date IS NOT NULL
	AND      ibro_hearing_date IS NOT NULL
	AND      ibro_discharge_date IS NULL
	AND      ibro_end_date IS NOT NULL)

CREATE TABLE #prevIDRRO (
	case_id int, 
	subj_refno int,
	ibro_order_date datetime,
	ibro_end_date datetime
)
INSERT INTO #prevIDRRO 
SELECT case_id, subj_refno, ibro_order_date, ibro_end_date
	FROM subject_ibro 
	LEFT JOIN #Cases c on case_id = c.CaseNo AND subj_refno = c.IndivNo
	AND	(ibro_order_date IS NOT NULL
	AND      ibro_app_filed_date IS NOT NULL
	AND      ibro_hearing_date IS NOT NULL
	AND      ibro_discharge_date IS NULL
	AND      ibro_end_date IS NOT NULL)

CREATE TABLE #caseParams (
    caseNo int,
    indivNo int, 
	dischargeType varchar(1),
	dischargeDate datetime,
	suspensionDate datetime,
	suspensionEndDate datetime,
	hasBru varchar(1),
	hasBro varchar(1),
	hasiBro varchar(1),
	PrevIBRONote varchar(1),
	PrevIDRRONote varchar(1),
	BROStartDate datetime,
	BROEndDate datetime,
	BRUStartDate datetime,
	BRUEndDate datetime,
	IBROStartDate datetime,
	IBROEndDate datetime,
	IDRROStartDate datetime,
	IDRROEndDate datetime,
	BROPrintCaseDetails varchar(1),
	MoratoriumPeriodEndingDate datetime, 
	RevokedDate datetime
) 

INSERT INTO #caseParams
SELECT c.CaseNo, c.IndivNo,
	(SELECT discharge_type FROM #discharge d where d.case_no = c.CaseNo and indiv_no = c.IndivNo) AS dischargeType,
	(ISNULL((SELECT discharge_date FROM #discharge d where d.case_no = c.CaseNo and indiv_no = c.IndivNo), NULL)) AS dischargeDate,
	(ISNULL((SELECT suspension_date FROM #discharge d where d.case_no = c.CaseNo and indiv_no = c.IndivNo), NULL)) AS suspensionDate,
	(ISNULL((SELECT suspension_end_date FROM #discharge d where d.case_no = c.CaseNo and indiv_no = c.IndivNo), NULL)) AS suspensionEndDate,
	(ISNULL((SELECT 'Y' FROM #bruDetails  WHERE case_id = c.CaseNo and subj_refno = c.IndivNo), 'N')) AS hasBru,
	(ISNULL((SELECT 'Y' FROM #broDetails  WHERE case_id = c.CaseNo and subj_refno = c.IndivNo), 'N')) AS hasBro,
	(ISNULL((SELECT 'Y' FROM #ibroDetails WHERE case_id = c.CaseNo and subj_refno = c.IndivNo), 'N')) AS hasiBro,
	
	(ISNULL((SELECT 'Y' FROM #prevIBRO where case_id = c.CaseNo and subj_refno = c.IndivNo), 'N')) as PrevIBRONote,
	(ISNULL((SELECT 'Y' FROM #prevIDRRO where case_id = c.CaseNo and subj_refno = c.IndivNo), 'N')) as PrevIDRRONote,
	(ISNULL((SELECT bro_order_date from #broDetails where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as BROStartDate,
	(ISNULL((SELECT bro_end_date from #broDetails where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as BROEndDate,
	(ISNULL((SELECT bru_accpt_date from #bruDetails where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as BRUStartDate,
	(ISNULL((SELECT bro_end_date from #bruDetails where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as BRUEndDate,
	(ISNULL((SELECT ibro_order_date from #prevIBRO where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as IBROStartDate,
	(ISNULL((SELECT ibro_end_date from #prevIBRO where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as IBROEndDate,
	(ISNULL((SELECT ibro_order_date from #prevIDRRO where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as IDRROStartDate,
	(ISNULL((SELECT ibro_end_date from #prevIDRRO where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as IDRROEndDate,
	
	(SELECT (CASE 
		WHEN ((c.InsolvencyType = 'B' AND (((ISNULL((SELECT 'Y' FROM #broDetails WHERE case_id = c.CaseNo and subj_refno = c.IndivNo), 'N')) = 'Y' OR
											(ISNULL((SELECT 'Y' FROM #bruDetails  WHERE case_id = c.CaseNo and subj_refno = c.IndivNo), 'N')) = 'Y' OR
											(ISNULL((SELECT 'Y' FROM #ibroDetails WHERE case_id = c.CaseNo and subj_refno = c.IndivNo), 'N')) = 'Y'))
			AND ISNULL((SELECT discharge_date FROM #discharge WHERE case_no = c.CaseNo and indiv_no = c.IndivNo), GETDATE()) < DATEADD(MONTH, -3, GETDATE())))
			THEN 'N'
		ELSE 'Y'
	END)) AS BROPrintCaseDetails,

	(ISNULL((SELECT MoratoriumPeriodEndingDate from #droDetails where CaseID = c.CaseNo and CIDebtorSubjectNo = c.IndivNo), NULL)) as MoratoriumPeriodEndingDate,
	(ISNULL((SELECT RevokedDate from #droDetails where CaseID = c.CaseNo and CIDebtorSubjectNo = c.IndivNo), NULL)) as RevokedDate

FROM #Cases c 

CREATE TABLE #Temp
(
    caseNo int,
    indivNo int, 
	totalEntries int,
	totalBanks int,
	totalIVAs int,
	newBanks int,
	totalDros int,
    individualForenames varchar(255),
    individualSurname varchar(255),
    individualTitle varchar(255),
    individualGender varchar(255),
    individualDOB varchar(255),
    individualOccupation varchar(255),
    individualAddress varchar(255), 
    individualPostcode varchar(255),
    individualAddressWithheld varchar(255),
    individualAlias varchar(255),
	deceasedDate varchar(255),
	restrictionsType varchar(255),
	restrictionsStartDate varchar(255),
	restrictionsEndDate varchar(255),
	droRestrictionsType varchar(255),
	droRestrictionsStartDate varchar(255),
	droRestrictionsEndDate varchar(255),
	restrictionsCourt varchar(255),
	restrictionsCourtNo varchar(255),
	restrictionsCaseYear varchar(255),
	previousIBRONote varchar(255),
	previousIBROStartDate varchar(255),
	previousIBROEndDate varchar(255),
	previousIDRRONote varchar(255),
	previousIDRROStartDate varchar(255),
	previousIDRROEndDate varchar(255),
    caseName varchar(255),
    courtName varchar(255),
	courtNumber varchar(255),
    caseYear varchar(255),
    insolvencyType varchar(255),
    notificationDate varchar(255),
    insolvencyDate varchar(255),
    caseStatus varchar(255),
	annulDate varchar(255),
	annulReason varchar(255),
    caseDescription nvarchar(max),
    tradingNames xml,
    insolvencyPractitionerName varchar(255),
    insolvencyPractitionerFirmName varchar(255),
    insolvencyPractitionerAddress varchar(255),
    insolvencyPractitionerPostcode varchar(255),
    insolvencyPractitionerTelephone varchar(255),
    insolvencyServiceOffice varchar(255),
    insolvencyServiceContact varchar(255),
    insolvencyServiceAddress varchar(255),
    insolvencyServicePostcode varchar(255),
    insolvencyServicePhone varchar(255),
	dateOrder datetime,
)

	INSERT INTO #Temp
    SELECT DISTINCT  

    individual.case_no AS caseNo,
    individual.indiv_no AS indivNo, 

	--extract details
	extract_entries AS totalEntries,
	extract_banks AS totalBanks,
	extract_ivas AS totalIVAs,
	new_banks AS newBanks,
	extract_dros AS totalDros,

    -- Individual details        
    ISNULL(NULLIF(UPPER(individual.forenames),''), 'No Forenames Found') AS individualForenames, 
    ISNULL(NULLIF(UPPER(individual.surname), ''), 'No Surname Found') AS individualSurname, 
    ISNULL(NULLIF(individual.title, ''), 'No Title Found') AS individualTitle,

    CASE 
        WHEN individual.sex = 'M' THEN 'Male'
        WHEN individual.sex = 'F' THEN 'Female'
        ELSE 'No Gender Found'
    END AS individualGender,
        
    ISNULL(NULLIF(CONVERT(CHAR(30), individual.date_of_birth, 103), ''), 'No Date of Birth Found') AS individualDOB, 

	CASE 
	  WHEN individual.job_title IS NULL OR individual.job_title = ''
		  THEN 'No Occupation Found'
	  WHEN CHARINDEX('-', individual.job_title, 1) > 1
		  THEN (SELECT  STRING_AGG(job_title , '-') job from (select trim(value) job_title FROM STRING_SPLIT(individual.job_title, '-')  ORDER BY (SELECT NULL) OFFSET 1 ROWS ) tbl)
	  ELSE
		  individual.job_title
	END AS individualOccupation,

    CASE WHEN wflag = 'Y' 
        THEN '(Sorry - this Address has been withheld)'
        ELSE  
            CONCAT_WS(', ',TRIM (NULLIF(individual.address_line_1, '')),TRIM (NULLIF(individual.address_line_2, '')),TRIM (NULLIF(individual.address_line_3, '')),TRIM (NULLIF(individual.address_line_4, '')),TRIM (NULLIF(individual.address_line_5, '')))             
    END AS individualAddress,

	CASE WHEN wflag = 'Y' 
		THEN '(Sorry - this Address has been withheld)'
		ELSE  ISNULL(CAST(individual.postcode AS VARCHAR(30)), 'No Last Known PostCode Found')
	END AS individualPostcode, 
    
	individual.address_withheld_flag AS individualAddressWithheld, 

	(SELECT 
		CASE WHEN 
		(SELECT STRING_AGG(UPPER(ci_other_name.surname) + ' ' + (UPPER(ci_other_name.forenames)), ', ') FROM ci_other_name  WHERE ci_other_name.case_no = snap.CaseNo AND ci_other_name.indiv_no = snap.IndivNo) IS NULL THEN 'No OtherNames Found'
		ELSE
		(SELECT STRING_AGG(UPPER(ci_other_name.surname) + ' ' + (UPPER(ci_other_name.forenames)), ', ') FROM ci_other_name  WHERE ci_other_name.case_no = snap.CaseNo AND ci_other_name.indiv_no = snap.IndivNo)
	END) AS individualAlias,    
	
	ISNULL(CONVERT(CHAR(10), snap.Deceased, 103), '') AS deceasedDate,

	-- Bankruptcy Details
	CASE WHEN cp.hasBro = 'Y' AND insolvency_type = 'B' 
			THEN 'BANKRUPTCY RESTRICTIONS ORDER (BRO)'
		WHEN cp.hasBru = 'Y' AND insolvency_type = 'B' 
			THEN 'BANKRUPTCY RESTRICTIONS UNDERTAKING (BRU)'
		WHEN cp.hasiBro = 'Y' AND insolvency_type = 'B' 
			THEN 'INTERIM BANKRUPTCY RESTRICTIONS ORDER (IBRO)'
		ELSE NULL
	END AS restrictionsType,

	CASE WHEN cp.hasBro = 'Y' AND insolvency_type = 'B' 
			THEN ISNULL(CONVERT(CHAR(10), cp.BROStartDate, 103), '')  
		WHEN cp.hasBru = 'Y' AND insolvency_type = 'B' 
			THEN ISNULL(CONVERT(CHAR(10), cp.BRUStartDate, 103), '') 
		WHEN cp.hasiBro = 'Y' AND insolvency_type = 'B' 
			THEN ISNULL(CONVERT(CHAR(10), cp.IBROStartDate, 103), '') 
		ELSE NULL
	END AS restrictionsStartDate,

	CASE WHEN cp.hasBro = 'Y' AND insolvency_type = 'B' 
			THEN ISNULL(CONVERT(CHAR(10), cp.BROEndDate, 103), '') 
		WHEN cp.hasBru = 'Y' AND insolvency_type = 'B' 
			THEN ISNULL(CONVERT(CHAR(10), cp.BRUEndDate, 103), '')
		WHEN cp.hasiBro = 'Y' AND insolvency_type = 'B' 
			THEN ISNULL(CONVERT(CHAR(10), cp.IBROEndDate, 103), '')
		ELSE NULL
	END AS restrictionsEndDate,

	CASE WHEN cp.hasBro = 'Y' AND insolvency_type = 'D'
			THEN 'DEBT RELIEF RESTRICTIONS ORDER (DRRO)'
		WHEN cp.hasBru = 'Y' AND insolvency_type = 'D' 
			THEN 'DEBT RELIEF RESTRICTION UNDERTAKING (DRRU)'
		ELSE NULL
	END AS droRestrictionsType,

	CASE WHEN cp.hasBro = 'Y' AND insolvency_type = 'D' 
			THEN ISNULL(CONVERT(CHAR(10), cp.BROStartDate, 103), '') 
		WHEN cp.hasBru = 'Y' AND insolvency_type = 'D' 
			THEN ISNULL(CONVERT(CHAR(10), cp.BRUStartDate, 103), '') 
		ELSE NULL
	END AS droRestrictionsStartDate,

	CASE WHEN cp.hasBro = 'Y' AND insolvency_type = 'D' 
			THEN ISNULL(CONVERT(CHAR(10),  cp.BROEndDate, 103), '')
		WHEN cp.hasBru = 'Y' AND insolvency_type = 'D' 
			THEN ISNULL(CONVERT(CHAR(10),  cp.BRUEndDate, 103), '')
		ELSE NULL
	END AS droRestrictionsEndDate,
		
	CASE WHEN ((cp.hasBro = 'Y' OR cp.hasBru = 'Y' OR cp.hasiBro = 'Y') AND cp.BROPrintCaseDetails = 'N')
		THEN (SELECT STRING_AGG( ISNULL(court_name, ' '), ', ' )  FROM ci_court WHERE court = inscase.court)
		ELSE NULL
	END AS restrictionsCourt,

	CASE WHEN ((cp.hasBro = 'Y' OR cp.hasBru = 'Y' OR cp.hasiBro = 'Y') AND cp.BROPrintCaseDetails = 'N')
		THEN TRIM(snap.Number)
		ELSE NULL
	END AS restrictionsCourtNo,

	CASE WHEN ((cp.hasBro = 'Y' OR cp.hasBru = 'Y' OR cp.hasiBro = 'Y') AND cp.BROPrintCaseDetails = 'N')
		THEN TRIM(cast(snap.Year as varchar(255)))
		ELSE NULL
	END AS restrictionsCaseYear,

	CASE WHEN cp.hasBro = 'Y' AND cp.PrevIBRONote = 'Y' AND insolvency_type = 'B'
		THEN 'This BRO was preceded by an Interim Bankruptcy Restrictions Order (IBRO)'
		ELSE NULL
	END AS previousIBRONote,

	CASE WHEN cp.hasBro = 'Y' AND cp.PrevIBRONote = 'Y' AND insolvency_type = 'B'
		THEN ISNULL(CONVERT(CHAR(10), cp.IBROStartDate, 103), '')
		ELSE ''
	END AS previousIBROStartDate,

	CASE WHEN cp.hasBro = 'Y' AND cp.PrevIBRONote = 'Y' AND insolvency_type = 'B'
		THEN ISNULL(CONVERT(CHAR(10), cp.IBROEndDate, 103), '')
		ELSE NULL
	END AS previousIBROEndDate,

	CASE WHEN cp.hasBro = 'Y' AND cp.PrevIDRRONote = 'Y' AND insolvency_type = 'D'
		THEN 'This DRRO was preceded by an Interim Debt Relief Restrictions Order (IDRRO)'
		ELSE NULL
	END AS previousIDRRONote,

	CASE WHEN cp.hasBro = 'Y' AND cp.PrevIDRRONote = 'Y' AND insolvency_type = 'D'
		THEN ISNULL(CONVERT(CHAR(10), cp.IDRROStartDate, 103), '')
		ELSE NULL
	END AS previousIDRROStartDate,

	CASE WHEN cp.hasBro = 'Y' AND cp.PrevIDRRONote = 'Y' AND insolvency_type = 'D'
		THEN ISNULL(CONVERT(CHAR(10), cp.IDRROEndDate, 103), '')
		ELSE NULL
	END AS previousIDRROEndDate,

    --  Insolvency case details
    inscase.case_name AS caseName, 

	CASE
		--APP-4951 following string aggregation on courtname appears unnecessary, DISTINCT required on court name as current duplicate records in ci_court where court='ADJ'
		WHEN insolvency_type = 'I' OR insolvency_type = 'B'
			THEN (SELECT STRING_AGG( ISNULL(tbl.court_name, ' '), ', ' )  FROM (SELECT DISTINCT court_name FROM ci_court WHERE court = inscase.court) tbl)
		WHEN insolvency_type = 'D' AND cp.hasBro = 'Y'
			THEN (SELECT STRING_AGG( ISNULL(tbl.court_name, ' '), ', ' )  FROM (SELECT DISTINCT court_name FROM ci_court WHERE court = inscase.court) tbl)
		ELSE '(Court does not apply to DRO)'
	END AS courtName,

	CASE
		WHEN insolvency_type = 'I' THEN
			TRIM(number)
		WHEN insolvency_type = 'B' THEN
				RIGHT('0000000'+ISNULL(number,''),7) 
		WHEN insolvency_type = 'D' THEN				
		replace(
			translate(
			DRONumber,
			'ABCDEFGHIJKLMNOPQRSTUVXYZ-',
			'                          '
			),
			' ', '')
	END AS courtNumber, 

    CASE
        WHEN insolvency_type = 'I' THEN '(Case Year does not apply to IVA)'
		WHEN insolvency_type = 'B' THEN TRIM(cast(snap.Year as varchar(255)))
        WHEN insolvency_type = 'D' THEN TRIM(cast(datepart(year, snap.DateOfOrder) AS varchar(255)))
    END AS caseYear,

    CASE
        WHEN insolvency_type = 'B' THEN 'Bankruptcy'
        WHEN insolvency_type = 'I' THEN 'Individual Voluntary Arrangement'
        WHEN insolvency_type = 'D' THEN 'Debt Relief Order'
        ELSE NULL
    END AS insolvencyType,

    ivaCase.date_of_notification AS notificationDate,
    ISNULL(CONVERT(CHAR(10), inscase.insolvency_date, 103), '') AS insolvencyDate,
        	
	CASE WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'D' AND cp.RevokedDate IS NULL AND cp.MoratoriumPeriodEndingDate > GETDATE()
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'DRO1') + ' will end on ' + CONVERT(CHAR(10), cp.MoratoriumPeriodEndingDate, 103))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'D' AND cp.RevokedDate IS NULL AND cp.MoratoriumPeriodEndingDate <= GETDATE()
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'DRO1') + ' ended on ' + CONVERT(CHAR(10), cp.MoratoriumPeriodEndingDate, 103))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'D' AND cp.RevokedDate IS NOT NULL 
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'DRO2') + ' on ' + CONVERT(CHAR(10), cp.MoratoriumPeriodEndingDate, 103))
		
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'D' AND DateofOrder IS NOT NULL AND cp.MoratoriumPeriodEndingDate IS NOT NULL
			AND  DateDiff(day, DATEADD(month, 12, DateofOrder), cp.MoratoriumPeriodEndingDate) > 1 
			THEN  TRIM(('Extended From ' + CONVERT(CHAR(10), (DATEADD(month, 12, DateofOrder))) + ' To ' + CONVERT(CHAR(10), cp.MoratoriumPeriodEndingDate, 103)))		
		
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I' AND ivaCase.date_of_failure IS NOT NULL 
			THEN  TRIM((SELECT TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'F') + ' On ' + CONVERT(CHAR(10), ivaCase.date_of_failure, 103))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I' AND ivaCase.date_of_completion IS NOT NULL 
			THEN  TRIM((SELECT TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'C') + ' On ' + CONVERT(CHAR(10), ivaCase.date_of_completion, 103))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I' AND ivaCase. date_of_revocation IS NOT NULL 
			THEN  TRIM((SELECT TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'R') + ' On ' + CONVERT(CHAR(10), ivaCase.date_of_revocation, 103))

		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I' AND ivaCase.date_of_suspension IS NOT NULL AND ivaCase.date_suspension_lifted IS NULL
			THEN  TRIM((SELECT TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O5') + ' On ' + CONVERT(CHAR(10), ivaCase.date_of_suspension, 103))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I' AND ivaCase.date_of_suspension IS NOT NULL AND ivaCase.date_suspension_lifted IS NOT NULL
			THEN  TRIM((SELECT TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O6') + ' On ' + CONVERT(CHAR(10), ivaCase.date_suspension_lifted, 103))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I'
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O1'))

		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND CONVERT(CHAR(10), DateofOrder, 103) >= '01/04/2004'
			AND AnnulmentTypeCASE = '' AND cp.dischargeDate <= GETDATE() 
			AND cp.dischargeDate < (DATEADD(month, 12, DateofOrder)))
			THEN TRIM('Discharge Suspended Indefinitely (from ' + CONVERT(CHAR(10), cp.suspensionDate, 103) + ') (Early Discharge)')

		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND cp.suspensionDate IS NOT NULL AND cp.suspensionEndDate IS NOT NULL
			AND ISNULL(CONVERT(CHAR(10), cp.suspensionEndDate, 103), '') = '31/12/2099')
			THEN TRIM('Discharge Suspended Indefinitely (from ' + CONVERT(CHAR(10), cp.suspensionDate, 103) + ')')

		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND cp.dischargeDate IS NULL 
			AND cp.suspensionDate IS NOT NULL AND cp.suspensionEndDate IS NOT NULL)
			THEN TRIM('Discharge Fixed Length Suspension (from ' + CONVERT(CHAR(10), cp.suspensionDate, 103) + ' to ' +  CONVERT(CHAR(10), cp.suspensionEndDate, 103) + ')')

		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = '' AND cp.dischargeDate <= GETDATE())
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'D') + ' On ' + CONVERT(CHAR(10), cp.dischargeDate, 103))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = '' AND cp.dischargeDate > GETDATE())
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O2') + ' will be ' + CONVERT(CHAR(10), cp.dischargeDate, 103))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND cp.dischargeDate IS NULL AND cp.dischargeType = 'A')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O3')) 
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND cp.dischargeDate IS NULL AND cp.dischargeType = 'G')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O4')) 

		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'P')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A1') + ' On ' + CONVERT(CHAR(10), AnnulmentDateCASE, 103))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'V')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A2') + ' On ' + CONVERT(CHAR(10), AnnulmentDateCASE, 103))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'R')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A3') + ' On ' + CONVERT(CHAR(10), AnnulmentDateCASE, 103))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'A')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A4') + ' On ' + + CONVERT(CHAR(10), AnnulmentDateCASE, 103))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypePARTNER = 'P')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A1') + ' On ' + CONVERT(CHAR(10), AnnulmentDatePARTNER, 103))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'V')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A2') + ' On ' + CONVERT(CHAR(10), AnnulmentDatePARTNER, 103))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'R')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A3') + ' On ' + CONVERT(CHAR(10), AnnulmentDatePARTNER, 103))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'A')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A4') + ' On ' + CONVERT(CHAR(10), AnnulmentDatePARTNER, 103))
	END AS caseStatus,

	CASE WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE <> '')
			THEN ISNULL(CONVERT(CHAR(10), AnnulmentDateCASE, 103), '')
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypePARTNER <> '')
			THEN ISNULL(CONVERT(CHAR(10), AnnulmentDatePARTNER, 103), '') 
		ELSE NULL
	END AS annulDate,

	CASE WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'P')
			THEN (select SelectionValue from #StatusCodes WHERE SelectionCode = 'A1') 
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'V')
			THEN (select SelectionValue from #StatusCodes WHERE SelectionCode = 'A2') 
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'R')
			THEN (select SelectionValue from #StatusCodes WHERE SelectionCode = 'A3') 
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'A')
			THEN (select SelectionValue from #StatusCodes WHERE SelectionCode = 'A4') 

		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypePARTNER = 'P')
			THEN (select SelectionValue from #StatusCodes WHERE SelectionCode = 'A1') 
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypePARTNER = 'V')
			THEN (select SelectionValue from #StatusCodes WHERE SelectionCode = 'A2') 
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypePARTNER = 'R')
			THEN (select SelectionValue from #StatusCodes WHERE SelectionCode = 'A3') 
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypePARTNER = 'A')
			THEN (select SelectionValue from #StatusCodes WHERE SelectionCode = 'A4') 
	END AS annulReason,


	CASE
		WHEN address_withheld_flag = 'Y' THEN '(Case Description withheld as Individual Address has been withheld)'
        WHEN insolvency_type = 'I' THEN '(Case Description does not apply to IVA)'
        ELSE 
		ISNULL((SELECT STRING_AGG(ci_case_desc.case_desc_line, '')
			FROM ci_case_desc
			WHERE  ci_case_desc.case_no = snap.CaseNo
			),'No Case Description Found')
    END AS caseDescription,

    CASE WHEN (SELECT 
		CASE WHEN 
			ci_trade.trading_name IS NULL THEN 'No Trading Names Found'
		ELSE ci_trade.trading_name
		END AS TradingName,       
        
		CASE 
			WHEN ci_trade.trading_name is NULL THEN NULL
			ELSE 
				CASE 
					WHEN REPLACE(TRIM(CONCAT(ci_trade.address_line_1,  ', ', ci_trade.address_line_2,  ', ', ci_trade.address_line_3,  ', ', ci_trade.address_line_4,  ', ', ci_trade.address_line_5, ', ', ci_trade.postcode)), ' ,', '') = ',' THEN '@@@@@@@@@@'
					ELSE 
						CASE
							WHEN REPLACE(TRIM(CONCAT(ci_trade.address_line_1,  ', ', ci_trade.address_line_2,  ', ', ci_trade.address_line_3,  ', ', ci_trade.address_line_4,  ', ', ci_trade.address_line_5, ', ', ci_trade.postcode)), ' ,', '') LIKE '%,'
								THEN LEFT(REPLACE(TRIM(CONCAT(ci_trade.address_line_1,  ', ', ci_trade.address_line_2,  ', ', ci_trade.address_line_3,  ', ', ci_trade.address_line_4,  ', ', ci_trade.address_line_5, ', ', ci_trade.postcode)), ' ,', ''), LEN(REPLACE(TRIM(CONCAT(ci_trade.address_line_1,  ', ', ci_trade.address_line_2,  ', ', ci_trade.address_line_3,  ', ', ci_trade.address_line_4,  ', ', ci_trade.address_line_5, ', ', ci_trade.postcode)), ' ,', ''))-1)
							ELSE REPLACE(TRIM(CONCAT(ci_trade.address_line_1,  ', ', ci_trade.address_line_2,  ', ', ci_trade.address_line_3,  ', ', ci_trade.address_line_4,  ', ', ci_trade.address_line_5, ', ', ci_trade.postcode)), ' ,', '')
						END					
				END
			END AS TradingAddress

		FROM ci_trade 
		where ci_trade.case_no = snap.CaseNo
		FOR XML PATH('')) IS NULL THEN 'No Trading Names Found'

	ELSE (SELECT 
		CASE WHEN 
			ci_trade.trading_name IS NULL THEN 'No Trading Names Found'
		ELSE ci_trade.trading_name
		END AS TradingName,       
        
		CASE 
			WHEN ci_trade.trading_name is NULL THEN NULL
			ELSE 
				CASE 
					WHEN REPLACE(TRIM(CONCAT(ci_trade.address_line_1,  ', ', ci_trade.address_line_2,  ', ', ci_trade.address_line_3,  ', ', ci_trade.address_line_4,  ', ', ci_trade.address_line_5, ', ', ci_trade.postcode)), ' ,', '') = ',' THEN '@@@@@@@@@@'
					ELSE REPLACE(TRIM(CONCAT(ci_trade.address_line_1,  ', ', ci_trade.address_line_2,  ', ', ci_trade.address_line_3,  ', ', ci_trade.address_line_4,  ', ', ci_trade.address_line_5, ', ', ci_trade.postcode)), ' ,', '')
				END
			END AS TradingAddress

		FROM ci_trade 
		where ci_trade.case_no = snap.CaseNo
		FOR XML PATH('')) 
	END AS tradingNames,

    --  Insolvency practitioner contact details
    TRIM((SELECT STRING_AGG( ISNULL(ci_ip.forenames +' '+ ci_ip.surname, ' '), ', ' ) FROM ci_ip  WHERE ci_ip.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL and insolvencyAppointment.case_no = inscase.case_no)) AS insolvencyPractitionerName,
    TRIM((SELECT TOP 1 ip_firm_name FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL and insolvencyAppointment.ip_address_no = ci_ip_address.ip_address_no and insolvencyAppointment.case_no = inscase.case_no)) AS insolvencyPractitionerFirmName,

	CASE 
		   WHEN CHARINDEX(',',REVERSE(TRIM((SELECT TOP 1 REPLACE(TRIM(CONCAT(ci_ip_address.address_line_1,  ', ', ci_ip_address.address_line_2,  ', ', ci_ip_address.address_line_3, ', ',  ci_ip_address.address_line_4,  ', ', ci_ip_address.address_line_5)), ' ,', '') FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL and insolvencyAppointment.ip_address_no = ci_ip_address.ip_address_no and insolvencyAppointment.case_no = inscase.case_no)))) = 1 
           THEN LEFT(TRIM((SELECT TOP 1 REPLACE(TRIM(CONCAT(TRIM(ci_ip_address.address_line_1),  ', ', TRIM(ci_ip_address.address_line_2),  ', ', TRIM(ci_ip_address.address_line_3), ', ',  TRIM(ci_ip_address.address_line_4),  ', ', TRIM(ci_ip_address.address_line_5))), ' ,', '') FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL)),LEN(TRIM((SELECT TOP 1 REPLACE(TRIM(CONCAT(TRIM(ci_ip_address.address_line_1),  ', ', TRIM(ci_ip_address.address_line_2),  ', ', TRIM(ci_ip_address.address_line_3), ', ',  TRIM(ci_ip_address.address_line_4),  ', ', TRIM(ci_ip_address.address_line_5))), ' ,', '')  FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL and insolvencyAppointment.ip_address_no = ci_ip_address.ip_address_no and insolvencyAppointment.case_no = inscase.case_no)))-1) 
           ELSE TRIM((SELECT TOP 1 REPLACE(TRIM(CONCAT(TRIM(ci_ip_address.address_line_1),  ', ', TRIM(ci_ip_address.address_line_2),  ', ', TRIM(ci_ip_address.address_line_3), ', ',  TRIM(ci_ip_address.address_line_4),  ', ', TRIM(ci_ip_address.address_line_5))), ' ,', '') FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL and insolvencyAppointment.ip_address_no = ci_ip_address.ip_address_no and insolvencyAppointment.case_no = inscase.case_no)) 
    END insolvencyPractitionerAddress,

    TRIM((SELECT TOP 1 postcode FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL and insolvencyAppointment.ip_address_no = ci_ip_address.ip_address_no and insolvencyAppointment.case_no = inscase.case_no))  AS insolvencyPractitionerPostcode,
    TRIM((SELECT TOP 1 phone FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL and insolvencyAppointment.ip_address_no = ci_ip_address.ip_address_no and insolvencyAppointment.case_no = inscase.case_no)) AS insolvencyPractitionerTelephone, 

    --  Insolvency Service contact details   
	CASE WHEN insolvency_type = 'D' THEN 
		ISNULL((SELECT office_name from ci_office where office_name LIKE 'DRO%'), '')
		ELSE 
		(insolvencyService.office_name)
	END AS insolvencyServiceOffice,

	CASE WHEN insolvency_type = 'D' THEN 
		ISNULL((SELECT RTRIM(register_contact) from ci_office where office_name LIKE 'DRO%'), '')
		ELSE 
		ISNULL((RTRIM(insolvencyService.register_contact)), '')
	END AS insolvencyServiceContact,

	CASE WHEN insolvency_type = 'D' THEN 
		ISNULL((SELECT CONCAT(address_line_1,  ', ', address_line_2,  ', ', address_line_3,  ', ', address_line_4,  ', ', address_line_5) 
		from ci_office where office_name LIKE 'DRO%'), '')
		ELSE 
		ISNULL((REPLACE(TRIM(CONCAT(insolvencyService.address_line_1,  ', ', insolvencyService.address_line_2,  ', ', insolvencyService.address_line_3,  ', ', insolvencyService.address_line_4,  ', ', insolvencyService.address_line_5)), ' ,', '')), '')
	END AS insolvencyServiceAddress,

	CASE WHEN insolvency_type = 'D' THEN 
		(SELECT postcode from ci_office where office_name LIKE 'DRO%')
		ELSE 
		(insolvencyService.postcode)
	END AS insolvencyServicePostcode,

	CASE WHEN insolvency_type = 'D' THEN 
		ISNULL((SELECT phone from ci_office where office_name LIKE 'DRO%'), '')
		ELSE 
		(insolvencyService.phone)
	END AS insolvencyServicePhone,
	cases.dateOrder

    FROM  #Cases cases
	INNER JOIN eiirSnapshotTABLE snap on cases.CaseNo = snap.CaseNo
    INNER JOIN ci_individual individual on individual.indiv_no = snap.IndivNo and individual.case_no = snap.CaseNo 
	INNER JOIN extract_availability ea on ea.extract_id = Convert(CHAR(8),GETDATE(),112)
	LEFT JOIN #caseParams cp on cp.caseNo = snap.CaseNo AND cp.indivNo = snap.IndivNo
    LEFT JOIN ci_case inscase ON snap.CaseNo = inscase.case_no
    LEFT JOIN ci_office insolvencyService ON inscase.office_id = insolvencyService.office_id
    LEFT JOIN ci_ip_appt insolvencyAppointment ON insolvencyAppointment.case_no = inscase.case_no AND insolvencyAppointment.appt_end_date IS NULL and insolvencyAppointment.ip_appt_type = 'M'
	LEFT JOIN ci_ip cip ON insolvencyAppointment.ip_no = cip.ip_no
	LEFT JOIN ci_ip_address cipa ON insolvencyAppointment.ip_no = cipa.ip_no
    LEFT JOIN ci_iva_case ivaCase ON ivaCase.case_no = inscase.case_no

DECLARE @resultExtractXML xml
DECLARE @resultDisclaimerXML xml
DECLARE @resultXML xml
DECLARE @outputXML xml
DECLARE @outputWrapper xml

SET @resultExtractXML = (SELECT TOP 1 	  
	(SELECT totalEntries AS TotalEntries,
	totalBanks AS TotalBanks,
	totalIVAs AS TotalIVAs,
	newBanks AS NewBanks,
	totalDros AS TotalDros FOR XML PATH ('ExtractVolumes'), TYPE)
	FROM #Temp t)
	
SET @resultDisclaimerXML = (SELECT  
	'While every effort has been made to ensure that the information provided is accurate, occasionally errors may occur. If you identify information which appears to be incorrect or omitted, please inform The Insolvency Service so that we can investigate the matter and correct the database as required.The Insolvency Case Details are taken from the Court Order made on the Order Date, and include the address(es) from which debts were incurred. They cannot be changed without the consent of the Court. The Individual Details may have changed since the Court Order but, even so, they might not reflect the person''s current address or occupation at the time you make your search, and they should not be relied on as such. The Insolvency Service cannot accept responsibility for any errors or ommissions as a result of negligence or otherwise. Please note that The Insolvenmcy Service and Official Receivers cannot provide legal or financial advice. You should seek this from a Citizen''s Advice Bureau, a solicitor, a qualified accountant, an authorised Insolvency Practitioner, reputable financial advisor or advice centre. The Individual Insolvency Register is a publicly available register and The Insolvency Service does not endorse, nor make any representations regarding, any use made of the data on the register by third parties.' FOR XML PATH ('Disclaimer'), TYPE)

SET @resultXML = (SELECT  	 
	(SELECT CONVERT(CHAR(10), GETDATE(), 103) AS ExtractDate,
        t.caseno AS CaseNoReportRequest,
        'Individual Details' AS IndividualDetailsText,
        (select t.caseno AS CaseNoIndividual,
        individualTitle AS Title,
        individualGender AS Gender,
        individualForenames AS FirstName,
        individualSurname AS Surname,
		individualOccupation AS Occupation,
        TRIM(individualDOB) AS DateofBirth,
        TRIM(individualAddress) AS LastKnownAddress,
		CASE 
			WHEN TRIM(individualPostcode) IS NULL THEN 'No Last Known PostCode Found'
			WHEN TRIM(individualPostcode) = '' THEN 'No Last Known PostCode Found'
			ELSE TRIM(individualPostcode)
		END AS LastKnownPostCode,
        individualAlias AS OtherNames
			FOR XML PATH ('IndividualDetails'), TYPE),

		(SELECT CASE 
			WHEN (t.insolvencyType = 'Bankruptcy' AND cp.BROPrintCaseDetails = 'N' AND (cp.hasBro = 'Y' OR cp.hasBru = 'Y' OR cp.hasiBro = 'Y'))
				THEN
				(SELECT restrictionsType AS RestrictionsType,
						restrictionsStartDate AS RestrictionsStartDate,
						restrictionsEndDate AS RestrictionsEndDate,
						courtName AS RestrictionsCourt,
						courtNumber AS RestrictionsCourtNo,
						caseYear AS RestrictionsCaseYear, 
						(CASE WHEN (cp.PrevIBRONote = 'Y' AND cp.hasBro = 'Y') THEN
							(SELECT previousIBRONote as PreviousIBRONote,
									previousIBROStartDate as PreviousIBROStartDate,
									previousIBROEndDate as PreviousIBROEndDate 
							FOR XML PATH ('PreviousIBRO'), TYPE) ELSE NULL END)
						FOR XML PATH ('BankruptcyRestrictionsDetails'), TYPE)

			WHEN (t.insolvencyType = 'Bankruptcy' AND cp.BROPrintCaseDetails = 'Y' AND (cp.hasBro = 'Y' OR cp.hasBru = 'Y' OR cp.hasiBro = 'Y'))
				THEN
				(SELECT restrictionsType AS RestrictionsType,
						restrictionsStartDate AS RestrictionsStartDate,
						restrictionsEndDate AS RestrictionsEndDate,
						(CASE WHEN (cp.PrevIBRONote = 'Y' AND cp.hasBro = 'Y') THEN
							(SELECT previousIBRONote as PreviousIBRONote,
									previousIBROStartDate as PreviousIBROStartDate,
									previousIBROEndDate as PreviousIBROEndDate 
							FOR XML PATH ('PreviousIBRO'), TYPE) ELSE NULL END)
						FOR XML PATH ('BankruptcyRestrictionsDetails'), TYPE)

			WHEN (t.insolvencyType = 'Debt Relief Order' AND cp.BROPrintCaseDetails = 'N' AND (cp.hasBro = 'Y' OR cp.hasBru = 'Y'))
				THEN
				(SELECT droRestrictionsType AS DRORestrictionsType,
						droRestrictionsStartDate AS DRORestrictionsStartDate,
						droRestrictionsEndDate AS DRORestrictionsEndDate,
					    courtName AS RestrictionsCourt,
						courtNumber AS RestrictionsCourtNo,
						caseYear AS RestrictionsCaseYear,
						(CASE WHEN (cp.PrevIDRRONote = 'Y' AND cp.hasBro = 'Y') THEN
							(SELECT previousIDRRONote as PreviousIDRRONote,
									previousIDRROStartDate as PreviousIDRROStartDate,
									previousIDRROEndDate as PreviousIDRROEndDate 
							FOR XML PATH ('PreviousIDRRO'), TYPE) ELSE NULL END)
						FOR XML PATH ('DebtReliefRestrictionsDetails'), TYPE)

			WHEN (t.insolvencyType = 'Debt Relief Order' AND cp.BROPrintCaseDetails = 'Y' AND (cp.hasBro = 'Y' OR cp.hasBru = 'Y'))
				THEN
				(SELECT droRestrictionsType AS DRORestrictionsType,
						droRestrictionsStartDate AS DRORestrictionsStartDate,
						droRestrictionsEndDate AS DRORestrictionsEndDate,
						(CASE WHEN (cp.PrevIDRRONote = 'Y' AND cp.hasBro = 'Y') THEN
							(SELECT previousIDRRONote as PreviousIDRRONote,
									previousIDRROStartDate as PreviousIDRROStartDate,
									previousIDRROEndDate as PreviousIDRROEndDate 
							FOR XML PATH ('PreviousIDRRO'), TYPE) ELSE NULL END)						
						FOR XML PATH ('DebtReliefRestrictionsDetails'), TYPE)
		END),

		(SELECT CASE WHEN cp.BROPrintCaseDetails = 'Y' 
			THEN 'Insolvency Case Details' ELSE NULL END) AS CaseDetailsText,
		(SELECT CASE WHEN cp.BROPrintCaseDetails = 'Y' 
			THEN
			(SELECT t.caseNo AS CaseNoCase,
					caseName AS CaseName,
					courtName AS Court,
					insolvencyType AS CaseType,
					CASE 
						WHEN courtNumber IS NULL THEN '@@@@@@@@@@'
						WHEN courtNumber = '' THEN '@@@@@@@@@@'
						ELSE courtNumber
					END As CourtNumber,
					caseYear AS CaseYear,
					insolvencyDate AS StartDate,
					TRIM(caseStatus) AS Status,
					caseDescription AS CaseDescription, 
					tradingNames AS TradingNames
					FOR XML PATH ('CaseDetails'), TYPE)
			ELSE NULL END),

		(SELECT CASE WHEN insolvencyPractitionerName IS NOT NULL 
			THEN 'Insolvency Practitioner Contact Details' ELSE NULL END) AS InsolvencyPractitionerText,

		(SELECT CASE WHEN insolvencyPractitionerName IS NOT NULL
			THEN (SELECT t.caseNo AS CaseNoIP,
				TRIM(insolvencyPractitionerName) AS MainIP,
				TRIM(insolvencyPractitionerFirmName) AS MainIPFirm,
				REPLACE(TRIM(insolvencyPractitionerAddress), ' ,', '') AS MainIPFirmAddress,
				TRIM(insolvencyPractitionerPostcode) AS MainIPFirmPostCode,
				CASE 
					WHEN TRIM(insolvencyPractitionerTelephone) IS NULL THEN '@@@@@@@@@@'
					WHEN TRIM(insolvencyPractitionerTelephone) = '' THEN '@@@@@@@@@@'
					ELSE TRIM(insolvencyPractitionerTelephone)
				END As MainIPFirmTelephone
				FOR XML PATH ('IP'), TYPE)
			ELSE NULL
		END),

        'Insolvency Service Contact Details' as InsolvencyContactText,
        (SELECT 
			t.caseNo AS CaseNoContact,
            TRIM(insolvencyServiceOffice) AS InsolvencyServiceOffice,
            TRIM(insolvencyServiceContact) AS Contact,
            REPLACE(TRIM(insolvencyServiceAddress), ' ,', '') AS ContactAddress,
            TRIM(insolvencyServicePostcode) AS ContactPostCode,
            TRIM(insolvencyServicePhone) AS ContactTelephone 
			FOR XML PATH ('InsolvencyContact'), TYPE)
		from #Temp t
		inner join #caseParams cp on cp.caseNo = t.caseNo
		order by t.dateOrder
        FOR XML PATH ('ReportRequest'), TYPE, ELEMENTS))

	SET @outputWrapper = (SELECT @resultExtractXML, @resultDisclaimerXML, @resultXML FOR XML PATH ('ReportDetails'), TYPE, ELEMENTS)

	SELECT REPLACE(REPLACE(((SELECT '<?xml version=''1.0'' encoding=''utf-8''?>') + CAST(@outputWrapper as varchar(MAX))), '@@@@@@@@@@',''),'"','&quot;')  AS 'Result'

If(OBJECT_ID('tempdb..#Temp') Is Not Null)
Begin
    DROP TABLE #Temp
End
If(OBJECT_ID('tempdb..#Cases') Is Not Null)
Begin
	DROP TABLE #Cases
End
If(OBJECT_ID('tempdb..#broDetails') Is Not Null)
Begin
	DROP TABLE #broDetails
End
If(OBJECT_ID('tempdb..#bruDetails') Is Not Null)
Begin
	DROP TABLE #bruDetails
End
If(OBJECT_ID('tempdb..#droDetails') Is Not Null)
Begin
	DROP TABLE #droDetails
End
If(OBJECT_ID('tempdb..#ibroDetails') Is Not Null)
Begin
	DROP TABLE #ibroDetails
End
If(OBJECT_ID('tempdb..#discharge') Is Not Null)
Begin
	DROP TABLE #discharge
End
If(OBJECT_ID('tempdb..#prevIBRO') Is Not Null)
Begin
	DROP TABLE #prevIBRO
End
If(OBJECT_ID('tempdb..#prevIDRRO') Is Not Null)
Begin
	DROP TABLE #prevIDRRO
End
If(OBJECT_ID('tempdb..#caseParams') Is Not Null)
Begin
	DROP TABLE #caseParams
End
If(OBJECT_ID('tempdb..#StatusCodes') Is Not Null)
Begin
	DROP TABLE #StatusCodes
End
END