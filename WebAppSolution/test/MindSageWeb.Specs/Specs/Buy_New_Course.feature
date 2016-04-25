Feature: Buy_New_Course
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "old@mindsage.com",
			"Name": "old",
			"ImageProfileUrl": "old.jpg",
			"Subscriptions": [
				{
					"id": "Subscription01",
					"Role": "SelfPurchaser",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "fresh@mindsage.com",
			"Name": "fresh",
			"ImageProfileUrl": "fresh.jpg",
			"Subscriptions": []
		}
    ]
    """  
    And System have CourseCatalog collection with JSON format are  
	"""  
	[
		{
			"id": "CourseCatalog01",
			"Grade": 7,
			"SideName": "COMPLETE 7th GRADE COURSE",
			"PriceUSD": 80.00,
			"Series": "Mind Sage Middle School Program: 6th,7th & 8th Grades",
			"Title": "GUIDING THE NATURAL DESIRE TO BECOME INDEPENDENT THINKERS",
			"FullDescription": "Middle school is a transitional period for students. They are no longer children, but entering phase were they in lift affords quite adults either. This wonderful period in life affords teachers the opportunity to build on the awareness of the Elementary MindSage course, developing and guiding the students as they natural begin to push for indepedents and control of their lives.",
			"DescriptionImageUrl": "http://placehold.it/350x110",
			"TotalWeeks": 1,
			"CreatedDate": "1/1/2016",
			"RelatedCourses": [],
			"Semesters": []
		}
	]
	"""  
	And System have LessonCatalog collection with JSON format are  
    """  
    [
        {
            "id": "LessonCatalog01",
			"Order": 1,
			"SemesterName": "A",
			"CourseCatalogId": "CourseCatalog01",
            "Title": "What Is Emotional Literacy?",
            "ShortTeacherLessonPlan": "short teacher lesson plan01",
            "FullTeacherLessonPlan": "full teacher lesson plan01",
			"PrimaryContentUrl": "PrimaryContent01",
			"ExtraContents": [ 
				{
					"id": "Extra01",
					"ContentURL": "www.extracontent01.com",
					"Description": "description01",
					"IconURL": "extra01.jpg"
				},
				{
					"id": "Extra02",
					"ContentURL": "www.extracontent02.com",
					"Description": "description03",
					"IconURL": "extra02.jpg"
				},
				{
					"id": "Extra03",
					"ContentURL": "www.extracontent03.com",
					"Description": "description03",
					"IconURL": "extra03.jpg"
				}
			],
			"TopicOfTheDays": [
				{
					"id": "TOTD01",
					"Message": "Message01",
					"SendOnDay": 1,
					"CreatedDate": "1/1/2016"
				},
				{
					"id": "TOTD02",
					"Message": "Message02",
					"SendOnDay": 3,
					"CreatedDate": "1/1/2016"
				}
			]
        }
    ]
    """  
	And System have ClassRoom collection with JSON format are  
    """
    [
        {
            "id": "ClassRoom01",
            "Name": "Emotional literacy",
            "CourseCatalogId": "CourseCatalog01",
            "CreatedDate": "1/1/2016",
			"IsPublic": true,
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
    
@mock  
Scenario: New account buy new course success Then system add new course to the user  
    Given Today is '1/1/2016 00:00 am'  
	And Payment system will return result 'approved'
    When UserProfileId 'fresh@mindsage.com' buy new course by using information in JSON format is  
	"""  
	{
		"CourseId": "CourseCatalog01",
		"CreditCardInfo":
		{
			"FirstName": "fresh",
			"LastName": "lastname",
			"CardNumber": "4877274905927862",
			"CardType": "Visa",
			"ExpiredMonth": "11",
			"ExpiredYear": "2018",
			"CVV": "874"
		},
		"PrimaryAddress":
		{
			"Address": "799 E DRAGRAM SUITE 5A",
			"State": "CA",
			"City": "SEATTLE",
			"Country": "US",
			"ZipCode": "85705"
		}
	}
	"""  
	Then System set course ClassCalendar collection with JSON format is  
	"""  
	{
		"BeginDate": "1/1/2016",
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
				"TopicOfTheDays": 
				[
					{
						"id": "TOTD01",
						"Message": "Message01",
						"SendOnDay": 1,
						"CreatedDate": "1/1/2016",
						"RequiredSendTopicOfTheDayDate": "1/1/2016"
					},
					{
						"id": "TOTD02",
						"Message": "Message02",
						"SendOnDay": 3,
						"CreatedDate": "1/1/2016",
						"RequiredSendTopicOfTheDayDate": "1/3/2016"
					}
				]
            }
        ],
		"Holidays": [],
		"ShiftDays": []
    }
	"""  
	And System upsert UserProfileId 'fresh@mindsage.com' for update new subscription with JSON format are  
    """
	[
		{
			"Role": "SelfPurchaser",
			"ClassRoomId": "ClassRoom01",
			"ClassRoomName": "Emotional literacy",
			"CourseCatalogId": "CourseCatalog01",
			"LastActiveDate": "1/1/2016",
			"CreatedDate": "1/1/2016"
		}
	]
    """  
	And System create new UserActivity with JSON format is
    """
    {
		"UserProfileId": "fresh@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
		"UserProfileName": "fresh",
		"UserProfileImageUrl": "fresh.jpg",
		"LessonActivities":
		[
			{
				"LessonId": "Lesson01",
				"BeginDate": "1/1/2016",
				"SawContentIds": [],
				"TotalContentsAmount": 4,
				"ParticipationAmount": 0,
				"CreatedCommentAmount": 0,
			}
		]
	}
    """  