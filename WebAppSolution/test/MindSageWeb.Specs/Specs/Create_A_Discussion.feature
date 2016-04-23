Feature: Create_A_Discussion
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
			"id": "teacher@mindsage.com",
			"Name": "teacher",
			"ImageProfileUrl": "teacher.jpg",
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
			"id": "student@mindsage.com",
			"Name": "student",
			"ImageProfileUrl": "student.jpg",
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
			"id": "publicStudent@mindsage.com",
			"Name": "publicStudent",
			"ImageProfileUrl": "publicStudent.jpg",
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
			"id": "privateStudent@mindsage.com",
			"Name": "privateStudent",
			"ImageProfileUrl": "privateStudent.jpg",
			"IsPrivateAccount": true,
			"Subscriptions":
			[
				{
					"id": "Subscription04",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
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
                    "LessonCatalogId": "LessonCatalog01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/1/2016",
                },
				{
                    "Id": "LessonCalendar02",
                    "LessonId": "Lesson02",
                    "LessonCatalogId": "LessonCatalog02",
                    "Order": 2,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/16/2016",
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
            "CreatedByUserProfileId": "student@mindsage.com",
            "Description": "Student comment",
            "TotalLikes": 0,
            "LessonId": "Lesson01",
            "Discussions": []
        },
		{
            "Id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "publicStudent@mindsage.com",
            "Description": "Private student comment",
            "TotalLikes": 10,
            "LessonId": "Lesson01",
            "Discussions": []
        },
		{
            "Id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher comment",
            "TotalLikes": 100,
            "LessonId": "Lesson01",
            "Discussions": []
        },
		{
            "Id": "Comment04",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "privateStudent@mindsage.com",
            "Description": "Private student comment",
            "TotalLikes": 1000,
            "LessonId": "Lesson01",
            "Discussions": []
        }
    ]
    """ 
    And System have UserActivity collection with JSON format are
    """
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "teacher@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity01",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		},
		{
			"id": "UserActivity02",
			"UserProfileId": "student@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 1,
				}
			]
		},
		{
			"id": "UserActivity03",
			"UserProfileId": "publicStudent@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 0,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		},
		{
			"id": "UserActivity04",
			"UserProfileId": "privateStudent@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity04",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 0,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		},
    ]
    """  

@mock  
Scenario: A student create new discussion on its owned comment Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System update Discussion collection with JSON format in the Comment 'Comment01' are
    """
    [
		{
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "student.jpg",
			"CreatorDisplayName": "student",
			"CreatedByUserProfileId": "student@mindsage.com",
			"CreatedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity02",
		"UserProfileId": "student@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"LessonActivities":
		[
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson01",
				"SawContentIds":  [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			}
		]
	}
    """  

@mock  
Scenario: A student create new discussion on public-account's comment Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment02' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System update Discussion collection with JSON format in the Comment 'Comment02' are
    """
    [
		{
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "student.jpg",
			"CreatorDisplayName": "student",
			"CreatedByUserProfileId": "student@mindsage.com",
			"CreatedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity02",
		"UserProfileId": "student@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"LessonActivities":
		[
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson01",
				"SawContentIds":  [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			}
		]
	}
    """  

@mock  
Scenario: A student create new discussion on teacher's comment Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment03' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System update Discussion collection with JSON format in the Comment 'Comment03' are
    """
    [
		{
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "student.jpg",
			"CreatorDisplayName": "student",
			"CreatedByUserProfileId": "student@mindsage.com",
			"CreatedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity02",
		"UserProfileId": "student@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"LessonActivities":
		[
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson01",
				"SawContentIds":  [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			}
		]
	}
    """  

@mock  
Scenario: A student create new discussion on private-account's comment Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment04' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on future teacher's comment Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have Comment collection with JSON format are  
    """
    [
		{
            "Id": "FutureTeacherComment",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Feture teacher comment",
            "TotalLikes": 10000,
            "LessonId": "Lesson02",
            "Discussions": []
        }
    ]
    """ 
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'FutureTeacherComment' in the lesson 'Lesson02' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A teacher create new discussion on its owned comment Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment03' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System update Discussion collection with JSON format in the Comment 'Comment03' are
    """
    [
		{
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
			"CreatedByUserProfileId": "teacher@mindsage.com",
			"CreatedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity01",
		"UserProfileId": "teacher@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"LessonActivities":
		[
			{
				"id": "LessonActivity01",
				"LessonId": "Lesson01",
				"SawContentIds":  [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			}
		]
	}
    """  

@mock  
Scenario: A teacher create new discussion on future its owned comment Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
	And System have Comment collection with JSON format are  
    """
    [
		{
            "Id": "FutureTeacherComment",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Feture teacher comment",
            "TotalLikes": 10000,
            "LessonId": "Lesson02",
            "Discussions": []
        }
    ]
    """  
	And System have UserActivity collection with JSON format are
    """
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "teacher@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity01",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 0,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'FutureTeacherComment' in the lesson 'Lesson02' of ClassRoom: 'ClassRoom01'  
    Then System update Discussion collection with JSON format in the Comment 'FutureTeacherComment' are
    """
    [
		{
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
			"CreatedByUserProfileId": "teacher@mindsage.com",
			"CreatedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity01",
		"UserProfileId": "teacher@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"LessonActivities":
		[
			{
				"id": "LessonActivity01",
				"LessonId": "Lesson01",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 0,
				"CreatedCommentAmount": 0,
			},
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson02",
				"SawContentIds": [],
				"TotalContentsAmount": 0,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			}
		]
	}
    """  

@mock  
Scenario: A teacher create new discussion on student's comment (public account) Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment02' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System update Discussion collection with JSON format in the Comment 'Comment02' are
    """
    [
		{
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
			"CreatedByUserProfileId": "teacher@mindsage.com",
			"CreatedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity01",
		"UserProfileId": "teacher@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"LessonActivities":
		[
			{
				"id": "LessonActivity01",
				"LessonId": "Lesson01",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			}
		]
	}
    """  

@mock  
Scenario: A teacher create new discussion on student's comment (private account) Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment04' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System update Discussion collection with JSON format in the Comment 'Comment02' are
    """
    [
		{
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
			"CreatedByUserProfileId": "teacher@mindsage.com",
			"CreatedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity01",
		"UserProfileId": "teacher@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"LessonActivities":
		[
			{
				"id": "LessonActivity01",
				"LessonId": "Lesson01",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			}
		]
	}
    """  
	
@mock  
Scenario: Incorrect user (unknow) create new discussion Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'unknow@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: Incorrect user (empty) create new discussion Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId '' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: Incorrect user (null) create new discussion Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'NULL' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion but used ClassRoom incorrect (unknow) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'UnknowClassRoom'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion but used ClassRoom incorrect (empty) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: ''  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion but used ClassRoom incorrect (NULL) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'NULL'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion but used LessonId incorrect (unknow) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'UnknowLesson' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion but used LessonId incorrect (empty) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson '' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion but used LessonId incorrect (null) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'NULL' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion but used comment's message incorrect (empty) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is '' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion but used comment's message incorrect (null) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'NULL' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on its owned comment but its subscription was deleted Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "student@mindsage.com",
			"Name": "student",
			"ImageProfileUrl": "student.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription02",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				}
			]
		}
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on its owned comment but its subscription doesn't existing Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "student@mindsage.com",
			"Name": "student",
			"ImageProfileUrl": "student.jpg",
			"Subscriptions": []
		}
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on public-account's comment but that accout's subscription was deleted Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "student@mindsage.com",
			"Name": "student",
			"ImageProfileUrl": "student.jpg",
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
			"id": "publicStudent@mindsage.com",
			"Name": "publicStudent",
			"ImageProfileUrl": "publicStudent.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				},
			]
		}
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment02' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System update Discussion collection with JSON format in the Comment 'Comment02' are
    """
    [
		{
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "student.jpg",
			"CreatorDisplayName": "student",
			"CreatedByUserProfileId": "student@mindsage.com",
			"CreatedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity02",
		"UserProfileId": "student@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"LessonActivities":
		[
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson01",
				"SawContentIds":  [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			}
		]
	}
    """  

