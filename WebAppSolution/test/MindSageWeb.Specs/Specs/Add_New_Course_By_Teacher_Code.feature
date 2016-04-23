Feature: Add_New_Course_By_Teacher_Code
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
			"Name": "Sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions": []
		}
    ]
    """  
	And System have Contract collection with JSON format are  
	"""  
	[
		{
			"id": "Contract01",
			"SchoolName": "SchoolName01",
			"State": "CA",
			"ZipCode": "95123",
			"CreatedDate": "1/1/2016",
			"Licenses": [
				{
					"id": "License01",
					"CourseCatalogId": "CourseCatalog01",
					"Grade": 7,
					"TeacherKeys": [
						{
							"id": "TeacherKey01",
							"Grade": 7,
							"Code": "07CA95123U48PSchoolName",
							"CreatedDate": "1/1/2016"
						}
					]
				}
			]
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
			"CreatedDate": "1/1/2016"
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
			"TopicOfTheDays": []
        },
    ]
    """  
    And System have StudentKey collection with JSON format are  
	"""
	[
		{
			"id": "StudentKey01",
			"Code": "StudentCode01",
			"Grade": "Grade01",
			"CourseCatalogId": "CourseCatalog01",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "2/1/2016",
		}
	]
	"""  

@mock  
Scenario: User add new course by using the right teacher code Then system add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System create new ClassRoom with JSON format is  
	"""  
	{
		"Name": "COMPLETE 7th GRADE COURSE",
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "2/8/2016 00:00 am",
		"LastUpdatedMessageDate": "2/8/2016 00:00 am",
		"Lessons": [
			{
				"id": "LessonCatalog01",
				"LessonCatalogId": "LessonCatalog01",
			}
		]
	}
	"""  
	And System create new ClassCalendar with JSON format is  
	"""  
	{
		"CreatedDate": "2/8/2016 00:00 am",
		"LessonCalendars": [
			{
				"Order": 1,
				"SemesterGroupName": "A",
				"BeginDate": "2/8/2016 00:00 am",
				"CreatedDate": "2/8/2016 00:00 am",
				"LessonId": "LessonCatalog01",
				"TopicOfTheDays": []
			}
		]
	}
	"""  
	And System upsert user id 'sakul@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"LessonId": "LessonCatalog01",
			"TotalContentsAmount": 4,
			"SawContentIds": [],
			"CreatedCommentAmount": 0,
			"ParticipationAmount": 0,
			"BeginDate": "2/8/2016 00:00 am",
		}
	]
    """  
	And System add new teacher subscription for user id 'sakul@mindsage.com' collection with JSON format are  
    """
	[
		{
			"Role": "Teacher",
			"ClassRoomName": "COMPLETE 7th GRADE COURSE",
			"LicenseId": "License01",
			"CourseCatalogId": "CourseCatalog01",
			"CreatedDate": "2/8/2016 00:00 am",
			"LastActiveDate": "2/8/2016 00:00 am",
		}
	]
    """  
	And System create new StudentKey with JSON format is  
	"""  
	{
		"Grade": 7,
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "2/8/2016 00:00 am",
	}
	"""  

