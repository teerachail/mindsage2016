Feature: Enter_Lesson
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
	And System have ClassRoom collection with JSON format are  
    """
    [
        {
            "id": "ClassRoom01",
            "Name": "Emotional literacy",
			"Grade": "7",
            "CourseCatalogId": "CourseCatalog01",
            "CreatedDate": "2/1/2016",
			"Message": "Don't forget to comment a lesson!",
            "Lessons":
            [
                {
                    "id": "Lesson01",
                    "TotalLikes": 1,
                    "LessonCatalogId": "LessonCatalog01"
                },
				{
                    "id": "Lesson02",
                    "TotalLikes": 2,
                    "LessonCatalogId": "LessonCatalog02"
                },
				{
                    "id": "Lesson03",
                    "TotalLikes": 3,
                    "LessonCatalogId": "LessonCatalog03"
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
            "BeginDate": "2/1/2016",
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
                    "BeginDate": "2/10/2016",
                },
                {
                    "Id": "LessonCalendar03",
                    "LessonId": "Lesson03",
                    "Order": 3,
                    "SemesterGroupName": "B",
                    "BeginDate": "2/20/2016",
                },
            ]
        },
    ]
    """  
	And System have LessonCatalog collection with JSON format are  
    """  
    [
        {
            "id": "LessonCatalog01",
			"Order": 1,
			"SemesterName": "A",
			"UnitNo": "1",
			"CourseCatalogId": "CourseCatalog01",
            "Title": "What Is Emotional Literacy?",
			"ShortDescription": "short desc01",
			"MoreDescription": "more desc01",
            "ShortTeacherLessonPlan": "short teacher lesson plan01",
            "MoreTeacherLessonPlan": "more teacher lesson plan01",
			"PrimaryContentUrl": "PrimaryContent01",
			"PrimaryContentDescription": "primary content desc01",
			"CreatedDate": "1/1/2016",
			"ExtraContents": 
			[ 
				{
					"id": "Extra01",
					"ContentURL": "www.extracontent01.com",
					"Description": "description01",
					"IconURL": "extra01.jpg"
				},
				{
					"id": "Extra02",
					"ContentURL": "www.extracontent02.com",
					"Description": "description02",
					"IconURL": "extra02.jpg"
				},
				{
					"id": "Extra03",
					"ContentURL": "www.extracontent03.com",
					"Description": "description03",
					"IconURL": "extra03.jpg"
				}
			],
			"Advertisments":
			[
				{
					"id": "ADS01",
					"ImageUrl": "img01.jpg",
					"CreatedDate": "1/1/2016"
				}
			],
			"TopicOfTheDays": [],
        },
		{
            "id": "LessonCatalog02",
			"Order": 2,
			"SemesterName": "A",
			"UnitNo": "1",
			"CourseCatalogId": "CourseCatalog01",
            "Title": "What Is Emotional Literacy?",
			"ShortDescription": "short desc02",
			"MoreDescription": "more desc02",
            "ShortTeacherLessonPlan": "short teacher lesson plan02",
            "MoreTeacherLessonPlan": "more teacher lesson plan02",
			"PrimaryContentUrl": "PrimaryContent02",
			"PrimaryContentDescription": "primary content desc02",
			"CreatedDate": "1/1/2016",
			"ExtraContents": 
			[ 
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
				}
			],
			"Advertisments":
			[
				{
					"id": "ADS02",
					"ImageUrl": "img02.jpg",
					"CreatedDate": "1/1/2016"
				}
			],
			"TopicOfTheDays": [],
        },
		{
            "id": "LessonCatalog03",
			"Order": 3,
			"SemesterName": "B",
			"UnitNo": "2",
			"CourseCatalogId": "CourseCatalog01",
            "Title": "What Is Emotional Literacy?",
			"ShortDescription": "short desc03",
			"MoreDescription": "more desc03",
            "ShortTeacherLessonPlan": "short teacher lesson plan03",
            "MoreTeacherLessonPlan": "more teacher lesson plan03",
			"PrimaryContentUrl": "PrimaryContent03",
			"PrimaryContentDescription": "primary content desc03",
			"CreatedDate": "1/1/2016",
			"ExtraContents": 
			[ 
				{
					"id": "Extra01",
					"ContentURL": "www.extracontent01.com",
					"Description": "description01",
					"IconURL": "extra01.jpg"
				}
			],
			"Advertisments": [],
			"TopicOfTheDays": [],
        },
    ]
    """  
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
		}
    ]
    """  
    And System have UserActivity collection with JSON format are
	"""
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "teacher@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"UserProfileName": "teacher",
			"UserProfileImageUrl": "teacher.jpg",
			"LessonActivities":
			[
				{
					"id": "LessonActivity01",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		},
		{
			"id": "UserActivity02",
			"UserProfileId": "student@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"UserProfileName": "student",
			"UserProfileImageUrl": "student.jpg",
			"LessonActivities":
			[
				{
					"id": "LessonActivity04",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity05",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity06",
					"LessonId": "Lesson03",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		}
    ]
    """  
    
@mock  
Scenario: Student enter the first lesson in the right time (first time) Then system send lesson information back without teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'student@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System upsert UserProfile with JSON format is  
    """
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
				"LastActiveDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert user id 'student@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"id": "LessonActivity04",
			"LessonId": "Lesson01",
			"SawContentIds": [ "PrimaryContent01" ],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity05",
			"LessonId": "Lesson02",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity06",
			"LessonId": "Lesson03",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		}
	]
    """  
	And System send lesson information back with JSON format is  
	"""  
	{
        "id": "Lesson01",
		"Order": 1,
		"SemesterName": "A",
		"UnitNo": 1,
		"CourseCatalogId": "CourseCatalog01",
        "Title": "What Is Emotional Literacy?",
		"ShortDescription": "short desc01",
		"MoreDescription": "more desc01",
		"ShortTeacherLessonPlan": "",
        "MoreTeacherLessonPlan": "",
		"PrimaryContentUrl": "PrimaryContent01",
		"PrimaryContentDescription": "primary content desc01",
		"CreatedDate": "1/1/2016",
		"CourseMessage": "Don't forget to comment a lesson!",
		"TotalLikes": 1,
		"ExtraContents":
		[ 
			{
				"id": "Extra01",
				"ContentURL": "www.extracontent01.com",
				"Description": "description01",
				"IconURL": "extra01.jpg"
			},
			{
				"id": "Extra02",
				"ContentURL": "www.extracontent02.com",
				"Description": "description02",
				"IconURL": "extra02.jpg"
			},
			{
				"id": "Extra03",
				"ContentURL": "www.extracontent03.com",
				"Description": "description03",
				"IconURL": "extra03.jpg"
			}
		],
		"Advertisments":
		[
			{
				"id": "ADS01",
				"ImageUrl": "img01.jpg",
				"CreatedDate": "1/1/2016"
			}
		],
		"TopicOfTheDays": [],
    }
	"""  

@mock  
Scenario: Student enter the first lesson in the right time (second time) Then system send lesson information back without teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserActivity collection with JSON format are
	"""
    [
		{
			"id": "UserActivity02",
			"UserProfileId": "student@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"UserProfileName": "student",
			"UserProfileImageUrl": "student.jpg",
			"LessonActivities":
			[
				{
					"id": "LessonActivity04",
					"LessonId": "Lesson01",
					"SawContentIds": [ "PrimaryContent01" ],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity05",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity06",
					"LessonId": "Lesson03",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		}
    ]
    """  
    When UserProfile 'student@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System upsert UserProfile with JSON format is  
    """
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
				"LastActiveDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System doesn't update UserActivity  
	And System send lesson information back with JSON format is  
	"""  
	{
        "id": "Lesson01",
		"Order": 1,
		"SemesterName": "A",
		"UnitNo": 1,
		"CourseCatalogId": "CourseCatalog01",
        "Title": "What Is Emotional Literacy?",
		"ShortDescription": "short desc01",
		"MoreDescription": "more desc01",
		"ShortTeacherLessonPlan": "",
        "MoreTeacherLessonPlan": "",
		"PrimaryContentUrl": "PrimaryContent01",
		"PrimaryContentDescription": "primary content desc01",
		"CreatedDate": "1/1/2016",
		"CourseMessage": "Don't forget to comment a lesson!",
		"TotalLikes": 1,
		"ExtraContents":
		[ 
			{
				"id": "Extra01",
				"ContentURL": "www.extracontent01.com",
				"Description": "description01",
				"IconURL": "extra01.jpg"
			},
			{
				"id": "Extra02",
				"ContentURL": "www.extracontent02.com",
				"Description": "description02",
				"IconURL": "extra02.jpg"
			},
			{
				"id": "Extra03",
				"ContentURL": "www.extracontent03.com",
				"Description": "description03",
				"IconURL": "extra03.jpg"
			}
		],
		"Advertisments":
		[
			{
				"id": "ADS01",
				"ImageUrl": "img01.jpg",
				"CreatedDate": "1/1/2016"
			}
		],
		"TopicOfTheDays": [],
    }
	"""  

@mock  
Scenario: Student enter a lesson in the right time Then system send lesson information back without teacher lesson plan  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfile 'student@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System upsert UserProfile with JSON format is  
    """
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
				"LastActiveDate": "2/1/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert user id 'student@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"id": "LessonActivity04",
			"LessonId": "Lesson01",
			"SawContentIds": [ "PrimaryContent01" ],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity05",
			"LessonId": "Lesson02",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity06",
			"LessonId": "Lesson03",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		}
	]
    """  
	And System send lesson information back with JSON format is  
	"""  
	{
        "id": "Lesson01",
		"Order": 1,
		"SemesterName": "A",
		"UnitNo": 1,
		"CourseCatalogId": "CourseCatalog01",
        "Title": "What Is Emotional Literacy?",
		"ShortDescription": "short desc01",
		"MoreDescription": "more desc01",
		"ShortTeacherLessonPlan": "",
        "MoreTeacherLessonPlan": "",
		"PrimaryContentUrl": "PrimaryContent01",
		"PrimaryContentDescription": "primary content desc01",
		"CreatedDate": "1/1/2016",
		"CourseMessage": "Don't forget to comment a lesson!",
		"TotalLikes": 1,
		"ExtraContents":
		[ 
			{
				"id": "Extra01",
				"ContentURL": "www.extracontent01.com",
				"Description": "description01",
				"IconURL": "extra01.jpg"
			},
			{
				"id": "Extra02",
				"ContentURL": "www.extracontent02.com",
				"Description": "description02",
				"IconURL": "extra02.jpg"
			},
			{
				"id": "Extra03",
				"ContentURL": "www.extracontent03.com",
				"Description": "description03",
				"IconURL": "extra03.jpg"
			}
		],
		"Advertisments":
		[
			{
				"id": "ADS01",
				"ImageUrl": "img01.jpg",
				"CreatedDate": "1/1/2016"
			}
		],
		"TopicOfTheDays": [],
    }
	"""  

@mock  
Scenario: Student enter the last lesson in the right time Then system send lesson information back without teacher lesson plan  
    Given Today is '2/20/2016 00:00 am'  
    When UserProfile 'student@mindsage.com' enter LessonId 'Lesson03' from ClassRoom 'ClassRoom01'
    Then System upsert UserProfile with JSON format is  
    """
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
				"LastActiveDate": "2/20/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert user id 'student@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"id": "LessonActivity04",
			"LessonId": "Lesson01",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity05",
			"LessonId": "Lesson02",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity06",
			"LessonId": "Lesson03",
			"SawContentIds": [ "PrimaryContent03" ],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		}
	]
    """  
	And System send lesson information back with JSON format is  
	"""  
	{
        "id": "Lesson03",
		"Order": 3,
		"SemesterName": "B",
		"UnitNo": 2,
		"CourseCatalogId": "CourseCatalog01",
        "Title": "What Is Emotional Literacy?",
		"ShortDescription": "short desc03",
		"MoreDescription": "more desc03",
		"ShortTeacherLessonPlan": "",
        "MoreTeacherLessonPlan": "",
		"PrimaryContentUrl": "PrimaryContent03",
		"PrimaryContentDescription": "primary content desc03",
		"CreatedDate": "1/1/2016",
		"CourseMessage": "Don't forget to comment a lesson!",
		"TotalLikes": 3,
		"ExtraContents":
		[ 
			{
				"id": "Extra01",
				"ContentURL": "www.extracontent01.com",
				"Description": "description01",
				"IconURL": "extra01.jpg"
			}
		],
		"Advertisments": [],
		"TopicOfTheDays": [],
    }
	"""  

@mock  
Scenario: Student enter a lesson in the future Then system doesn't send lesson information back  
    Given Today is '1/1/2016 00:00 am'  
    When UserProfile 'student@mindsage.com' enter LessonId 'Lesson03' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Student enter a lesson in the future but the subscription was deleted Then system doesn't send lesson information back  
    Given Today is '2/20/2016 00:00 am'  
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
					"DeletedDate": "1/1/2016"
				}
			]
		}
    ]
    """  
    When UserProfile 'student@mindsage.com' enter LessonId 'Lesson03' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Student enter a lesson in the future but the subscription doesn't existing Then system doesn't send lesson information back  
    Given Today is '2/20/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "student@mindsage.com",
			"Name": "student",
			"ImageProfileUrl": "student.jpg",
			"Subscriptions": []
		}
    ]
    """  
    When UserProfile 'student@mindsage.com' enter LessonId 'Lesson03' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter the first lesson in the right time (first time) Then system send lesson information back with teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System upsert UserProfile with JSON format is  
    """
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
				"LastActiveDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert user id 'teacher@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"id": "LessonActivity01",
			"LessonId": "Lesson01",
			"SawContentIds": [ "PrimaryContent01" ],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity02",
			"LessonId": "Lesson02",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity03",
			"LessonId": "Lesson03",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		}
	]
    """  
	And System send lesson information back with JSON format is  
	"""  
	{
        "id": "Lesson01",
		"Order": 1,
		"SemesterName": "A",
		"UnitNo": 1,
		"CourseCatalogId": "CourseCatalog01",
        "Title": "What Is Emotional Literacy?",
		"ShortDescription": "short desc01",
		"MoreDescription": "more desc01",
		"ShortTeacherLessonPlan": "short teacher lesson plan01",
        "MoreTeacherLessonPlan": "more teacher lesson plan01",
		"PrimaryContentUrl": "PrimaryContent01",
		"PrimaryContentDescription": "primary content desc01",
		"CreatedDate": "1/1/2016",
		"CourseMessage": "Don't forget to comment a lesson!",
		"TotalLikes": 1,
		"IsTeacher": true,
		"ExtraContents":
		[ 
			{
				"id": "Extra01",
				"ContentURL": "www.extracontent01.com",
				"Description": "description01",
				"IconURL": "extra01.jpg"
			},
			{
				"id": "Extra02",
				"ContentURL": "www.extracontent02.com",
				"Description": "description02",
				"IconURL": "extra02.jpg"
			},
			{
				"id": "Extra03",
				"ContentURL": "www.extracontent03.com",
				"Description": "description03",
				"IconURL": "extra03.jpg"
			}
		],
		"Advertisments":
		[
			{
				"id": "ADS01",
				"ImageUrl": "img01.jpg",
				"CreatedDate": "1/1/2016"
			}
		],
		"TopicOfTheDays": [],
    }
	"""  

