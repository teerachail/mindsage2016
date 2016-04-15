Feature: Add_New_Course_By_Student_Code
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
			"Name": "Teacher",
			"ImageProfileUrl": "teacher.jpg",
			"Subscriptions": [
				{
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"CreatedDate": "1/1/2016 00:00 am",
					"ClassRoomName": "Emotional literacy",
					"LicenseId": "License01"
				}
			]
		},
		{
			"id": "sakul@mindsage.com",
			"Name": "Sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions": []
		},
    ]
    """  
	And System have ClassRoom collection with JSON format are  
    """
    [
        {
            "id": "ClassRoom01",
            "Name": "Emotional literacy",
            "CourseCatalogId": "CourseCatalog01",
            "CreatedDate": "2/1/2016",
            "Message": "Don't forget to comment a lesson!",
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
	And System have LessonCatalog collection with JSON format are
    """  
    [
        {
            "id": "LessonCatalog01",
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
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "LessonId": "Lesson01",
                    "BeginDate": "2/1/2016",
                    "LessonCatalogId": "LessonCatalog01"
                },
            ]
        },
    ]
    """  
	And System have UserActivity collection with JSON format are
    """
    [
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
Scenario: User add new course by using the right code Then system add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using code 'StudentCode01' and grade 'Grade01'
    Then System upsert user id 'sakul@mindsage.com' UserProfile's subscriptions collection with JSON format are  
    """
	[
		{
			"Role": "Student",
			"ClassRoomId": "ClassRoom01",
			"ClassCalendarId": "ClassCalendar01",
			"CreatedDate": "2/8/2016 00:00 am",
			"ClassRoomName": "Emotional literacy",
			"LicenseId": "License01",
			"CourseCatalogId": "CourseCatalog01",
			"LastActiveDate": "2/8/2016 00:00 am",
		}
	]
    """  
	And System upsert user id 'sakul@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"LessonId": "Lesson01",
			"TotalContentsAmount": 4,
			"SawContentIds": [],
			"CreatedCommentAmount": 0,
			"ParticipationAmount": 0,
			"BeginDate": "2/1/2016",
		}
	]
    """  

@mock  
Scenario: User add new course by using the incorrect code Then system don't add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using code 'IncorrectCode' and grade 'Grade01'
    Then System doesn't add new subscription  
	And System doesn't add UserActivity  

@mock  
Scenario: User add new course by using the right code but incorrect grade Then system don't add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using code 'StudentCode01' and grade 'IncorrectGrade'
    Then System doesn't add new subscription  
	And System doesn't add UserActivity  

@mock  
Scenario: User add new course by using code and grade are incorrect Then system don't add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' Add new course by using code 'IncorrectCode' and grade 'IncorrectGrade'
    Then System doesn't add new subscription  
	And System doesn't add UserActivity  

@mock  
Scenario: User add new course by using the right code but the class doesn't have a teacher account Then system doesn't add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "Sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions": []
		},
    ]
    """  
    When UserProfile 'sakul@mindsage.com' Add new course by using code 'StudentCode01' and grade 'Grade01'
	Then System doesn't add new subscription  
	And System doesn't add UserActivity  

@mock  
Scenario: User add new course by using the right code but the ClassRoom doesn't existing Then system doesn't add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassRoom collection with JSON format are  
    """
    []
    """  
    When UserProfile 'sakul@mindsage.com' Add new course by using code 'StudentCode01' and grade 'Grade01'
	Then System doesn't add new subscription  
	And System doesn't add UserActivity  

@mock  
Scenario: User add new course by using the right code but the ClassCalendar doesn't existing Then system doesn't add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
    When UserProfile 'sakul@mindsage.com' Add new course by using code 'StudentCode01' and grade 'Grade01'
	Then System doesn't add new subscription  
	And System doesn't add UserActivity  

@mock  
Scenario: Incorrect user add new course by using the right informations Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Name": "Teacher",
			"ImageProfileUrl": "teacher.jpg",
			"Subscriptions": [
				{
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"CreatedDate": "1/1/2016 00:00 am",
					"ClassRoomName": "Emotional literacy",
					"LicenseId": "License01"
				}
			]
		}
    ]
    """  
    When UserProfile 'sakul@mindsage.com' Add new course by using code 'StudentCode01' and grade 'Grade01'
	Then System doesn't add new subscription  
	And System doesn't add UserActivity  

@mock  
Scenario: User who already have course want to add the same course Then system doesn't add new course to the user  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Name": "Teacher",
			"ImageProfileUrl": "teacher.jpg",
			"Subscriptions": [
				{
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"CreatedDate": "1/1/2016 00:00 am",
					"ClassRoomName": "Emotional literacy",
					"LicenseId": "License01"
				}
			]
		},
		{
			"id": "sakul@mindsage.com",
			"Name": "Sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions": [
				{
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"CreatedDate": "1/1/2016 00:00 am",
					"ClassRoomName": "Emotional literacy",
					"LicenseId": "License01"
				}
			]
		},
    ]
    """  
    When UserProfile 'sakul@mindsage.com' Add new course by using code 'StudentCode01' and grade 'Grade01'
	Then System doesn't add new subscription  
	And System doesn't add UserActivity  


@mock  
Scenario: User add new course by using deleted student code Then system doesn't add new course to the user's subscription  
    Given Today is '2/8/2016 00:00 am'  
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
			"DeletedDate": "2/1/2016",
		}
	]
	"""  
    When UserProfile 'sakul@mindsage.com' Add new course by using code 'StudentCode01' and grade 'Grade01'
	Then System doesn't add new subscription  
	And System doesn't add UserActivity  