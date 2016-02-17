Feature: Show_Lesson_Comments
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
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "ClassCalendar01",
                    "LessonId": "Lesson01",
                    "BeginDate": "2/1/2016",
                    "LessonCatalogId": "LessonCatalog01"
                },
                {
                    "Id": "LC02",
                    "LessonId": "Lesson02",
                    "BeginDate": "2/8/2016",
                    "LessonCatalogId": "LessonCatalog02"
                },
                {
                    "Id": "LC03",
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
    And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "earn@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
        }
    ]
    """  
    And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg01",
            "TotalLikes": 0,
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg02",
            "TotalLikes": 5,
            "LessonId": "Lesson02",
			"CreatedDate": "2/1/2016 02:00 am",
            "Discussions":
            [
                {
                    "Id": "DiscussionId01",
                    "Description": "Discussion01",
                    "TotalLikes": 100,
                    "CreatedByUserProfileId": "sakul@mindsage.com",
					"CreatedDate": "2/1/2016 02:01 am",
                }
            ]
        },
        {
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "earn@mindsage.com",
            "Description": "Msg03",
            "TotalLikes": 10,
            "LessonId": "Lesson02",
			"CreatedDate": "2/1/2016 03:00 am",
            "Discussions":
            [
                {
                    "Id": "DiscussionId02",
                    "Description": "Discussion02",
                    "TotalLikes": 200,
                    "CreatedByUserProfileId": "someone@mindsage.com",
					"CreatedDate": "2/1/2016 03:01 am",
                },
                {
                    "Id": "DiscussionId03",
                    "Description": "Discussion03",
                    "TotalLikes": 300,
                    "CreatedByUserProfileId": "sakul@mindsage.com",
					"CreatedDate": "2/1/2016 03:02 am",
                }
            ]
        },
        {
            "id": "Comment04",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "someone@mindsage.com",
            "Description": "Msg04",
            "TotalLikes": 15,
            "LessonId": "Lesson02",
			"CreatedDate": "2/1/2016 04:00 am",
            "Discussions":
            [
                {
                    "Id": "DiscussionId04",
                    "Description": "Discussion04",
                    "TotalLikes": 400,
					"CreatedByUserProfileId": "someone@mindsage.com",
					"CreatedDate": "2/1/2016 04:01 am",
                }
            ]
        },
    ]
    """ 

@mock  
Scenario: User request lesson's comments and their discussions Then system send the lesson's comments and their discussions back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request comment & discussion from the lesson 'Lesson02' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment and their discussions with JSON format are  
    """
    [
		{
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "earn@mindsage.com",
            "Description": "Msg03",
            "TotalLikes": 10,
            "LessonId": "Lesson02",
			"CreatedDate": "2/1/2016 03:00 am",
            "TotalDiscussions": 2
        },
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg02",
            "TotalLikes": 5,
            "LessonId": "Lesson02",
			"CreatedDate": "2/1/2016 02:00 am",
            "TotalDiscussions": 1
        },
    ]
    """ 