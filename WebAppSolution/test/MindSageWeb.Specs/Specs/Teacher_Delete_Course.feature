Feature: Teacher_Delete_Course
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
			"Holidays": [],
			"ShiftDays": [],
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "LessonId": "Lesson01",
                    "BeginDate": "2/1/2016",
                    "LessonCatalogId": "LessonCatalog01",
					"TopicOfTheDays": []
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
		},
		{
			"id": "UserActivity03",
			"UserProfileId": "teacherWithDeletedSubscription@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity03",
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
Scenario: Teacher delete course Then system delete the selected course
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System upsert UserProfile with JSON format is  
    """
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
				"DeletedDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert UserProfile with JSON format is  
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
		"id": "UserActivity01",
		"UserProfileId": "teacher@mindsage.com",
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
	And System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "Emotional literacy",
		"Grade": "7",
        "CourseCatalogId": "CourseCatalog01",
        "CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am",
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
    """  
	And System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
        "BeginDate": "2/1/2016",
        "ClassRoomId": "ClassRoom01",
		"DeletedDate": "2/8/2016 00:00 am",
		"Holidays": [],
		"ShiftDays": [],
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "LessonId": "Lesson01",
                "BeginDate": "2/1/2016",
                "LessonCatalogId": "LessonCatalog01",
				"TopicOfTheDays": []
            },
        ]
    }
	"""  
	And System upsert StudentKey with JSON format is  
	"""
	{
		"id": "StudentKey01",
		"Grade": "7",
		"Code": "StudentCode01",
		"CourseCatalogId": "CourseCatalog01",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am"
	}
	"""  

@mock  
Scenario: Teacher delete course but used ClassRoom incorrect (unknow) Then system doesn't delete the selected course
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'UnknowClassRoom' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Teacher delete course but used ClassRoom incorrect (empty) Then system doesn't delete the selected course
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' delete ClassRoom '' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Teacher delete course but used ClassRoom incorrect (null) Then system doesn't delete the selected course
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'NULL' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Incorrect user (unknow) delete course Then system doesn't delete the selected course
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'unknow@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Incorrect user (empty) delete course Then system doesn't delete the selected course
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile '' delete ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Incorrect user (null) delete course Then system doesn't delete the selected course
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'NULL' delete ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Incorrect user (Role = Student) delete course Then system doesn't delete the selected course
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'student@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Teacher delete course but its subscription was deleted Then system doesn't delete the selected course
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacherWithDeletedSubscription@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Teacher delete course but its subscription doesn't existing Then system doesn't delete the selected course
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Name": "teacher",
			"ImageProfileUrl": "teacher.jpg",
			"Subscriptions": []
		}
    ]
    """  
    When UserProfile 'student@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Teacher delete course but the class calendar was deleted Then system delete the selected course
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
            "ClassRoomId": "ClassRoom01",
			"Holidays": [],
			"ShiftDays": [],
			"DeletedDate": "1/1/2016",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "LessonId": "Lesson01",
                    "BeginDate": "2/1/2016",
                    "LessonCatalogId": "LessonCatalog01",
					"TopicOfTheDays": []
                },
            ]
        },
    ]
    """  
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System upsert UserProfile with JSON format is  
    """
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
				"DeletedDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert UserProfile with JSON format is  
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
		"id": "UserActivity01",
		"UserProfileId": "teacher@mindsage.com",
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
	And System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "Emotional literacy",
		"Grade": "7",
        "CourseCatalogId": "CourseCatalog01",
        "CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am",
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
    """  
	And System doesn't set course ClassCalendar  
	And System upsert StudentKey with JSON format is  
	"""
	{
		"id": "StudentKey01",
		"Grade": "7",
		"Code": "StudentCode01",
		"CourseCatalogId": "CourseCatalog01",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am"
	}
	"""  

@mock  
Scenario: Teacher delete course but the class calendar doesn't existing Then system delete the selected course
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Teacher delete course but the class room was deleted Then system delete the selected course
    Given Today is '2/8/2016 00:00 am'  
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
			"DeletedDate": "1/1/2016",
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
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System upsert UserProfile with JSON format is  
    """
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
				"DeletedDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert UserProfile with JSON format is  
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
		"id": "UserActivity01",
		"UserProfileId": "teacher@mindsage.com",
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
	And System don't upsert ClassRoom  
	And System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
        "BeginDate": "2/1/2016",
        "ClassRoomId": "ClassRoom01",
		"DeletedDate": "2/8/2016 00:00 am",
		"Holidays": [],
		"ShiftDays": [],
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "LessonId": "Lesson01",
                "BeginDate": "2/1/2016",
                "LessonCatalogId": "LessonCatalog01",
				"TopicOfTheDays": []
            },
        ]
    }
	"""  
	And System upsert StudentKey with JSON format is  
	"""
	{
		"id": "StudentKey01",
		"Grade": "7",
		"Code": "StudentCode01",
		"CourseCatalogId": "CourseCatalog01",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am"
	}
	"""  

@mock  
Scenario: Teacher delete course but the class room doesn't existing Then system doesn't delete the selected course
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassRoom collection with JSON format are  
    """
    []
    """  
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System don't upsert ClassRoom  
	And System doesn't set course ClassCalendar  
	And System don't upsert StudentKey  

@mock  
Scenario: Teacher delete course but the student key was deleted Then system delete the selected course
    Given Today is '2/8/2016 00:00 am'  
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
			"DeletedDate": "2/1/2016",
		}
	]
	"""  
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System upsert UserProfile with JSON format is  
    """
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
				"DeletedDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert UserProfile with JSON format is  
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
		"id": "UserActivity01",
		"UserProfileId": "teacher@mindsage.com",
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
	And System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "Emotional literacy",
		"Grade": "7",
        "CourseCatalogId": "CourseCatalog01",
        "CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am",
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
    """  
	And System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
        "BeginDate": "2/1/2016",
        "ClassRoomId": "ClassRoom01",
		"DeletedDate": "2/8/2016 00:00 am",
		"Holidays": [],
		"ShiftDays": [],
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "LessonId": "Lesson01",
                "BeginDate": "2/1/2016",
                "LessonCatalogId": "LessonCatalog01",
				"TopicOfTheDays": []
            },
        ]
    }
	"""  
	And System don't upsert StudentKey 

@mock  
Scenario: Teacher delete course but the student key doesn't existing Then system delete the selected course
    Given Today is '2/8/2016 00:00 am'  
	And System have StudentKey collection with JSON format are  
	"""
	[]
	"""  
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System upsert UserProfile with JSON format is  
    """
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
				"DeletedDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert UserProfile with JSON format is  
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
		"id": "UserActivity01",
		"UserProfileId": "teacher@mindsage.com",
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
	And System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "Emotional literacy",
		"Grade": "7",
        "CourseCatalogId": "CourseCatalog01",
        "CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am",
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
    """  
	And System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
        "BeginDate": "2/1/2016",
        "ClassRoomId": "ClassRoom01",
		"DeletedDate": "2/8/2016 00:00 am",
		"Holidays": [],
		"ShiftDays": [],
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "LessonId": "Lesson01",
                "BeginDate": "2/1/2016",
                "LessonCatalogId": "LessonCatalog01",
				"TopicOfTheDays": []
            },
        ]
    }
	"""  
	And System don't upsert StudentKey 

