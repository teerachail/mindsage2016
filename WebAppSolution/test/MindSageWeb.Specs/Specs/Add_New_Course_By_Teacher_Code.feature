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