Feature: Create_A_Comment
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
    And System have UserActivity collection with JSON format are
	"""
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "teacher@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"UserProfileName": "teacher",
			"UserProfileImageUrl": "teacher.jpg",
			"LessonActivities":
			[
				{
					"id": "LessonActivity01",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				}
			]
		},
		{
			"id": "UserActivity02",
			"UserProfileId": "student@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"UserProfileName": "student",
			"UserProfileImageUrl": "student.jpg",
			"LessonActivities":
			[
				{
					"id": "LessonActivity04",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity05",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity06",
					"LessonId": "Lesson03",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				}
			]
		}
    ]
    """  
    
@mock  
Scenario: a student create new comment Then system create new comment and update user's activity
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System add new Comment by JSON format is  
    """
    {
        "ClassRoomId": "ClassRoom01",
        "CreatedByUserProfileId": "student@mindsage.com",
		"CreatorDisplayName": "student",
		"CreatorImageUrl": "student.jpg",
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
		"id": "UserActivity02",
		"UserProfileId": "student@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
		"UserProfileName": "student",
		"UserProfileImageUrl": "student.jpg",
		"LessonActivities":
		[
			{
				"id": "LessonActivity04",
				"LessonId": "Lesson01",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			},
			{
				"id": "LessonActivity05",
				"LessonId": "Lesson02",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			},
			{
				"id": "LessonActivity06",
				"LessonId": "Lesson03",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			}
		]
	}
    """  

@mock  
Scenario: a student create new comment in the unaccessable lesson Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson03' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a student create new comment but used incorrect message (empty) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is '' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a student create new comment but used incorrect message (null) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'NULL' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a student create new comment but used incorrect ClassRoomId (unknow) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'UnknowClassRoom'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a student create new comment but used incorrect ClassRoomId (empty) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: ''  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a student create new comment but used incorrect ClassRoomId (null) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'NULL'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a student create new comment but used incorrect LessonId (unknow) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'UnknowLesson' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a student create new comment but used incorrect LessonId (empty) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson '' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a student create new comment but used incorrect LessonId (null) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'NULL' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  


#a teacher create new comment Then system create new comment and update user's activity
#a teacher create new comment in the unaccessable lesson Then system create new comment and update user's activity
#a teacher create new comment but used incorrect message (empty) Then system do nothing
#a teacher create new comment but used incorrect message (null) Then system do nothing

#a teacher create new comment but used incorrect ClassRoomId (unknow) Then system do nothing
#a teacher create new comment but used incorrect ClassRoomId (empty) Then system do nothing
#a teacher create new comment but used incorrect ClassRoomId (null) Then system do nothing

#a teacher create new comment but used incorrect LessonId (unknow) Then system do nothing
#a teacher create new comment but used incorrect LessonId (empty) Then system do nothing
#a teacher create new comment but used incorrect LessonId (null) Then system do nothing

@mock  
Scenario: a teacher create new comment Then system create new comment and update user's activity
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System add new Comment by JSON format is  
    """
    {
        "ClassRoomId": "ClassRoom01",
        "CreatedByUserProfileId": "teacher@mindsage.com",
		"CreatorDisplayName": "teacher",
		"CreatorImageUrl": "teacher.jpg",
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
		"UserProfileId": "teacher@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
		"UserProfileName": "teacher",
		"UserProfileImageUrl": "teacher.jpg",
		"LessonActivities":
		[
			{
				"id": "LessonActivity01",
				"LessonId": "Lesson01",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			},
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson02",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			},
			{
				"id": "LessonActivity03",
				"LessonId": "Lesson03",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			}
		]
	}
    """  

@mock  
Scenario: a teacher create new comment in the unaccessable lesson Then system create new comment and update user's activity
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson03' of ClassRoom: 'ClassRoom01'  
    Then System add new Comment by JSON format is  
    """
    {
        "ClassRoomId": "ClassRoom01",
        "CreatedByUserProfileId": "teacher@mindsage.com",
		"CreatorDisplayName": "teacher",
		"CreatorImageUrl": "teacher.jpg",
        "Description": "Hello lesson 1",
        "TotalLikes": 0,
        "LessonId": "Lesson03",
        "Discussions": [],
		"CreatedDate": "2/8/2016 00:00 am"
    }
    """
    And System update UserActivity collection with JSON format is
    """
    {
		"id": "UserActivity01",
		"UserProfileId": "teacher@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
		"UserProfileName": "teacher",
		"UserProfileImageUrl": "teacher.jpg",
		"LessonActivities":
		[
			{
				"id": "LessonActivity01",
				"LessonId": "Lesson01",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			},
			{
				"id": "LessonActivity02",
				"LessonId": "Lesson02",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			},
			{
				"id": "LessonActivity03",
				"LessonId": "Lesson03",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			}
		]
	}
    """  

