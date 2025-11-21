--Apply script

IF EXISTS (SELECT 1 FROM information_schema.table_constraints 
        WHERE table_schema='dbo' AND table_name='CI_Case_Feedback' AND 
        constraint_name='FK_Feedback_ci_case')
BEGIN
	ALTER TABLE dbo.CI_Case_Feedback DROP CONSTRAINT [FK_Feedback_ci_case]
END
GO


----Rollback script - Uncomment following section to apply
----THIS ROLLBACK SCRIPT WILL NOT WORK IF A FEEDBACK RECORD HAS BEEN CREATED WHICH WILL BREAK THE CONSTRAINT
----i.e. a CaseId exists in CI_Case_Feedback which does not exist in ci_case, to re-apply you would first need to 
----delete the offending records

--IF NOT EXISTS (SELECT 1 FROM information_schema.table_constraints 
--        WHERE table_schema='dbo' AND table_name='CI_Case_Feedback' AND 
--        constraint_name='FK_Feedback_ci_case')
--BEGIN
--	ALTER TABLE [dbo].[CI_Case_Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_ci_case] FOREIGN KEY([CaseId])
--	REFERENCES [dbo].[ci_case] ([case_no])

--	ALTER TABLE [dbo].[CI_Case_Feedback] CHECK CONSTRAINT [FK_Feedback_ci_case]
--END
--GO