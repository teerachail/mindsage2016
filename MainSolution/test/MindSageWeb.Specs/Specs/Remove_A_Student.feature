Feature: Remove_A_Student
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "Sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				},
			]
		},
		{
			"id": "earn@mindsage.com",
			"Name": "Earn",
			"ImageProfileUrl": "earn.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription02",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				},
			]
		},
    ]
    """  
    
@mock  
Scenario: Teacher remove a student Then system remove the student from class room  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' remove student 'earn@mindsage.com' from class room 'ClassRoom01' 
    Then System upsert UserProfile with JSON format is  
    """
    {
		"id": "earn@mindsage.com",
		"Name": "Earn",
		"ImageProfileUrl": "earn.jpg",
		"Subscriptions":
		[
			{
				"id": "Subscription02",
				"Role": "Student",
				"ClassRoomId": "ClassRoom01",
				"ClassCalendarId": "ClassCalendar01",
				"DeletedDate": "2/8/2016 00:00 am"
			},
		]
	}
    """ 