Feature: ATU_333 EIIR Search results page aligned with consolidated index model
	EIIR Search results page aligned with consolidated index model

Background: 	
Given I navigate to the Search results page by searching for "Adrian Adams"


@CitizenSearchResultsPage @Regression
Scenario: ATU_333 Verify the Search results page
Then the URL, page title and page heading will be displayed for the Search results page


@CitizenSearchResultsPage @Regression
Scenario: ATU_333 Verify the Search results page breadcrumbs and links
Then the breadcrumb text will be as expected
And I click the Search the Individual Insolvency Register breadcrumb
Then the Search page for EIIR will be displayed
Given I navigate to the Search results page by searching for "Adrian Adams"
And I click the Home breadcrumb on the Search results page
Then the EIIR Start page will be displayed and the URL, page title and H1 will be as per requirements
Given I navigate to the Search results page by searching for "Adrian Adams"
And I click the Tell the Insolvency Service link
Then the following URL is displayed "https://www.insolvencydirect.bis.gov.uk/ExternalOnlineForms/GeneralEnquiry.aspx"


@CitizenSearchResultsPage @Regression
Scenario: ATU_333 
When I click the individual link with postcode "PL20 7PE"