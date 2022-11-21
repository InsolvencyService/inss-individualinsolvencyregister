Feature: ATU_82 Admin - View subscriber list

	As an Admin user 
    I need to be able to view subscribers in the updated EIIRAdmin service
    So that I can perform the required admin tasks 


Background: 
Given I login as an admin user and navigate to view subscriber list page
Then I am navigated to the Admin Subscriber List page
And the Active check box is selected by default

@SubscriberList
Scenario: Subscriber table is sorted correctly when active Subscribers are displayed
Then the table is sorted by subscription end date and displays "Active subscribers" only

@SubscriberList
Scenario: Subscriber table is sorted correctly when inactive Subscribers are displayed
When I check the Inactive check box only
Then the table is sorted by subscription end date and displays "Inactive subscribers" only

@SubscriberList
Scenario: Subscriber table is sorted correctly when all subscribers are displayed
When I check the Active and Inactive check boxes
Then the table is sorted by subscription end date and displays "All subscribers" only

@SubscriberList
Scenario: Verify the Home breadcrumb takes the user back to the Admin dahsboard
When I click the Home breadcrumb on the Subscriber List page
Then I am navigated to the Admin Dashboard page