@mock  
Scenario: Teacher enter the first lesson in the right time (second time) Then system send lesson information back with teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserActivity collection with JSON format are
	"""
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "teacher@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"UserProfileName": "teacher",
			"UserProfileImageUrl": "teacher.jpg",
			"LessonActivities":
			[
				{
					"id": "LessonActivity01",
					"LessonId": "Lesson01",
					"SawContentIds": [ "PrimaryContent01" ],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System upsert UserProfile with JSON format is  
    """
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
				"LastActiveDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
	And System doesn't update UserActivity  
	And System send lesson information back with JSON format is  
	"""  
	{
        "id": "Lesson01",
		"Order": 1,
		"SemesterName": "A",
		"UnitNo": 1,
		"CourseCatalogId": "CourseCatalog01",
        "Title": "What Is Emotional Literacy?",
		"ShortDescription": "short desc01",
		"MoreDescription": "more desc01",
		"ShortTeacherLessonPlan": "short teacher lesson plan01",
        "MoreTeacherLessonPlan": "more teacher lesson plan01",
		"PrimaryContentUrl": "PrimaryContent01",
		"PrimaryContentDescription": "primary content desc01",
		"CreatedDate": "1/1/2016",
		"CourseMessage": "Don't forget to comment a lesson!",
		"TotalLikes": 1,
		"IsTeacher": true,
		"ExtraContents":
		[ 
			{
				"id": "Extra01",
				"ContentURL": "www.extracontent01.com",
				"Description": "description01",
				"IconURL": "extra01.jpg"
			},
			{
				"id": "Extra02",
				"ContentURL": "www.extracontent02.com",
				"Description": "description02",
				"IconURL": "extra02.jpg"
			},
			{
				"id": "Extra03",
				"ContentURL": "www.extracontent03.com",
				"Description": "description03",
				"IconURL": "extra03.jpg"
			}
		],
		"Advertisments":
		[
			{
				"id": "ADS01",
				"ImageUrl": "img01.jpg",
				"CreatedDate": "1/1/2016"
			}
		],
		"TopicOfTheDays": [],
    }
	"""  

