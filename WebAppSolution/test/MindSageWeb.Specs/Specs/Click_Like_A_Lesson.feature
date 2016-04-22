Feature: Click-like-a-lesson
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
	And Initialize mocking notifications' repositories  
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
    And System have ClassRoom collection with JSON format are  
    """
    [
        {
            "id": "ClassRoom01",
            "Name": "Emotional literacy",
            "CourseCatalogId": "CourseCatalog01",
            "CreatedDate": "2/1/2016",
            "Lessons":
            [
                {
                    "id": "Lesson01",
                    "TotalLikes": 1,
                    "LessonCatalogId": "LessonCatalog01"
                },
                {
                    "id": "Lesson02",
                    "TotalLikes": 0,
                    "LessonCatalogId": "LessonCatalog02"
                },
            ]
        }
    ]
    """  
    And System have LikeLesson collection with JSON format are  
    """
    [
        {
            "id": "LikeLesson01",
            "ClassRoomId": "ClassRoom01",
            "LessonId": "Lesson01",
            "LikedByUserProfileId": "miolynet@perfenterprise.com",
            "CreatedDate": "2/1/2016",
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
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				}
			]
		}
    ]
    """  

@mock  
Scenario: User click like a lesson Then system update lesson's total like  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'sakul@mindsage.com' click the like button in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System update total likes in the lesson 'Lesson01' of ClassRoom 'ClassRoom01' to '2' likes   
    And System add new LikeLesson by JSON format is  
    """
    {
        "ClassRoomId": "ClassRoom01",
        "LessonId": "Lesson01",
        "LikedByUserProfileId": "sakul@mindsage.com",
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
				"CreatedCommentAmount": 0,
				"ParticipationAmount": 1
			}
		]
	}
    """  