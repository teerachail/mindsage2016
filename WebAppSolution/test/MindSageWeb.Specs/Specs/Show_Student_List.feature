Feature: Show_Student_List
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
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
                    "LessonCatalogId": "LessonCatalog01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/1/2016",
                },
				{
                    "Id": "LessonCalendar02",
                    "LessonId": "Lesson02",
                    "LessonCatalogId": "LessonCatalog02",
                    "Order": 2,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/16/2016",
                }
            ]
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
				}
			]
		},
		{
			"id": "UserActivity02",
			"UserProfileId": "student@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity04",
					"LessonId": "Lesson02",
					"SawContentIds": [ "Content02" ],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 1,
				}
			]
		}
    ]
    """  
    
@mock  
Scenario: A teacher request student list when it's the only one in the class room Then system send empty list back  
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
				},
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id | Name | ImageUrl | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |

@mock  
Scenario: A teacher request student list when have only one lesson available Then system send student list back  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id                   | Name    | ImageUrl    | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |
	| student@mindsage.com | student | student.jpg | 0                 | 0                      | 100                           |

@mock  
Scenario: A teacher request student list when have more than one lesson available Then system send student list back  
    Given Today is '2/1/2017 00:00 am'  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id                   | Name    | ImageUrl    | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |
	| student@mindsage.com | student | student.jpg | 50                | 50                     | 100                           |

@mock  
Scenario: #A teacher request student list when some sudent's subscription was deleted Then system send student list with valid subscription only back  
    Given Today is '2/1/2017 00:00 am'  
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
		{
			"id": "student-subscription-was-deleted@mindsage.com",
			"Name": "student-subscription-was-deleted",
			"ImageProfileUrl": "student-subscription-was-deleted.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				},
			]
		},
		{
			"id": "student-subscription-does-not-existing@mindsage.com",
			"Name": "student-subscription-does-not-existing",
			"ImageProfileUrl": "student-subscription-does-not-existing.jpg",
			"Subscriptions": []
		},
    ]
    """  
	And System have UserActivity collection with JSON format are
    """
    [
		{
			"id": "UserActivity01",
			"UserProfileId": "teacher@mindsage.com",
			"ClassRoomId": "ClassRoom01",
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
				}
			]
		},
		{
			"id": "UserActivity02",
			"UserProfileId": "student@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson01",
					"SawContentIds": [ "Content01" ],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 1,
				},
				{
					"id": "LessonActivity04",
					"LessonId": "Lesson02",
					"SawContentIds": [ "Content02" ],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 0,
					"CreatedCommentAmount": 1,
				}
			]
		},
		{
			"id": "UserActivity03",
			"UserProfileId": "student-subscription-was-deleted@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity05",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity06",
					"LessonId": "Lesson02",
					"SawContentIds": [ "Content01" ],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 1,
				}
			]
		},
		{
			"id": "UserActivity04",
			"UserProfileId": "student-subscription-does-not-existing@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity07",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity08",
					"LessonId": "Lesson02",
					"SawContentIds": [ "Content01" ],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 1,
				}
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id                   | Name    | ImageUrl    | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |
	| student@mindsage.com | student | student.jpg | 100               | 100                    | 0                             |

@mock  
Scenario: A teacher request student list but student's UserActivity was deleted Then system send student list back  
    Given Today is '2/1/2017 00:00 am'  
	And System have UserActivity collection with JSON format are
    """
    [
		{
			"id": "UserActivity02",
			"UserProfileId": "student@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"DeletedDate": "1/1/2016",
			"LessonActivities":
			[
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson01",
					"SawContentIds": [],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 0,
				},
				{
					"id": "LessonActivity04",
					"LessonId": "Lesson02",
					"SawContentIds": [ "Content02" ],
					"TotalContentsAmount": 1,
					"ParticipationAmount": 1,
					"CreatedCommentAmount": 1,
				}
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id                   | Name    | ImageUrl    | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |
	| student@mindsage.com | student | student.jpg | 50                | 50                     | 100                           |

@mock  
Scenario: A teacher request student list but student's LessonActivities don't existing Then system send student list back  
    Given Today is '2/1/2017 00:00 am'  
	And System have UserActivity collection with JSON format are
    """
    [
		{
			"id": "UserActivity02",
			"UserProfileId": "student@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities": []
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id                   | Name    | ImageUrl    | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |
	| student@mindsage.com | student | student.jpg | 0                 | 0                      | 0                             |

@mock  
Scenario: A teacher request student list but student's UserActivity don't existing Then system send student list back  
    Given Today is '2/1/2017 00:00 am'  
	And System have UserActivity collection with JSON format are
    """
    []
    """  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id                   | Name    | ImageUrl    | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |
	| student@mindsage.com | student | student.jpg | 0                 | 0                      | 0                             |

@mock  
Scenario: Invalid user (subscription was deleted) request student list Then system send empty list back  
    Given Today is '2/1/2016 00:00 am'  
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
					"DeletedDate": "1/1/2016"
				},
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id | Name | ImageUrl | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |

@mock  
Scenario: Invalid user (subscription doesn't existing) request student list Then system send empty list back  
    Given Today is '2/1/2016 00:00 am'  
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
			"Subscriptions": []
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id | Name | ImageUrl | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |

@mock  
Scenario: Invalid user (Role = student) request student list Then system send empty list back  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfile 'student@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id | Name | ImageUrl | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |

@mock  
Scenario: Invalid user (unknow) request student list Then system send empty list back  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfile 'unknow@mindsage.com' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id | Name | ImageUrl | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |

@mock  
Scenario: Invalid user (empty) request student list Then system send empty list back  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfile '' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id | Name | ImageUrl | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |

@mock  
Scenario: Invalid user (null) request student list Then system send empty list back  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfile 'NULL' request student list from ClassRoom: 'ClassRoom01' 
    Then System send student list are  
	| id | Name | ImageUrl | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |

@mock  
Scenario: A teacher request student list but used ClassRoom invalid (unknow) Then system send empty list back  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'UnknowClassRoom' 
    Then System send student list are  
	| id | Name | ImageUrl | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |

@mock  
Scenario: A teacher request student list but used ClassRoom invalid (empty) Then system send empty list back  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: '' 
    Then System send student list are  
	| id | Name | ImageUrl | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |

@mock  
Scenario: A teacher request student list but used ClassRoom invalid (null) Then system send empty list back  
    Given Today is '2/1/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' request student list from ClassRoom: 'NULL' 
    Then System send student list are  
	| id | Name | ImageUrl | CommentPercentage | OnlineExtrasPercentage | SocialParticipationPercentage |