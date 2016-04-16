Feature: Set_Course_Schedule_By_Set
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
Scenario: Teacher set all sunday are holiday Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsSunday": true
	}
	"""  
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
		"Holidays": [ "1/4/2016", "1/11/2016" ],
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
		"Holidays": [ "1/4/2016", "1/11/2016" ],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher set all monday are holiday Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsMonday": true
	}
	"""  
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
		"Holidays": [ "1/5/2016" ],
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
		"Holidays": [ "1/5/2016" ],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher set all tuesday are holiday Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsTuesday": true
	}
	"""  
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
		"Holidays": [ "1/6/2016" ],
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
		"Holidays": [ "1/6/2016" ],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher set all wenesday are holiday Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsWednesday": true
	}
	"""  
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
		"Holidays": [ "1/7/2016" ],
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
		"Holidays": [ "1/7/2016" ],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher set all thursday are holiday Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsThursday": true
	}
	"""  
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
		"Holidays": [ "1/1/2016", "1/8/2016" ],
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
		"Holidays": [ "1/1/2016", "1/8/2016" ],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher set all friday are holiday Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsFriday": true
	}
	"""  
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
		"Holidays": [ "1/2/2016", "1/9/2016" ],
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
		"Holidays": [ "1/2/2016", "1/9/2016" ],
		"ShiftDays": []
	}
	"""  

@mock  
Scenario: Teacher set all saturday are holiday Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsSaturday": true
	}
	"""  
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
		"Holidays": [ "1/3/2016", "1/10/2016" ],
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
		"Holidays": [ "1/3/2016", "1/10/2016" ],
		"ShiftDays": []
	}
	"""  


@mock  
Scenario: Teacher set all sunday are holiday and shift day Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsSunday": true,
		"IsShift": true
	}
	"""  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/12/2016",
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
                "BeginDate": "1/7/2016",
				"TopicOfTheDays": [],
                "LessonId": "Lesson01",
            }
        ],
		"Holidays": [ "1/4/2016", "1/11/2016" ],
		"ShiftDays": [ "1/4/2016", "1/11/2016" ],
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/12/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/7/2016"
			}
		],
		"Holidays": [ "1/4/2016", "1/11/2016" ],
		"ShiftDays":[ "1/4/2016", "1/11/2016" ],
	}
	"""  

@mock  
Scenario: Teacher set all monday are holiday and shift day Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsMonday": true,
		"IsShift": true
	}
	"""  
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
                "BeginDate": "1/1/2016",
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
		"Holidays": [ "1/5/2016" ],
		"ShiftDays": [ "1/5/2016" ]
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
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/7/2016"
			}
		],
		"Holidays": [ "1/5/2016" ],
		"ShiftDays": [ "1/5/2016" ]
	}
	"""  

@mock  
Scenario: Teacher set all tuesday are holiday and shift day Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsTuesday": true,
		"IsShift": true
	}
	"""  
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
                "BeginDate": "1/1/2016",
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
		"Holidays": [ "1/6/2016" ],
		"ShiftDays": [ "1/6/2016" ],
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
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/7/2016"
			}
		],
		"Holidays": [ "1/6/2016" ],
		"ShiftDays": [ "1/6/2016" ],
	}
	"""  

@mock  
Scenario: Teacher set all wenesday are holiday and shift day Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsWednesday": true,
		"IsShift": true
	}
	"""  
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
		"Holidays": [ "1/7/2016" ],
		"ShiftDays": [ "1/7/2016" ],
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
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/6/2016"
			}
		],
		"Holidays": [ "1/7/2016" ],
		"ShiftDays": [ "1/7/2016" ],
	}
	"""  

@mock  
Scenario: Teacher set all thursday are holiday and shift day Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsThursday": true,
		"IsShift": true
	}
	"""  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/12/2016",
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
		"Holidays": [ "1/1/2016", "1/8/2016" ],
		"ShiftDays": [ "1/1/2016", "1/8/2016" ],
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/12/2016",
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
		"Holidays": [ "1/1/2016", "1/8/2016" ],
		"ShiftDays": [ "1/1/2016", "1/8/2016" ],
	}
	"""  

@mock  
Scenario: Teacher set all friday are holiday and shift day Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsFriday": true,
		"IsShift": true
	}
	"""  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/12/2016",
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
                "BeginDate": "1/7/2016",
				"TopicOfTheDays": [],
                "LessonId": "Lesson01",
            }
        ],
		"Holidays": [ "1/2/2016", "1/9/2016" ],
		"ShiftDays": [ "1/2/2016", "1/9/2016" ],
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/12/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/7/2016"
			}
		],
		"Holidays": [ "1/2/2016", "1/9/2016" ],
		"ShiftDays": [ "1/2/2016", "1/9/2016" ],
	}
	"""  

@mock  
Scenario: Teacher set all saturday are holiday and shift day Then system set course Calendar and send new schedule back  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsSaturday": true,
		"IsShift": true
	}
	"""  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
        "id": "ClassCalendar01",
		"BeginDate": "1/1/2016",
		"ExpiredDate": "1/12/2016",
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
                "BeginDate": "1/7/2016",
				"TopicOfTheDays": [],
                "LessonId": "Lesson01",
            }
        ],
		"Holidays": [ "1/3/2016", "1/10/2016" ],
		"ShiftDays": [ "1/3/2016", "1/10/2016" ],
    }
	"""  
	And System send course schedule with JSON format is  
	"""  
	{
		"IsComplete": true,
		"BeginDate": "1/1/2016",
		"EndDate": "1/12/2016",
		"Lessons": [
			{
				"Name": "Lesson 1",
				"BeginDate": "1/1/2016"
			},
			{
				"Name": "Lesson 2",
				"BeginDate": "1/7/2016"
			}
		],
		"Holidays": [ "1/3/2016", "1/10/2016" ],
		"ShiftDays": [ "1/3/2016", "1/10/2016" ],
	}
	"""  

@mock  
Scenario: Teacher set schedule set but using incorrect ClassRoom Then system doesn't set course Calendar  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "IncorrectClassRoom",
		"IsHoliday": true,
		"IsSaturday": true,
		"IsShift": true
	}
	"""  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Teacher set schedule set but using incorrect (empty) ClassRoom Then system doesn't set course Calendar  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "",
		"IsHoliday": true,
		"IsSaturday": true,
		"IsShift": true
	}
	"""  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Teacher set schedule set but using incorrect (null) ClassRoom Then system doesn't set course Calendar  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "NULL",
		"IsHoliday": true,
		"IsSaturday": true,
		"IsShift": true
	}
	"""  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Incorrect (unknow) user set schedule set ClassRoom Then system doesn't set course Calendar  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "unknow@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsSaturday": true,
		"IsShift": true
	}
	"""  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Incorrect (empty) user set schedule set ClassRoom Then system doesn't set course Calendar  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsSaturday": true,
		"IsShift": true
	}
	"""  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Incorrect (null) user set schedule set ClassRoom Then system doesn't set course Calendar  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "NULL",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsSaturday": true,
		"IsShift": true
	}
	"""  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Incorrect (role = student) user set schedule set ClassRoom Then system doesn't set course Calendar  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "student@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsSaturday": true,
		"IsShift": true
	}
	"""  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Incorrect (role = selfpurchase) user set schedule set ClassRoom Then system doesn't set course Calendar  
	Given Today is '1/1/2016 00:00 am'  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "selfpurchase@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsSaturday": true,
		"IsShift": true
	}
	"""  
	Then System doesn't set course ClassCalendar  
	And System send null back  

@mock  
Scenario: Teacher user set schedule set but the ClassCalendar doesn't existing ClassRoom Then system doesn't set course Calendar  
	Given Today is '1/1/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
	When User set course schedule by set by JSON format is  
	"""  
	{
		"UserProfileId": "sakul@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"IsHoliday": true,
		"IsSaturday": true,
		"IsShift": true
	}
	"""  
	Then System doesn't set course ClassCalendar  
	And System send null back  