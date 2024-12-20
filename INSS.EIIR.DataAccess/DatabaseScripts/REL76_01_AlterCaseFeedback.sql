IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[CI_Case_FeedBack]') 
         AND name = 'InsolvencyType'
)
ALTER TABLE dbo.CI_Case_FeedBack 
	ADD InsolvencyType CHAR(1) NOT NULL DEFAULT 'x'
GO

IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[CI_Case_FeedBack]') 
         AND name = 'CaseName'
)
ALTER TABLE dbo.CI_Case_FeedBack 
	ADD CaseName varchar(96) NOT NULL DEFAULT 'Unknown'
GO

IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[CI_Case_FeedBack]') 
         AND name = 'InsolvencyDate'
)
ALTER TABLE dbo.CI_Case_FeedBack
	ADD InsolvencyDate datetime NULL
GO

IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[CI_Case_FeedBack]') 
         AND name = 'IndivNo'
)
ALTER TABLE dbo.CI_Case_FeedBack
	ADD
		IndivNo int NOT NULL DEFAULT -1
GO

UPDATE cf SET cf.InsolvencyType = c.insolvency_type, cf.CaseName = c.case_name, cf.InsolvencyDate = c.insolvency_date, cf.IndivNo = ci.indiv_no
	FROM CI_Case_Feedback cf 
		INNER JOIN ci_case c ON cf.CaseId = c.case_no
		INNER JOIN ci_individual ci ON cf.CaseId = ci.case_no
GO
