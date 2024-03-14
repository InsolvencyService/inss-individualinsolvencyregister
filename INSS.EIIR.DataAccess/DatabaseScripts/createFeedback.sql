IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'dbo' 
        AND TABLE_NAME = 'CI_Case_Feedback')
BEGIN

	CREATE TABLE [dbo].[CI_Case_Feedback](
		[FeedbackId] [int] IDENTITY(2022102601,1) NOT NULL,
		[FeedbackDate] [datetime] NOT NULL,
		[CaseId] [int] NOT NULL,
		[Message] [varchar](max) NOT NULL,
		[ReporterFullname] [varchar](255) NOT NULL,
		[ReporterEmailAddress] [varchar](100) NOT NULL,
		[ReporterOrganisation] [varchar](100) NOT NULL,
		[Viewed] [bit] NOT NULL,
		[ViewedDate] [datetime] NULL,
	 CONSTRAINT [PK_CI_Case_Feedback] PRIMARY KEY CLUSTERED 
	(
		[FeedbackId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[CI_Case_Feedback] ADD  DEFAULT ((0)) FOR [Viewed]

	ALTER TABLE [dbo].[CI_Case_Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_ci_case] FOREIGN KEY([CaseId])
	REFERENCES [dbo].[ci_case] ([case_no])

	ALTER TABLE [dbo].[CI_Case_Feedback] CHECK CONSTRAINT [FK_Feedback_ci_case]
END
