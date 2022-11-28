Feature: ATU_84_Update Subscriber Details Page

	As an Admin user
	I need to be able to update subscriber details in the updated EIIRAdmin service
	So that for example I can correct any details that may be incorrect 
	and add multiple email addresses for the subscriber organisation if needed


Background: 
Given I login as an admin user and navigate to view subscriber list page
Then I am navigated to the Admin Subscriber List page
And the Active check box is selected by default
And I click the link for subscriber "Insolvency Service Internal Account"


@UpdateSubscriberDetails @Regression
Scenario: ATU_84 Verify the breadcrumb navigation on the Update subscriber page
When I click on the Subscriber details change link for "Organisation Name"
And I click the Subscriber details breadcrumb on the Update subscriber details page
Then the Subscriber Details page for "Insolvency Service Internal Account" is displayed with details for this subscriber
Given I navigate back to the Update subscriber details page
And I click the Subscribers breadcrumb on the Update subscriber details page
Then I am navigated to the Admin Subscriber List page
Given I navigate back to the Update subscriber details page from the Subscriber List page
And I click the Home breadcrumb on the Update subscriber details page
Then the Admin landing page will be displayed and the URL, page title and H1 will be as per requirements


@UpdateSubscriberDetails @Regression
Scenario: ATU_84 Verify the Update subscriber details page URL, page title
When I click on the Subscriber details change link for "Organisation Name"
Then the Update Subscriber Details page is displayed with the expected URL, page title and header

@UpdateSubscriberDetails @Regression
Scenario: ATU_84 Verify the error messages for the text entry fields
When I click on the Subscriber details change link for "Organisation Name"
And I clear the <TextField> field
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


@UpdateSubscriberDetails @Regression
Scenario: ATU_84 Verify the incorretc postcode format error message
When I click on the Subscriber details change link for "Organisation Name"
And I clear the <TextField> field
And I enter the following invalid <Postcode>
And I press the Save and return to subscriber button 
Then the user is shown the following error message <ErrorMessage>

Examples: 
| TextField               | Postcode       | ErrorMessage									     	    | 
| "Postcode"              | "B20"		   | "Enter the postcode in the correct format"					|
| "Postcode"              | "B"            | "Enter the postcode in the correct format"					|
| "Postcode"              | "!!!"          | "Enter the postcode in the correct format"					|



@UpdateSubscriberDetails @Regression
Scenario: ATU_84 Verify the error messages for Application submitted date
When I click on the Subscriber details change link for "Organisation Name"
Then the Update Subscriber Details page is displayed with the expected URL, page title and header
And I update the Application Date to have a blank Day, Month and Year 
And I press the Save and return to subscriber button 
Then the user is shown error messages stating the Day, Month and Year are missing
When I enter non numeric details in to the Application submitted date fields
And I press the Save and return to subscriber button 
Then the user is shown error messages stating which numeric values are acceptable in the application submitted date
When I enter an invalid date in to the Application submitted date fields such as "30-02-2025"
And I press the Save and return to subscriber button 
Then the user is shown error messages stating the application start date entered must be a real date
When I enter an invalid year in to the Application submitted date fields for example "1000"
And I press the Save and return to subscriber button 
Then the user is shown error messages stating "the application submitted date YEAR entered must be between 1900 and 3000"

@UpdateSubscriberDetails @Regression
Scenario: ATU_84 Update the details for a subscriber with between 1 and 3 data extract emails and verify changes are reflected on the Subscriber Details page 
When I click on the Subscriber details change link for "Organisation Name"
And I update the subscriber details using the "Update values"
And I update <NumberOfEmailAddresses> email addresses on the update subscriber details
And I press the Save and return to subscriber button 
Then the Subscriber Details page for "Insolvency Service Internal Account" is displayed with the updated details for this subscriber
And the updated email addressess for <NumberOfEmailAddresses> are displayed for the subscriber
When I click on the Subscriber details change link for "Organisation Name"
And I update the subscriber details using the "Original values"

Examples: 
| NumberOfEmailAddresses    | 
| 1							| 
| 2							| 
| 3							| 


@UpdateSubscriberDetails @Regression
Scenario:  ATU_84 Verify the subscription start date must be before subscription end date
When I click on the Subscriber details change link for "Organisation Name"
And I populate the subscription start date to be later than the subscription end date on the update subscriber details page
And I press the Save and return to subscriber button 
Then the user is shown an error message stating the subscription end date must be later than the subscription start date on the update subscriber page

