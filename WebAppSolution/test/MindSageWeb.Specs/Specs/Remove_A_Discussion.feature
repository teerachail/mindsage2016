Feature: Remove_A_Discussion
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
                    "LessonCatalogId": "LessonCatalog01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/1/2016",
                }
            ]
        },
    ]
    """  
    And System have Comment collection with JSON format are  
    """
    [
        {
            "Id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Hello lesson 1",
            "TotalLikes": 0,
            "LessonId": "Lesson01",
            "Discussions":
			[
				{
					"id": "Discussion01",
					"Description": "This is a discussion",
					"TotalLikes": 0,
					"CreatorImageUrl": "ImgURL01",
					"CreatorDisplayName": "Sakul jaruthanaset",
					"CreatedByUserProfileId": "sakul@mindsage.com",
				}
			]
        }
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
		}
    ]
    """  
    
@mock  
Scenario: User remove his comment Then system remove his comment  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'sakul@mindsage.com' remove the discussion 'Discussion01' from comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
	Then System update Discussion collection with JSON format in the Comment 'Comment01' are
    """
    [
		{
			"id": "Discussion01",
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "ImgURL01",
			"CreatorDisplayName": "Sakul jaruthanaset",
			"CreatedByUserProfileId": "sakul@mindsage.com",
			"DeletedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System doesn't update UserActivity  