Feature: Show_Friend_List
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
			"ExpiredDate": "2/1/2017",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "LessonId": "Lesson01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/1/2016",
                },
            ]
        },
    ]
    """  
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
		{
			"id": "joker@mindsage.com",
			"Name": "Joker",
			"ImageProfileUrl": "joker.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				},
			]
		},
		{
			"id": "perawatt@mindsage.com",
			"Name": "Perawatt",
			"ImageProfileUrl": "perawatt.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription04",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				},
			]
		},
		{
			"id": "bank@mindsage.com",
			"Name": "Bank",
			"ImageProfileUrl": "bank.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription05",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				},
			]
		},
    ]
    """  
    And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "earn@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016",
        },
		{
            "id": "FriendRequest02",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "joker@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016",
        },
		{
            "id": "FriendRequest03",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "perawatt@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
        },
		{
            "id": "FriendRequest04",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "bank@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016",
        },
    ]
    """  
    
@mock  
Scenario: User request friend list Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' 
    Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "bank@mindsage.com",
			"Name": "Bank",
			"ImageUrl": "bank.jpg",
			"Status": "Unfriend",
			"RequestId": "FriendRequest04",
			"IsTeacher": false
		},
		{
			"UserProfileId": "earn@mindsage.com",
			"Name": "Earn",
			"ImageUrl": "earn.jpg",
			"Status": "SendRequest",
			"RequestId": "FriendRequest01",
			"IsTeacher": false
		},
		{
			"UserProfileId": "joker@mindsage.com",
			"Name": "Joker",
			"ImageUrl": "joker.jpg",
			"Status": "ReceiveRequest",
			"RequestId": "FriendRequest02",
			"IsTeacher": false
		},
		{
			"UserProfileId": "perawatt@mindsage.com",
			"Name": "Perawatt",
			"ImageUrl": "perawatt.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest03",
			"IsTeacher": false
		},
    ]
    """ 