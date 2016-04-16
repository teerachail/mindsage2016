Feature: Set_Course_Schedule_By_Range
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
            "ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"BeginDate": "1/1/2016",
			"ExpiredDate": "1/10/2016",
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
Scenario: Teacher set course schedule holiday only (Single day) Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/10/2016",
        "ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
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
		"Holidays": [ "1/1/2016" ],
		"ShiftDays": []
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/10/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/6/2016"
			}
		],
		"Holidays": [ "1/1/2016" ],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher set course schedule holiday only (Multiple days) Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '1/5/2016' IsHoliday 'true' IsShift 'false'  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/10/2016",
        "ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
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
		"Holidays": [ "1/1/2016", "1/2/2016", "1/3/2016", "1/4/2016", "1/5/2016" ],
		"ShiftDays": []
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/10/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/6/2016"
			}
		],
		"Holidays": [ "1/1/2016", "1/2/2016", "1/3/2016", "1/4/2016", "1/5/2016" ],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher set course schedule shift date only (Single day) Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'false' IsShift 'true'  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/11/2016",
        "ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "Order": 1,
                "SemesterGroupName": "A",
                "BeginDate": "1/2/2016",
				"TopicOfTheDays": [],
                "LessonId": "Lesson01",
            },
			{
                "Id": "LessonCalendar02",
                "Order": 2,
                "SemesterGroupName": "A",
                "BeginDate": "1/7/2016",
				"TopicOfTheDays": [],
                "LessonId": "Lesson01",
            }
        ],
		"Holidays": [],
		"ShiftDays": [ "1/1/2016" ]
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/11/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/2/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/7/2016"
			}
		],
		"Holidays": [],
		"ShiftDays": [ "1/1/2016" ]
	}
	"""  

@mock  
Scenario: Teacher set course schedule shift date only (Multiple days) Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '1/5/2016' IsHoliday 'false' IsShift 'true'  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/15/2016",
        "ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "Order": 1,
                "SemesterGroupName": "A",
                "BeginDate": "1/6/2016",
				"TopicOfTheDays": [],
                "LessonId": "Lesson01",
            },
			{
                "Id": "LessonCalendar02",
                "Order": 2,
                "SemesterGroupName": "A",
                "BeginDate": "1/11/2016",
				"TopicOfTheDays": [],
                "LessonId": "Lesson01",
            }
        ],
		"Holidays": [],
		"ShiftDays": [ "1/1/2016", "1/2/2016", "1/3/2016", "1/4/2016", "1/5/2016" ]
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/15/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/6/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/11/2016"
			}
		],
		"Holidays": [],
		"ShiftDays": [ "1/1/2016", "1/2/2016", "1/3/2016", "1/4/2016", "1/5/2016" ]
	}
	"""  

@mock  
Scenario: Teacher set course schedule holiday and shiftday (Single day) Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'true'  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/11/2016",
        "ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "Order": 1,
                "SemesterGroupName": "A",
                "BeginDate": "1/2/2016",
				"TopicOfTheDays": [],
                "LessonId": "Lesson01",
            },
			{
                "Id": "LessonCalendar02",
                "Order": 2,
                "SemesterGroupName": "A",
                "BeginDate": "1/7/2016",
				"TopicOfTheDays": [],
                "LessonId": "Lesson01",
            }
        ],
		"Holidays": [ "1/1/2016" ],
		"ShiftDays": [ "1/1/2016" ]
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/11/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/2/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/7/2016"
			}
		],
		"Holidays": [ "1/1/2016" ],
		"ShiftDays": [ "1/1/2016" ]
	}
	"""  

@mock  
Scenario: Teacher set course schedule holiday and shiftday (Multiple days) Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/2/2016' ToDate '1/5/2016' IsHoliday 'true' IsShift 'true'  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/14/2016",
        "ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
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
                "BeginDate": "1/10/2016",
				"TopicOfTheDays": [],
                "LessonId": "Lesson01",
            }
        ],
		"Holidays": [ "1/2/2016", "1/3/2016", "1/4/2016", "1/5/2016" ],
		"ShiftDays": [ "1/2/2016", "1/3/2016", "1/4/2016", "1/5/2016" ]
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/14/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/10/2016"
			}
		],
		"Holidays": [ "1/2/2016", "1/3/2016", "1/4/2016", "1/5/2016" ],
		"ShiftDays": [ "1/2/2016", "1/3/2016", "1/4/2016", "1/5/2016" ]
	}
	"""  

