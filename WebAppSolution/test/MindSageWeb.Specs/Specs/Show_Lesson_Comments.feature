Feature: Show_Lesson_Comments
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
                    "Id": "ClassCalendar01",
                    "LessonId": "Lesson01",
                    "BeginDate": "2/1/2016",
                    "LessonCatalogId": "LessonCatalog01"
                },
                {
                    "Id": "LC02",
                    "LessonId": "Lesson02",
                    "BeginDate": "2/8/2016",
                    "LessonCatalogId": "LessonCatalog02"
                }
            ]
        }
    ]
    """  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
    And System have FriendRequest collection with JSON format are
    """
    []
    """  
    And System have Comment collection with JSON format are  
    """
    []
    """ 

@mock  
Scenario: Student request lesson's comments but no comment in the lesson Then system send empty list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments": []
	}
    """ 

@mock  
Scenario: Student request lesson's comments but have itself comments in the lesson Then system send comments back  
    Given Today is '2/8/2016 00:00 am'  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg01",
            "TotalLikes": 5,
			"CreatorImageUrl": "sakul.jpg",
			"CreatorDisplayName": "sakul",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment01",
				"Order": 1,
				"Description": "Msg01",
				"TotalLikes": 5,
				"CreatorImageUrl": "sakul.jpg",
				"CreatorDisplayName": "sakul",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "sakul@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Student request lesson's comments but have itself comments in the difference lesson in the lesson Then system send empty list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg01",
            "TotalLikes": 5,
			"CreatorImageUrl": "sakul.jpg",
			"CreatorDisplayName": "sakul",
            "LessonId": "Lesson02",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
        
    ]
    """ 
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments": []
	}
    """ 

@mock  
Scenario: Student request lesson's comments but have only teacher's comments in the lesson Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment00",
				"Order": 1,
				"Description": "Teacher Msg01",
				"TotalLikes": 7,
				"CreatorImageUrl": "teacher.jpg",
				"CreatorDisplayName": "teacher",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "teacher@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Student request lesson's comments but have only friends' comments in the lesson Then system send comments back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageProfileUrl": "friend01.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription02",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageProfileUrl": "friend02.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend01@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest01-B",
			"FromUserProfileId": "friend01@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend02@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-B",
			"FromUserProfileId": "friend02@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend01@mindsage.com",
            "Description": "friend01 msg",
            "TotalLikes": 2,
			"CreatorImageUrl": "friend01.jpg",
			"CreatorDisplayName": "friend01",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend02@mindsage.com",
            "Description": "friend02 msg",
            "TotalLikes": 3,
			"CreatorImageUrl": "friend02.jpg",
			"CreatorDisplayName": "friend02",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment02",
				"Order": 1,
				"Description": "friend01 msg",
				"TotalLikes": 2,
				"CreatorImageUrl": "friend01.jpg",
				"CreatorDisplayName": "friend01",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend01@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment03",
				"Order": 2,
				"Description": "friend02 msg",
				"TotalLikes": 3,
				"CreatorImageUrl": "friend02.jpg",
				"CreatorDisplayName": "friend02",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend02@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Student request lesson's comments but have only not friends' comments in the lesson Then system send empty list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageProfileUrl": "waitingForRespond.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription04",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageProfileUrl": "requestedToBeYourFriend.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription05",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "sender@mindsage.com",
			"Name": "sender",
			"ImageProfileUrl": "sender.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription06",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
		{
            "id": "FriendRequest03-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "waitingForRespond@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-B",
			"FromUserProfileId": "waitingForRespond@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-B",
			"FromUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "unfriend@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment04",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "waitingForRespond@mindsage.com",
            "Description": "waitingForRespond msg",
            "TotalLikes": 4,
			"CreatorImageUrl": "waitingForRespond.jpg",
			"CreatorDisplayName": "waitingForRespond",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment05",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "requestedToBeYourFriend@mindsage.com",
            "Description": "requestedToBeYourFriend msg",
            "TotalLikes": 5,
			"CreatorImageUrl": "requestedToBeYourFriend.jpg",
			"CreatorDisplayName": "requestedToBeYourFriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment06",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 6,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments": []
	}
    """ 

@mock  
Scenario: Student request lesson's comments but have only unfriends' comments in the lesson Then system send empty list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageProfileUrl": "unfriend.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription07",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
		{
            "id": "FriendRequest05-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "unfriend@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-B",
			"FromUserProfileId": "unfriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
		{
            "id": "Comment07",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 7,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments": []
	}
    """ 

