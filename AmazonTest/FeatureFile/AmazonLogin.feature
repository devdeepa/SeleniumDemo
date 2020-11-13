Feature: AmazonLogin
	In order to work 
	As a customer
	I want to Login

@Login
Scenario: Login in Amazon
	Given I go to "URL" on "<Browser>"
	When I click on "Signin_Xpath"
	And I enter "Username_Id" as "<Username>"
	When I click on "continue_Id"
	And I enter "Password_Id" as "<Password>"
	When I click on "submit_Id"
	Then login should be "<ExpectedResult>"

Examples:
| Browser | Username                  | Password   | ExpectedResult |
| Mozilla | deepakchopade14@gmail.com | Guru@12345 | Success        |
| Mozilla | deepakchopade14@gmail.com | Guru@11111 | Failure        |