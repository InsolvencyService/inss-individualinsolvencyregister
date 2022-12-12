Feature: ATU_175 Citizen - Search via single search box
	As a user
	I need the EIIR search screen updated in line with the new prototype screen
	So that I can search EIIR using this new screen design

Background: 	
Given I navigate to the Citizen Search page


@CitizenSearchPage @Regression
Scenario: ATU_175 Verify the Citizen Search page
Then the URL, page title and page heading will be as per the requirements

@CitizenSearchPage @Regression
Scenario: ATU_175 Verify error message when pressing search without entering a search term
When I click the Search button without entering any text
Then I am shown an error message stating a name or trading name must be entered 

@CitizenSearchPage @Regression
Scenario: ATU_175 Search using a term which doesn't return any results
When I enter a search term which returns no results
Then I am shown a message stating no results have been returned

@CitizenSearchPage @Regression
Scenario: ATU_175 Click the Home breadcrumb
When I click the Home breadcrumb on the Citizen Search page
Then the EIIR Start page will be displayed and the URL, page title and H1 will be as per requirements