Feature: Show_Course_Map_Content
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
            ]
        },
    ]
    """  

@mock  
Scenario: User request course map's content Then system send the content back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'sakul@mindsage.com' reuqest course map's content of ClassRoom: 'ClassRoom01'  
    Then System add new Comment by JSON format is  
    """
    {
        "ClassRoomId": "ClassRoom01",
        "CreatedByUserProfileId": "sakul@mindsage.com",
		"CreatorDisplayName": "Sakul jaruthanaset",
		"CreatorImageUrl": "ImgURL01",
        "Description": "Hello lesson 1",
        "TotalLikes": 0,
        "LessonId": "Lesson01",
        "Discussions": [],
		"CreatedDate": "2/8/2016 00:00 am"
    }
    """
    And System update UserActivity collection with JSON format is
    """
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
			}
		]
	}
    """  