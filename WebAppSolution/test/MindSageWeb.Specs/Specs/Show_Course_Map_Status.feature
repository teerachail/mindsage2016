Feature: Show_Course_Map_Status
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
				}
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
			"UserProfileId": "teacher@mindsage.com",
			"ClassRoomId": "ClassRoom01",
			"LessonActivities":
			[
				{
					"id": "LessonActivity01",
					"LessonId": "Lesson01",
					"TotalContentsAmount": 1,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  

@mock  
Scenario: A student request CourseMap's status (never do anythings) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | false           | false               |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | false               |

@mock  
Scenario: A student request CourseMap's status (already saw the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [ "PrimaryContent01" ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | false           | true                |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | false               |

@mock  
Scenario: A student request CourseMap's status (already commented the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | true            | false               |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | false               |

@mock  
Scenario: A student request CourseMap's status (already liked the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | false           | false               |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | false               |

@mock  
Scenario: A student request CourseMap's status (already saw & commented the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [ "PrimaryContent01" ],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [ "PrimaryContent02" ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | true            | true                |
	| Lesson02 | false           | true                |
	| Lesson03 | true            | false               |

@mock  
Scenario: A1 student request CourseMap's status (already saw & commented & liked the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [ "PrimaryContent01" ],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [ "PrimaryContent02" ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 1
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | true            | true                |
	| Lesson02 | false           | true                |
	| Lesson03 | true            | false               |

@mock  
Scenario: A student request CourseMap's status (already commented & liked the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 1
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | true            | false               |
	| Lesson02 | true            | false               |
	| Lesson03 | false           | false               |

@mock  
Scenario: A student request CourseMap's status (already liked & saw the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [ "PrimaryContent01" ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [ "PrimaryContent03" ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | false           | true                |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | true                |

@mock  
Scenario: A teacher request CourseMap's status (never do anythings) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | false           | false               |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | false               |

@mock  
Scenario: A teacher request CourseMap's status (already saw the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [ "PrimaryContent01" ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | false           | true                |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | false               |

@mock  
Scenario: A teacher request CourseMap's status (already commented the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | true            | false               |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | false               |

@mock  
Scenario: A teacher request CourseMap's status (already liked the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | false           | false               |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | false               |

@mock  
Scenario: A teacher request CourseMap's status (already saw & commented the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [ "PrimaryContent01" ],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [ "PrimaryContent02" ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | true            | true                |
	| Lesson02 | false           | true                |
	| Lesson03 | true            | false               |

@mock  
Scenario: A1 teacher request CourseMap's status (already saw & commented & liked the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [ "PrimaryContent01" ],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [ "PrimaryContent02" ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 1
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | true            | true                |
	| Lesson02 | false           | true                |
	| Lesson03 | true            | false               |

@mock  
Scenario: A teacher request CourseMap's status (already commented & liked the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 1,
					"ParticipationAmount": 0
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 1
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | true            | false               |
	| Lesson02 | true            | false               |
	| Lesson03 | false           | false               |

@mock  
Scenario: A teacher request CourseMap's status (already liked & saw the first lesson) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
					"TotalContentsAmount": 1,
					"SawContentIds": [ "PrimaryContent01" ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity02",
					"LessonId": "Lesson02",
					"TotalContentsAmount": 2,
					"SawContentIds": [],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 1
				},
				{
					"id": "LessonActivity03",
					"LessonId": "Lesson03",
					"TotalContentsAmount": 3,
					"SawContentIds": [ "PrimaryContent03" ],
					"CreatedCommentAmount": 0,
					"ParticipationAmount": 0
				},
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |
	| Lesson01 | false           | true                |
	| Lesson02 | false           | false               |
	| Lesson03 | false           | true                |

@mock  
Scenario: A teacher request CourseMap's status but input ClassRoomId incorrect (unknow) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'UnknowClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |

@mock  
Scenario: A teacher request CourseMap's status but input ClassRoomId incorrect (empty) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: '' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |

@mock  
Scenario: A teacher request CourseMap's status but input ClassRoomId incorrect (null) Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'NULL' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |

@mock  
Scenario: Incorrect user (unknow) request CourseMap's status Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'unknow@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |

@mock  
Scenario: Incorrect user (empty) request CourseMap's status Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId '' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |

@mock  
Scenario: Incorrect user (null) request CourseMap's status Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'NULL' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |

@mock  
Scenario: A teacher request CourseMap's status but its subscription was deleted Then system send statuss back  
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
				}
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |

@mock  
Scenario: A teacher request CourseMap's status but its subscription doesn't existing Then system send statuss back  
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
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |

@mock  
Scenario: A teacher request CourseMap's status but the account was deleted Then system send statuss back  
    Given Today is '2/8/2016 00:00 am'  
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
				}
			]
		}
    ]
    """  
    When UserProfileId 'teacher@mindsage.com' reuqest course map status of ClassRoom: 'ClassRoom01' and ClassCalendarId: 'ClassCalendar01'  
    Then System send course map status back are
	| LessonId | HaveAnyComments | IsReadedAllContents |