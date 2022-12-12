SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

/****** Object:  Stored Procedure dbo.getSearchIndex    Script Date: 24/11/2022 08:11:59 ******/


CREATE OR ALTER PROCEDURE [dbo].[getEiirIndex]
AS
                 
    ;WITH getSnaphotData(
            caseNo,
            indivNo, 
            individualForenames,
            individualSurname,
            individualTitle,
            individualGender,
            individualDOB,
            individualOccupation,
            individualTown,
            individualAddress,
            individualPostcode,
            individualAddressWithheld,
            individualAlias,
            caseName,
            courtName,
			courtNumber,
            caseYear,
            insolvencyType,
            notificationDate,
            insolvencyDate,
            caseStatus,
            caseDescription,
            tradingNames,
            insolvencyPractitionerName,
            insolvencyPractitionerFirmName,
            insolvencyPractitionerAddress,
            insolvencyPractitionerPostcode,
            insolvencyPractitionerTelephone,
            insolvencyServiceOffice,
            insolvencyServiceContact,
            insolvencyServiceAddress,
            insolvencyServicePostcode,
            insolvencyServicePhone 
        ) AS (

        SELECT DISTINCT  


        individual.case_no AS caseNo,
        individual.indiv_no AS indivNo, 

        -- Individual details        
        UPPER(individual.forenames) AS individualForenames, 
        UPPER(individual.surname) AS individualSurname, 
        individual.title as individualTitle,
        CASE 
            WHEN individual.sex = 'M' THEN 'MALE'
            WHEN individual.sex = 'F' THEN 'Female'
            ELSE NULL
        END AS individualGender,
        
        CONVERT(CHAR(10), individual.date_of_birth, 103) AS individualDOB, 
        individual.job_title AS individualOccupation,
        individual.address_line_3 AS individualTown,
        CONCAT(individual.address_line_1, ' ', individual.address_line_2, ' ', individual.address_line_3, ' ', individual.address_line_4, ' ', individual.address_line_5) AS individualAddress,
        individual.postcode AS individualPostcode, 
        individual.address_withheld_flag AS individualAddressWithheld, 
        (SELECT STRING_AGG( ISNULL(UPPER(ci_other_name.surname) +' '+ UPPER(ci_other_name.forenames), ' '), ', ' ) FROM ci_other_name  WHERE ci_other_name.case_no = snap.CaseNo AND ci_other_name.indiv_no = snap.IndivNo) AS individualAlias,       
       
        --  Insolvency case details
        UPPER(inscase.case_name) AS caseName, 

		CASE
			WHEN insolvency_type = 'I' OR
				insolvency_type = 'B' THEN
				(SELECT STRING_AGG( ISNULL(court_name, ' '), ', ' )  FROM ci_court WHERE court = inscase.court)
			WHEN insolvency_type = 'D' THEN				
				'(Court does not apply to DRO)'
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
			WHEN insolvency_type = 'B' THEN cast(case_year as varchar(255))
            WHEN insolvency_type = 'D' THEN cast(datepart(year, snap.DateOfOrder) AS varchar(255))
        END AS caseYear,

        CASE
            WHEN insolvency_type = 'B' THEN 'Bankruptcy'
            WHEN insolvency_type = 'I' THEN 'Individual Voluntary Arrangement'
            WHEN insolvency_type = 'D' THEN 'Debt Relief Order'
            ELSE NULL
        END AS insolvencyType,

        ivaCase.date_of_notification AS notificationDate,
        CONVERT(CHAR(10), inscase.insolvency_date, 103) AS insolvencyDate,
        
        CASE case_status
            WHEN 'N' THEN 'Unknown'
            WHEN 'A' THEN 'Unknown'
            WHEN 'D' THEN 'Declined'
        ELSE 'CURRENT' 
        END AS caseStatus,

        CASE
			WHEN address_withheld_flag = 'Y' THEN '(Case Description withheld as Individual Address has been withheld)'
            WHEN insolvency_type = 'I' THEN '(Case Description does not apply to IVA)'
            ELSE 
			STUFF((SELECT CONCAT(ci_case_desc.case_desc_line, '')
				FROM ci_case_desc
				WHERE  ci_case_desc.case_no = snap.CaseNo
				for XML path ('')),1,0,'')
        END AS caseDescription,

        CASE WHEN (SELECT 
			CASE WHEN 
				ci_trade.trading_name IS NULL THEN 'No Trading Names'
			ELSE ci_trade.trading_name
			END AS TradingName,       
        
			CASE 
				WHEN ci_trade.trading_name is NULL THEN NULL
				ELSE CONCAT(ci_trade.address_line_1,  ', ', ci_trade.address_line_2,  ', ', ci_trade.address_line_3,  ', ', ci_trade.address_line_4,  ', ', ci_trade.address_line_5, ', ', ci_trade.postcode) 
				END AS TradingAddress

			FROM ci_trade 
			where ci_trade.case_no = snap.CaseNo
			FOR XML PATH('')) IS NULL THEN 'No Trading Names'

		ELSE (SELECT 
			CASE WHEN 
				ci_trade.trading_name IS NULL THEN 'No Trading Names'
			ELSE ci_trade.trading_name
			END AS TradingName,       
        
			CASE 
				WHEN ci_trade.trading_name is NULL THEN NULL
				ELSE CONCAT(ci_trade.address_line_1,  ', ', ci_trade.address_line_2,  ', ', ci_trade.address_line_3,  ', ', ci_trade.address_line_4,  ', ', ci_trade.address_line_5, ', ', ci_trade.postcode) 
				END AS TradingAddress

			FROM ci_trade 
			where ci_trade.case_no = snap.CaseNo
			FOR XML PATH('')) 
			END AS tradingNames,

        --  Insolvency practitioner contact details
        (SELECT STRING_AGG( ISNULL(UPPER(ci_ip.surname) +' '+ UPPER(ci_ip.forenames), ' '), ', ' ) FROM ci_ip  WHERE ci_ip.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL) AS insolvencyPractitionerName,
        (SELECT TOP 1 ip_firm_name FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL) AS insolvencyPractitionerFirmName,
        (SELECT TOP 1 CONCAT(ci_ip_address.address_line_1,  ' ', ci_ip_address.address_line_2,  ' ', ci_ip_address.address_line_3, ' ',  ci_ip_address.address_line_4,  ' ', ci_ip_address.address_line_5) FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL) AS insolvencyPractitionerAddress,
        (SELECT TOP 1 postcode FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL)  AS insolvencyPractitionerPostcode,
        (SELECT TOP 1 phone FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no and insolvencyAppointment.ip_appt_type = 'M' and insolvencyAppointment.appt_end_date IS NULL) AS insolvencyPractitionerTelephone, 


        --  Insolvency Service contact details   
		CASE WHEN insolvency_type = 'D' THEN 
			(SELECT office_name from ci_office where office_name LIKE 'DRO%')
			ELSE 
			(insolvencyService.office_name)
		END AS insolvencyServiceOffice,

		CASE WHEN insolvency_type = 'D' THEN 
			(SELECT RTRIM(register_contact) from ci_office where office_name LIKE 'DRO%')
			ELSE 
			(RTRIM(insolvencyService.register_contact))
		END AS insolvencyServiceContact,

		CASE WHEN insolvency_type = 'D' THEN 
			(SELECT CONCAT(address_line_1,  ' ', address_line_2,  ' ', address_line_3,  ' ', address_line_4,  ' ', address_line_5) 
			from ci_office where office_name LIKE 'DRO%')
			ELSE 
			(CONCAT(insolvencyService.address_line_1,  ' ', insolvencyService.address_line_2,  ' ', insolvencyService.address_line_3,  ' ', insolvencyService.address_line_4,  ' ', insolvencyService.address_line_5))
		END AS insolvencyServiceContact,

		CASE WHEN insolvency_type = 'D' THEN 
			(SELECT postcode from ci_office where office_name LIKE 'DRO%')
			ELSE 
			(insolvencyService.postcode)
		END AS insolvencyServicePostcode,

		CASE WHEN insolvency_type = 'D' THEN 
			(SELECT phone from ci_office where office_name LIKE 'DRO%')
			ELSE 
			(insolvencyService.phone)
		END AS insolvencyServicePhone

        FROM  eiirSnapshotTABLE snap
        INNER JOIN ci_individual individual on snap.IndivNo = individual.indiv_no and individual.case_no = snap.CaseNo 
        LEFT JOIN ci_case inscase ON snap.CaseNo = inscase.case_no
        LEFT JOIN ci_case_desc casedesc ON snap.CaseNo = casedesc.case_no and casedesc.case_desc_line_no = 1
        LEFT JOIN ci_office insolvencyService ON inscase.office_id = insolvencyService.office_id
        LEFT JOIN ci_ip_appt insolvencyAppointment ON insolvencyAppointment.case_no = inscase.case_no AND insolvencyAppointment.appt_end_date IS NULL and insolvencyAppointment.ip_appt_type = 'M'
		LEFT JOIN ci_ip cip ON insolvencyAppointment.ip_no = cip.ip_no
		LEFT JOIN ci_ip_address cipa ON insolvencyAppointment.ip_no = cipa.ip_no
        LEFT JOIN ci_iva_case ivaCase ON ivaCase.case_no = inscase.case_no
    )

    SELECT  caseNo,
            indivNo,
            individualForenames,
            individualSurname,
            individualTitle,
            individualGender,
            individualDOB,
            individualOccupation,
            individualTown,
            individualAddress,
            individualPostcode,
            individualAddressWithheld,
            individualAlias,
            caseName,
            courtName,
			courtNumber,
            caseYear,
            insolvencyType,
            notificationDate,
            insolvencyDate,
            caseStatus,
            caseDescription,
            tradingNames,
            insolvencyPractitionerName,
            insolvencyPractitionerFirmName,
            insolvencyPractitionerAddress,
            insolvencyPractitionerPostcode,
            insolvencyPractitionerTelephone,
            insolvencyServiceOffice,
            insolvencyServiceContact,
            insolvencyServiceAddress,
            insolvencyServicePostcode,
            insolvencyServicePhone 

    FROM getSnaphotData

GO