@mock  
Scenario: A student create new discussion on public-account's comment but that accout's subscription doesn't existing Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "student@mindsage.com",
			"Name": "student",
			"ImageProfileUrl": "student.jpg",
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
			"id": "publicStudent@mindsage.com",
			"Name": "publicStudent",
			"ImageProfileUrl": "publicStudent.jpg",
			"Subscriptions": []
		}
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment02' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System update Discussion collection with JSON format in the Comment 'Comment02' are
    """
    [
		{
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "student.jpg",
			"CreatorDisplayName": "student",
			"CreatedByUserProfileId": "student@mindsage.com",
			"CreatedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity02",
		"UserProfileId": "student@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"LessonActivities":
		[
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson01",
				"SawContentIds":  [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			}
		]
	}
    """  

@mock  
Scenario: A student create new discussion on public-account's comment but that accout was deleted Then system create new discussion and update user activity  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "student@mindsage.com",
			"Name": "student",
			"ImageProfileUrl": "student.jpg",
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
			"id": "publicStudent@mindsage.com",
			"Name": "publicStudent",
			"ImageProfileUrl": "publicStudent.jpg",
			"DeletedDate": "1/1/2016",
			"Subscriptions": []
		}
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment02' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System update Discussion collection with JSON format in the Comment 'Comment02' are
    """
    [
		{
			"Description": "This is a discussion",
			"TotalLikes": 0,
			"CreatorImageUrl": "student.jpg",
			"CreatorDisplayName": "student",
			"CreatedByUserProfileId": "student@mindsage.com",
			"CreatedDate": "2/8/2016 00:00 am"
		}
	]
    """  
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity02",
		"UserProfileId": "student@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"LessonActivities":
		[
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson01",
				"SawContentIds":  [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			}
		]
	}
    """  

@mock  
Scenario: A student create new discussion on public-account's comment but that accout doesn't existing Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "student@mindsage.com",
			"Name": "student",
			"ImageProfileUrl": "student.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription02",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				},
			]
		}
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment02' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on its owned comment but the LessonCalendar was deleted Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
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
					"DeletedDate": "1/1/2016"
                }
            ]
        },
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on its owned comment but the LessonCalendar doesn't existing Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars": []
        },
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on its owned comment but the ClassCalendar was deleted Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
            "ClassRoomId": "ClassRoom01",
			"DeletedDate": "1/1/2016",
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
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on its owned comment but the ClassCalendar doesn't existing Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on its owned comment but the comment was deleted Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "Id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "student@mindsage.com",
            "Description": "Student comment",
            "TotalLikes": 0,
            "LessonId": "Lesson01",
            "Discussions": [],
			"DeletedDate": "1/1/2016"
        }
    ]
    """ 
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on its owned comment but the comment doesn't existing Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have Comment collection with JSON format are  
    """
    []
    """ 
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on its owned comment but its UserActivity was deleted Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserActivity collection with JSON format are
    """
    [
		{
			"id": "UserActivity02",
			"UserProfileId": "student@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"DeletedDate": "1/1/2016",
			"LessonActivities":
			[
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 1,
				}
			]
		}
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  

@mock  
Scenario: A student create new discussion on its owned comment but its UserActivity doesn't existing Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserActivity collection with JSON format are
    """
    []
    """  
    When UserProfileId 'student@mindsage.com' create a new discussion with a message is 'This is a discussion' for comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't create new discussion  
    And System doesn't update UserActivity  