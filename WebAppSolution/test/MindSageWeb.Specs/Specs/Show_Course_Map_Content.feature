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
    ]
    """  
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

@mock  
Scenario: User request course map's content Then system send the content back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'sakul@mindsage.com' reuqest course map content of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
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
				},
				{
					"LessonId": "Lesson02",
					"IsLocked": false,
					"LessonWeekName": "Week02",
					"IsCurrent": true
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
				}
			]
		},
	]
	"""