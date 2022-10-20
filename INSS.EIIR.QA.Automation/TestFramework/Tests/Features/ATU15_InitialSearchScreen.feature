Feature: Citizen - View initial search (start) screen

As a user
I need the EIIR search screen updated in line with the new prototype screen [URL]
So that I can search EIIR using this new screen design

@Regression
Scenario: Verify the Initial Search Screen for EIIR
	Given I navigate to the EIIR home page
	Then the header and page title will match the expected values
