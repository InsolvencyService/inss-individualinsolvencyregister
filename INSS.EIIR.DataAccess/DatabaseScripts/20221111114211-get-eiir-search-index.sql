SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

/****** Object:  Stored Procedure dbo.getSearchIndex    Script Date: 11/11/2022 11:42:11 ******/


CREATE OR ALTER PROCEDURE [dbo].[getEiirSearchIndex] AS


;WITH
    getEiirSearchIndex(
        caseNo,
        indivNo,
        individualForenames,
        individualSurname,
        individualTown,
        individualPostcode,
        individualAlias,
        companyName
    )
    AS
    (

        SELECT DISTINCT


            individual.case_no AS caseNo,
            individual.indiv_no AS indivNo,

            -- Individual details        
            UPPER(individual.forenames) AS individualForenames,
            UPPER(individual.surname) AS individualSurname,
            CONCAT(individual.address_line_1, ' ', individual.address_line_2, ' ', individual.address_line_3, ' ', individual.address_line_4, ' ', individual.address_line_5) AS individualTown,
            individual.postcode AS individualPostcode,
            ISNULL((SELECT STRING_AGG( ISNULL(UPPER(ci_other_name.surname) +' '+ UPPER(ci_other_name.forenames), ' '), ', ' )
            FROM ci_other_name
            WHERE ci_other_name.case_no = individual.case_no AND ci_other_name.indiv_no = individual.indiv_no), '') AS individualAlias,

            ISNULL(trade.trading_name, 'N/A') AS companyName


        FROM ci_individual individual

            LEFT JOIN ci_trade trade ON individual.case_no = trade.case_no

    )

SELECT 
    caseNo,
    indivNo,
    individualForenames,
    individualSurname,
    individualTown,
    individualPostcode,
    individualAlias,
    companyName

FROM getEiirSearchIndex

GO