@mock  
Scenario: Teacher enter a lesson in the right time Then system send lesson information back with teacher lesson plan  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System upsert UserProfile with JSON format is  
    """
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
				"LastActiveDate": "2/1/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert user id 'teacher@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"id": "LessonActivity01",
			"LessonId": "Lesson01",
			"SawContentIds": [ "PrimaryContent01" ],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity02",
			"LessonId": "Lesson02",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity03",
			"LessonId": "Lesson03",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		}
	]
    """  
	And System send lesson information back with JSON format is  
	"""  
	{
        "id": "Lesson01",
		"Order": 1,
		"SemesterName": "A",
		"UnitNo": 1,
		"CourseCatalogId": "CourseCatalog01",
        "Title": "What Is Emotional Literacy?",
		"ShortDescription": "short desc01",
		"MoreDescription": "more desc01",
		"ShortTeacherLessonPlan": "short teacher lesson plan01",
        "MoreTeacherLessonPlan": "more teacher lesson plan01",
		"PrimaryContentUrl": "PrimaryContent01",
		"PrimaryContentDescription": "primary content desc01",
		"CreatedDate": "1/1/2016",
		"CourseMessage": "Don't forget to comment a lesson!",
		"TotalLikes": 1,
		"IsTeacher": true,
		"ExtraContents":
		[ 
			{
				"id": "Extra01",
				"ContentURL": "www.extracontent01.com",
				"Description": "description01",
				"IconURL": "extra01.jpg"
			},
			{
				"id": "Extra02",
				"ContentURL": "www.extracontent02.com",
				"Description": "description02",
				"IconURL": "extra02.jpg"
			},
			{
				"id": "Extra03",
				"ContentURL": "www.extracontent03.com",
				"Description": "description03",
				"IconURL": "extra03.jpg"
			}
		],
		"Advertisments":
		[
			{
				"id": "ADS01",
				"ImageUrl": "img01.jpg",
				"CreatedDate": "1/1/2016"
			}
		],
		"TopicOfTheDays": [],
    }
	"""  

