Feature: ATU_81_Admin - Add new subscriber

	As an Admin user
	I need to be able to add new subscribers to the updated EIIRAdmin service
	So that I can perform the required admin tasks 


Background: 
Given I login as an admin user and navigate to the Admin landing page
Given I click the Add New Subscriber link

@AddSubscriberDetails @Regression
Scenario: ATU_81 Verify the breadcrumb navigation on the Add subscriber page1
When I click the Home breadcrumb on the Add new subscriber page
Then the Admin landing page will be displayed and the URL, page title and H1 will be as per requirements



@AddSubscriberDetails @Regression
Scenario: ATU_81 Verify the Add subscriber details page URL, page title1
Then the Add New Subscriber page is displayed with the expected URL, page title and header
##reactivate the line below once the defect for setting the status to active by default is resolved.
##And the status is set to Active by default

@AddSubscriberDetails @Regression
Scenario: ATU_81 Verify the error messages for the text entry fields on the add new subscriber page
Given I first populate all of the fields on the Add new subscriber page
When I clear the <TextField> field on the Add subscriber page
And I press the Save and return to subscriber button 
Then the user is shown the following error message <ErrorMessage>

Examples: 
| TextField              | ErrorMessage                                       | 
| "Organisation Name"    | "Enter the name of the company or organisation"    |
| "Forename"             | "Enter the first name"						      |
| "Surname"              | "Enter the last name"							  |
| "Address line 1"       | "Enter line 1 of the address"				      |
| "Town or city"         | "Enter the town or city"							  |
| "Postcode"             | "Enter the postcode"								  |
| "Email address"        | "Enter the contact email address"				  |
| "Telephone number"     | "Enter the telephone number"						  |
| "Email address 1"      | "Enter data extract email address 1"               |


@AddSubscriberDetails @Regression
Scenario: ATU_81 Verify the incorrect postcode format error message on the Add New Subscriber page
Given I first populate all of the fields on the Add new subscriber page
When I enter the following invalid <Postcode>
And I press the Save and continue button 
Then the user is shown the following error message <ErrorMessage> on Add new subscriber page

Examples: 
| TextField               | Postcode       | ErrorMessage									     	    | 
| "Postcode"              | "B20"		   | "Enter the postcode in the correct format"					|
| "Postcode"              | "B"            | "Enter the postcode in the correct format"					|
| "Postcode"              | "!!!"          | "Enter the postcode in the correct format"					|



@AddSubscriberDetails @Regression
Scenario: ATU_81 Verify the error messages for Application submitted date on the Add New Subscriber page
Given I first populate all of the fields on the Add new subscriber page
When I update the Add new subscriber page Application Date to have a blank Day, Month and Year 
And I press the Save and continue button 
Then the user is shown error messages stating the Day, Month and Year are missing on the Add new subscriber page
When I enter non numeric details in to the Application submitted date fields on the Add new subscriber page
And I press the Save and continue button 
Then the user is shown error messages stating which numeric values are acceptable in the application submitted date on the Add new subscriber page
When I enter an invalid date in to the on the Add new subscriber page Application submitted date fields such as "30-02-2025"
And I press the Save and continue button 
Then the user is shown error messages stating the Add new subscriber page, application start date entered must be a real date
When I enter an invalid year in to the Add new subscriber page, Application submitted date fields for example "1000"
And I press the Save and continue button 
Then the user is shown an error message stating "the application submitted date YEAR entered must be between 1900 and 3000" on the Add new subscriber page

@AddSubscriberDetails @Regression
Scenario: ATU_81 Add a new subscriber with between 1 and 3 data extract emails and verify the new subscriber details are reflected on the Subscriber Details page 
Given I enter new subscriber details in the text fields
And I enter the application date, start date and end date
And I enter <NumberOfEmailAddresses> email addresses
And I press the Save and continue button 
Then I am navigated to the Admin landing page
When I navigate to the Subscriber List page and I search for my new subscriber "New Updated Subscriber"  
Then the Subscriber Details page for "New Updated Subscriber" is displayed with details for this subscriber
And the correct email addressess are displayed for the subscriber "New Updated Subscriber"
And the new subscription for subscriber "New Updated Subscriber" is deleted in readiness for the next test run

Examples: 
| NumberOfEmailAddresses    | 
| 1							| 
| 2							| 
| 3							| 