@mock  
Scenario: User add new course by using the right teacher code (more than one lessone) Then system add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
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
        },
		{
            "id": "LessonCatalog02",
			"Order": 2,
			"SemesterName": "A",
			"CourseCatalogId": "CourseCatalog01",
            "Title": "Why should we learn?",
            "ShortTeacherLessonPlan": "short teacher lesson plan02",
            "FullTeacherLessonPlan": "full teacher lesson plan02",
			"PrimaryContentUrl": "PrimaryContent02",
			"ExtraContents": [ 
				{
					"id": "Extra04",
					"ContentURL": "www.extracontent04.com",
					"Description": "description04",
					"IconURL": "extra04.jpg"
				},
				{
					"id": "Extra05",
					"ContentURL": "www.extracontent05.com",
					"Description": "description05",
					"IconURL": "extra05.jpg"
				},
				{
					"id": "Extra06",
					"ContentURL": "www.extracontent06.com",
					"Description": "description06",
					"IconURL": "extra06.jpg"
				}
			],
			"TopicOfTheDays": [
				{
					"id": "TOTD03",
					"Message": "Message03",
					"SendOnDay": 2,
					"CreatedDate": "1/1/2016"
				},
				{
					"id": "TOTD04",
					"Message": "Message04",
					"SendOnDay": 5,
					"CreatedDate": "1/1/2016"
				}
			]
        },
    ]
    """  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System create new ClassRoom with JSON format is  
	"""  
	{
		"Name": "COMPLETE 7th GRADE COURSE",
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "2/8/2016 00:00 am",
		"LastUpdatedMessageDate": "2/8/2016 00:00 am",
		"Lessons": [
			{
				"id": "LessonCatalog01",
				"LessonCatalogId": "LessonCatalog01",
			},
			{
				"id": "LessonCatalog02",
				"LessonCatalogId": "LessonCatalog02",
			}
		]
	}
	"""  
	And System create new ClassCalendar with JSON format is  
	"""  
	{
		"CreatedDate": "2/8/2016 00:00 am",
		"LessonCalendars": [
			{
				"Order": 1,
				"SemesterGroupName": "A",
				"BeginDate": "2/8/2016 00:00 am",
				"CreatedDate": "2/8/2016 00:00 am",
				"LessonId": "LessonCatalog01",
				"TopicOfTheDays": [
					{
						"id": "TOTD01",
						"Message": "Message01",
						"SendOnDay": 1,
						"CreatedDate": "2/8/2016 00:00 am"
					},
					{
						"id": "TOTD02",
						"Message": "Message02",
						"SendOnDay": 3,
						"CreatedDate": "2/8/2016 00:00 am"
					}
				]
			},
			{
				"Order": 2,
				"SemesterGroupName": "A",
				"BeginDate": "2/8/2016 00:00 am",
				"CreatedDate": "2/8/2016 00:00 am",
				"LessonId": "LessonCatalog02",
				"TopicOfTheDays": [
					{
						"id": "TOTD03",
						"Message": "Message03",
						"SendOnDay": 2,
						"CreatedDate": "2/8/2016 00:00 am"
					},
					{
						"id": "TOTD04",
						"Message": "Message04",
						"SendOnDay": 5,
						"CreatedDate": "2/8/2016 00:00 am"
					}
				]
			}
		]
	}
	"""  
	And System upsert user id 'sakul@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"LessonId": "LessonCatalog01",
			"TotalContentsAmount": 4,
			"SawContentIds": [],
			"CreatedCommentAmount": 0,
			"ParticipationAmount": 0,
			"BeginDate": "2/8/2016 00:00 am",
		},
		{
			"LessonId": "LessonCatalog02",
			"TotalContentsAmount": 4,
			"SawContentIds": [],
			"CreatedCommentAmount": 0,
			"ParticipationAmount": 0,
			"BeginDate": "2/8/2016 00:00 am",
		}
	]
    """  
	And System add new teacher subscription for user id 'sakul@mindsage.com' collection with JSON format are  
    """
	[
		{
			"Role": "Teacher",
			"ClassRoomName": "COMPLETE 7th GRADE COURSE",
			"LicenseId": "License01",
			"CourseCatalogId": "CourseCatalog01",
			"CreatedDate": "2/8/2016 00:00 am",
			"LastActiveDate": "2/8/2016 00:00 am",
		}
	]
    """  
	And System create new StudentKey with JSON format is  
	"""  
	{
		"Grade": 7,
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "2/8/2016 00:00 am",
	}
	"""  

@mock  
Scenario: User add new course by using a incorrect teacher code Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code 'IncorrectCode' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course by using a incorrect teacher grade Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade 'IncorrectGrade'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course by using a incorrect teacher code and grade Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code 'IncorrectCode' and grade 'IncorrectGrade'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: Incorrect user add new course by using a correct teacher code Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'unknow@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course by using a incorrect teacher code (empty) Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course by using a incorrect teacher grade (empty) Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade ''
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course by using a incorrect teacher code and grade (empty) Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '' and grade ''
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: Incorrect user add new course by using a correct teacher code (empty) Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile '' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course by using a incorrect teacher code (null) Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code 'NULL' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course by using a incorrect teacher grade (null) Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade 'NULL'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course by using a incorrect teacher code and grade (null) Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code 'NULL' and grade 'NULL'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: Incorrect user add new course by using a correct teacher code (null) Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'NULL' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course but contracts in system doens't existing Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
	And System have Contract collection with JSON format are  
	"""  
	[]
	"""  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  