@mock  
Scenario: Student request lesson's comments when the lesson have a lot of multi comments' status Then system send only comments created by itself, teachers and friends back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageProfileUrl": "friend01.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription02",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageProfileUrl": "friend02.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageProfileUrl": "waitingForRespond.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription04",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageProfileUrl": "requestedToBeYourFriend.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription05",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "sender@mindsage.com",
			"Name": "sender",
			"ImageProfileUrl": "sender.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription06",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageProfileUrl": "unfriend.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription07",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend01@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest01-B",
			"FromUserProfileId": "friend01@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend02@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-B",
			"FromUserProfileId": "friend02@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "waitingForRespond@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-B",
			"FromUserProfileId": "waitingForRespond@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-B",
			"FromUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "unfriend@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-B",
			"FromUserProfileId": "unfriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
		{
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg01",
            "TotalLikes": 5,
			"CreatorImageUrl": "sakul.jpg",
			"CreatorDisplayName": "sakul",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend01@mindsage.com",
            "Description": "friend01 msg",
            "TotalLikes": 2,
			"CreatorImageUrl": "friend01.jpg",
			"CreatorDisplayName": "friend01",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend02@mindsage.com",
            "Description": "friend02 msg",
            "TotalLikes": 3,
			"CreatorImageUrl": "friend02.jpg",
			"CreatorDisplayName": "friend02",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment04",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "waitingForRespond@mindsage.com",
            "Description": "waitingForRespond msg",
            "TotalLikes": 4,
			"CreatorImageUrl": "waitingForRespond.jpg",
			"CreatorDisplayName": "waitingForRespond",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment05",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "requestedToBeYourFriend@mindsage.com",
            "Description": "requestedToBeYourFriend msg",
            "TotalLikes": 5,
			"CreatorImageUrl": "requestedToBeYourFriend.jpg",
			"CreatorDisplayName": "requestedToBeYourFriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment06",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 6,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment07",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 7,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment00",
				"Order": 1,
				"Description": "Teacher Msg01",
				"TotalLikes": 7,
				"CreatorImageUrl": "teacher.jpg",
				"CreatorDisplayName": "teacher",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "teacher@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment01",
				"Order": 2,
				"Description": "Msg01",
				"TotalLikes": 5,
				"CreatorImageUrl": "sakul.jpg",
				"CreatorDisplayName": "sakul",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "sakul@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment02",
				"Order": 3,
				"Description": "friend01 msg",
				"TotalLikes": 2,
				"CreatorImageUrl": "friend01.jpg",
				"CreatorDisplayName": "friend01",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend01@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment03",
				"Order": 4,
				"Description": "friend02 msg",
				"TotalLikes": 3,
				"CreatorImageUrl": "friend02.jpg",
				"CreatorDisplayName": "friend02",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend02@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Student request lesson's comments but have only friends' comments in the lesson and thier subscriptions was deleted Then system send comments back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageProfileUrl": "friend01.jpg",
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
		},
		{
			"id": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageProfileUrl": "friend02.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend01@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest01-B",
			"FromUserProfileId": "friend01@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend02@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-B",
			"FromUserProfileId": "friend02@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend01@mindsage.com",
            "Description": "friend01 msg",
            "TotalLikes": 2,
			"CreatorImageUrl": "friend01.jpg",
			"CreatorDisplayName": "friend01",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend02@mindsage.com",
            "Description": "friend02 msg",
            "TotalLikes": 3,
			"CreatorImageUrl": "friend02.jpg",
			"CreatorDisplayName": "friend02",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment02",
				"Order": 1,
				"Description": "friend01 msg",
				"TotalLikes": 2,
				"CreatorImageUrl": "friend01.jpg",
				"CreatorDisplayName": "friend01",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend01@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment03",
				"Order": 2,
				"Description": "friend02 msg",
				"TotalLikes": 3,
				"CreatorImageUrl": "friend02.jpg",
				"CreatorDisplayName": "friend02",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend02@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Student request lesson's comments but have only friends' comments in the lesson and users was deleted Then system send comments back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageProfileUrl": "friend01.jpg",
			"DeletedDate": "1/1/2016",
			"Subscriptions":
			[
				{
					"id": "Subscription02",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageProfileUrl": "friend02.jpg",
			"DeletedDate": "1/1/2016",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend01@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest01-B",
			"FromUserProfileId": "friend01@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend02@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-B",
			"FromUserProfileId": "friend02@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend01@mindsage.com",
            "Description": "friend01 msg",
            "TotalLikes": 2,
			"CreatorImageUrl": "friend01.jpg",
			"CreatorDisplayName": "friend01",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend02@mindsage.com",
            "Description": "friend02 msg",
            "TotalLikes": 3,
			"CreatorImageUrl": "friend02.jpg",
			"CreatorDisplayName": "friend02",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment02",
				"Order": 1,
				"Description": "friend01 msg",
				"TotalLikes": 2,
				"CreatorImageUrl": "friend01.jpg",
				"CreatorDisplayName": "friend01",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend01@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment03",
				"Order": 2,
				"Description": "friend02 msg",
				"TotalLikes": 3,
				"CreatorImageUrl": "friend02.jpg",
				"CreatorDisplayName": "friend02",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend02@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Student request lesson's comments but have only friends' comments in the lesson and users don't existing Then system send comments back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend01@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest01-B",
			"FromUserProfileId": "friend01@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend02@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-B",
			"FromUserProfileId": "friend02@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend01@mindsage.com",
            "Description": "friend01 msg",
            "TotalLikes": 2,
			"CreatorImageUrl": "friend01.jpg",
			"CreatorDisplayName": "friend01",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend02@mindsage.com",
            "Description": "friend02 msg",
            "TotalLikes": 3,
			"CreatorImageUrl": "friend02.jpg",
			"CreatorDisplayName": "friend02",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'sakul@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment02",
				"Order": 1,
				"Description": "friend01 msg",
				"TotalLikes": 2,
				"CreatorImageUrl": "friend01.jpg",
				"CreatorDisplayName": "friend01",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend01@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment03",
				"Order": 2,
				"Description": "friend02 msg",
				"TotalLikes": 3,
				"CreatorImageUrl": "friend02.jpg",
				"CreatorDisplayName": "friend02",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend02@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Teacher request lesson's comments but no comment in the lesson Then system send empty list back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments": []
	}
    """ 

@mock  
Scenario: Teacher request lesson's comments but have only itself comments in the lesson Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment00",
				"Order": 1,
				"Description": "Teacher Msg01",
				"TotalLikes": 7,
				"CreatorImageUrl": "teacher.jpg",
				"CreatorDisplayName": "teacher",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "teacher@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Teacher request lesson's comments when the lesson have a lot of multi comments' status Then system send all comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageProfileUrl": "friend01.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription02",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageProfileUrl": "friend02.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageProfileUrl": "waitingForRespond.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription04",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageProfileUrl": "requestedToBeYourFriend.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription05",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "sender@mindsage.com",
			"Name": "sender",
			"ImageProfileUrl": "sender.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription06",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageProfileUrl": "unfriend.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription07",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend01@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest01-B",
			"FromUserProfileId": "friend01@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend02@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-B",
			"FromUserProfileId": "friend02@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "waitingForRespond@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-B",
			"FromUserProfileId": "waitingForRespond@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-B",
			"FromUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "unfriend@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-B",
			"FromUserProfileId": "unfriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
		{
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg01",
            "TotalLikes": 5,
			"CreatorImageUrl": "sakul.jpg",
			"CreatorDisplayName": "sakul",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend01@mindsage.com",
            "Description": "friend01 msg",
            "TotalLikes": 2,
			"CreatorImageUrl": "friend01.jpg",
			"CreatorDisplayName": "friend01",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend02@mindsage.com",
            "Description": "friend02 msg",
            "TotalLikes": 3,
			"CreatorImageUrl": "friend02.jpg",
			"CreatorDisplayName": "friend02",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment04",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "waitingForRespond@mindsage.com",
            "Description": "waitingForRespond msg",
            "TotalLikes": 4,
			"CreatorImageUrl": "waitingForRespond.jpg",
			"CreatorDisplayName": "waitingForRespond",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment05",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "requestedToBeYourFriend@mindsage.com",
            "Description": "requestedToBeYourFriend msg",
            "TotalLikes": 5,
			"CreatorImageUrl": "requestedToBeYourFriend.jpg",
			"CreatorDisplayName": "requestedToBeYourFriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment06",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 6,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment07",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 7,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment00",
				"Order": 1,
				"Description": "Teacher Msg01",
				"TotalLikes": 7,
				"CreatorImageUrl": "teacher.jpg",
				"CreatorDisplayName": "teacher",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "teacher@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment01",
				"Order": 2,
				"Description": "Msg01",
				"TotalLikes": 5,
				"CreatorImageUrl": "sakul.jpg",
				"CreatorDisplayName": "sakul",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "sakul@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment02",
				"Order": 3,
				"Description": "friend01 msg",
				"TotalLikes": 2,
				"CreatorImageUrl": "friend01.jpg",
				"CreatorDisplayName": "friend01",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend01@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment03",
				"Order": 4,
				"Description": "friend02 msg",
				"TotalLikes": 3,
				"CreatorImageUrl": "friend02.jpg",
				"CreatorDisplayName": "friend02",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend02@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment04",
				"Order": 5,
				"Description": "waitingForRespond msg",
				"TotalLikes": 4,
				"CreatorImageUrl": "waitingForRespond.jpg",
				"CreatorDisplayName": "waitingForRespond",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "waitingForRespond@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment05",
				"Order": 6,
				"Description": "requestedToBeYourFriend msg",
				"TotalLikes": 5,
				"CreatorImageUrl": "requestedToBeYourFriend.jpg",
				"CreatorDisplayName": "requestedToBeYourFriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "requestedToBeYourFriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment06",
				"Order": 7,
				"Description": "unfriend msg",
				"TotalLikes": 6,
				"CreatorImageUrl": "unfriend.jpg",
				"CreatorDisplayName": "unfriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "unfriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment07",
				"Order": 8,
				"Description": "unfriend msg",
				"TotalLikes": 7,
				"CreatorImageUrl": "unfriend.jpg",
				"CreatorDisplayName": "unfriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "unfriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Teacher request lesson's comments when the lesson have a lot of multi comments' and their users' subscriptions was deleted status Then system send all comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				}
			]
		},
		{
			"id": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageProfileUrl": "friend01.jpg",
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
		},
		{
			"id": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageProfileUrl": "friend02.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				}
			]
		},
		{
			"id": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageProfileUrl": "waitingForRespond.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription04",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				}
			]
		},
		{
			"id": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageProfileUrl": "requestedToBeYourFriend.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription05",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				}
			]
		},
		{
			"id": "sender@mindsage.com",
			"Name": "sender",
			"ImageProfileUrl": "sender.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription06",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				}
			]
		},
		{
			"id": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageProfileUrl": "unfriend.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription07",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend01@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest01-B",
			"FromUserProfileId": "friend01@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend02@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-B",
			"FromUserProfileId": "friend02@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "waitingForRespond@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-B",
			"FromUserProfileId": "waitingForRespond@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-B",
			"FromUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "unfriend@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-B",
			"FromUserProfileId": "unfriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
		{
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg01",
            "TotalLikes": 5,
			"CreatorImageUrl": "sakul.jpg",
			"CreatorDisplayName": "sakul",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend01@mindsage.com",
            "Description": "friend01 msg",
            "TotalLikes": 2,
			"CreatorImageUrl": "friend01.jpg",
			"CreatorDisplayName": "friend01",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend02@mindsage.com",
            "Description": "friend02 msg",
            "TotalLikes": 3,
			"CreatorImageUrl": "friend02.jpg",
			"CreatorDisplayName": "friend02",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment04",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "waitingForRespond@mindsage.com",
            "Description": "waitingForRespond msg",
            "TotalLikes": 4,
			"CreatorImageUrl": "waitingForRespond.jpg",
			"CreatorDisplayName": "waitingForRespond",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment05",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "requestedToBeYourFriend@mindsage.com",
            "Description": "requestedToBeYourFriend msg",
            "TotalLikes": 5,
			"CreatorImageUrl": "requestedToBeYourFriend.jpg",
			"CreatorDisplayName": "requestedToBeYourFriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment06",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 6,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment07",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 7,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment00",
				"Order": 1,
				"Description": "Teacher Msg01",
				"TotalLikes": 7,
				"CreatorImageUrl": "teacher.jpg",
				"CreatorDisplayName": "teacher",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "teacher@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment01",
				"Order": 2,
				"Description": "Msg01",
				"TotalLikes": 5,
				"CreatorImageUrl": "sakul.jpg",
				"CreatorDisplayName": "sakul",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "sakul@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment02",
				"Order": 3,
				"Description": "friend01 msg",
				"TotalLikes": 2,
				"CreatorImageUrl": "friend01.jpg",
				"CreatorDisplayName": "friend01",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend01@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment03",
				"Order": 4,
				"Description": "friend02 msg",
				"TotalLikes": 3,
				"CreatorImageUrl": "friend02.jpg",
				"CreatorDisplayName": "friend02",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend02@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment04",
				"Order": 5,
				"Description": "waitingForRespond msg",
				"TotalLikes": 4,
				"CreatorImageUrl": "waitingForRespond.jpg",
				"CreatorDisplayName": "waitingForRespond",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "waitingForRespond@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment05",
				"Order": 6,
				"Description": "requestedToBeYourFriend msg",
				"TotalLikes": 5,
				"CreatorImageUrl": "requestedToBeYourFriend.jpg",
				"CreatorDisplayName": "requestedToBeYourFriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "requestedToBeYourFriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment06",
				"Order": 7,
				"Description": "unfriend msg",
				"TotalLikes": 6,
				"CreatorImageUrl": "unfriend.jpg",
				"CreatorDisplayName": "unfriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "unfriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment07",
				"Order": 8,
				"Description": "unfriend msg",
				"TotalLikes": 7,
				"CreatorImageUrl": "unfriend.jpg",
				"CreatorDisplayName": "unfriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "unfriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Teacher request lesson's comments when the lesson have a lot of multi comments' and users was deleted status Then system send all comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageProfileUrl": "sakul.jpg",
			"DeletedDate": "1/1/2016",
			"Subscriptions":
			[
				{
					"id": "Subscription01",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageProfileUrl": "friend01.jpg",
			"DeletedDate": "1/1/2016",
			"Subscriptions":
			[
				{
					"id": "Subscription02",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageProfileUrl": "friend02.jpg",
			"DeletedDate": "1/1/2016",
			"Subscriptions":
			[
				{
					"id": "Subscription03",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageProfileUrl": "waitingForRespond.jpg",
			"DeletedDate": "1/1/2016",
			"Subscriptions":
			[
				{
					"id": "Subscription04",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageProfileUrl": "requestedToBeYourFriend.jpg",
			"DeletedDate": "1/1/2016",
			"Subscriptions":
			[
				{
					"id": "Subscription05",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "sender@mindsage.com",
			"Name": "sender",
			"ImageProfileUrl": "sender.jpg",
			"DeletedDate": "1/1/2016",
			"Subscriptions":
			[
				{
					"id": "Subscription06",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageProfileUrl": "unfriend.jpg",
			"DeletedDate": "1/1/2016",
			"Subscriptions":
			[
				{
					"id": "Subscription07",
					"Role": "Student",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend01@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest01-B",
			"FromUserProfileId": "friend01@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend02@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-B",
			"FromUserProfileId": "friend02@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "waitingForRespond@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-B",
			"FromUserProfileId": "waitingForRespond@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-B",
			"FromUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "unfriend@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-B",
			"FromUserProfileId": "unfriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
		{
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg01",
            "TotalLikes": 5,
			"CreatorImageUrl": "sakul.jpg",
			"CreatorDisplayName": "sakul",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend01@mindsage.com",
            "Description": "friend01 msg",
            "TotalLikes": 2,
			"CreatorImageUrl": "friend01.jpg",
			"CreatorDisplayName": "friend01",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend02@mindsage.com",
            "Description": "friend02 msg",
            "TotalLikes": 3,
			"CreatorImageUrl": "friend02.jpg",
			"CreatorDisplayName": "friend02",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment04",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "waitingForRespond@mindsage.com",
            "Description": "waitingForRespond msg",
            "TotalLikes": 4,
			"CreatorImageUrl": "waitingForRespond.jpg",
			"CreatorDisplayName": "waitingForRespond",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment05",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "requestedToBeYourFriend@mindsage.com",
            "Description": "requestedToBeYourFriend msg",
            "TotalLikes": 5,
			"CreatorImageUrl": "requestedToBeYourFriend.jpg",
			"CreatorDisplayName": "requestedToBeYourFriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment06",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 6,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment07",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 7,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment00",
				"Order": 1,
				"Description": "Teacher Msg01",
				"TotalLikes": 7,
				"CreatorImageUrl": "teacher.jpg",
				"CreatorDisplayName": "teacher",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "teacher@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment01",
				"Order": 2,
				"Description": "Msg01",
				"TotalLikes": 5,
				"CreatorImageUrl": "sakul.jpg",
				"CreatorDisplayName": "sakul",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "sakul@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment02",
				"Order": 3,
				"Description": "friend01 msg",
				"TotalLikes": 2,
				"CreatorImageUrl": "friend01.jpg",
				"CreatorDisplayName": "friend01",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend01@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment03",
				"Order": 4,
				"Description": "friend02 msg",
				"TotalLikes": 3,
				"CreatorImageUrl": "friend02.jpg",
				"CreatorDisplayName": "friend02",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend02@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment04",
				"Order": 5,
				"Description": "waitingForRespond msg",
				"TotalLikes": 4,
				"CreatorImageUrl": "waitingForRespond.jpg",
				"CreatorDisplayName": "waitingForRespond",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "waitingForRespond@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment05",
				"Order": 6,
				"Description": "requestedToBeYourFriend msg",
				"TotalLikes": 5,
				"CreatorImageUrl": "requestedToBeYourFriend.jpg",
				"CreatorDisplayName": "requestedToBeYourFriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "requestedToBeYourFriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment06",
				"Order": 7,
				"Description": "unfriend msg",
				"TotalLikes": 6,
				"CreatorImageUrl": "unfriend.jpg",
				"CreatorDisplayName": "unfriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "unfriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment07",
				"Order": 8,
				"Description": "unfriend msg",
				"TotalLikes": 7,
				"CreatorImageUrl": "unfriend.jpg",
				"CreatorDisplayName": "unfriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "unfriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Teacher request lesson's comments when the lesson have a lot of multi comments' and their users was deleted status Then system all comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have FriendRequest collection with JSON format are
    """
    [
        {
            "id": "FriendRequest01-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend01@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest01-B",
			"FromUserProfileId": "friend01@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "friend02@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest02-B",
			"FromUserProfileId": "friend02@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Friend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "waitingForRespond@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest03-B",
			"FromUserProfileId": "waitingForRespond@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest04-B",
			"FromUserProfileId": "requestedToBeYourFriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-A",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "unfriend@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        },
		{
            "id": "FriendRequest05-B",
			"FromUserProfileId": "unfriend@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "Unfriend",
			"AcceptedDate": "2/1/2016",
			"CreatedDate": "2/1/2016",
			"DeletedDate": "2/1/2016"
        }
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
		{
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg01",
            "TotalLikes": 5,
			"CreatorImageUrl": "sakul.jpg",
			"CreatorDisplayName": "sakul",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
        {
            "id": "Comment02",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend01@mindsage.com",
            "Description": "friend01 msg",
            "TotalLikes": 2,
			"CreatorImageUrl": "friend01.jpg",
			"CreatorDisplayName": "friend01",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment03",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "friend02@mindsage.com",
            "Description": "friend02 msg",
            "TotalLikes": 3,
			"CreatorImageUrl": "friend02.jpg",
			"CreatorDisplayName": "friend02",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment04",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "waitingForRespond@mindsage.com",
            "Description": "waitingForRespond msg",
            "TotalLikes": 4,
			"CreatorImageUrl": "waitingForRespond.jpg",
			"CreatorDisplayName": "waitingForRespond",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment05",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "requestedToBeYourFriend@mindsage.com",
            "Description": "requestedToBeYourFriend msg",
            "TotalLikes": 5,
			"CreatorImageUrl": "requestedToBeYourFriend.jpg",
			"CreatorDisplayName": "requestedToBeYourFriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment06",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 6,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        },
		{
            "id": "Comment07",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "unfriend@mindsage.com",
            "Description": "unfriend msg",
            "TotalLikes": 7,
			"CreatorImageUrl": "unfriend.jpg",
			"CreatorDisplayName": "unfriend",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """ 
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments":
		[
			{
				"id": "Comment00",
				"Order": 1,
				"Description": "Teacher Msg01",
				"TotalLikes": 7,
				"CreatorImageUrl": "teacher.jpg",
				"CreatorDisplayName": "teacher",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "teacher@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment01",
				"Order": 2,
				"Description": "Msg01",
				"TotalLikes": 5,
				"CreatorImageUrl": "sakul.jpg",
				"CreatorDisplayName": "sakul",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "sakul@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment02",
				"Order": 3,
				"Description": "friend01 msg",
				"TotalLikes": 2,
				"CreatorImageUrl": "friend01.jpg",
				"CreatorDisplayName": "friend01",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend01@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment03",
				"Order": 4,
				"Description": "friend02 msg",
				"TotalLikes": 3,
				"CreatorImageUrl": "friend02.jpg",
				"CreatorDisplayName": "friend02",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "friend02@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment04",
				"Order": 5,
				"Description": "waitingForRespond msg",
				"TotalLikes": 4,
				"CreatorImageUrl": "waitingForRespond.jpg",
				"CreatorDisplayName": "waitingForRespond",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "waitingForRespond@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment05",
				"Order": 6,
				"Description": "requestedToBeYourFriend msg",
				"TotalLikes": 5,
				"CreatorImageUrl": "requestedToBeYourFriend.jpg",
				"CreatorDisplayName": "requestedToBeYourFriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "requestedToBeYourFriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment06",
				"Order": 7,
				"Description": "unfriend msg",
				"TotalLikes": 6,
				"CreatorImageUrl": "unfriend.jpg",
				"CreatorDisplayName": "unfriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "unfriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			},
			{
				"id": "Comment07",
				"Order": 8,
				"Description": "unfriend msg",
				"TotalLikes": 7,
				"CreatorImageUrl": "unfriend.jpg",
				"CreatorDisplayName": "unfriend",
				"ClassRoomId": "ClassRoom01",
				"LessonId": "Lesson01",
				"CreatedByUserProfileId": "unfriend@mindsage.com",
				"CreatedDate": "2/1/2016 01:00 am",
			}
		]
	}
    """ 

@mock  
Scenario: Teacher request lesson's comments but used incorrect ClassRoom (unknow) Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'UnknowClassRoom'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": false,
		"Comments": []
	}
    """  

@mock  
Scenario: Teacher request lesson's comments but used incorrect ClassRoom (empty) Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: ''
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": false,
		"Comments": []
	}
    """  

@mock  
Scenario: Teacher request lesson's comments but used incorrect ClassRoom (null) Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'NULL'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": false,
		"Comments": []
	}
    """  

@mock  
Scenario: Teacher request lesson's comments but used incorrect LessonId (unknow) Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'UnknowLesson' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": true,
		"Comments": []
	}
    """  

@mock  
Scenario: Teacher request lesson's comments but used incorrect LessonId (empty) Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request comment from the lesson '' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": false,
		"Comments": []
	}
    """  

@mock  
Scenario: Teacher request lesson's comments but used incorrect LessonId (null) Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """  
    When UserProfile 'teacher@mindsage.com' request comment from the lesson 'NULL' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": false,
		"Comments": []
	}
    """  

@mock  
Scenario: Incorrect user (unknow) request lesson's comments Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """  
    When UserProfile 'unknow@mindsage.com' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": false,
		"Comments": []
	}
    """  

@mock  
Scenario: Incorrect user (empty) request lesson's comments Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """  
    When UserProfile '' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": false,
		"Comments": []
	}
    """  

@mock  
Scenario: Incorrect user (null) request lesson's comments Then system send comments back  
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
					"id": "Subscription00",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		}
    ]
    """  
	And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment00",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "teacher@mindsage.com",
            "Description": "Teacher Msg01",
            "TotalLikes": 7,
			"CreatorImageUrl": "teacher.jpg",
			"CreatorDisplayName": "teacher",
            "LessonId": "Lesson01",
			"CreatedDate": "2/1/2016 01:00 am",
			"Discussions": []
        }
    ]
    """  
    When UserProfile 'NULL' request comment from the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'
    Then System send lesson's comment with JSON format is  
    """
	{
		"IsPrivateAccount": false,
		"IsDiscussionAvailable": false,
		"Comments": []
	}
    """  