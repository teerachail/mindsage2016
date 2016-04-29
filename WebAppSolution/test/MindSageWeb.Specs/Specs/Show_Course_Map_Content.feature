Feature: Show_Course_Map_Content
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
    ]
    """  
    And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
			"ExpiredDate": "2/20/2017",
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
                }
            ]
        },
    ]
    """  

@mock  
Scenario: A student request CourseMap's content (Single semester) Then system send the content back  
    Given Today is '2/1/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
			"ExpiredDate": "2/1/2017",
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
                    "BeginDate": "2/6/2016",
                },
                {
                    "Id": "LessonCalendar03",
                    "LessonId": "Lesson03",
                    "Order": 3,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/11/2016",
                },
				{
                    "Id": "LessonCalendar04",
                    "LessonId": "Lesson04",
                    "Order": 4,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/16/2016",
					"DeletedDate": "1/1/2016",
                },
            ]
        },
    ]
    """  
    When UserProfileId 'student@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[
		{
			"SemesterName": "A",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson01",
					"IsLocked": false,
					"LessonWeekName": "Week01",
					"IsCurrent": true
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": true,
					"LessonWeekName": "Week02",
					"IsCurrent": false
				},
				{
					"LessonId": "Lesson03",
					"IsLocked": true,
					"LessonWeekName": "Week03",
					"IsCurrent": false
				}
			]
		}
	]
	"""  

@mock  
Scenario: A student request CourseMap's content (Multi semester) Then system send the content back  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[
		{
			"SemesterName": "A",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson01",
					"IsLocked": false,
					"LessonWeekName": "Week01",
					"IsCurrent": true
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": true,
					"LessonWeekName": "Week02",
					"IsCurrent": false
				},
			]
		},
		{
			"SemesterName": "B",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson03",
					"IsLocked": true,
					"LessonWeekName": "Week03",
					"IsCurrent": false
				}
			]
		},
	]
	"""  

@mock  
Scenario: A student request CourseMap's content (Some lesson was deleted) Then system send the content back  
    Given Today is '2/1/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
			"ExpiredDate": "2/1/2017",
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
				{
                    "Id": "LessonCalendar04",
                    "LessonId": "Lesson04",
                    "Order": 4,
                    "SemesterGroupName": "B",
                    "BeginDate": "2/22/2016",
					"DeletedDate": "1/1/2016",
                },
            ]
        },
    ]
    """  
    When UserProfileId 'student@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[
		{
			"SemesterName": "A",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson01",
					"IsLocked": false,
					"LessonWeekName": "Week01",
					"IsCurrent": true
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": true,
					"LessonWeekName": "Week02",
					"IsCurrent": false
				},
			]
		},
		{
			"SemesterName": "B",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson03",
					"IsLocked": true,
					"LessonWeekName": "Week03",
					"IsCurrent": false
				}
			]
		},
	]
	"""  

@mock  
Scenario: A student request CourseMap's content when time is the last week of the course Then system send the content back  
    Given Today is '2/15/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[
		{
			"SemesterName": "A",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson01",
					"IsLocked": false,
					"LessonWeekName": "Week01",
					"IsCurrent": false
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": false,
					"LessonWeekName": "Week02",
					"IsCurrent": false
				},
			]
		},
		{
			"SemesterName": "B",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson03",
					"IsLocked": false,
					"LessonWeekName": "Week03",
					"IsCurrent": true
				}
			]
		},
	]
	"""  

@mock  
Scenario: A student request CourseMap's content when the course was expired Then system send the content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId 'student@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[
		{
			"SemesterName": "A",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson01",
					"IsLocked": false,
					"LessonWeekName": "Week01",
					"IsCurrent": false
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": false,
					"LessonWeekName": "Week02",
					"IsCurrent": false
				},
			]
		},
		{
			"SemesterName": "B",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson03",
					"IsLocked": false,
					"LessonWeekName": "Week03",
					"IsCurrent": true
				}
			]
		},
	]
	"""  

@mock  
Scenario: A student request CourseMap's content when the course was closed Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
			"ExpiredDate": "2/20/2017",
			"CloseDate": "2/21/2017",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "LessonId": "Lesson01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/1/2016",
                }
            ]
        }
    ]
    """  
    When UserProfileId 'student@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A student request CourseMap's content when the course was deleted Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
			"ExpiredDate": "2/20/2017",
			"DeletedDate": "2/1/2017",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "LessonId": "Lesson01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/1/2016",
                }
            ]
        }
    ]
    """  
    When UserProfileId 'student@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A student request CourseMap's content when the course doesn't existing Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
    When UserProfileId 'student@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A student request CourseMap's content but its subscription was deleted Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
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
					"DeletedDate": "1/1/2016",
				},
			]
		},
    ]
    """  
    When UserProfileId 'student@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A student request CourseMap's content but its subscription doesn't existing Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "student@mindsage.com",
			"Name": "student",
			"ImageProfileUrl": "student.jpg",
			"Subscriptions": []
		},
    ]
    """  
    When UserProfileId 'student@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content (Single semester) Then system send the content back  
    Given Today is '2/1/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
			"ExpiredDate": "2/1/2017",
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
                    "BeginDate": "2/6/2016",
                },
                {
                    "Id": "LessonCalendar03",
                    "LessonId": "Lesson03",
                    "Order": 3,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/11/2016",
                },
				{
                    "Id": "LessonCalendar04",
                    "LessonId": "Lesson04",
                    "Order": 4,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/16/2016",
					"DeletedDate": "1/1/2016",
                },
            ]
        },
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[
		{
			"SemesterName": "A",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson01",
					"IsLocked": false,
					"LessonWeekName": "Week01",
					"IsCurrent": true
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": true,
					"LessonWeekName": "Week02",
					"IsCurrent": false
				},
				{
					"LessonId": "Lesson03",
					"IsLocked": true,
					"LessonWeekName": "Week03",
					"IsCurrent": false
				}
			]
		}
	]
	"""  

