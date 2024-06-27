/****** Object:  StoredProcedure [dbo].[getEiirIndex]    Script Date: 04/01/2023 10:37:08 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE OR ALTER PROCEDURE [dbo].[getEiirIndex]
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
	CaseNo int,
	IndivNo int,
	InsolvencyType varchar(1)
)

INSERT INTO #Cases 
SELECT DISTINCT CaseNo, IndivNo, Type as InsolvencyType FROM eiirSnapshotTABLE

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
	suspension_end_date datetime,
	previous_order_date datetime,
	previous_order_status varchar(1)
)

INSERT INTO #discharge
	SELECT  case_no, indiv_no, discharge_type, discharge_date, suspension_date, suspension_end_date,previous_order_date, previous_order_status
	FROM	ci_indiv_discharge 
	LEFT JOIN #Cases c on case_no = c.CaseNo AND indiv_no = c.IndivNo

CREATE TABLE #prevInterimRestrictionsOrder (
	case_id int, 
	subj_refno int,
	ibro_order_date datetime,
	ibro_end_date datetime
) 

INSERT INTO #prevInterimRestrictionsOrder
	SELECT case_id, subj_refno,	ibro_order_date, ibro_end_date
	FROM	subject_ibro 
	INNER JOIN #Cases c on case_id = c.CaseNo AND subj_refno = c.IndivNo
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
	PrevIRONote varchar(1),
	PrevIROStartDate datetime,
	PrevIROEndDate datetime,
	BROStartDate datetime,
	BROEndDate datetime,
	BRUStartDate datetime,
	BRUEndDate datetime,
	IBROStartDate datetime,
	IBROEndDate datetime,
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
	
	(ISNULL((SELECT 'Y' FROM #prevInterimRestrictionsOrder where case_id = c.CaseNo and subj_refno = c.IndivNo), 'N')) as PrevIRONote,
	(ISNULL((SELECT ibro_order_date from #prevInterimRestrictionsOrder where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as PrevIROStartDate,
	(ISNULL((SELECT ibro_end_date from #prevInterimRestrictionsOrder where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as PrevIROEndDate,
	(ISNULL((SELECT bro_order_date from #broDetails where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as BROStartDate,
	(ISNULL((SELECT bro_end_date from #broDetails where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as BROEndDate,
	(ISNULL((SELECT bru_accpt_date from #bruDetails where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as BRUStartDate,
	(ISNULL((SELECT bro_end_date from #bruDetails where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as BRUEndDate,
	(ISNULL((SELECT ibro_order_date from #ibroDetails where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as IBROStartDate,
	(ISNULL((SELECT ibro_end_date from #ibroDetails where case_id = c.CaseNo and subj_refno = c.IndivNo), NULL)) as IBROEndDate,
	
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

    SELECT DISTINCT 

    individual.case_no AS caseNo,
    individual.indiv_no AS indivNo, 

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
		  THEN (SELECT  STRING_AGG(job_title , ' ') job from (select trim(value) job_title FROM STRING_SPLIT(individual.job_title, '-')  ORDER BY (SELECT NULL) OFFSET 1 ROWS ) tbl)
	  ELSE
		  individual.job_title

	END AS individualOccupation,

	CASE WHEN wflag = 'Y' 
		THEN '(Sorry - this Address has been withheld)'
		ELSE  ISNULL(individual.address_line_3, 'No Last Known Town Found')
	END AS individualTown, 

    CASE WHEN wflag = 'Y' 
        THEN '(Sorry - this Address has been withheld)'
        ELSE  
            STUFF ('' + CASE TRIM(COALESCE(individual.address_line_1, '')) WHEN '' THEN '' ELSE ', ' + TRIM(individual.address_line_1) END 
				+ CASE TRIM(COALESCE(individual.address_line_2, '')) WHEN '' THEN '' ELSE ', ' + TRIM(individual.address_line_2) END
				+ CASE TRIM(COALESCE(individual.address_line_3, '')) WHEN '' THEN '' ELSE ', ' + TRIM(individual.address_line_3) END
				+ CASE TRIM(COALESCE(individual.address_line_4, '')) WHEN '' THEN '' ELSE ', ' + TRIM(individual.address_line_4) END
				+ CASE TRIM(COALESCE(individual.address_line_5, '')) WHEN '' THEN '' ELSE ', ' + TRIM(individual.address_line_5) END
			,1,2, '')             
    END AS individualAddress,

	CASE WHEN wflag = 'Y' 
		THEN '(Sorry - this Address has been withheld)'
		ELSE  ISNULL(CAST(individual.postcode as VARCHAR(30)), 'No Last Known PostCode Found')
	END AS individualPostcode, 
    
	individual.address_withheld_flag AS individualAddressWithheld, 

	(SELECT 
		CASE WHEN 
		(SELECT STRING_AGG(UPPER(ci_other_name.surname) + ' ' + (UPPER(ci_other_name.forenames)), ', ') FROM ci_other_name  WHERE ci_other_name.case_no = snap.CaseNo AND ci_other_name.indiv_no = snap.IndivNo) IS NULL THEN 'No OtherNames Found'
		ELSE
		(SELECT STRING_AGG(UPPER(ci_other_name.surname) + ' ' + (UPPER(ci_other_name.forenames)), ', ') FROM ci_other_name  WHERE ci_other_name.case_no = snap.CaseNo AND ci_other_name.indiv_no = snap.IndivNo)
	END) AS individualAlias,    
	
	ISNULL(CONVERT(VARCHAR(10), snap.Deceased, 103), '') AS deceasedDate,

    --  Insolvency case details
    inscase.case_name AS caseName, 

	--APP-5018 following string aggregation on courtname appears unnecessary, DISTINCT required on court name as current duplicate records in ci_court where court='ADJ'
	CASE
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
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'DRO2') + ' on ' + CONVERT(CHAR(10), cp.RevokedDate, 103))
		
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
			THEN 'Discharged On ' + CONVERT(CHAR(10), cp.dischargeDate, 103) + ' (Early Discharge)'

		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND cp.suspensionDate IS NOT NULL AND cp.suspensionEndDate IS NOT NULL
			AND ISNULL(CONVERT(CHAR(10), cp.suspensionEndDate, 103), '') = '31/12/2099')
			THEN 'Discharge Suspended Indefinitely (from ' + CONVERT(CHAR(10), cp.suspensionDate, 103) + ')'

		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND cp.dischargeDate IS NULL 
			AND cp.suspensionDate IS NOT NULL AND cp.suspensionEndDate IS NOT NULL)
			THEN 'Discharge Fixed Length Suspension (from ' + CONVERT(CHAR(10), cp.suspensionDate, 103) + ' to ' +  CONVERT(CHAR(10), cp.suspensionEndDate, 103) + ')'

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
			(SELECT (STRING_AGG(TRIM(REPLACE(REPLACE(REPLACE(d.case_desc_line,CHAR(10),' '),CHAR(13),' '),CHAR(9),' ')), ''))			
			FROM ci_case_desc d
			WHERE d.case_no = snap.CaseNo
			)
    END AS caseDescription,

    CASE
	    WHEN EXISTS (
        SELECT 1 
        FROM ci_trade 
        WHERE ci_trade.case_no = snap.CaseNo 
          AND trading_name IS NOT NULL
    )
    THEN (
        SELECT (
            SELECT 
                trading_name AS TradingName,
                ISNULL(
					STUFF ('' + CASE TRIM(COALESCE(ci_trade_CTE.address_line_1, '')) WHEN '' THEN '' ELSE ', ' + TRIM(ci_trade_CTE.address_line_1) END 
								+ CASE TRIM(COALESCE(ci_trade_CTE.address_line_2, '')) WHEN '' THEN '' ELSE ', ' + TRIM(ci_trade_CTE.address_line_2) END
								+ CASE TRIM(COALESCE(ci_trade_CTE.address_line_3, '')) WHEN '' THEN '' ELSE ', ' + TRIM(ci_trade_CTE.address_line_3) END
								+ CASE TRIM(COALESCE(ci_trade_CTE.address_line_4, '')) WHEN '' THEN '' ELSE ', ' + TRIM(ci_trade_CTE.address_line_4) END
								+ CASE TRIM(COALESCE(ci_trade_CTE.address_line_5, '')) WHEN '' THEN '' ELSE ', ' + TRIM(ci_trade_CTE.address_line_5) END
								+ CASE TRIM(COALESCE(ci_trade_CTE.postcode, '')) WHEN '' THEN '' ELSE ', ' + TRIM(ci_trade_CTE.postcode) END
					,1,2, ''), 
                    ''
                ) AS TradingAddress 
            FROM (
                SELECT cit1.* 
                FROM ci_trade cit1  
                INNER JOIN ci_trade cit2 ON cit1.trading_no = cit2.trading_no 
                                        AND cit1.case_no = cit2.case_no 
                WHERE cit1.case_no = snap.CaseNo
            ) AS ci_trade_CTE
            FOR XML PATH('TradingDetails'), ROOT('Trading')
        )
    )
    ELSE '<No Trading Names Found>'
END AS tradingNames,

	CAST(CASE WHEN cp.hasBro = 'Y' OR cp.hasBru = 'Y' OR cp.hasiBro = 'Y' THEN 1 ELSE 0 END as bit) as hasRestrictions,
	CASE WHEN cp.hasBro = 'Y' THEN 'Order'
			WHEN cp.hasBru = 'Y' THEN 'Undertaking'
			WHEN cp.hasiBro = 'Y' THEN 'Interim Order'
			ELSE null END as restrictionsType,
	CAST(CASE 
		WHEN cp.hasBro = 'Y' THEN cp.BROStartDate 
		WHEN cp.hasBru = 'Y' THEN cp.BRUStartDate
		WHEN cp.hasiBro = 'Y' THEN cp.IBROStartDate
		ELSE null END as datetime) as restrictionsStartDate,
	CAST(CASE  
		WHEN cp.hasBro = 'Y' THEN cp.BROEndDate
		WHEN cp.hasBru = 'Y' THEN cp.BRUEndDate
		WHEN cp.hasiBro = 'Y' THEN cp.IBROEndDate
		ELSE null END as datetime) as restrictionsEndDate,
	--previous Interim Restriction Orders are not currently used for DRRO, though technically the schema could support it
	--in practice you can have prevIBRO, but prevIDRRO do not occur
	CAST(CASE WHEN cp.PrevIRONote = 'Y' AND cp.hasBro = 'Y' THEN 1 ELSE 0 END as bit) as hasaPrevInterimRestrictionsOrder,
	CAST(CASE WHEN cp.PrevIRONote = 'Y' AND cp.hasBro = 'Y' THEN cp.PrevIROStartDate ELSE null END as datetime) as prevInterimRestrictionsOrderStartDate, 
	CAST(CASE WHEN cp.PrevIRONote = 'Y' AND cp.hasBro = 'Y' THEN cp.PrevIROEndDate ELSE null END as datetime) as prevInterimRestrictionsOrderEndDate, 

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
		ISNULL((SELECT REPLACE(TRIM(CONCAT(address_line_1,  ', ', address_line_2,  ', ', address_line_3,  ', ', address_line_4,  ', ', address_line_5)), ' ,', '') 
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
	CASE WHEN 
		discharge.previous_order_status IN ('D', '', NULL) AND  discharge.previous_order_date NOT  IN ('', NULL) AND discharge.previous_order_date BETWEEN DATEADD(yy, -6, inscase.insolvency_date) AND inscase.insolvency_date 
	THEN 
		discharge.previous_order_date ELSE NULL END AS dateOfPreviousOrder

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
	LEFT JOIN #discharge discharge ON discharge.case_no = cases.CaseNo AND discharge.indiv_no = cases.IndivNo  

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
If(OBJECT_ID('tempdb..#prevInterimRestrictionsOrder') Is Not Null)
Begin
	DROP TABLE #prevInterimRestrictionsOrder
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