@mock  
Scenario: User add new course but licenses in system doens't existing Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
	And System have Contract collection with JSON format are  
	"""  
	[
		{
			"id": "Contract01",
			"SchoolName": "SchoolName01",
			"State": "CA",
			"ZipCode": "95123",
			"CreatedDate": "1/1/2016",
			"Licenses": []
		}
	]
	"""  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course but TeacherKeys in system doens't existing Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
	And System have Contract collection with JSON format are  
	"""  
	[
		{
			"id": "Contract01",
			"SchoolName": "SchoolName01",
			"State": "CA",
			"ZipCode": "95123",
			"CreatedDate": "1/1/2016",
			"Licenses": [
				{
					"id": "License01",
					"CourseCatalogId": "CourseCatalog01",
					"Grade": 7,
					"TeacherKeys": []
				}
			]
		}
	]
	"""  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course but the contract was deleted Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
	And System have Contract collection with JSON format are  
	"""  
	[
		{
			"id": "Contract01",
			"SchoolName": "SchoolName01",
			"State": "CA",
			"ZipCode": "95123",
			"CreatedDate": "1/1/2016",
			"DeletedDate": "1/1/2016",
			"Licenses": [
				{
					"id": "License01",
					"CourseCatalogId": "CourseCatalog01",
					"Grade": 7,
					"TeacherKeys": [
						{
							"id": "TeacherKey01",
							"Grade": 7,
							"Code": "07CA95123U48PSchoolName",
							"CreatedDate": "1/1/2016"
						}
					]
				}
			]
		}
	]
	"""  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course but the license was deleted Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
	And System have Contract collection with JSON format are  
	"""  
	[
		{
			"id": "Contract01",
			"SchoolName": "SchoolName01",
			"State": "CA",
			"ZipCode": "95123",
			"CreatedDate": "1/1/2016",
			"Licenses": [
				{
					"id": "License01",
					"CourseCatalogId": "CourseCatalog01",
					"Grade": 7,
					"DeletedDate": "1/1/2016",
					"TeacherKeys": [
						{
							"id": "TeacherKey01",
							"Grade": 7,
							"Code": "07CA95123U48PSchoolName",
							"CreatedDate": "1/1/2016"
						}
					]
				}
			]
		}
	]
	"""  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course but the TeacherKey was deleted Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
	And System have Contract collection with JSON format are  
	"""  
	[
		{
			"id": "Contract01",
			"SchoolName": "SchoolName01",
			"State": "CA",
			"ZipCode": "95123",
			"CreatedDate": "1/1/2016",
			"Licenses": [
				{
					"id": "License01",
					"CourseCatalogId": "CourseCatalog01",
					"Grade": 7,
					"TeacherKeys": [
						{
							"id": "TeacherKey01",
							"Grade": 7,
							"Code": "07CA95123U48PSchoolName",
							"CreatedDate": "1/1/2016",
							"DeletedDate": "1/1/2016",
						}
					]
				}
			]
		}
	]
	"""  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System doesn't create new ClassRoom  
	And System doesn't create new ClassCalendar  
	Then System doesn't add new subscription  
	And System doesn't update UserActivity  
	And System doesn't create new StudentKey  

