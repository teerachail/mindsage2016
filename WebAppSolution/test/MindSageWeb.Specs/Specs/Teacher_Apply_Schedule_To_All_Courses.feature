Feature: Teacher_Apply_Schedule_To_All_Courses
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
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				},
				{
					"id": "Subscription02",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom02",
					"ClassCalendarId": "ClassCalendar02",
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
            "BeginDate": "3/1/2016",
			"ExpiredDate": "3/10/2016",
            "ClassRoomId": "ClassRoom01",
			"CreatedDate": "2/1/2016",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
					"Order": 1,
					"SemesterGroupName": "A",
                    "LessonId": "Lesson01",
                    "BeginDate": "1/1/2016",
                    "LessonCatalogId": "LessonCatalog01",
					"TopicOfTheDays": [],
                },
                {
                    "Id": "LessonCalendar02",
					"Order": 2,
					"SemesterGroupName": "A",
                    "LessonId": "Lesson02",
                    "BeginDate": "1/8/2016",
                    "LessonCatalogId": "LessonCatalog02",
					"TopicOfTheDays": [],
                }
            ],
			"Holidays": [ "1/2/2016", "1/3/2016" ],
			"ShiftDays": [ "1/4/2016" ]
        },
		{
            "id": "ClassCalendar02",
            "BeginDate": "1/1/2016",
			"ExpiredDate": "1/1/2017",
            "ClassRoomId": "ClassRoom02",
			"CreatedDate": "2/1/2017",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar100",
					"Order": 1,
					"SemesterGroupName": "A",
                    "LessonId": "Lesson01",
                    "BeginDate": "1/1/2016",
                    "LessonCatalogId": "LessonCatalog01",
					"TopicOfTheDays": [],
                }
            ],
			"Holidays": [],
			"ShiftDays": []
        }
    ]
    """  

@mock  
Scenario: A teacher apply schedule to all its courses but have only one course available Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
    And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "3/1/2016",
			"ExpiredDate": "3/10/2016",
            "ClassRoomId": "ClassRoom01",
			"CreatedDate": "2/1/2016",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
					"Order": 1,
					"SemesterGroupName": "A",
                    "LessonId": "Lesson01",
                    "BeginDate": "1/1/2016",
                    "LessonCatalogId": "LessonCatalog01",
					"TopicOfTheDays": [],
                },
                {
                    "Id": "LessonCalendar02",
					"Order": 2,
					"SemesterGroupName": "A",
                    "LessonId": "Lesson02",
                    "BeginDate": "1/8/2016",
                    "LessonCatalogId": "LessonCatalog02",
					"TopicOfTheDays": [],
                }
            ],
			"Holidays": [ "1/2/2016", "1/3/2016" ],
			"ShiftDays": [ "1/4/2016" ]
        }
    ]
    """  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: A teacher apply schedule to all its courses Then system update schedule to all courses  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar02",
        "BeginDate": "3/1/2016",
		"ExpiredDate": "3/5/2016",
        "ClassRoomId": "ClassRoom02",
		"CreatedDate": "2/1/2017",
        "LessonCalendars":
        [
			{
                "Id": "LessonCalendar100",
				"Order": 1,
				"SemesterGroupName": "A",
                "LessonId": "Lesson100",
                "BeginDate": "1/1/2016",
                "LessonCatalogId": "LessonCatalog100",
				"TopicOfTheDays": [],
            }
        ],
		"Holidays": [ "1/2/2016", "1/3/2016" ],
		"ShiftDays": [ "1/4/2016" ]
    }
	"""  

@mock  
Scenario: A teacher apply schedule to all its courses but other courses' role are Student Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				},
				{
					"id": "Subscription02",
					"Role": "Student",
					"ClassRoomId": "ClassRoom02",
					"ClassCalendarId": "ClassCalendar02",
				},
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: A teacher apply schedule to all its courses but other courses' role are SelfPurchaser Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				},
				{
					"id": "Subscription02",
					"Role": "SelfPurchaser",
					"ClassRoomId": "ClassRoom02",
					"ClassCalendarId": "ClassCalendar02",
				},
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: A teacher apply schedule to all its courses but those courses were deleted (master course was deleted) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				},
				{
					"id": "Subscription02",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom02",
					"ClassCalendarId": "ClassCalendar02",
				},
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: A teacher apply schedule to all its courses but those courses were deleted (target courses was deleted) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				},
				{
					"id": "Subscription02",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom02",
					"ClassCalendarId": "ClassCalendar02",
					"DeletedDate": "1/1/2016"
				},
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: A teacher apply schedule to all its courses but used incorrect ClassRoom (unknow) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'UnknowClassRoom0'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: A teacher apply schedule to all its courses but used incorrect ClassRoom (empty) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: ''
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: A teacher apply schedule to all its courses but used incorrect ClassRoom (null) Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'NULL'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: Incorrect user (unknow) apply schedule to all its courses Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'unknow@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: Incorrect user (empty) apply schedule to all its courses Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile '' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: Incorrect user (null) apply schedule to all its courses Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'NULL' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: Incorrect user (Role = student) apply schedule to all its courses Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				},
				{
					"id": "Subscription02",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom02",
					"ClassCalendarId": "ClassCalendar02",
				},
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: Incorrect user (Role = selfpurchaser) apply schedule to all its courses Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "SelfPurchaser",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				},
				{
					"id": "Subscription02",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom02",
					"ClassCalendarId": "ClassCalendar02",
				},
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: A teacher apply schedule to all its courses but the ClassRoom was deleted Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "3/1/2016",
			"ExpiredDate": "3/10/2016",
            "ClassRoomId": "ClassRoom01",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "1/1/2016",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
					"Order": 1,
					"SemesterGroupName": "A",
                    "LessonId": "Lesson01",
                    "BeginDate": "1/1/2016",
                    "LessonCatalogId": "LessonCatalog01",
					"TopicOfTheDays": [],
                },
                {
                    "Id": "LessonCalendar02",
					"Order": 2,
					"SemesterGroupName": "A",
                    "LessonId": "Lesson02",
                    "BeginDate": "1/8/2016",
                    "LessonCatalogId": "LessonCatalog02",
					"TopicOfTheDays": [],
                }
            ],
			"Holidays": [ "1/2/2016", "1/3/2016" ],
			"ShiftDays": [ "1/4/2016" ]
        },
		{
            "id": "ClassCalendar02",
            "BeginDate": "1/1/2016",
			"ExpiredDate": "1/1/2017",
            "ClassRoomId": "ClassRoom02",
			"CreatedDate": "2/1/2017",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar100",
					"Order": 1,
					"SemesterGroupName": "A",
                    "LessonId": "Lesson01",
                    "BeginDate": "1/1/2016",
                    "LessonCatalogId": "LessonCatalog01",
					"TopicOfTheDays": [],
                }
            ],
			"Holidays": [],
			"ShiftDays": []
        }
    ]
    """  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  

@mock  
Scenario: A teacher apply schedule to all its courses but the ClassRoom doesn't existing Then system do nothing  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
		{
            "id": "ClassCalendar02",
            "BeginDate": "1/1/2016",
			"ExpiredDate": "1/1/2017",
            "ClassRoomId": "ClassRoom02",
			"CreatedDate": "2/1/2017",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar100",
					"Order": 1,
					"SemesterGroupName": "A",
                    "LessonId": "Lesson01",
                    "BeginDate": "1/1/2016",
                    "LessonCatalogId": "LessonCatalog01",
					"TopicOfTheDays": [],
                }
            ],
			"Holidays": [],
			"ShiftDays": []
        }
    ]
    """  
    When UserProfile 'teacher@mindsage.com' click Apply to all courses button from the ClassRoomId: 'ClassRoom01'
	Then System doesn't set course ClassCalendar  