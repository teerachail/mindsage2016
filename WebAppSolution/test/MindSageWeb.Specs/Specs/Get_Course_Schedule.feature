Feature: Get_Course_Schedule
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
			"Name": "Sakul jaruthanaset",
			"ImageProfileUrl": "ImgURL01",
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
			"Name": "Student jaruthanaset",
			"ImageProfileUrl": "ImgURL02",
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
			"id": "selfpurchase@mindsage.com",
			"Name": "Selfpurchase jaruthanaset",
			"ImageProfileUrl": "ImgURL03",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "SelfPurchaser",
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
            "BeginDate": "1/1/2016",
			"ExpiredDate": "1/1/2017",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "1/1/2016",
					"TopicOfTheDays": [],
                    "LessonId": "Lesson01",
                },
				{
                    "Id": "LessonCalendar02",
                    "Order": 2,
                    "SemesterGroupName": "A",
                    "BeginDate": "1/6/2016",
					"TopicOfTheDays": [],
                    "LessonId": "Lesson01",
                }
            ],
			"Holidays": [],
			"ShiftDays": []
        }
    ]
    """  

@mock  
Scenario: Teacher request course schedule Then system send course schedule to the teacher
    Given Today is '2/8/2016 00:00 am'  
	When User 'sakul@mindsage.com' request course schedule from ClassRoomId 'ClassRoom01'  
	Then System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/1/2017",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016",
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/6/2016",
			}
		],
		"Holidays": [],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher request incorrect course schedule Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	When User 'sakul@mindsage.com' request course schedule from ClassRoomId 'IncorrectClassRoom'  
	Then System send null back  

@mock  
Scenario: Teacher request incorrect course schedule (empty) Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	When User 'sakul@mindsage.com' request course schedule from ClassRoomId ''  
	Then System send null back  

@mock  
Scenario: Teacher request incorrect course schedule (null) Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	When User 'sakul@mindsage.com' request course schedule from ClassRoomId 'NULL'  
	Then System send null back  

@mock  
Scenario: Unknow user request course schedule Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	When User 'unknow@mindsage.com' request course schedule from ClassRoomId 'ClassRoom01'  
	Then System send null back  

@mock  
Scenario: Unknow user request incorrect course schedule Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	When User 'unknow@mindsage.com' request course schedule from ClassRoomId 'IncorrectClassRoom'  
	Then System send null back  

@mock  
Scenario: Unknow user (empty) request incorrect course schedule Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	When User '' request course schedule from ClassRoomId 'IncorrectClassRoom'  
	Then System send null back  

@mock  
Scenario: Unknow user (null) request incorrect course schedule Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	When User 'NULL' request course schedule from ClassRoomId 'IncorrectClassRoom'  
	Then System send null back  

@mock  
Scenario: Student request course schedule Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	When User 'student@mindsage.com' request course schedule from ClassRoomId 'ClassRoom01'  
	Then System send null back  

@mock  
Scenario: Selfpurchase request course schedule Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	When User 'selfpurchase@mindsage.com' request course schedule from ClassRoomId 'ClassRoom01'  
	Then System send null back  

@mock  
Scenario: Teacher request course schedule when the course doesn't set the begin date Then system send course schedule to the teacher
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "1/1/2016",
					"TopicOfTheDays": [],
                    "LessonId": "Lesson01",
                },
				{
                    "Id": "LessonCalendar02",
                    "Order": 2,
                    "SemesterGroupName": "A",
                    "BeginDate": "1/6/2016",
					"TopicOfTheDays": [],
                    "LessonId": "Lesson01",
                }
            ],
			"Holidays": [],
			"ShiftDays": []
        }
    ]
    """  
	When User 'sakul@mindsage.com' request course schedule from ClassRoomId 'ClassRoom01'  
	Then System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016",
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/6/2016",
			}
		],
		"Holidays": [],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher request course schedule but the ClassCalendar was deleted Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "1/1/2016",
			"ExpiredDate": "1/1/2017",
			"DeletedDate": "1/1/2016",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "1/1/2016",
					"TopicOfTheDays": [],
                    "LessonId": "Lesson01",
                },
				{
                    "Id": "LessonCalendar02",
                    "Order": 2,
                    "SemesterGroupName": "A",
                    "BeginDate": "1/6/2016",
					"TopicOfTheDays": [],
                    "LessonId": "Lesson01",
                }
            ],
			"Holidays": [],
			"ShiftDays": []
        }
    ]
    """  
	When User 'sakul@mindsage.com' request course schedule from ClassRoomId 'ClassRoom01'  
	Then System send null back  

@mock  
Scenario: Teacher request course schedule but the ClassCalendar was closed Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "1/1/2016",
			"ExpiredDate": "1/1/2017",
			"CloseDate": "1/1/2016",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "1/1/2016",
					"TopicOfTheDays": [],
                    "LessonId": "Lesson01",
                },
				{
                    "Id": "LessonCalendar02",
                    "Order": 2,
                    "SemesterGroupName": "A",
                    "BeginDate": "1/6/2016",
					"TopicOfTheDays": [],
                    "LessonId": "Lesson01",
                }
            ],
			"Holidays": [],
			"ShiftDays": []
        }
    ]
    """  
	When User 'sakul@mindsage.com' request course schedule from ClassRoomId 'ClassRoom01'  
	Then System send null back  

@mock  
Scenario: Teacher request course schedule but the ClassCalendar doesn't existing Then system send null back
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
	When User 'sakul@mindsage.com' request course schedule from ClassRoomId 'ClassRoom01'  
	Then System send null back  