@mock  
Scenario: A teacher request CourseMap's content (Multi semester) Then system send the content back  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[
		{
			"SemesterName": "A",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson01",
					"IsLocked": false,
					"LessonWeekName": "Week01",
					"IsCurrent": true
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": true,
					"LessonWeekName": "Week02",
					"IsCurrent": false
				},
			]
		},
		{
			"SemesterName": "B",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson03",
					"IsLocked": true,
					"LessonWeekName": "Week03",
					"IsCurrent": false
				}
			]
		},
	]
	"""  

@mock  
Scenario: A teacher request CourseMap's content (Some lesson was deleted) Then system send the content back  
    Given Today is '2/1/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
			"ExpiredDate": "2/1/2017",
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
				{
                    "Id": "LessonCalendar04",
                    "LessonId": "Lesson04",
                    "Order": 4,
                    "SemesterGroupName": "B",
                    "BeginDate": "2/22/2016",
					"DeletedDate": "1/1/2016",
                },
            ]
        },
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[
		{
			"SemesterName": "A",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson01",
					"IsLocked": false,
					"LessonWeekName": "Week01",
					"IsCurrent": true
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": true,
					"LessonWeekName": "Week02",
					"IsCurrent": false
				},
			]
		},
		{
			"SemesterName": "B",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson03",
					"IsLocked": true,
					"LessonWeekName": "Week03",
					"IsCurrent": false
				}
			]
		},
	]
	"""  

@mock  
Scenario: A teacher request CourseMap's content when time is the last week of the course Then system send the content back  
    Given Today is '2/15/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[
		{
			"SemesterName": "A",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson01",
					"IsLocked": false,
					"LessonWeekName": "Week01",
					"IsCurrent": false
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": false,
					"LessonWeekName": "Week02",
					"IsCurrent": false
				},
			]
		},
		{
			"SemesterName": "B",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson03",
					"IsLocked": false,
					"LessonWeekName": "Week03",
					"IsCurrent": true
				}
			]
		},
	]
	"""  

@mock  
Scenario: A teacher request CourseMap's content when the course was expired Then system send the content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[
		{
			"SemesterName": "A",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson01",
					"IsLocked": false,
					"LessonWeekName": "Week01",
					"IsCurrent": false
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": false,
					"LessonWeekName": "Week02",
					"IsCurrent": false
				},
			]
		},
		{
			"SemesterName": "B",
			"LessonStatus":
			[
				{
					"LessonId": "Lesson03",
					"IsLocked": false,
					"LessonWeekName": "Week03",
					"IsCurrent": true
				}
			]
		},
	]
	"""  

@mock  
Scenario: A teacher request CourseMap's content when the course was closed Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
			"ExpiredDate": "2/20/2017",
			"CloseDate": "2/21/2017",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "LessonId": "Lesson01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/1/2016",
                }
            ]
        }
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content when the course was deleted Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
			"ExpiredDate": "2/20/2017",
			"DeletedDate": "2/1/2017",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "LessonId": "Lesson01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/1/2016",
                }
            ]
        }
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content when the course doesn't existing Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content but its subscription was deleted Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
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
					"id": "Subscription02",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016",
				},
			]
		},
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content but its subscription doesn't existing Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Name": "teacher",
			"ImageProfileUrl": "teacher.jpg",
			"Subscriptions": []
		},
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content but input ClassCalendarId incorrect (unknow) Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'UnknowClassCalendar'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content but input ClassCalendarId incorrect (empty) Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: ''  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content but input ClassCalendarId incorrect (null) Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'NULL'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content but input ClassRoom incorrect (unknow) Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'UnknowClassRoom' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content but input ClassRoom incorrect (empty) Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: '' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher request CourseMap's content but input ClassRoom incorrect (null) Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'NULL' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: Incorrec user (unknow) request CourseMap's content Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId 'unknow@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: Incorrec user (empty) request CourseMap's content Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId '' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: Incorrec user (null) request CourseMap's content Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
    When UserProfileId 'NULL' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

@mock  
Scenario: A teacher user request CourseMap's content but its account was deleted Then system send an empty content back  
    Given Today is '2/25/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Name": "teacher",
			"ImageProfileUrl": "teacher.jpg",
			"DeletedDate": "1/1/2016",
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
    When UserProfileId 'teacher@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map content collection with JSON format are  
	"""
	[]
	"""  

#Incorrect user deleted & doesn't existing