Feature: Teacher_Leave_Course
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
	And System have ClassRoom collection with JSON format are  
    """
    [
        {
            "id": "ClassRoom01",
            "Name": "Emotional literacy",
            "CourseCatalogId": "CourseCatalog01",
            "CreatedDate": "2/1/2016",
            "Message": "Don't forget to comment a lesson!",
            "Lessons":
            [
                {
                    "id": "Lesson01",
                    "TotalLikes": 0,
                    "LessonCatalogId": "LessonCatalog01"
                },
            ]
        }
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
                    "BeginDate": "2/1/2016",
                    "LessonCatalogId": "LessonCatalog01"
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
				}
			]
		},
		{
			"id": "UserActivity02",
			"UserProfileId": "earn@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson01",
					"TotalContentsAmount": 1,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				}
			]
		}
    ]
    """  

@mock  
Scenario: Teacher leave course Then system remove the course and their students' subscriptions  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' leave class room 'ClassRoom01' 
    Then System upsert UserProfile with JSON format is  
    """
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
				"DeletedDate": "2/8/2016 00:00 am"
			},
		]
	}
    """  
	And System upsert UserProfile with JSON format is  
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
	And System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "Emotional literacy",
        "CourseCatalogId": "CourseCatalog01",
        "CreatedDate": "2/1/2016",
        "Message": "Don't forget to comment a lesson!",
		"DeletedDate": "2/8/2016 00:00 am",
        "Lessons":
        [
            {
                "id": "Lesson01",
                "TotalLikes": 0,
                "LessonCatalogId": "LessonCatalog01"
            },
        ]
    }
    """  
	And System upsert ClassCalendar with JSON format is
    """
    {
        "id": "ClassCalendar01",
        "BeginDate": "2/1/2016",
        "ClassRoomId": "ClassRoom01",
		"DeletedDate": "2/8/2016 00:00 am",
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "LessonId": "Lesson01",
                "BeginDate": "2/1/2016",
                "LessonCatalogId": "LessonCatalog01"
            },
        ]
    }
    """  
	And System upsert UserActivity with JSON format is
    """
	{
		"id": "UserActivity01",
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"DeletedDate": "2/8/2016 00:00 am",
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
	And System upsert UserActivity with JSON format is
    """
	{
		"id": "UserActivity02",
		"UserProfileId": "earn@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"DeletedDate": "2/8/2016 00:00 am",
		"LessonActivities":
		[
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson01",
				"TotalContentsAmount": 1,
				"SawContentIds": [],
				"CreatedCommentAmount": 0,
				"ParticipationAmount": 0
			}
		]
	}
    """  