Feature: Show_Lesson_Content
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
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
                {
                    "id": "Lesson02",
                    "TotalLikes": 5,
                    "LessonCatalogId": "LessonCatalog02"
                },
                {
                    "id": "Lesson03",
                    "TotalLikes": 0,
                    "LessonCatalogId": "LessonCatalog03"
                },
            ]
        }
    ]
    """  
    And System have LessonCatalog collection with JSON format are
    """  
    [
        {
            "id": "LessonCatalog01",
            "Title": "What Is Emotional Literacy?",
            "ShortTeacherLessonPlan": "short teacher lesson plan01",
            "FullTeacherLessonPlan": "full teacher lesson plan01",
			"PrimaryContentUrl": "PrimaryContent01",
        },
        {
            "id": "LessonCatalog02",
            "Title": "Intrinsic Education vs. Associative Learning",
            "ShortTeacherLessonPlan": "short teacher lesson plan02",
            "FullTeacherLessonPlan": "full teacher lesson plan02",
			"PrimaryContentUrl": "PrimaryContent02",
        },
        {
            "id": "LessonCatalog03",
            "Title": "Three: What Is Goodness?",
            "ShortTeacherLessonPlan": "short teacher lesson plan03",
            "FullTeacherLessonPlan": "full teacher lesson plan03",
			"PrimaryContentUrl": "PrimaryContent03",
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
                    "BeginDate": "2/1/2016",
                    "LessonCatalogId": "LessonCatalog01"
                },
                {
                    "Id": "LessonCalendar02",
                    "LessonId": "Lesson02",
                    "BeginDate": "2/8/2016",
                    "LessonCatalogId": "LessonCatalog02"
                },
                {
                    "Id": "LessonCalendar03",
                    "LessonId": "Lesson03",
                    "BeginDate": "2/15/2016",
                    "LessonCatalogId": "LessonCatalog03"
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
					"SawContentIds": [ ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",

					"TotalContentsAmount": 1,
					"SawContentIds": [ ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				}
			]
		}
    ]
    """  

@mock  
Scenario: User open a course's lesson Then system send the selected lesson's content back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' open the lesson 'Lesson02' of ClassRoom: 'ClassRoom01'
    Then System send lesson's content with JSON format is  
    """
    {
        "id": "Lesson02",
        "Title": "Intrinsic Education vs. Associative Learning",
        "ShortTeacherLessonPlan": "short teacher lesson plan02",
        "FullTeacherLessonPlan": "full teacher lesson plan02",
        "CourseMessage": "Don't forget to comment a lesson!",
        "TotalLikes": 5,
        "IsTeacher": true
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
				"SawContentIds": [ ],
				"CreatedCommentAmount": 0,
				"ParticipationAmount": 0
			},
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson02",

				"TotalContentsAmount": 1,
				"SawContentIds": 
				[
					"PrimaryContent02"
				],
				"CreatedCommentAmount": 0,
				"ParticipationAmount": 0
			}
		]
	}
    """  
	And System update last active class room is 'ClassRoom01' for user 'sakul@mindsage.com'