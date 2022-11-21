Feature: ATU_213 Admin - View subscriber details page

	As an Admin user
    I need to be able to view subscribers details in the updated EIIRAdmin service
    So that I can perform the required admin tasks 


Background: 
Given I login as an admin user and navigate to view subscriber list page
Then I am navigated to the Admin Subscriber List page
And the Active check box is selected by default
And I click the link for subscriber "Insolvency Service Internal Account"

@SubscriberDetails @Regression
Scenario: Verify the Subscriber details page URL, page title, header and the subscription ends in X days value
Then the Subscriber Details page is displayed with the expected URL, page title and header
And the warning for when the subscription will end is shown for subscriber "Insolvency Service Internal Account"

@SubscriberDetails @Regression
Scenario: Verify the Subscriber details page displays the correct details for a subscriber
Then the Subscriber Details page for "Insolvency Service Internal Account" is displayed with details for this subscriber
And the correct email addressess are displayed for the subscriber "Insolvency Service Internal Account"

@SubscriberDetails @Regression
Scenario: Click the Return to admin area button
When I click the Return to admin area button 
Then I am navigated to the Admin Dashboard page

@SubscriberDetails @Regression
Scenario: Click the View another subscriber link
When I click the View another subscriber link
Then I am navigated to the Admin Subscriber List page

@SubscriberDetails @Regression
Scenario: Verify the Subscribers breadcrumb navigation on the Subscriber details page
When I click the Subscribers breadcrumb on the Subscriber details page
Then I am navigated to the Admin Subscriber List page

@SubscriberDetails @Regression
Scenario: Verify the Home breadcrumb navigation on the Subscriber details page
When I click the Home breadcrumb on the Subscriber details page
Then I am navigated to the Admin Dashboard page