@mock  
Scenario: User add new course all data correct (some licenses was deleted) Then system add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
	And System have Contract collection with JSON format are  
	"""  
	[
		{
			"id": "Contract01",
			"SchoolName": "SchoolName01",
			"State": "CA",
			"ZipCode": "95123",
			"CreatedDate": "1/1/2016",
			"Licenses": [
				{
					"id": "License01",
					"CourseCatalogId": "CourseCatalog01",
					"Grade": 7,
					"DeletedDate": "1/1/2016",
					"TeacherKeys": [
						{
							"id": "TeacherKey01",
							"Grade": 7,
							"Code": "07CA95123U48PSchoolName",
							"CreatedDate": "1/1/2016"
						}
					]
				},
				{
					"id": "License02",
					"CourseCatalogId": "CourseCatalog01",
					"Grade": 7,
					"TeacherKeys": [
						{
							"id": "TeacherKey01",
							"Grade": 7,
							"Code": "07CA95123U48PSchoolName",
							"CreatedDate": "1/1/2016"
						}
					]
				}
			]
		}
	]
	"""  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System create new ClassRoom with JSON format is  
	"""  
	{
		"Name": "COMPLETE 7th GRADE COURSE",
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "2/8/2016 00:00 am",
		"LastUpdatedMessageDate": "2/8/2016 00:00 am",
		"Lessons": [
			{
				"id": "LessonCatalog01",
				"LessonCatalogId": "LessonCatalog01",
			}
		]
	}
	"""  
	And System create new ClassCalendar with JSON format is  
	"""  
	{
		"CreatedDate": "2/8/2016 00:00 am",
		"LessonCalendars": [
			{
				"Order": 1,
				"SemesterGroupName": "A",
				"BeginDate": "2/8/2016 00:00 am",
				"CreatedDate": "2/8/2016 00:00 am",
				"LessonId": "LessonCatalog01",
				"TopicOfTheDays": []
			}
		]
	}
	"""  
	And System upsert user id 'sakul@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"LessonId": "LessonCatalog01",
			"TotalContentsAmount": 4,
			"SawContentIds": [],
			"CreatedCommentAmount": 0,
			"ParticipationAmount": 0,
			"BeginDate": "2/8/2016 00:00 am",
		}
	]
    """  
	And System add new teacher subscription for user id 'sakul@mindsage.com' collection with JSON format are  
    """
	[
		{
			"Role": "Teacher",
			"ClassRoomName": "COMPLETE 7th GRADE COURSE",
			"LicenseId": "License02",
			"CourseCatalogId": "CourseCatalog01",
			"CreatedDate": "2/8/2016 00:00 am",
			"LastActiveDate": "2/8/2016 00:00 am",
		}
	]
    """  
	And System create new StudentKey with JSON format is  
	"""  
	{
		"Grade": 7,
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "2/8/2016 00:00 am",
	}
	"""  

@mock  
Scenario: User add new course all data correct (some teacheTeacherKey was deleted) Then system add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
	And System have Contract collection with JSON format are  
	"""  
	[
		{
			"id": "Contract01",
			"SchoolName": "SchoolName01",
			"State": "CA",
			"ZipCode": "95123",
			"CreatedDate": "1/1/2016",
			"Licenses": [
				{
					"id": "License01",
					"CourseCatalogId": "CourseCatalog01",
					"Grade": 7,
					"TeacherKeys": [
						{
							"id": "TeacherKey01",
							"Grade": 7,
							"Code": "07CA95123U48PSchoolName",
							"CreatedDate": "1/1/2016",
							"DeletedDate": "1/1/2016"
						},
						{
							"id": "TeacherKey02",
							"Grade": 7,
							"Code": "07CA95123U48PSchoolName",
							"CreatedDate": "1/1/2016"
						}
					]
				}
			]
		}
	]
	"""  
    When UserProfile 'sakul@mindsage.com' Add new course by using teteacher code '07CA95123U48PSchoolName' and grade '7'
    Then System create new ClassRoom with JSON format is  
	"""  
	{
		"Name": "COMPLETE 7th GRADE COURSE",
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "2/8/2016 00:00 am",
		"LastUpdatedMessageDate": "2/8/2016 00:00 am",
		"Lessons": [
			{
				"id": "LessonCatalog01",
				"LessonCatalogId": "LessonCatalog01",
			}
		]
	}
	"""  
	And System create new ClassCalendar with JSON format is  
	"""  
	{
		"CreatedDate": "2/8/2016 00:00 am",
		"LessonCalendars": [
			{
				"Order": 1,
				"SemesterGroupName": "A",
				"BeginDate": "2/8/2016 00:00 am",
				"CreatedDate": "2/8/2016 00:00 am",
				"LessonId": "LessonCatalog01",
				"TopicOfTheDays": []
			}
		]
	}
	"""  
	And System upsert user id 'sakul@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"LessonId": "LessonCatalog01",
			"TotalContentsAmount": 4,
			"SawContentIds": [],
			"CreatedCommentAmount": 0,
			"ParticipationAmount": 0,
			"BeginDate": "2/8/2016 00:00 am",
		}
	]
    """  
	And System add new teacher subscription for user id 'sakul@mindsage.com' collection with JSON format are  
    """
	[
		{
			"Role": "Teacher",
			"ClassRoomName": "COMPLETE 7th GRADE COURSE",
			"LicenseId": "License01",
			"CourseCatalogId": "CourseCatalog01",
			"CreatedDate": "2/8/2016 00:00 am",
			"LastActiveDate": "2/8/2016 00:00 am",
		}
	]
    """  
	And System create new StudentKey with JSON format is  
	"""  
	{
		"Grade": 7,
		"CourseCatalogId": "CourseCatalog01",
		"CreatedDate": "2/8/2016 00:00 am",
	}
	"""  