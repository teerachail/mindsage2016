Feature: Show_Course_Map_Status
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
			"Name": "Sakul jaruthanaset",
			"ImageProfileUrl": "ImgURL01",
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
    ]
    """  
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
                {
                    "Id": "LessonCalendar02",
                    "LessonId": "Lesson02",
                    "Order": 2,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/8/2016",
                },
                {
                    "Id": "LessonCalendar03",
                    "LessonId": "Lesson03",
                    "Order": 3,
                    "SemesterGroupName": "B",
                    "BeginDate": "2/15/2016",
                },
				{
                    "Id": "LessonCalendar04",
                    "LessonId": "Lesson04",
                    "Order": 4,
                    "SemesterGroupName": "B",
                    "BeginDate": "2/22/2016",
					"DeletedDate": "1/1/2016",
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
			"UserProfileId": "sakul@mindsage.com",
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
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": 
					[
						"Content02"
					],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  

@mock  
Scenario: User request course map's status Then system send the status back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'sakul@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | true            | true                |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | false               |