@mock  
Scenario: Teacher set course without any thing changed Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'false' IsShift 'false'  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/10/2016",
        "ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
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
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/10/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/6/2016"
			}
		],
		"Holidays": [],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher set course schedule but using incorrect ClassRoom Then system doesn't set course Calendar and doesn't send schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'IncorrectClassRoom' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Teacher set course schedule but using incorrect (empty) ClassRoom Then system doesn't set course Calendar and doesn't send schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId '' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Teacher set course schedule but using incorrect (null) ClassRoom Then system doesn't set course Calendar and doesn't send schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'NULL' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Incorrect user (unknow) try to set course schedule Then system doesn't set course Calendar and doesn't send schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'unknow@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Incorrect user (empty) try to set course schedule Then system doesn't set course Calendar and doesn't send schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User '' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Incorrect user (null) try to set course schedule Then system doesn't set course Calendar and doesn't send schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'NULL' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Incorrect user (role = student) try to set course schedule Then system doesn't set course Calendar and doesn't send schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'student@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Incorrect user (role = selfpurchase) try to set course schedule Then system doesn't set course Calendar and doesn't send schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User 'selfpurchase@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Teacher set course schedule but the ClassCalendar doesn't existing  Then system doesn't set course Calendar and doesn't send schedule back  
	Given Today is '1/1/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Teacher set course schedule holiday only (Single day with TOTD) Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"BeginDate": "1/1/2016",
			"ExpiredDate": "1/10/2016",
            "LessonCalendars":
            [
				{
					"Id": "LessonCalendar01",
					"Order": 1,
					"SemesterGroupName": "A",
					"BeginDate": "1/1/2016",
					"TopicOfTheDays": [
						{
							"id": "TOTD01",
							"Message": "Msg01",
							"SendOnDay": 1,
							"CreatedDate": "1/1/2016"
						}
					],
					"LessonId": "Lesson01",
				},
				{
					"Id": "LessonCalendar02",
					"Order": 2,
					"SemesterGroupName": "A",
					"BeginDate": "1/6/2016",
					"TopicOfTheDays": [
						{
							"id": "TOTD02",
							"Message": "Msg02",
							"SendOnDay": 2,
							"CreatedDate": "1/1/2016",
							"DeletedDate": "1/1/2016"
						},
						{
							"id": "TOTD03",
							"Message": "Msg03",
							"SendOnDay": 2,
							"CreatedDate": "1/1/2016",
							"DeletedDate": "1/1/2016"
						},
						{
							"id": "TOTD04",
							"Message": "Msg04",
							"SendOnDay": 7,
							"CreatedDate": "1/1/2016"
						},
						{
							"id": "TOTD05",
							"Message": "Msg05",
							"SendOnDay": 8,
							"RequiredSendTopicOfTheDayDate": "1/13/2016",
							"SendTopicOfTheDayDate": "1/1/2016",
							"CreatedDate": "1/1/2016"
						}
					],
					"LessonId": "Lesson01",
				}
			],
			"Holidays": [],
			"ShiftDays": []
        }
    ]
    """  
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'true' IsShift 'false'  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/10/2016",
        "ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
        "LessonCalendars":
        [
            {
                "Id": "LessonCalendar01",
                "Order": 1,
                "SemesterGroupName": "A",
                "BeginDate": "1/1/2016",
				"TopicOfTheDays": [
					{
						"id": "TOTD01",
						"Message": "Msg01",
						"SendOnDay": 1,
						"RequiredSendTopicOfTheDayDate": "1/1/2016",
						"CreatedDate": "1/1/2016"
					}
				],
                "LessonId": "Lesson01",
            },
			{
                "Id": "LessonCalendar02",
                "Order": 2,
                "SemesterGroupName": "A",
                "BeginDate": "1/6/2016",
				"TopicOfTheDays": [
					{
						"id": "TOTD02",
						"Message": "Msg02",
						"SendOnDay": 2,
						"RequiredSendTopicOfTheDayDate": "1/7/2016",
						"CreatedDate": "1/1/2016",
						"DeletedDate": "1/1/2016"
					},
					{
						"id": "TOTD03",
						"Message": "Msg03",
						"SendOnDay": 2,
						"RequiredSendTopicOfTheDayDate": "1/7/2016",
						"CreatedDate": "1/1/2016",
						"DeletedDate": "1/1/2016"
					},
					{
						"id": "TOTD04",
						"Message": "Msg04",
						"SendOnDay": 7,
						"RequiredSendTopicOfTheDayDate": "1/12/2016",
						"CreatedDate": "1/1/2016"
					},
					{
						"id": "TOTD05",
						"Message": "Msg05",
						"SendOnDay": 8,
						"RequiredSendTopicOfTheDayDate": "1/13/2016",
						"SendTopicOfTheDayDate": "1/1/2016",
						"CreatedDate": "1/1/2016"
					}
				],
                "LessonId": "Lesson01",
            }
        ],
		"Holidays": [ "1/1/2016" ],
		"ShiftDays": []
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/10/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/6/2016"
			}
		],
		"Holidays": [ "1/1/2016" ],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher set course schedule but the course doesn't set started date Then system doesn't set course Calendar and send the schedule back  
	Given Today is '1/1/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
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
	When User 'sakul@mindsage.com' set schedule of ClassRoomId 'ClassRoom01' FromDate '1/1/2016' ToDate '' IsHoliday 'false' IsShift 'false'  
	Then System doesn't set course ClassCalendar  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": false,
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/6/2016"
			}
		],
		"Holidays": [],
		"ShiftDays": []
	}
	"""  