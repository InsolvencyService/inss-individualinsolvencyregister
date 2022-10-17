IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'dbo' 
        AND TABLE_NAME = 'subscriber_contact')
BEGIN
    CREATE TABLE [dbo].[subscriber_contact] (
        subscriber_id VARCHAR(10) NOT NULL,
        email_address VARCHAR(60) NOT NULL,
        created_on DATETIME NOT NULL)
END
GO