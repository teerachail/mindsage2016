Feature: Teacher_Change_Course_Information
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

@mock  
Scenario: Teacher change course name Then system update course name  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode ''
    Then System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "TheNewName",
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
    """  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change student code Then system create new student key  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName '' and StudentCode 'NewStudentCode'
    Then System upsert StudentKey with JSON format is  
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
	And System create new StudentKey with JSON format is  
	"""
	{
		"Grade": "7",
		"Code": "NewStudentCode",
		"CourseCatalogId": "CourseCatalog01",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "2/8/2016",
	}
	"""  
	And System don't upsert ClassRoom  

@mock  
Scenario: Teacher change course name and student code Then system update course name and student code  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "TheNewName",
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
	And System create new StudentKey with JSON format is  
	"""
	{
		"Grade": "7",
		"Code": "NewStudentCode",
		"CourseCatalogId": "CourseCatalog01",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "2/8/2016",
	}
	"""  

@mock  
Scenario: Teacher change course name (empty) Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName '' and StudentCode ''
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change course name (null) Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'NULL' and StudentCode ''
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change student code (empty) Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName '' and StudentCode ''
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change student code (null) Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName '' and StudentCode 'NULL'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change course information but used incorrect ClassRoom (unknow) Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'UnknowClassRoomId' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change course information but used incorrect ClassRoom (empty) Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom '' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change course information but used incorrect ClassRoom (null) Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'NULL' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Incorrect user (unknow) change course information Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'unknow@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Incorrect user (empty) change course information Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile '' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Incorrect user (null) change course information Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'NULL' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Incorrect user (Role = student) change course information Then system doesn't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'student@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change course information but its subscription was deleted Then system don't update course information  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacherWithDeletedSubscription@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change course information but its subscription doesn't existing Then system don't update course information  
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
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change course information but the ClassRoom was deleted Then system don't update course information  
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
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change course information but the ClassRoom doesn't existing Then system don't update course information  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassRoom collection with JSON format are  
    """
    []
    """  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  

@mock  
Scenario: Teacher change course information but the ClassCalendar was deleted Then system update course information  
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
                    "BeginDate": "2/1/2016",
                    "LessonCatalogId": "LessonCatalog01"
                },
            ]
        },
    ]
    """  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "TheNewName",
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
	And System create new StudentKey with JSON format is  
	"""
	{
		"Grade": "7",
		"Code": "NewStudentCode",
		"CourseCatalogId": "CourseCatalog01",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "2/8/2016",
	}
	"""  

@mock  
Scenario: Teacher change course information but the ClassCalendar doesn't existing Then system update course information  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName 'TheNewName' and StudentCode 'NewStudentCode'
    Then System upsert ClassRoom with JSON format is  
    """
    {
        "id": "ClassRoom01",
        "Name": "TheNewName",
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
	And System create new StudentKey with JSON format is  
	"""
	{
		"Grade": "7",
		"Code": "NewStudentCode",
		"CourseCatalogId": "CourseCatalog01",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "2/8/2016",
	}
	"""  

@mock  
Scenario: Teacher change student code but the StudentKey was deleted Then system create new student code  
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
			"DeletedDate": "2/1/2016"
		}
	]
	"""  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName '' and StudentCode 'NewStudentCode'
    Then System create new StudentKey with JSON format is  
	"""
	{
		"Grade": "7",
		"Code": "NewStudentCode",
		"CourseCatalogId": "CourseCatalog01",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "2/8/2016",
	}
	"""  
	And System don't upsert ClassRoom  
	And System don't upsert StudentKey  

@mock  
Scenario: Teacher change student code but the StudentKey doesn't existing Then system doesn't create new student code  
    Given Today is '2/8/2016 00:00 am'  
	And System have StudentKey collection with JSON format are  
	"""
	[]
	"""  
    When UserProfile 'teacher@mindsage.com' change course info from ClassRoom 'ClassRoom01' to ClassName '' and StudentCode 'NewStudentCode'
    Then System don't upsert ClassRoom  
	And System don't upsert StudentKey  
	And System don't create new StudentKey  