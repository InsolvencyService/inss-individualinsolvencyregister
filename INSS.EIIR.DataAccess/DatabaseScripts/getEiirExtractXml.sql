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
	InsolvencyType varchar(1),
PRIMARY KEY NONCLUSTERED 
(
	[CaseNo] ASC,
	[IndivNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

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
	INNER JOIN #Cases c on case_id = c.CaseNo AND subj_refno = c.IndivNo
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
	RevokedDate datetime,
PRIMARY KEY NONCLUSTERED 
(
	[caseNo] ASC,
	[indivNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

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

--Beginning of Unicode Handling

--There are multiple columns varchar columns (windows-1252) which have had characters inserted using Unicode encoding, resulting in odd character output.  Fields include:
--ci_individual.forenames
--ci_individual.surname
--ci_case_desc.case_desc_line
--ci_other_name.forenames
--ci_other_name.surname

--This section resolves those discrepancies for each affected field table
--	Reading affected columns into unicode enabled column in a temp table using binary
--  Detecting invalid unicode characters '?' -> characters in range 128-255 range and replacing with unicode equivalent
--  Pulling fields from unicode columns rather than windows-1252 varchar columns

--Variable declarations for text replacement
	DECLARE @Idx INT;
	DECLARE @EndIdx INT;
	DECLARE @StrLen INT;
	DECLARE @ReverseStr VARCHAR(200); 
	DECLARE @SrcChr CHAR(1);
	DECLARE @NewChr CHAR(1); 

--Stores values for what unicode should be used
CREATE TABLE #TempUniCodeLookUp
(
	chr CHAR(1) COLLATE Latin1_General_CS_AS NOT NULL PRIMARY KEY,
	newChr NCHAR(1) COLLATE Latin1_General_100_CI_AI_SC_UTF8
)

--Unicode Lookup values for Windows-1252 between 0x80 (128) and 0xFF (255)
INSERT INTO #TempUnicodeLookUp (chr, newChr) VALUES
	(CHAR(128), NCHAR(8364)),
	(CHAR(130), NCHAR(8218)),
	(CHAR(131), NCHAR(402)),
	(CHAR(132), NCHAR(8222)),
	(CHAR(133), NCHAR(8230)),
	(CHAR(134), NCHAR(8224)),
	(CHAR(135), NCHAR(8225)),
	(CHAR(136), NCHAR(710)),
	(CHAR(137), NCHAR(8240)),
	(CHAR(138), NCHAR(352)),
	(CHAR(139), NCHAR(8249)),
	(CHAR(140), NCHAR(338)),
	(CHAR(142), NCHAR(381)),
	(CHAR(145), NCHAR(8216)),
	(CHAR(146), NCHAR(8217)),
	(CHAR(147), NCHAR(8220)),
	(CHAR(148), NCHAR(8221)),
	(CHAR(149), NCHAR(8226)),
	(CHAR(150), NCHAR(8211)),
	(CHAR(151), NCHAR(8212)),
	(CHAR(152), NCHAR(732)),
	(CHAR(153), NCHAR(8482)),
	(CHAR(154), NCHAR(353)),
	(CHAR(155), NCHAR(8250)),
	(CHAR(156), NCHAR(339)),
	(CHAR(158), NCHAR(382)),
	(CHAR(159), NCHAR(376)),
	(CHAR(160), NCHAR(160)),
	(CHAR(161), NCHAR(161)),
	(CHAR(162), NCHAR(162)),
	(CHAR(163), NCHAR(163)),
	(CHAR(164), NCHAR(164)),
	(CHAR(165), NCHAR(165)),
	(CHAR(166), NCHAR(166)),
	(CHAR(167), NCHAR(167)),
	(CHAR(168), NCHAR(168)),
	(CHAR(169), NCHAR(169)),
	(CHAR(170), NCHAR(170)),
	(CHAR(171), NCHAR(171)),
	(CHAR(172), NCHAR(172)),
	(CHAR(173), NCHAR(173)),
	(CHAR(174), NCHAR(174)),
	(CHAR(175), NCHAR(175)),
	(CHAR(176), NCHAR(176)),
	(CHAR(177), NCHAR(177)),
	(CHAR(178), NCHAR(178)),
	(CHAR(179), NCHAR(179)),
	(CHAR(180), NCHAR(180)),
	(CHAR(181), NCHAR(181)),
	(CHAR(182), NCHAR(182)),
	(CHAR(183), NCHAR(183)),
	(CHAR(184), NCHAR(184)),
	(CHAR(185), NCHAR(185)),
	(CHAR(186), NCHAR(186)),
	(CHAR(187), NCHAR(187)),
	(CHAR(188), NCHAR(188)),
	(CHAR(189), NCHAR(189)),
	(CHAR(190), NCHAR(190)),
	(CHAR(191), NCHAR(191)),
	(CHAR(192), NCHAR(192)),
	(CHAR(193), NCHAR(193)),
	(CHAR(194), NCHAR(194)),
	(CHAR(195), NCHAR(195)),
	(CHAR(196), NCHAR(196)),
	(CHAR(197), NCHAR(197)),
	(CHAR(198), NCHAR(198)),
	(CHAR(199), NCHAR(199)),
	(CHAR(200), NCHAR(200)),
	(CHAR(201), NCHAR(201)),
	(CHAR(202), NCHAR(202)),
	(CHAR(203), NCHAR(203)),
	(CHAR(204), NCHAR(204)),
	(CHAR(205), NCHAR(205)),
	(CHAR(206), NCHAR(206)),
	(CHAR(207), NCHAR(207)),
	(CHAR(208), NCHAR(208)),
	(CHAR(209), NCHAR(209)),
	(CHAR(210), NCHAR(210)),
	(CHAR(211), NCHAR(211)),
	(CHAR(212), NCHAR(212)),
	(CHAR(213), NCHAR(213)),
	(CHAR(214), NCHAR(214)),
	(CHAR(215), NCHAR(215)),
	(CHAR(216), NCHAR(216)),
	(CHAR(217), NCHAR(217)),
	(CHAR(218), NCHAR(218)),
	(CHAR(219), NCHAR(219)),
	(CHAR(220), NCHAR(220)),
	(CHAR(221), NCHAR(221)),
	(CHAR(222), NCHAR(222)),
	(CHAR(223), NCHAR(223)),
	(CHAR(224), NCHAR(224)),
	(CHAR(225), NCHAR(225)),
	(CHAR(226), NCHAR(226)),
	(CHAR(227), NCHAR(227)),
	(CHAR(228), NCHAR(228)),
	(CHAR(229), NCHAR(229)),
	(CHAR(230), NCHAR(230)),
	(CHAR(231), NCHAR(231)),
	(CHAR(232), NCHAR(232)),
	(CHAR(233), NCHAR(233)),
	(CHAR(234), NCHAR(234)),
	(CHAR(235), NCHAR(235)),
	(CHAR(236), NCHAR(236)),
	(CHAR(237), NCHAR(237)),
	(CHAR(238), NCHAR(238)),
	(CHAR(239), NCHAR(239)),
	(CHAR(240), NCHAR(240)),
	(CHAR(241), NCHAR(241)),
	(CHAR(242), NCHAR(242)),
	(CHAR(243), NCHAR(243)),
	(CHAR(244), NCHAR(244)),
	(CHAR(245), NCHAR(245)),
	(CHAR(246), NCHAR(246)),
	(CHAR(247), NCHAR(247)),
	(CHAR(248), NCHAR(248)),
	(CHAR(249), NCHAR(249)),
	(CHAR(250), NCHAR(250)),
	(CHAR(251), NCHAR(251)),
	(CHAR(252), NCHAR(252)),
	(CHAR(253), NCHAR(253)),
	(CHAR(254), NCHAR(254)),
	(CHAR(255), NCHAR(255));



--*******************************************************************
-- ./IndividualDetails/Firstname
-- ./IndividualDetails/Surname
--*******************************************************************

--ci_individual affected columns
CREATE TABLE #TempIndividual
(
	[case_no] int NOT NULL,
	[indiv_no] int NOT NULL,
	[forenames] [varchar](100) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
	[surname] [varchar](100) COLLATE Latin1_General_100_CI_AI_SC_UTF8

PRIMARY KEY NONCLUSTERED 
(
	[case_no] ASC,
	[indiv_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


--This converts the case_desc_line into properly UTF8 encoded field from using binary from source
--The magic CHAR(74) "J" - overcomes issue when last char in sourece is in unicode continutation code range of 0x80 -> 0x9F
--See word document linked/attached to APP-4944, References 4944/20 & 4944/21
INSERT INTO #TempIndividual (case_no, indiv_no, forenames, surname) 
SELECT DISTINCT i.case_no, i.indiv_no, CONVERT(varbinary(200),i.forenames + CHAR(74)), CONVERT(varbinary(200),i.surname + CHAR(74))
FROM ci_individual i
INNER JOIN eiirSnapshotTABLE s on i.case_no = s.CaseNo AND i.indiv_no = s.IndivNo 

--Use cursor to go over differences and replace any ? characters 
DECLARE @ncase_no INT,@nindiv_no INT,
    @trg_forenames VARCHAR(100), @src_forenames VARCHAR(100), @trg_surname VARCHAR(100), @src_surname VARCHAR(100);

DECLARE diff_cursor CURSOR FOR
SELECT t.case_no, t.indiv_no, CAST(t.forenames as VARCHAR(100)) COLLATE Latin1_General_CI_AS, i.forenames, CAST(t.surname as VARCHAR(100)) COLLATE Latin1_General_CI_AS, i.surname
FROM ci_individual i 
INNER JOIN #TempIndividual t ON i.case_no = t.case_no AND i.indiv_no = t.indiv_no
WHERE CAST(t.forenames as VARCHAR(200)) COLLATE Latin1_General_CI_AS != i.forenames OR CAST(t.surname as VARCHAR(200)) COLLATE Latin1_General_CI_AS != i.surname

OPEN diff_cursor

FETCH NEXT FROM diff_cursor
INTO @ncase_no, @nindiv_no, @trg_forenames, @src_forenames, @trg_surname, @src_surname 

WHILE @@FETCH_STATUS = 0
BEGIN
	--Find any bad "?" characters in the converted text, working backwards from end of string, replace with corect unicode char
	--ForeNames
	SET @Idx = 0;
	SET @EndIdx = 0;
	SET @StrLen = LEN(@trg_forenames);
	SET @ReverseStr = REVERSE(@trg_forenames);

	SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx)

	WHILE @EndIdx != 0
	BEGIN 
		SET @Idx = @StrLen - @EndIdx + 1;

		SELECT @SrcChr = SUBSTRING(@src_forenames, @Idx, 1);

		IF @SrcChr != CHAR(63) 
		BEGIN
			UPDATE #TempIndividual SET forenames = STUFF(t.forenames, @Idx, 1, c.newChr)
				FROM #TempIndividual t
				JOIN #TempUniCodeLookUp c ON c.chr = @SrcChr
			WHERE t.case_no = @ncase_no AND t.indiv_no = @nindiv_no;
		END

		SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx + 1)
	END 

	--Surname
	SET @Idx = 0;
	SET @EndIdx = 0;
	SET @StrLen = LEN(@trg_surname);
	SET @ReverseStr = REVERSE(@trg_surname);

	SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx)

	WHILE @EndIdx != 0
	BEGIN 
		SET @Idx = @StrLen - @EndIdx + 1;

		SELECT @SrcChr = SUBSTRING(@src_surname, @Idx, 1);

		IF @SrcChr != CHAR(63) 
		BEGIN
			UPDATE #TempIndividual SET surname = STUFF(t.surname, @Idx, 1, c.newChr)
				FROM #TempIndividual t
				JOIN #TempUniCodeLookUp c ON c.chr = @SrcChr
			WHERE t.case_no = @ncase_no AND t.indiv_no = @nindiv_no;
		END

		SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx + 1)
	END


    FETCH NEXT FROM diff_cursor
    INTO @ncase_no, @nindiv_no, @trg_forenames, @src_forenames, @trg_surname, @src_surname
END
CLOSE diff_cursor;
DEALLOCATE diff_cursor;

--Drop the magic "J" - needs to be left in until after the cursor as CAST to VARCHAR in cursor SELECT strips it out resulting in ? not being replaced
UPDATE #TempIndividual SET forenames = LEFT(forenames, LEN(forenames)-1), surname = LEFT(surname, LEN(surname)-1) 

--*******************************************************************
-- ./Othernames/Othername
--*******************************************************************

CREATE TABLE #TempOtherName
(
	[case_no] int NOT NULL,
	[indiv_no] int NOT NULL,
	[alias_no] int NOT NULL,
	[forenames] [varchar](100) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
	[surname] [varchar](100) COLLATE Latin1_General_100_CI_AI_SC_UTF8

PRIMARY KEY NONCLUSTERED 
(
	[case_no] ASC,
	[indiv_no] ASC,
	[alias_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


--This converts the case_desc_line into properly UTF8 encoded field from using binary from source
--The magic CHAR(74) "J" - overcomes issue when last char in sourece is in unicode continutation code range of 0x80 -> 0x9F
--See word document linked/attached to APP-4944, References 4944/20 & 4944/21
INSERT INTO #TempOtherName (case_no, indiv_no, alias_no, forenames, surname) 
SELECT DISTINCT i.case_no, i.indiv_no, i.alias_no, CONVERT(varbinary(200),i.forenames + CHAR(74)), CONVERT(varbinary(200),i.surname + CHAR(74))
FROM ci_other_name i
INNER JOIN eiirSnapshotTABLE s on i.case_no = s.CaseNo AND i.indiv_no = s.IndivNo
WHERE COALESCE(s.Type, 'N') != 'I'
ORDER BY i.case_no ASC, i.indiv_no ASC, i.alias_no ASC

--Use cursor to go over differences and replace any ? characters 
DECLARE @ocase_no INT,@oindiv_no INT, @oalias_no INT,
    @trg_oforenames VARCHAR(100), @src_oforenames VARCHAR(100), @trg_osurname VARCHAR(100), @src_osurname VARCHAR(100);

DECLARE diff_cursor CURSOR FOR
SELECT t.case_no, t.indiv_no, t.alias_no, CAST(t.forenames as VARCHAR(100)) COLLATE Latin1_General_CI_AS, i.forenames, CAST(t.surname as VARCHAR(100)) COLLATE Latin1_General_CI_AS, i.surname
FROM ci_other_name i 
INNER JOIN #TempOtherName t ON i.case_no = t.case_no AND i.indiv_no = t.indiv_no AND i.alias_no = t.alias_no
WHERE CAST(t.forenames as VARCHAR(200)) COLLATE Latin1_General_CI_AS != i.forenames OR CAST(t.surname as VARCHAR(200)) COLLATE Latin1_General_CI_AS != i.surname

OPEN diff_cursor

FETCH NEXT FROM diff_cursor
INTO @ocase_no, @oindiv_no, @oalias_no, @trg_oforenames, @src_oforenames, @trg_osurname, @src_osurname 

WHILE @@FETCH_STATUS = 0
BEGIN
	--Find any bad "?" characters in the converted text, working backwards from end of string, replace with corect unicode char
	--ForeNames
	SET @Idx = 0;
	SET @EndIdx = 0;
	SET @StrLen = LEN(@trg_oforenames);
	SET @ReverseStr = REVERSE(@trg_oforenames);

	SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx)

	WHILE @EndIdx != 0
	BEGIN 
		SET @Idx = @StrLen - @EndIdx + 1;

		SELECT @SrcChr = SUBSTRING(@src_oforenames, @Idx, 1);

		IF @SrcChr != CHAR(63) 
		BEGIN
			UPDATE #TempOtherName SET forenames = STUFF(t.forenames, @Idx, 1, c.newChr)
				FROM #TempOtherName t
				JOIN #TempUniCodeLookUp c ON c.chr = @SrcChr
			WHERE t.case_no = @ocase_no AND t.indiv_no = @oindiv_no AND t.alias_no = @oalias_no;
		END

		SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx + 1)
	END 

	--Surname
	SET @Idx = 0;
	SET @EndIdx = 0;
	SET @StrLen = LEN(@trg_osurname);
	SET @ReverseStr = REVERSE(@trg_osurname);

	SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx)

	WHILE @EndIdx != 0
	BEGIN 
		SET @Idx = @StrLen - @EndIdx + 1;

		SELECT @SrcChr = SUBSTRING(@src_osurname, @Idx, 1);

		IF @SrcChr != CHAR(63) 
		BEGIN
			UPDATE #TempOtherName SET surname = STUFF(t.surname, @Idx, 1, c.newChr)
				FROM #TempOtherName t
				JOIN #TempUniCodeLookUp c ON c.chr = @SrcChr
			WHERE t.case_no = @ocase_no AND t.indiv_no = @oindiv_no AND t.alias_no = @oalias_no;
		END

		SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx + 1)
	END


    FETCH NEXT FROM diff_cursor
    INTO @ocase_no, @oindiv_no, @oalias_no, @trg_oforenames, @src_oforenames, @trg_osurname, @src_osurname
END
CLOSE diff_cursor;
DEALLOCATE diff_cursor;

--Drop the magic "J" - needs to be left in until after the cursor as CAST to VARCHAR in cursor SELECT strips it out resulting in ? not being replaced
UPDATE #TempOtherName SET forenames = LEFT(forenames, LEN(forenames)-1), surname = LEFT(surname, LEN(surname)-1)


--*******************************************************************
-- ./CaseDetails/CaseName
--*******************************************************************

CREATE TABLE #TempCase
(
	[case_no] int NOT NULL,
	[case_name] [varchar](200) COLLATE Latin1_General_100_CI_AI_SC_UTF8
PRIMARY KEY NONCLUSTERED 
(
	[case_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


--This converts the case_desc_line into properly UTF8 encoded field from using binary from source
--The magic CHAR(74) "J" - overcomes issue when last char in sourece is in unicode continutation code range of 0x80 -> 0x9F
--See word document linked/attached to APP-4944, References 4944/20 & 4944/21
INSERT INTO #TempCase (case_no, case_name) 
SELECT DISTINCT c.case_no, CONVERT(varbinary(200),c.case_name + CHAR(74))
FROM ci_case c
INNER JOIN eiirSnapshotTABLE s on c.case_no = s.CaseNo 

--Use cursor to go over differences and replace any ? characters 
DECLARE @ccase_no INT,
    @trg_casename VARCHAR(200), @src_casename VARCHAR(200);

DECLARE diff_cursor CURSOR FOR
SELECT t.case_no, CAST(t.case_name as VARCHAR(200)) COLLATE Latin1_General_CI_AS, c.case_name 
FROM #TempCase t
INNER JOIN  ci_case c ON t.case_no = c.case_no
WHERE CAST(t.case_name as VARCHAR(200)) COLLATE Latin1_General_CI_AS != c.case_name 

OPEN diff_cursor

FETCH NEXT FROM diff_cursor
INTO @ccase_no, @trg_casename, @src_casename

WHILE @@FETCH_STATUS = 0
BEGIN
	--Find any bad "?" characters in the converted text, working backwards from end of string, replace with corect unicode char
	--ForeNames
	SET @Idx = 0;
	SET @EndIdx = 0;
	SET @StrLen = LEN(@trg_casename);
	SET @ReverseStr = REVERSE(@trg_casename);

	SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx)

	WHILE @EndIdx != 0
	BEGIN 
		SET @Idx = @StrLen - @EndIdx + 1;

		SELECT @SrcChr = SUBSTRING(@src_casename, @Idx, 1);

		IF @SrcChr != CHAR(63) 
		BEGIN
			UPDATE #TempCase SET case_name = STUFF(t.case_name, @Idx, 1, c.newChr)
				FROM #TempCase t
				JOIN #TempUniCodeLookUp c ON c.chr = @SrcChr
			WHERE t.case_no = @ccase_no 
		END

		SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx + 1)
	END 

    FETCH NEXT FROM diff_cursor
    INTO @ccase_no, @trg_casename, @src_casename
END
CLOSE diff_cursor;
DEALLOCATE diff_cursor;

--Drop the magic "J" - needs to be left in until after the cursor as CAST to VARCHAR in cursor SELECT strips it out resulting in ? not being replaced
UPDATE #TempCase SET case_name = LEFT(case_name, LEN(case_name)-1)



--*******************************************************************
-- ./CaseDetails/CaseDescription
--*******************************************************************

--Table to take care of UTF8 codes in case description fields 
CREATE TABLE #TempCaseDesc 
(
	[case_no] [int] NOT NULL,
	[case_desc_no] [int] NOT NULL,
	[case_desc_line_no] [int] NOT NULL,
	[case_desc_line] [varchar](100) COLLATE Latin1_General_100_CI_AI_SC_UTF8 NOT NULL ,
PRIMARY KEY NONCLUSTERED 
(
	[case_no] ASC,
	[case_desc_no] ASC,
	[case_desc_line_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

--This converts the case_desc_line into properly UTF8 encoded field from using binary from source
--The magic CHAR(74) "J" - overcomes issue when last char in sourece is in unicode continutation code range of 0x80 -> 0x9F
--See word document linked/attached to APP-4944, Reference 4944/6
INSERT INTO #TempCaseDesc (case_no, case_desc_no, case_desc_line_no, case_desc_line) 
SELECT c.case_no, c.case_desc_no, c.case_desc_line_no, CONVERT(varbinary(200),c.case_desc_line + CHAR(74)) as case_desc_line
FROM ci_case_desc c
WHERE c.case_no IN (SELECT DISTINCT CaseNo FROM eiirSnapshotTABLE)



--Use cursor to go over differences and replace any ? characters 
DECLARE @case_no INT,@case_desc_no INT, @case_desc_line_no INT,
    @trg_case_desc_line VARCHAR(200), @src_case_desc_line VARCHAR(100);

DECLARE diff_cursor CURSOR FOR
SELECT t.case_no, t.case_desc_no, t.case_desc_line_no, CAST(t.case_desc_line as VARCHAR(200)) COLLATE Latin1_General_CI_AS, c.case_desc_line
FROM ci_case_desc c 
INNER JOIN #TempCaseDesc t ON c.case_no = t.case_no AND c.case_desc_no = t.case_desc_no AND c.case_desc_line_no = t.case_desc_line_no
WHERE CAST(t.case_desc_line as VARCHAR(200)) COLLATE Latin1_General_CI_AS != c.case_desc_line

OPEN diff_cursor

FETCH NEXT FROM diff_cursor
INTO @case_no, @case_desc_no, @case_desc_line_no, @trg_case_desc_line, @src_case_desc_line

WHILE @@FETCH_STATUS = 0
BEGIN
	--Find any bad "?" characters in the converted text, working backwards from end of string, replace with corect unicode char
	SET @Idx = 0;
	SET @EndIdx = 0;
	SET @StrLen = LEN(@trg_case_desc_line);
	SET @ReverseStr = REVERSE(@trg_case_desc_line);


	SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx)

	WHILE @EndIdx != 0
	BEGIN 
		SET @Idx = @StrLen - @EndIdx + 1;

		SELECT @SrcChr = SUBSTRING(@src_case_desc_line, @Idx, 1);

		IF @SrcChr != CHAR(63) 
		BEGIN
			UPDATE #TempCaseDesc SET case_desc_line = STUFF(t.case_desc_line, @Idx, 1, c.newChr)
				FROM #TempCaseDesc t
				JOIN #TempUniCodeLookUp c ON c.chr = @SrcChr
			WHERE t.case_no = @case_no AND t.case_desc_no = @case_desc_no AND t.case_desc_line_no = @case_desc_line_no;
		END

		SET @EndIdx = CHARINDEX(CHAR(63), @ReverseStr, @EndIdx + 1)
	END 

    FETCH NEXT FROM diff_cursor
    INTO @case_no, @case_desc_no, @case_desc_line_no, @trg_case_desc_line, @src_case_desc_line
END
CLOSE diff_cursor;
DEALLOCATE diff_cursor;

--Drop the magic "J" - needs to be left in until after the cursor as CAST to VARCHAR in cursor SELECT strips it out resulting in ? not being replaced
UPDATE #TempCaseDesc SET case_desc_line = LEFT(case_desc_line, LEN(case_desc_line)-1)

--End of Unicode Handling

CREATE TABLE #Temp
(
    caseNo int,
    indivNo int, 
	totalEntries int,
	totalBanks int,
	totalIVAs int,
	newBanks int,
	totalDros int,
    individualForenames varchar(255) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
    individualSurname varchar(255) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
    individualTitle varchar(255),
    individualGender varchar(255),
    individualDOB varchar(255),
    individualOccupation varchar(255),
    individualAddress varchar(255), 
    individualPostcode varchar(255),
    individualAddressWithheld varchar(255),
    individualAlias varchar(255) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
	deceasedDate datetime,
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
    caseName varchar(255) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
    courtName varchar(255),
	courtNumber varchar(255),
    caseYear varchar(255),
    insolvencyType varchar(255),
    notificationDate varchar(255),
    insolvencyDate varchar(255),
    caseStatus varchar(255),
	annulDate datetime,
	annulReason varchar(255),
    caseDescription varchar(max) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
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
PRIMARY KEY NONCLUSTERED 
(
	[caseNo] ASC,
	[indivNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

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
    ISNULL(NULLIF(UPPER(#TempIndividual.forenames),''), 'No Forenames Found') AS individualForenames, 
    ISNULL(NULLIF(UPPER(#TempIndividual.surname), ''), 'No Surname Found') AS individualSurname, 
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
            (SELECT CASE 
			   WHEN REPLACE(TRIM(CONCAT(individual.address_line_1,  ', ', individual.address_line_2,  ', ', individual.address_line_3,  ', ', individual.address_line_4,  ', ', individual.address_line_5)), ' ,', '') LIKE '%,'
			   THEN 
					LEFT(REPLACE(TRIM(CONCAT(individual.address_line_1, ', ', individual.address_line_2, ', ', individual.address_line_3, ', ', individual.address_line_4, ', ', individual.address_line_5)), ' ,', ''),
					LEN(REPLACE(TRIM(CONCAT(individual.address_line_1, ', ', individual.address_line_2, ', ', individual.address_line_3, ', ', individual.address_line_4, ', ', individual.address_line_5)), ' ,', ''))-1) 
               ELSE 
					REPLACE(TRIM(CONCAT(individual.address_line_1, ', ', individual.address_line_2, ', ', individual.address_line_3, ', ', individual.address_line_4, ', ', individual.address_line_5)), ' ,', '')
           END)             
    END AS individualAddress,

	CASE WHEN wflag = 'Y' 
		THEN '(Sorry - this Address has been withheld)'
		ELSE  ISNULL(CAST(individual.postcode AS VARCHAR(30)), 'No Last Known PostCode Found')
	END AS individualPostcode, 
    
	individual.address_withheld_flag AS individualAddressWithheld, 

	(SELECT 
		CASE WHEN 
		(SELECT STRING_AGG(UPPER(t.forenames) + ' ' + (UPPER(t.surname)), ', ') FROM #TempOtherName t  WHERE t.case_no = snap.CaseNo AND t.indiv_no = snap.IndivNo) IS NULL THEN 'No OtherNames Found'
		ELSE
		(SELECT STRING_AGG(UPPER(t.forenames) + ' ' + (UPPER(t.surname)), ', ') FROM #TempOtherName t  WHERE t.case_no = snap.CaseNo AND t.indiv_no = snap.IndivNo)
	END) AS individualAlias,    
	
	snap.Deceased AS deceasedDate,

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
    #TempCase.case_name AS caseName, 

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
        	
	CASE
		--DroEF.[Text] => DRO ExtendedFrom is additional text affecting approx 10 out of 40000 records where the MoratoriumPeriodEndingDate has been extend out past DateOfOrder + 12 months
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'D' AND cp.RevokedDate IS NULL AND cp.MoratoriumPeriodEndingDate > GETDATE()
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'DRO1') + ' will end on ' + FORMAT(cp.MoratoriumPeriodEndingDate, 'dd MMMM yyyy') + ' ' + ISNULL(DroEF.[Text], ''))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'D' AND cp.RevokedDate IS NULL AND cp.MoratoriumPeriodEndingDate <= GETDATE()
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'DRO1') + ' ended on ' + FORMAT(cp.MoratoriumPeriodEndingDate, 'dd MMMM yyyy') + ' ' + ISNULL(DroEF.[Text], ''))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'D' AND cp.RevokedDate IS NOT NULL 
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'DRO2') + ' on ' + FORMAT(cp.RevokedDate, 'dd MMMM yyyy') + ' ' + ISNULL(DroEF.[Text], ''))
		
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I' AND ivaCase.date_of_failure IS NOT NULL 
			THEN  TRIM((SELECT TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'F') + ' On ' + FORMAT(ivaCase.date_of_failure, 'dd MMMM yyyy'))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I' AND ivaCase.date_of_completion IS NOT NULL 
			THEN  TRIM((SELECT TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'C') + ' On ' + FORMAT(ivaCase.date_of_completion, 'dd MMMM yyyy'))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I' AND ivaCase. date_of_revocation IS NOT NULL 
			THEN  TRIM((SELECT TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'R') + ' On ' + FORMAT(ivaCase.date_of_revocation, 'dd MMMM yyyy'))

		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I' AND ivaCase.date_of_suspension IS NOT NULL AND ivaCase.date_suspension_lifted IS NULL
			THEN  TRIM((SELECT TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O5') + ' On ' + FORMAT(ivaCase.date_of_suspension, 'dd MMMM yyyy'))
		WHEN cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'I' AND ivaCase.date_of_suspension IS NOT NULL AND ivaCase.date_suspension_lifted IS NOT NULL
			THEN  TRIM((SELECT TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O6') + ' On ' + FORMAT(ivaCase.date_suspension_lifted, 'dd MMMM yyyy'))
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
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'D') + ' On ' + FORMAT(cp.dischargeDate, 'dd MMMM yyyy'))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = '' AND cp.dischargeDate > GETDATE())
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O2') + '  will be  ' + FORMAT(cp.dischargeDate, 'dd MMMM yyyy'))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND cp.dischargeDate IS NULL AND cp.dischargeType = 'A')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O3')) 
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND cp.dischargeDate IS NULL AND cp.dischargeType = 'G')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'O4')) 

		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'P')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A1') + ' On ' + FORMAT(AnnulmentDateCASE, 'dd MMMM yyyy'))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'V')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A2') + ' On ' + FORMAT(AnnulmentDateCASE, 'dd MMMM yyyy'))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'R')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A3') + ' On ' + FORMAT(AnnulmentDateCASE, 'dd MMMM yyyy'))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'A')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A4') + ' On ' + + FORMAT(AnnulmentDateCASE, 'dd MMMM yyyy'))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypePARTNER = 'P')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A1') + ' On ' + FORMAT(AnnulmentDatePARTNER, 'dd MMMM yyyy'))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'V')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A2') + ' On ' + FORMAT(AnnulmentDatePARTNER, 'dd MMMM yyyy'))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'R')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A3') + ' On ' + FORMAT(AnnulmentDatePARTNER, 'dd MMMM yyyy'))
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE = 'A')
			THEN TRIM((select TRIM(SelectionValue) from #StatusCodes WHERE SelectionCode = 'A4') + ' On ' + FORMAT(AnnulmentDatePARTNER, 'dd MMMM yyyy'))
	END AS caseStatus,

	CASE WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypeCASE <> '')
			THEN AnnulmentDateCASE
		WHEN (cp.BROPrintCaseDetails = 'Y' AND insolvency_type = 'B' AND AnnulmentTypePARTNER <> '')
			THEN AnnulmentDatePARTNER
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
		ELSE NULL
	END AS annulReason,

	CASE
		WHEN address_withheld_flag = 'Y' THEN '(Case Description withheld as Individual Address has been withheld)'
        WHEN insolvency_type = 'I' THEN '(Case Description does not apply to IVA)'
        ELSE 
		ISNULL((SELECT (STRING_AGG(TRIM(REPLACE(REPLACE(REPLACE(#TempCaseDesc.case_desc_line,CHAR(10),' '),CHAR(13),' '),CHAR(9),' ')), ''))			
			FROM #TempCaseDesc
			WHERE #TempCaseDesc.case_no = snap.CaseNo
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
					ELSE REPLACE(TRIM(CONCAT(ci_trade.address_line_1,  ', ', ci_trade.address_line_2,  ', ', ci_trade.address_line_3,  ', ', ci_trade.address_line_4,  ', ', ci_trade.address_line_5, ', ', ci_trade.postcode)), ' ,', '')
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
	INNER JOIN #TempIndividual on #TempIndividual.indiv_no = snap.IndivNo and #TempIndividual.case_no = snap.CaseNo
	INNER JOIN extract_availability ea on ea.extract_id = Convert(CHAR(8),GETDATE(),112)
	LEFT JOIN #caseParams cp on cp.caseNo = snap.CaseNo AND cp.indivNo = snap.IndivNo
    LEFT JOIN ci_case inscase ON snap.CaseNo = inscase.case_no
	LEFT JOIN #TempCase ON snap.CaseNo = #TempCase.case_no
    LEFT JOIN ci_office insolvencyService ON inscase.office_id = insolvencyService.office_id
    LEFT JOIN ci_ip_appt insolvencyAppointment ON insolvencyAppointment.case_no = inscase.case_no AND insolvencyAppointment.appt_end_date IS NULL and insolvencyAppointment.ip_appt_type = 'M'
	LEFT JOIN ci_ip cip ON insolvencyAppointment.ip_no = cip.ip_no
	LEFT JOIN ci_ip_address cipa ON insolvencyAppointment.ip_no = cipa.ip_no
    LEFT JOIN ci_iva_case ivaCase ON ivaCase.case_no = inscase.case_no
	--Extended From for DRO CaseStatus applied as OUT APPLY due to complexity CASE statement affects in order of 10 records out of 40000
	OUTER APPLY (Select '(' + TRIM(('Extended From ' + FORMAT(DATEADD(month, 12, s1.DateOrder), 'dd/MM/yyyy HH:mm:ss') + ' To ' + CONVERT(CHAR(10),d1.MoratoriumPeriodEndingDate, 103))) + ')' as [Text]
						FROM
							#Cases s1
							LEFT JOIN #droDetails d1 ON s1.CaseNo = d1.CaseID AND s1.IndivNo = d1.CIDebtorSubjectNo
						WHERE
										s1.CaseNo = cases.CaseNo
										AND s1.IndivNo = cases.IndivNo
										AND s1.InsolvencyType = 'D'
										AND s1.DateOrder IS NOT NULL 
										AND d1.MoratoriumPeriodEndingDate IS NOT NULL
										AND  DateDiff(day, DATEADD(month, 12, s1.DateOrder), d1.MoratoriumPeriodEndingDate) > 1) as DroEF

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
		CONVERT(VARCHAR(10), deceasedDate, 103) AS DeceasedDate,
		CASE 
			WHEN TRIM(individualAddress) IS NULL THEN '@@@@@@@@@@'
			WHEN TRIM(individualAddress) = '' THEN '@@@@@@@@@@'
			ELSE TRIM(individualAddress)
		END AS LastKnownAddress,

		CASE 
			WHEN TRIM(individualPostcode) IS NULL THEN 'No Last Known PostCode Found'
			WHEN TRIM(individualPostcode) = '' THEN 'No Last Known PostCode Found'
			ELSE TRIM(individualPostcode)
		END AS LastKnownPostCode,

		CASE
			WHEN insolvencyType = 'Individual Voluntary Arrangement'
				THEN (SELECT 'No OtherNames Found' FOR XML PATH ('OtherNames'), TYPE)
			WHEN individualAlias = 'No OtherNames Found'
				THEN (SELECT individualAlias FOR XML PATH ('OtherNames'), TYPE)
			ELSE
				(SELECT TRIM(Value) FROM STRING_SPLIT(individualAlias, ',')
				FOR XML PATH ('OtherName'), root('OtherNames'), TYPE)
		END		
			FOR XML PATH ('IndividualDetails'), TYPE),

		(SELECT CASE 
			WHEN (t.insolvencyType = 'Bankruptcy' AND cp.BROPrintCaseDetails = 'N' AND (cp.hasBro = 'Y' OR cp.hasBru = 'Y' OR cp.hasiBro = 'Y'))
				THEN
				(SELECT restrictionsType AS RestrictionsType,
						restrictionsStartDate AS RestrictionsStartDate,
						restrictionsEndDate AS RestrictionsEndDate,
						courtName AS RestrictionsCourt,
						LTRIM(courtNumber,'0') AS RestrictionsCourtNo,
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
						LTRIM(courtNumber,'0') AS RestrictionsCourtNo,
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
					FORMAT(annulDate, 'dd/MM/yyyy HH:mm:ss') AS AnnulDate,
					annulReason AS AnnulReason,
					'Please note that this person is deceased (Deceased Date ' + CONVERT(VARCHAR(10), deceasedDate, 103) + ')' AS SpecialNote,
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
		inner join #caseParams cp on cp.caseNo = t.caseNo AND cp.indivNo = t.indivNo
		order by t.dateOrder
        FOR XML PATH ('ReportRequest'), TYPE, ELEMENTS))

	SET @outputWrapper = (SELECT @resultExtractXML, @resultDisclaimerXML, @resultXML FOR XML PATH ('ReportDetails'), TYPE, ELEMENTS)

	SELECT REPLACE(REPLACE(((SELECT '<?xml version=''1.0'' encoding=''utf-8''?>') + CAST(@outputWrapper as nvarchar(MAX))), '@@@@@@@@@@',''),'"','&quot;') AS 'Result'

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
If(OBJECT_ID('tempdb..#TempCaseDesc') Is Not Null)
Begin
	DROP TABLE #TempCaseDesc
End
If(OBJECT_ID('tempdb..#TempUnicodeLookUp') Is Not Null)
Begin
	DROP TABLE #TempUnicodeLookUp
End
If(OBJECT_ID('tempdb..#TempIndividual') Is Not Null)
Begin
	DROP TABLE #TempIndividual
End
If(OBJECT_ID('tempdb..#TempCase') Is Not Null)
Begin
	DROP TABLE #TempCase
End
If(OBJECT_ID('tempdb..#TempOtherName') Is Not Null)
Begin
	DROP TABLE #TempOtherName
End
END