@mock  
Scenario: Teacher enter the last lesson in the right time Then system send lesson information back with teacher lesson plan  
    Given Today is '2/20/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson03' from ClassRoom 'ClassRoom01'
    Then System upsert UserProfile with JSON format is  
    """
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
				"LastActiveDate": "2/20/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert user id 'teacher@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"id": "LessonActivity01",
			"LessonId": "Lesson01",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity02",
			"LessonId": "Lesson02",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity03",
			"LessonId": "Lesson03",
			"SawContentIds": [ "PrimaryContent03" ],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		}
	]
    """  
	And System send lesson information back with JSON format is  
	"""  
	{
        "id": "Lesson03",
		"Order": 3,
		"SemesterName": "B",
		"UnitNo": 2,
		"CourseCatalogId": "CourseCatalog01",
        "Title": "What Is Emotional Literacy?",
		"ShortDescription": "short desc03",
		"MoreDescription": "more desc03",
		"ShortTeacherLessonPlan": "short teacher lesson plan03",
        "MoreTeacherLessonPlan": "more teacher lesson plan03",
		"PrimaryContentUrl": "PrimaryContent03",
		"PrimaryContentDescription": "primary content desc03",
		"CreatedDate": "1/1/2016",
		"CourseMessage": "Don't forget to comment a lesson!",
		"TotalLikes": 3,
		"IsTeacher": true,
		"ExtraContents":
		[ 
			{
				"id": "Extra01",
				"ContentURL": "www.extracontent01.com",
				"Description": "description01",
				"IconURL": "extra01.jpg"
			}
		],
		"Advertisments": [],
		"TopicOfTheDays": [],
    }
	"""  

