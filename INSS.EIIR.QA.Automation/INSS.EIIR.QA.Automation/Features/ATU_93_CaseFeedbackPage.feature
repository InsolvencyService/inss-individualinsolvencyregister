Feature: ATU_93 Case Feedback Page
	As an Admin user 
	I need to be able to view detailed case error reports that have been submitted
	So that I can review errors and action it as required


Background: 
Given I login as an admin user and navigate to the Admin landing page
Then the Admin landing page will be displayed and the URL, page title and H1 will be as per requirements

@AdminCaseFeedback @Regression
Scenario: ATU_93 Verify the Case Feedback Page and bradcrumb links
Given I click the View feedback link
Then I am taken to the Admin - View case errors or issues page
When I click the home breadcrumb on the case feedback page
Then the Admin landing page will be displayed and the URL, page title and H1 will be as per requirements


@AdminCaseFeedback @Regression
Scenario: ATU_93 Verify the Case details are correctly displayed on the Case Feedback page
Given I create case feedback data for this test
Given I click the View feedback link
And I select the <Organisation> dropdown list in the organisation dropdown on the Case Feedback page
And I select the <Type> dropdpwn in the organisation dropdown on the Case Feedback page
Then the case feedback details are displayed <Organisation> <Type>

Examples: 
| Organisation                | Type			     | 
| "Other"				      | "IVAs"			     |
| "Debt recovery agency"      | "IVAs"			     |
| "Financial services"		  | "IVAs"		         |
| "Government department"     | "Bankruptcies"       |
| "Mortgage provider"		  | "Bankruptcies"       |
| "Bank or building society"  | "Bankruptcies"       |
| "Credit card issuer"		  | "Debt relief orders" |
| "Credit reference agency"   | "Debt relief orders" |
| "Member of the public"	  | "Debt relief orders" |



           


