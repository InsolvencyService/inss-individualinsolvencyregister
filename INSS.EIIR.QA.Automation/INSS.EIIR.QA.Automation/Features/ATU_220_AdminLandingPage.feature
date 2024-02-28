Feature: ATU_220 Admin Landing Page
	As an Admin user
	I need to see a landing screen after I login
	So that I can access the different areas I need to manage 


Background: 
Given I login as an admin user and navigate to the Admin landing page

@AdminLandingPage @Regression
Scenario: ATU_220 Verify the Admin landing page URL, title and page heading
Then the Admin landing page will be displayed and the URL, page title and H1 will be as per requirements
