Feature: Update Terms and Conditions Page

	As a user
	I need the terms and conditions updated (currently located here: www.insolvencydirect.bis.gov.uk/eiir/disclaimer.asp)
	So that they reflect any changes recently made to the service, and also appear with a styling consistent to the rest of the service


Background: 
Given I navigate to the Terms and conditions page

@TermsAndConditions
Scenario: Verify the Terms and conditions page
	Then the Terms and conditions page is displayed

@TermsAndConditions
Scenario: Verify the breadcrumb navigation on the T&C page
	When I click the Home breadcrumb on the T&C page
	Then I am navigated to the Home page of the EIIR service

@TermsAndConditions
Scenario: Verify the navigation of the tell the Insolvency Service link
	When I click the tell the Insolvency Service link on the T&C page
	Then I am navigated to the General Enquiry page
