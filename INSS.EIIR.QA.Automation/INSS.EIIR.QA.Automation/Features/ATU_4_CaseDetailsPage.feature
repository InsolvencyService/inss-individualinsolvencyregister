Feature: ATU_4 Citizen - View search (case) details screen
	As a user
	I need the EIIR search details screen updated in line with the new prototype screen
	So that I can view each EIIR search results record using this new screen design

@CitizenSearchResultsPage @Regression
Scenario: ATU_4 Verify the Case Details page
Given I navigate to the Search results page by searching for "Adrian Adams"
When I click the individual link with postcode "PL20 7PE"
Then the URL, page title and page heading will be displayed for the Case Details page for "Adrian Adams"
And the Individual case details are displayed


@CitizenSearchResultsPage @Regression
Scenario: ATU_4 Verify the Search results page breadcrumbs - Journey from Start page
Given I navigate to the Search results page by searching for "Adrian Adams"
When I click the individual link with postcode "PL20 7PE"
Then the breadcrumb text will be as expected on the Case Details page
When I click the Home breadcrumb on the Case Details page
Then the EIIR Start page will be displayed and the URL, page title and H1 will be as per requirements
Given I navigate to the Search results page by searching for "Adrian Adams" with Postcode "PL20 7PE"
And I click the Search the Individual Insolvency Register breadcrumb on the Case Details page
Then the Search page for EIIR will be displayed
Given I navigate to the Search results page by searching for "Adrian Adams" with Postcode "PL20 7PE"
And I click the Search results breadcrumb on the Case Details page
Then the URL, page title and page heading will be displayed for the Search results page

@CitizenSearchResultsPage @Regression
Scenario: ATU_4 Verify the Search results page breadcrumbs - Journey from Case Feedback page
Given I login as an admin user and navigate to the Admin landing page
And I create case feedback data for this test
And I click the View feedback link
And I click the case name "Mark Wilkinson"
##Then the breadcrumb text will be as expected on the Case Details page when coming to this page from the Case Feedback page


@CitizenSearchResultsPage @Regression
Scenario: ATU_4 Clicking the Start new search button takes the user to the Search page
Given I navigate to the Search results page by searching for "Adrian Adams"
When I click the individual link with postcode "PL20 7PE"
Given I click the Start new search button on the Case Details page
Then the Search page for EIIR will be displayed






