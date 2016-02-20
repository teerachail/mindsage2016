Feature: Show_Student_List
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
	And System have UserActivity collection with JSON format are
    """
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "joker@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity01",
					"LessonId": "Lesson01",

					"TotalContentsAmount": 1,
					"SawContentIds": 
					[
						"Content01"
					],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 1
				}
			]
		},
		{
			"id": "UserActivity02",
			"UserProfileId": "bank@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson01",

					"TotalContentsAmount": 1,
					"SawContentIds": 
					[
						"Content01"
					],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson01",

					"TotalContentsAmount": 1,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		},
		{
			"id": "UserActivity03",
			"UserProfileId": "perawatt@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity04",
					"LessonId": "Lesson01",

					"TotalContentsAmount": 1,
					"SawContentIds": 
					[
						"Content01"
					],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				}
			]
		},
    ]
    """  
    
@mock  
Scenario: Teacher request student list Then system send student list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id                    | Name     | ImageUrl     | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |
	| bank@mindsage.com     | Bank     | bank.jpg     | 50                | 50                     | 50                            |
	| earn@mindsage.com     | Earn     | earn.jpg     | 0                 | 0                      | 0                             |
	| joker@mindsage.com    | Joker    | joker.jpg    | 100               | 100                    | 100                           |
	| perawatt@mindsage.com | Perawatt | perawatt.jpg | 0                 | 100                    | 0                             |
