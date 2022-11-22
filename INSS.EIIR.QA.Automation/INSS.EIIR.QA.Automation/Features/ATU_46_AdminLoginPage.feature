Feature: ATU_46 Admin login page
	As an Admin user
    I need to be able to login to the updated EIIRAdmin service 
    So that I can perform admin tasks 


Background: 
Given I navigate to the Admin Login page

@Admin @Regression
Scenario: ATU_46 Verify the Admin login page header, URL and page title
Then I am navigated Admin Login page

@Admin @Regression
Scenario: ATU_46 Click the Sign in button without entering a Username
When I enter a password only and press the Sign in button
Then I am shown the enter username error message 

@Admin @Regression
Scenario: ATU_46 Click the Sign in button without entering a password
When I enter a username only and press the Sign in button
Then I am shown the enter password error message 

@Admin @Regression
Scenario: ATU_46 Click the Sign in button without entering a Username or password
When I don't enter a password or username and press the Sign in button
Then I am shown the enter username and enter password error messages 

@Admin @Regression
Scenario: ATU_46 Click the Sign in button after entering an invalid Username and password
When I enter an invalid username and password combination and I press the Sign in button
Then I am shown the Your login details are incorrect error message 


