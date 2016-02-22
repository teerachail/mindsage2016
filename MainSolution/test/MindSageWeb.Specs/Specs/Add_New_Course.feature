Feature: Add_New_Course
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
			"ExtraContentUrls": [ "Extra01", "Extra02", "Extra03" ]
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
			"ClassRoomName": "Emotional literacy"
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