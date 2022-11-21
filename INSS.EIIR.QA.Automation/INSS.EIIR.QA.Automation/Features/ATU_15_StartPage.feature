Feature: Citizen - Start Page
	As a user
	I need the EIIR sinitial start page updated in line with the new prototype screen [URL]
	So that I can search EIIR using this new screen design


Background: 
Given I navigate to the EIIR Start Page

@StartPage @Regression
Scenario: Verify the EIIR start page URL, title and page heading
Then the EIIR Start page will be displayed and the URL, page title and H1 will be as per requirements

@StartPage @Regression
Scenario: Verify the 'Start now' button takes the user to the Search screen
When I click the 'Start now' button
Then I am taken to the Search page

@StartPage @Regression
Scenario: Verify the navigation of the Terms and conditions footer link on the EIIR start page
When I click the "Terms and conditions - footer" link
Then the following URL is displayed "https://app-uksouth-sit-eiir-web.azurewebsites.net/home/terms-and-conditions"

@StartPage @Regression
Scenario: Verify the navigation of the Privacy footer link on the EIIR start page
When I click the "Privacy - footer" link
Then the following URL is displayed "https://app-uksouth-sit-eiir-web.azurewebsites.net/home/privacy"

@StartPage @Regression
Scenario: Verify the navigation of the Accessibility Statement footer link on the EIIR start page
When I click the "Accessibility statement - footer" link
Then the following URL is displayed "https://app-uksouth-sit-eiir-web.azurewebsites.net/home/accessibility-statement"

@StartPage @Regression
Scenario: Verify the navigation of the Terms and conditions hyperlink on the EIIR start page
When I click the "main content Terms and conditions" link
Then the following URL is displayed "https://app-uksouth-sit-eiir-web.azurewebsites.net/home/terms-and-conditions"

@StartPage @Regression
Scenario: Verify the navigation of the Insolvency Service hyperlink on the EIIR start page
When I click the "main content Insolvency Service" link
Then the following URL is displayed "https://www.gov.uk/government/organisations/insolvency-service"

@StartPage @Regression
Scenario: Verify the (Get help from the Insolvency Service) link in the Related content section on the EIIR start page
When I click the "Related content - Get help from the Insolvency Service" link
Then the following URL is displayed "https://www.gov.uk/get-help-insolvency-service"

@StartPage @Regression
Scenario: Verify the (Find out more about bankruptcy and insolvency) link in the Related content section on the EIIR start page
When I click the "Related content - Find out more about bankruptcy and insolvency" link
Then the following URL is displayed "https://www.gov.uk/browse/tax/court-claims-debt-bankruptcy"

@StartPage @Regression
Scenario: Verify the (Give feedback about the Individual Insolvency Register) link in the Related content section on the EIIR start page
When I click the "Related content - Give feedback about the Individual Insolvency Register" link
Then the following URL is displayed "https://www.insolvencydirect.bis.gov.uk/eiir/IIRFeedbackPage.asp"

