/****** Object:  StoredProcedure [dbo].[getCaseByCaseNoIndivNo]    Script Date: 10/31/2022 2:28:00 PM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO


/****** Object:  Stored Procedure dbo.getIndividual    Script Date: 26/10/2022 08:22:16 ******/



CREATE OR ALTER PROCEDURE [dbo].[getCaseByCaseNoIndivNo]

(
	@CaseNo int,
	@IndivNo int
)

AS
            
    ;WITH getCase(
			caseNo,
			indivNo, 
            individualForenames,
            individualSurname,
            individualTitle,
            individualGender,
            individualDOB,
            individualOccupation,
            individualAddress,
            individualPostcode,
            individualAddressWithheld,
            individualAlias,
            caseName,
            courtName,
            insolvencyType,
            notificationDate,
            insolvencyDate,
            caseStatus,
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
        
        individual.date_of_birth AS individualDOB, 
        individual.job_title AS individualOccupation,
        CONCAT(individual.address_line_1, ' ', individual.address_line_2, ' ', individual.address_line_3, ' ', individual.address_line_4, ' ', individual.address_line_5) AS individualAddress,
        individual.postcode AS individualPostcode, 
        individual.address_withheld_flag AS individualAddressWithheld, 
        (SELECT STRING_AGG( ISNULL(UPPER(ci_other_name.surname) +' '+ UPPER(ci_other_name.forenames), ' '), ', ' ) FROM ci_other_name  WHERE ci_other_name.case_no = @CaseNo AND ci_other_name.indiv_no = @IndivNo) AS individualAlias,
       
       
        --  Insolvency case details
        UPPER(inscase.case_name) AS caseName, 
        (SELECT STRING_AGG( ISNULL(court_name, ' '), ', ' )  FROM ci_court WHERE court = inscase.court) AS courtName,

        CASE
            WHEN insolvency_type = 'B' THEN 'Bankruptcy'
            WHEN insolvency_type = 'I' THEN 'Individual Voluntary Arrangement'
            WHEN insolvency_type = 'D' THEN 'Debt Relief Order'
            ELSE NULL
        END AS insolvencyType,
        ivaCase.date_of_notification AS notificationDate,
        inscase.insolvency_date AS insolvencyDate,
        
            CASE case_status
                WHEN 'N' THEN 'Unknown'
                WHEN 'A' THEN 'Unknown'
                WHEN 'D' THEN 'Declined'
            ELSE 'CURRENT' 
            END AS caseStatus,

        
        --  Insolvency practitioner contact details
        (SELECT STRING_AGG( ISNULL(UPPER(ci_ip.surname) +' '+ UPPER(ci_ip.forenames), ' '), ', ' ) FROM ci_ip  WHERE ci_ip.ip_no = insolvencyAppointment.ip_no) AS insolvencyPractitionerName,
        (SELECT TOP 1 ip_firm_name FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no) AS insolvencyPractitionerFirmName,
        (SELECT TOP 1 CONCAT(ci_ip_address.address_line_1,  ' ', ci_ip_address.address_line_2,  ' ', ci_ip_address.address_line_3, ' ',  ci_ip_address.address_line_4,  ' ', ci_ip_address.address_line_5) FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no) AS insolvencyPractitionerAddress,
        (SELECT TOP 1 postcode FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no)  AS insolvencyPractitionerPostcode,
        (SELECT TOP 1 phone FROM ci_ip_address WHERE ci_ip_address.ip_no = insolvencyAppointment.ip_no) AS insolvencyPractitionerTelephone, 


        --  Insolvency Service contact details   
        insolvencyService.office_name  AS insolvencyServiceOffice,
        insolvencyService.register_contact  AS insolvencyServiceContact,
        CONCAT(insolvencyService.address_line_1,  ' ', insolvencyService.address_line_2,  ' ', insolvencyService.address_line_3,  ' ', insolvencyService.address_line_4,  ' ', insolvencyService.address_line_5) AS insolvencyServiceAddress,
        insolvencyService.postcode AS insolvencyServicePostcode,
        insolvencyService.phone AS insolvencyServicePhone


        FROM  ci_individual individual

        LEFT JOIN ci_case inscase ON individual.case_no = inscase.case_no
        LEFT JOIN ci_office insolvencyService ON inscase.office_id =  insolvencyService.office_id
        LEFT JOIN ci_ip_appt insolvencyAppointment ON insolvencyAppointment.case_no =  inscase.case_no
        LEFT JOIN ci_iva_case ivaCase ON ivaCase.case_no = inscase.case_no

        

       WHERE individual.case_no = @CaseNo AND individual.indiv_no = @IndivNo

       AND insolvencyAppointment.ip_appt_type = 'M'
    )



    SELECT  caseNo,
			indivNo,
			individualForenames,
            individualSurname,
            individualTitle,
            individualGender,
            individualDOB,
            individualOccupation,
            individualAddress,
            individualPostcode,
            individualAddressWithheld,
            individualAlias,
            caseName,
            courtName,
            insolvencyType,
            notificationDate,
            caseStatus,
            insolvencyDate,
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

    FROM getCase

GO


