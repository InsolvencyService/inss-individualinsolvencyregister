Feature: ATU_186 Update MVC with logout options
	As an Admin user
	I need the ability to logout of the service
	So that I can exit the service securely when I have completed my tasks 


Background: 
Given I login as an admin user and navigate to the Admin landing page
Then the Admin landing page will be displayed and the URL, page title and H1 will be as per requirements

@AdminLogout @Regression
Scenario: Verify the Admin pages cannot be accessed once the user signs out
Given I navigate to the update subscriber page and capture the URLs on the way so I can try and access them once logged out
When I click the Sign out link
Then I am navigated Admin Login page (use this temporarily until the capital A is removed from the URL
When I attempt to access the Admin landing page using the URL
Then I am navigated Admin Login page (use this temporarily until the capital A is removed from the URL
##Then I am navigated Admin Login page
When I attempt to access the subscriber list page using the URL
Then I am navigated Admin Login page (use this temporarily until the capital A is removed from the URL
##Then I am navigated Admin Login page
When I attempt to access the subscriber details page using the URL
Then I am navigated Admin Login page (use this temporarily until the capital A is removed from the URL
##Then I am navigated Admin Login page
When I attempt to access the update subscriber details page using the URL
##Then I am navigated Admin Login page
Then I am navigated Admin Login page (use this temporarily until the capital A is removed from the URL