@mock  
Scenario: Teacher enter a lesson in the future Then system send lesson information back with teacher lesson plan  
    Given Today is '1/1/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson03' from ClassRoom 'ClassRoom01'
    Then System upsert UserProfile with JSON format is  
    """
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
				"LastActiveDate": "1/1/2016 00:00 am"
			}
		]
	}
    """  
	And System upsert user id 'teacher@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"id": "LessonActivity01",
			"LessonId": "Lesson01",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity02",
			"LessonId": "Lesson02",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity03",
			"LessonId": "Lesson03",
			"SawContentIds": [ "PrimaryContent03" ],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		}
	]
    """  
	And System send lesson information back with JSON format is  
	"""  
	{
        "id": "Lesson03",
		"Order": 3,
		"SemesterName": "B",
		"UnitNo": 2,
		"CourseCatalogId": "CourseCatalog01",
        "Title": "What Is Emotional Literacy?",
		"ShortDescription": "short desc03",
		"MoreDescription": "more desc03",
		"ShortTeacherLessonPlan": "short teacher lesson plan03",
        "MoreTeacherLessonPlan": "more teacher lesson plan03",
		"PrimaryContentUrl": "PrimaryContent03",
		"PrimaryContentDescription": "primary content desc03",
		"CreatedDate": "1/1/2016",
		"CourseMessage": "Don't forget to comment a lesson!",
		"TotalLikes": 3,
		"IsTeacher": true,
		"ExtraContents":
		[ 
			{
				"id": "Extra01",
				"ContentURL": "www.extracontent01.com",
				"Description": "description01",
				"IconURL": "extra01.jpg"
			}
		],
		"Advertisments": [],
		"TopicOfTheDays": [],
    }
	"""  

@mock  
Scenario: Teacher enter a lesson in the future but the subscription was deleted Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
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
					"DeletedDate": "1/1/2016"
				},
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson in the future but the subscription doesn't existing Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "teacher@mindsage.com",
			"Name": "teacher",
			"ImageProfileUrl": "teacher.jpg",
			"Subscriptions": []
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Incorrect user (Unknow) enter a lesson Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'unknow@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Incorrect user (empty) enter a lesson Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile '' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Incorrect user (null) enter a lesson Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'NULL' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Selfpurchase user enter a lesson Then system send lesson information back without teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "selfpurchase@mindsage.com",
			"Name": "selfpurchase",
			"ImageProfileUrl": "selfpurchase.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "SelfPurchaser",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
    And System have UserActivity collection with JSON format are
	"""
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "selfpurchase@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"UserProfileName": "selfpurchase",
			"UserProfileImageUrl": "selfpurchase.jpg",
			"LessonActivities":
			[
				{
					"id": "LessonActivity01",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		}
    ]
    """  
    When UserProfile 'selfpurchase@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
	Then System upsert UserProfile with JSON format is  
    """
	{
		"id": "selfpurchase@mindsage.com",
		"Name": "selfpurchase",
		"ImageProfileUrl": "selfpurchase.jpg",
		"Subscriptions":
		[
			{
				"id": "Subscription01",
				"Role": "SelfPurchaser",
				"ClassRoomId": "ClassRoom01",
				"ClassCalendarId": "ClassCalendar01",
				"LastActiveDate": "2/8/2016 00:00 am"
			}
		]
	}
    """  
    And System upsert user id 'selfpurchase@mindsage.com' UserActivity's LessonActivities collection with JSON format are
    """
	[
		{
			"id": "LessonActivity01",
			"LessonId": "Lesson01",
			"SawContentIds": [ "PrimaryContent01" ],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity02",
			"LessonId": "Lesson02",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		},
		{
			"id": "LessonActivity03",
			"LessonId": "Lesson03",
			"SawContentIds": [],
			"TotalContentsAmount": 1,
			"ParticipationAmount": 0,
			"CreatedCommentAmount": 0,
		}
	]
    """  
	And System send lesson information back with JSON format is  
	"""  
	{
        "id": "Lesson01",
		"Order": 1,
		"SemesterName": "A",
		"UnitNo": 1,
		"CourseCatalogId": "CourseCatalog01",
        "Title": "What Is Emotional Literacy?",
		"ShortDescription": "short desc01",
		"MoreDescription": "more desc01",
		"ShortTeacherLessonPlan": "",
        "MoreTeacherLessonPlan": "",
		"PrimaryContentUrl": "PrimaryContent01",
		"PrimaryContentDescription": "primary content desc01",
		"CreatedDate": "1/1/2016",
		"CourseMessage": "Don't forget to comment a lesson!",
		"TotalLikes": 1,
		"ExtraContents":
		[ 
			{
				"id": "Extra01",
				"ContentURL": "www.extracontent01.com",
				"Description": "description01",
				"IconURL": "extra01.jpg"
			},
			{
				"id": "Extra02",
				"ContentURL": "www.extracontent02.com",
				"Description": "description02",
				"IconURL": "extra02.jpg"
			},
			{
				"id": "Extra03",
				"ContentURL": "www.extracontent03.com",
				"Description": "description03",
				"IconURL": "extra03.jpg"
			}
		],
		"Advertisments":
		[
			{
				"id": "ADS01",
				"ImageUrl": "img01.jpg",
				"CreatedDate": "1/1/2016"
			}
		],
		"TopicOfTheDays": [],
    }
	"""  

@mock  
Scenario: Selfpurchase user enter a lesson in the future Then system doesn't send lesson information back  
    Given Today is '1/1/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "selfpurchase@mindsage.com",
			"Name": "selfpurchase",
			"ImageProfileUrl": "selfpurchase.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "SelfPurchaser",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
    And System have UserActivity collection with JSON format are
	"""
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "selfpurchase@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"UserProfileName": "selfpurchase",
			"UserProfileImageUrl": "selfpurchase.jpg",
			"LessonActivities":
			[
				{
					"id": "LessonActivity01",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		}
    ]
    """  
    When UserProfile 'selfpurchase@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
	Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but the ClassRoom was deleted Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassRoom collection with JSON format are  
    """
    [
        {
            "id": "ClassRoom01",
            "Name": "Emotional literacy",
			"Grade": "7",
            "CourseCatalogId": "CourseCatalog01",
            "CreatedDate": "2/1/2016",
			"Message": "Don't forget to comment a lesson!",
			"DeletedDate": "1/1/2016",
            "Lessons":
            [
                {
                    "id": "Lesson01",
                    "TotalLikes": 1,
                    "LessonCatalogId": "LessonCatalog01"
                },
				{
                    "id": "Lesson02",
                    "TotalLikes": 2,
                    "LessonCatalogId": "LessonCatalog02"
                },
				{
                    "id": "Lesson03",
                    "TotalLikes": 3,
                    "LessonCatalogId": "LessonCatalog03"
                },
            ]
        }
    ]
	"""  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but the ClassRoom doesn't existing Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassRoom collection with JSON format are  
    """
    []
	"""  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but the ClassCalendar was deleted Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
            "ClassRoomId": "ClassRoom01",
			"DeletedDate": "1/1/2016",
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
                    "BeginDate": "2/10/2016",
                },
                {
                    "Id": "LessonCalendar03",
                    "LessonId": "Lesson03",
                    "Order": 3,
                    "SemesterGroupName": "B",
                    "BeginDate": "2/20/2016",
                },
            ]
        },
    ]
    """  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but the ClassCalendar doesn't existing Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassCalendar collection with JSON format are
    """
    []
    """  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but the UserActivity was deleted Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserActivity collection with JSON format are
	"""
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "teacher@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"CreatedDate": "1/1/2016",
			"UserProfileName": "teacher",
			"UserProfileImageUrl": "teacher.jpg",
			"DeletedDate": "1/1/2016",
			"LessonActivities":
			[
				{
					"id": "LessonActivity01",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 0,
				}
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but the UserActivity doesn't existing Then system doesn't send lesson information back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserActivity collection with JSON format are
	"""
    []
    """  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but used ClassRoom incorrect (unknow) Then system send lesson information back with teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'UnknowClassRoom'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but used ClassRoom incorrect (empty) Then system send lesson information back with teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom ''
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but used ClassRoom incorrect (null) Then system send lesson information back with teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'Lesson01' from ClassRoom 'NULL'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but used LessonId incorrect (unknow) Then system send lesson information back with teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'UnknowLesson' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but used LessonId incorrect (empty) Then system send lesson information back with teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' enter LessonId '' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  

@mock  
Scenario: Teacher enter a lesson but used LessonId incorrect (null) Then system send lesson information back with teacher lesson plan  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' enter LessonId 'NULL' from ClassRoom 'ClassRoom01'
    Then System doesn't upsert UserProfile  
	And System doesn't update UserActivity  
	And System doesn't send lesson information back  