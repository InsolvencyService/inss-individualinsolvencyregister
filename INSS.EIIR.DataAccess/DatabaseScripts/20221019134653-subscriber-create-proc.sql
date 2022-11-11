
/****** Object:  StoredProcedure [dbo].[subscriber_create]    Script Date: 19/10/2022 13:48:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Stored Procedure dbo.subscr_appl_INS    Script Date: 15/06/2010 09:37:32 ******/


CREATE OR ALTER PROCEDURE [dbo].[subscriber_create]
(
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

set dateformat dmy
declare @hold_subscriber_id int

BEGIN

-- NOTE : subscriber_id on subscriber_application is an IDENTITY field with a seed being incremented 
--        by 1 starting from 2005112001 - and the generated subscriber_id from subscriber_application 
--        will be used to populate the associated subscriber_id column on subscriber_account ....

INSERT INTO subscriber_application
       (
       organisation_name, 
       organisation_type,
       application_ipaddress,  
       contact_title,
       contact_forename,
       contact_surname,
       contact_address1,
       contact_address2,
       contact_postcode,
       contact_city,
       contact_telephone,
       contact_email,
       organisation_website,
       contact_country,
       application_date,
       application_viewed,
       application_viewed_by,
       application_viewed_date,
       application_approved,
       approved_ipaddress
)
  VALUES
(
       @OrganisationName,
       @OrganisationType,
       '',
       '',
       @ContactForename,
       @ContactSurname,
       @ContactAddress1,
       @ContactAddress2,
       @ContactPostcode,
       @ContactCity,
       @ContactTelephone,
       @ContactEmail,
       '',
       '',
       @ApplicationDate,
       'N',
       '',
       NULL,
       @AccountActive,
       ''
)
       
select @hold_subscriber_id = @@identity

-- Now generate a random 8-character password for the Subscriber
     DECLARE @generated_password varchar(128)
     Set @generated_password = ''

     Declare @Loop int
     Declare @Chr int
     Set @Loop = 0
     While @Loop < 8
     BEGIN
          Select @Chr = Abs(Cast(NewID() As Binary(16)) % 36)
          If @Chr < 10
          BEGIN
               Set @generated_password = @generated_password + Cast (@Chr as char(1))
          END
          ELSE
          BEGIN
               Set @generated_password = @generated_password + Char(@Chr + 54)
          END
          Set @Loop = @Loop + 1
     END

INSERT INTO subscriber_account
(
    subscriber_id,
    organisation_name,
    subscriber_login,
    subscriber_password,
    account_active,
    authorised_by,
    authorised_date,
    authorised_ipaddress,
    subscribed_from,
    subscribed_to
)
VALUES
(
    @hold_subscriber_id,
    @OrganisationName,
    @hold_subscriber_id,
    @generated_password,
    @AccountActive,
    '',
    NULL,
    '',
    @SubscribedFrom,
    @SubscribedTo
);

WITH cteEmailContacts As
(
    SELECT
		@hold_subscriber_id as 'subscriber_id',
        value as 'email_address',
        GETDATE() as 'created_on'
    FROM
        STRING_SPLIT(@EmailContacts, ',')
)
INSERT INTO subscriber_contact  
SELECT subscriber_id, email_address, created_on from cteEmailContacts
       
END
GO


