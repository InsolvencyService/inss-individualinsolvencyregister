/****** Object:  StoredProcedure [dbo].[subscriber_update]    Script Date: 19/10/2022 13:48:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Stored Procedure dbo.subscr_appl_INS    Script Date: 15/06/2010 09:37:32 ******/


CREATE PROCEDURE [dbo].[subscriber_update]
(
	   @SubscriberId        varchar(10),
       @OrganisationName    varchar(50),
       @OrganisationType    varchar(40),
       @ContactForename     varchar(40),
       @ContactSurname      varchar(40),
       @ContactAddress1     varchar(60),
       @ContactAddress2     varchar(60),
       @ContactCity         varchar(60),
       @ContactPostcode     varchar(10),
       @ContactTelephone    varchar(20),
       @ContactEmail        varchar(60),
	   @ApplicationDate     datetime,
	   @SubscribedFrom      datetime,
	   @SubscribedTo        datetime,
	   @AccountActive       varchar(1),
	   @EmailContacts       varchar(max)
)
AS

SET dateformat dmy

BEGIN

-- NOTE : subscriber_id on subscriber_application is an IDENTITY field with a seed being incremented 
--        by 1 starting from 2005112001 - and the generated subscriber_id from subscriber_application 
--        will be used to populate the associated subscriber_id column on subscriber_account ....

UPDATE subscriber_application
SET
       organisation_name    = @OrganisationName, 
       organisation_type    = @OrganisationType,
       contact_forename     = @ContactForename,
       contact_surname      = @ContactSurname,
       contact_address1     = @ContactAddress1,
       contact_address2     = @ContactAddress2,
       contact_city         = @ContactCity,
       contact_postcode     = @ContactPostcode,
       contact_telephone    = @ContactTelephone,
       contact_email	    = @ContactEmail,
       application_approved = @AccountActive
WHERE
      subscriber_id = @SubscriberId
       

UPDATE subscriber_account
SET
       organisation_name   = @OrganisationName, 
       account_active      = @AccountActive,
       subscribed_from     = @SubscribedFrom,
       subscribed_to	   = @SubscribedTo
WHERE
      subscriber_id = @SubscriberId;

DECLARE @currentEmailContacts TABLE (subscriber_id varchar(10), email_address varchar(60), created_on datetime)
DECLARE @updateEmailContacts TABLE (subscriber_id varchar(10), email_address varchar(60), created_on datetime)

INSERT INTO @currentEmailContacts
	SELECT  subscriber_id, email_address, created_on
		FROM subscriber_contact
		WHERE subscriber_id = @SubscriberId

INSERT INTO @updateEmailContacts
    SELECT
		@SubscriberId as 'subscriber_id', 
		value as 'email_address', 
		GETDATE() as 'created_on'
    FROM
        STRING_SPLIT(@EmailContacts, ',')

DELETE FROM subscriber_contact 
WHERE email_address NOT IN (
	SELECT email_address FROM @updateEmailContacts
)

INSERT INTO subscriber_contact
	SELECT ec.subscriber_id, ec.email_address, ec.created_on
     FROM @updateEmailContacts ec
LEFT OUTER JOIN subscriber_contact sc ON sc.email_address = ec.email_address
WHERE sc.email_address is null

END
GO
