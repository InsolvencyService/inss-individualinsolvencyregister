Feature: ATU_108_PrivacyPolicyPage

	As Application Services
    I need users to have access to the new privacy policy 
    So that this page can be used across a number of apps

Background: 
Given I navigate to the Privacy policy page


@TermsAndConditions @Regression
Scenario: ATU_108 Verify the Privacy policy page
	Then the Privacy policy page is displayed with the correct URL, page title and header

@TermsAndConditions @Regression
Scenario: ATU_108 Verify the breadcrumb navigation on the Privacy policy page
	Given I click the "Home" breadcrumb on the Privacy policy page
	Then I am navigated to the Home page of the EIIR service

@TermsAndConditions @Regression
Scenario: ATU_108 Verify the navigation of the 'personal information charter' link
	Given I click the "personal information charter" link on the Privacy policy page
	Then the following URL is displayed "https://www.gov.uk/government/organisations/insolvency-service/about/personal-information-charter"

@TermsAndConditions @Regression
Scenario: ATU_108 Verify the navigation of the 'cookies on GOV UK' link
	Given I click the "cookies on GOV UK" link on the Privacy policy page
	Then the following URL is displayed "https://www.gov.uk/help/cookies"