@mock  
Scenario: Teacher delete course but user activities was deleted Then system delete the selected course
    Given Today is '2/8/2016 00:00 am'  
	And System have UserActivity collection with JSON format are
    """
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "teacher@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"DeletedDate": "1/1/2016",
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
			"DeletedDate": "1/1/2016",
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
		},
		{
			"id": "UserActivity03",
			"UserProfileId": "teacherWithDeletedSubscription@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"DeletedDate": "1/1/2016",
			"LessonActivities":
			[
				{
					"id": "LessonActivity03",
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
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System upsert UserProfile with JSON format is  
    """
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
				"DeletedDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert UserProfile with JSON format is  
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
	And System doesn't update UserActivity  
	And System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "Emotional literacy",
		"Grade": "7",
        "CourseCatalogId": "CourseCatalog01",
        "CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am",
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
    """  
	And System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
        "BeginDate": "2/1/2016",
        "ClassRoomId": "ClassRoom01",
		"DeletedDate": "2/8/2016 00:00 am",
		"Holidays": [],
		"ShiftDays": [],
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "LessonId": "Lesson01",
                "BeginDate": "2/1/2016",
                "LessonCatalogId": "LessonCatalog01",
				"TopicOfTheDays": []
            },
        ]
    }
	"""  
	And System upsert StudentKey with JSON format is  
	"""
	{
		"id": "StudentKey01",
		"Grade": "7",
		"Code": "StudentCode01",
		"CourseCatalogId": "CourseCatalog01",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am"
	}
	"""  

@mock  
Scenario: Teacher delete course but user activities don't existing Then system delete the selected course
    Given Today is '2/8/2016 00:00 am'  
	And System have UserActivity collection with JSON format are
    """
    []
    """  
    When UserProfile 'teacher@mindsage.com' delete ClassRoom 'ClassRoom01' 
    Then System upsert UserProfile with JSON format is  
    """
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
				"DeletedDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert UserProfile with JSON format is  
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
	And System doesn't update UserActivity  
	And System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "Emotional literacy",
		"Grade": "7",
        "CourseCatalogId": "CourseCatalog01",
        "CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am",
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
    """  
	And System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
        "BeginDate": "2/1/2016",
        "ClassRoomId": "ClassRoom01",
		"DeletedDate": "2/8/2016 00:00 am",
		"Holidays": [],
		"ShiftDays": [],
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "LessonId": "Lesson01",
                "BeginDate": "2/1/2016",
                "LessonCatalogId": "LessonCatalog01",
				"TopicOfTheDays": []
            },
        ]
    }
	"""  
	And System upsert StudentKey with JSON format is  
	"""
	{
		"id": "StudentKey01",
		"Grade": "7",
		"Code": "StudentCode01",
		"CourseCatalogId": "CourseCatalog01",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "2/1/2016",
		"DeletedDate": "2/8/2016 00:00 am"
	}
	"""  