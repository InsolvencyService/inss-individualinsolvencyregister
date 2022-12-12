Feature: ATU_13 Citizen - Report error (case data)  
	As a user
	I need the ability to report inaccurate data on a particular case
     So that INSS can check inaccuracies and update the relevant data where possible (via manual processes)


@ReportErrorPage @Regression
Scenario: ATU_13 Verify the Report an error page
Given I navigate to the Search results page by searching for "Adrian Adams"
When I click the individual link with postcode "PL20 7PE"
And I click the Report an error or issue link on the Case Details page
Then the Report an error page is displayed 


@ReportErrorPage @Regression
Scenario: ATU_13 Verify the Report an error page breadcrumbs - Journey from Start Page
Given I navigate to the Search results page by searching for "Adrian Adams"
When I click the individual link with postcode "PL20 7PE"
And I click the Report an error or issue link on the Case Details page
Then the breadcrumb text will be as expected on the Report an error page when the journey started from the Start page
When I click the Home breadcrumb on the Report an error page
Then the EIIR Start page will be displayed and the URL, page title and H1 will be as per requirements
Given I navigate to the Report an error page from the Start page by searching for "Adrian Adams" with Postcode "PL20 7PE"
And I click the Search the Individual Insolvency Register breadcrumb on the Report an error page
Then the Search page for EIIR will be displayed
Given I navigate to the Report an error page from the Start page by searching for "Adrian Adams" with Postcode "PL20 7PE"
And I click the Search results breadcrumb on the Report an error page
Then the URL, page title and page heading will be displayed for the Search results page
Given I navigate to the Report an error page from the Start page by searching for "Adrian Adams" with Postcode "PL20 7PE"
And I click the Case details breadcrumb on the Report an error page
Then the URL, page title and page heading will be displayed for the Case Details page for "Adrian Adams"
And the Individual case details are displayed


@ReportErrorPage @Regression
Scenario: ATU_13 Verify the Report an error page breadcrumbs - Journey from the View feedback Page
Given I login as an admin user and navigate to the Admin landing page
And I create case feedback data for this test
And I click the View feedback link
And I click the case name "Mark Wilkinson"
And I click the Report an error or issue link on the Case Details page
Then the breadcrumb text will be as expected on the Report an error page when the journey started from the Feedback page

@ReportErrorPage @Regression
Scenario: ATU_13 Verify the error messages for field validation on the Report error page
Given I navigate to the Search results page by searching for "Adrian Adams"
When I click the individual link with postcode "PL20 7PE"
And I click the Report an error or issue link on the Case Details page
When I don't select an organisation and I press Confirm and send
Then the user is shown the following error "Select an organisation"
When I don't enter a description and I press Confirm and send
Then the user is shown the following error "Enter an error or issue"
When I enter a description with invalid characters and I press Confirm and send
Then the user is shown the following error "Enter only letters, numbers, - , or '"
When I don't enter a Full name and I press Confirm and send
Then the user is shown the following error "Enter full name"
When I enter a Full name with invalid characters and I press Confirm and send
Then the user is shown the following error "Enter only letters, numbers, - , or '"
When I don't enter an email address and I press Confirm and send
Then the user is shown the following error "Enter an email address"
When I enter an invalid email address and I press Confirm and send
Then the user is shown the following error "Enter an email address in the correct format, like name@example.com"

@ReportErrorPage @Regression
Scenario: ATU_13 Submit an error report and validate against the database (journey from start page)
Given I navigate to the Search results page by searching for "Adrian Adams"
When I click the individual link with postcode "PL20 7PE"
And I click the Report an error or issue link on the Case Details page
And I fill in all of the fields with valid values and press Confirm and send
Then the Report an error record will be written to the database using case data from "Start page" journey

@ReportErrorPage @Regression
Scenario: ATU_13 Submit an error report and validate against the database (journey from feedback page)
Given I login as an admin user and navigate to the Admin landing page
And I create case feedback data for this test
And I click the View feedback link
And I click the case name "Mark Wilkinson"
And I click the Report an error or issue link on the Case Details page
And I fill in all of the fields with valid values and press Confirm and send
Then the Report an error record will be written to the database using case data from "Case Feedback page" journey

