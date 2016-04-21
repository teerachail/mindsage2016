Feature: Teacher_Remove_Student
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
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
			"id": "teacherWithDeletedSubscription@mindsage.com",
			"Name": "teacher",
			"ImageProfileUrl": "teacher.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
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
                    "BeginDate": "2/1/2016",
                    "LessonCatalogId": "LessonCatalog01"
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
			"Grade": "7",
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
	And System have StudentKey collection with JSON format are  
	"""
	[
		{
			"id": "StudentKey01",
			"Code": "StudentCode01",
			"Grade": "7",
			"CourseCatalogId": "CourseCatalog01",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "2/1/2016",
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
			"UserProfileId": "student@mindsage.com",
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
Scenario: Teacher remove a student Then system remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' remove StudentId 'student@mindsage.com' from ClassRoom 'ClassRoom01' 
    Then System upsert UserProfile with JSON format is  
    """
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
				"DeletedDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert UserActivity collection with JSON format is
    """
	{
		"id": "UserActivity02",
		"UserProfileId": "student@mindsage.com",
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

@mock  
Scenario: Teacher remove a student but the student doesn't existing (unknow) Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' remove StudentId 'unknow@mindsage.com' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Teacher remove a student but the student doesn't existing (empty) Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' remove StudentId '' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Teacher remove a student but the student doesn't existing (null) Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' remove StudentId 'NULL' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Teacher remove a student but the ClassRoom doesn't existing (unknow) Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' remove StudentId 'student@mindsage.com' from ClassRoom 'UnknowClassRoom' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Teacher remove a student but the ClassRoom doesn't existing (empty) Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' remove StudentId 'student@mindsage.com' from ClassRoom '' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Teacher remove a student but the ClassRoom doesn't existing (null) Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' remove StudentId 'student@mindsage.com' from ClassRoom 'NULL' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Incorrect user (unknow) remove a student Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'unknow@mindsage.com' remove StudentId 'student@mindsage.com' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Incorrect user (empty) remove a student Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile '' remove StudentId 'student@mindsage.com' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Incorrect user (null) remove a student Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'NULL' remove StudentId 'student@mindsage.com' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Incorrect user (Role = student) remove a student Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'student@mindsage.com' remove StudentId 'student@mindsage.com' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Incorrect user (Subscription was deleted) remove a student Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacherWithDeletedSubscription@mindsage.com' remove StudentId 'student@mindsage.com' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Teacher try to remove himself from the class room Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' remove StudentId 'teacher@mindsage.com' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Teacher remove a student but the student's subscription was deleted Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
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
					"DeletedDate": "1/1/2016"
				},
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' remove StudentId 'teacher@mindsage.com' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  

@mock  
Scenario: Teacher remove a student but the student doesn't existing Then system doesn't remove the student from the course  
    Given Today is '2/8/2016 00:00 am'  
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
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' remove StudentId 'teacher@mindsage.com' from ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't upsert UserActivity  