@mock  
Scenario: a teacher create new comment but used incorrect message (empty) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new comment with a message is '' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a teacher create new comment but used incorrect message (null) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new comment with a message is 'NULL' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a teacher create new comment but used incorrect ClassRoomId (unknow) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'UnknowClassRoom'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a teacher create new comment but used incorrect ClassRoomId (empty) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: ''  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a teacher create new comment but used incorrect ClassRoomId (null) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'NULL'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a teacher create new comment but used incorrect LessonId (unknow) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'UnknowLesson' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a teacher create new comment but used incorrect LessonId (empty) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson '' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: a teacher create new comment but used incorrect LessonId (null) Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'NULL' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: Incorrect user (unknow) create new comment Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'unknow@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: Incorrect user (empty) create new comment Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId '' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: Incorrect user (null) create new comment Then system do nothing
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'NULL' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: Incorrect user (subscription was deleted) create new comment Then system do nothing
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
				},
			]
		}
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: Incorrect user (subscription doesn't existing) create new comment Then system do nothing
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
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  

@mock  
Scenario: User create new comment but the ClassCalendar was deleted Then system create new comment and update user activity  
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
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System add new Comment by JSON format is  
    """
    {
        "ClassRoomId": "ClassRoom01",
        "CreatedByUserProfileId": "student@mindsage.com",
		"CreatorDisplayName": "student",
		"CreatorImageUrl": "student.jpg",
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
		"id": "UserActivity02",
		"UserProfileId": "student@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
		"UserProfileName": "student",
		"UserProfileImageUrl": "student.jpg",
		"LessonActivities":
		[
			{
				"id": "LessonActivity04",
				"LessonId": "Lesson01",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			},
			{
				"id": "LessonActivity05",
				"LessonId": "Lesson02",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			},
			{
				"id": "LessonActivity06",
				"LessonId": "Lesson03",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			}
		]
	}
    """  

@mock  
Scenario: User create new comment but the ClassCalendar doesn't existing Then the system do nothing
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  
	
@mock  
Scenario: User create new comment but the UserActivity was deleted Then system create new comment and update user activity  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserActivity collection with JSON format are
	"""
    [
		{
			"id": "UserActivity02",
			"UserProfileId": "student@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"UserProfileName": "student",
			"UserProfileImageUrl": "student.jpg",
			"DeletedDate": "1/1/2016",
			"LessonActivities":
			[
				{
					"id": "LessonActivity04",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity05",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity06",
					"LessonId": "Lesson03",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				}
			]
		}
    ]
    """  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System add new Comment by JSON format is  
    """
    {
        "ClassRoomId": "ClassRoom01",
        "CreatedByUserProfileId": "student@mindsage.com",
		"CreatorDisplayName": "student",
		"CreatorImageUrl": "student.jpg",
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
		"id": "UserActivity02",
		"UserProfileId": "student@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
		"UserProfileName": "student",
		"UserProfileImageUrl": "student.jpg",
		"DeletedDate": "1/1/2016",
		"LessonActivities":
		[
			{
				"id": "LessonActivity04",
				"LessonId": "Lesson01",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 1,
			},
			{
				"id": "LessonActivity05",
				"LessonId": "Lesson02",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			},
			{
				"id": "LessonActivity06",
				"LessonId": "Lesson03",
				"SawContentIds": [],
				"TotalContentsAmount": 1,
				"ParticipationAmount": 1,
				"CreatedCommentAmount": 0,
			}
		]
	}
    """  

@mock  
Scenario: User create new comment but the UserActivity doesn't existing Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserActivity collection with JSON format are
	"""
    []
    """  
    When UserProfileId 'student@mindsage.com' create a new comment with a message is 'Hello lesson 1' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System doesn't add new Comment  
    And System doesn't update UserActivity  