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
					"CourseCatalogId": "CourseCatalog01"
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
			"PriceUSD": 80.01,
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
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "1/1/2016",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "LessonId": "Lesson01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "1/1/2016",
                }
            ]
        },
    ]
    """  
    
@mock  
Scenario: New account buy new course success Then system record the payment and add new course to the user  
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
	Then System record the payment information with JSON format is  
	"""  
	{
		"FirstName": "fresh",
		"LastName": "lastname",
		"Last4Digits": "7862",
		"CardType": "Visa",
		"CardNumber": "48772749xxxx7862",
		"TotalChargedAmount": 80.01,
		"BillingAddress": "799 E DRAGRAM SUITE 5A",
		"State": "CA",
		"City": "SEATTLE",
		"Country": "US",
		"ZipCode": "85705",
		"CourseName": "COMPLETE 7th GRADE COURSE",
		"IsCompleted": true,
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "1/1/2016"
	}
	"""  
	And System set course ClassCalendar collection with JSON format is  
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

@mock  
Scenario: New account buy new course failed Then system record the failed payment but doesn't add new course to the user  
    Given Today is '1/1/2016 00:00 am'  
	And Payment system will return result 'failed'
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
	Then System record the payment information with JSON format is  
	"""  
	{
		"FirstName": "fresh",
		"LastName": "lastname",
		"Last4Digits": "7862",
		"CardType": "Visa",
		"CardNumber": "48772749xxxx7862",
		"TotalChargedAmount": 80.01,
		"BillingAddress": "799 E DRAGRAM SUITE 5A",
		"State": "CA",
		"City": "SEATTLE",
		"Country": "US",
		"ZipCode": "85705",
		"CourseName": "COMPLETE 7th GRADE COURSE",
		"IsCompleted": false,
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "1/1/2016"
	}
	"""  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  

@mock  
Scenario: An account try to buy the same course Then system do nothing  
    Given Today is '1/1/2016 00:00 am'  
	And Payment system will return result 'failed'
    When UserProfileId 'old@mindsage.com' buy new course by using information in JSON format is  
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  

@mock  
Scenario: A user buy a course but used incorrect CourseId (unknow) Then system do nithing  
    Given Today is '1/1/2016 00:00 am'  
	And Payment system will return result 'approved'
    When UserProfileId 'fresh@mindsage.com' buy new course by using information in JSON format is  
	"""  
	{
		"CourseId": "Unknow",
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  

@mock  
Scenario: A user buy a course but used incorrect CourseId (empty) Then system do nithing  
    Given Today is '1/1/2016 00:00 am'  
	And Payment system will return result 'approved'
    When UserProfileId 'fresh@mindsage.com' buy new course by using information in JSON format is  
	"""  
	{
		"CourseId": "",
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  

@mock  
Scenario: A user buy a course but used incorrect CourseId (null) Then system do nithing  
    Given Today is '1/1/2016 00:00 am'  
	And Payment system will return result 'approved'
    When UserProfileId 'fresh@mindsage.com' buy new course by using information in JSON format is  
	"""  
	{
		"CourseId": "NULL",
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  

@mock  
Scenario: A user buy a course but the selected course was deleted Then system do nithing  
    Given Today is '1/1/2016 00:00 am'  
	And System have CourseCatalog collection with JSON format are  
	"""  
	[
		{
			"id": "CourseCatalog01",
			"Grade": 7,
			"SideName": "COMPLETE 7th GRADE COURSE",
			"PriceUSD": 80.01,
			"Series": "Mind Sage Middle School Program: 6th,7th & 8th Grades",
			"Title": "GUIDING THE NATURAL DESIRE TO BECOME INDEPENDENT THINKERS",
			"FullDescription": "Middle school is a transitional period for students. They are no longer children, but entering phase were they in lift affords quite adults either. This wonderful period in life affords teachers the opportunity to build on the awareness of the Elementary MindSage course, developing and guiding the students as they natural begin to push for indepedents and control of their lives.",
			"DescriptionImageUrl": "http://placehold.it/350x110",
			"TotalWeeks": 1,
			"CreatedDate": "1/1/2016",
			"RelatedCourses": [],
			"Semesters": [],
			"DeletedDate": "1/1/2016"
		}
	]
	"""  
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  

@mock  
Scenario: A user buy a course but the selected course doesn't existing Then system do nithing  
    Given Today is '1/1/2016 00:00 am'  
	And System have CourseCatalog collection with JSON format are  
	"""  
	[]
	"""  
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  

@mock  
Scenario: Invalid user (unknow) buy a course Then system do nithing  
    Given Today is '1/1/2016 00:00 am'  
	And Payment system will return result 'approved'
    When UserProfileId 'unknow@mindsage.com' buy new course by using information in JSON format is  
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  

@mock  
Scenario: Invalid user (empty) buy a course Then system do nithing  
    Given Today is '1/1/2016 00:00 am'  
	And Payment system will return result 'approved'
    When UserProfileId '' buy new course by using information in JSON format is  
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  
	
@mock  
Scenario: Invalid user (null) buy a course Then system do nithing  
    Given Today is '1/1/2016 00:00 am'  
	And Payment system will return result 'approved'
    When UserProfileId 'NULL' buy new course by using information in JSON format is  
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  

@mock  
Scenario: Invalid user (the accout was deleted) buy a course Then system do nithing  
    Given Today is '1/1/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "fresh@mindsage.com",
			"Name": "fresh",
			"ImageProfileUrl": "fresh.jpg",
			"Subscriptions": [],
			"DeletedDate": "1/1/2016"
		}
    ]
    """  
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  

@mock  
Scenario: An account who already have course subscription but its was deleted buy the same course Then system record the payment and add new course to the user  
    Given Today is '1/1/2016 00:00 am'  
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
					"CourseCatalogId": "CourseCatalog01",
					"DeletedDate": "1/1/2016"
				}
			]
		}
    ]
    """  
	And Payment system will return result 'approved'
    When UserProfileId 'old@mindsage.com' buy new course by using information in JSON format is  
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
	Then System record the payment information with JSON format is  
	"""  
	{
		"FirstName": "fresh",
		"LastName": "lastname",
		"Last4Digits": "7862",
		"CardType": "Visa",
		"CardNumber": "48772749xxxx7862",
		"TotalChargedAmount": 80.01,
		"BillingAddress": "799 E DRAGRAM SUITE 5A",
		"State": "CA",
		"City": "SEATTLE",
		"Country": "US",
		"ZipCode": "85705",
		"CourseName": "COMPLETE 7th GRADE COURSE",
		"IsCompleted": true,
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "1/1/2016"
	}
	"""  
	And System set course ClassCalendar collection with JSON format is  
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
	And System upsert UserProfileId 'old@mindsage.com' for update new subscription with JSON format are  
    """
	[
		{
			"id": "Subscription01",
			"Role": "SelfPurchaser",
			"ClassRoomId": "ClassRoom01",
			"ClassCalendarId": "ClassCalendar01",
			"CourseCatalogId": "CourseCatalog01",
			"DeletedDate": "1/1/2016"
		},
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
		"UserProfileId": "old@mindsage.com",
		"ClassRoomId": "ClassRoom01",
		"CreatedDate": "1/1/2016",
		"UserProfileName": "old",
		"UserProfileImageUrl": "old.jpg",
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
#no user

@mock  
Scenario: New account buy new course but ClassRoom was deleted Then system do nothing  
    Given Today is '1/1/2016 00:00 am'  
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
                }
            ],
			"DeletedDate": "1/1/2016"
        }
    ]
    """  
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  
	
@mock  
Scenario: New account buy new course but ClassRoom doesn't existing Then system do nothing  
    Given Today is '1/1/2016 00:00 am'  
	And System have ClassRoom collection with JSON format are  
    """
    []
    """  
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
	Then System doesn't record the payment information  
	And System doesn't set course ClassCalendar  
	And System doesn't upsert UserProfile   
	And System doesn't update UserActivity  