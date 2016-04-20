Feature: Show_Friends_List_Student
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
		},
		{
			"id": "teacher@mindsage.com",
			"Name": "teacher",
			"ImageProfileUrl": "teacher.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription08",
					"Role": "Teacher",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
				}
			]
		},
		{
			"id": "selfpurchase@mindsage.com",
			"Name": "selfpurchase",
			"ImageProfileUrl": "selfpurchase.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription09",
					"Role": "SelfPurchaser",
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
    
@mock  
Scenario: A student request friend list Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' 
    Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageUrl": "friend01.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest01-A",
		},
		{
			"UserProfileId": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageUrl": "friend02.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest02-A",
		},
		{
			"UserProfileId": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageUrl": "waitingForRespond.jpg",
			"Status": "SendRequest",
			"RequestId": "FriendRequest03-A",
		},
		{
			"UserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageUrl": "requestedToBeYourFriend.jpg",
			"Status": "ReceiveRequest",
			"RequestId": "FriendRequest04-A",
		},
		{
			"UserProfileId": "sender@mindsage.com",
			"Name": "sender",
			"ImageUrl": "sender.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageUrl": "unfriend.jpg",
			"Status": "Unfriend",
			"RequestId": "FriendRequest05-A",
		},
		{
			"UserProfileId": "teacher@mindsage.com",
			"Name": "teacher",
			"ImageUrl": "teacher.jpg",
			"Status": "Unfriend",
			"IsTeacher": true
		},
		{
			"UserProfileId": "selfpurchase@mindsage.com",
			"Name": "selfpurchase",
			"ImageUrl": "selfpurchase.jpg",
			"Status": "Unfriend",
		}
    ]
    """  

@mock  
Scenario: A student who has no friends request friend list Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have FriendRequest collection with JSON format are
    """
    []
    """  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' 
    Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageUrl": "friend01.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageUrl": "friend02.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageUrl": "waitingForRespond.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageUrl": "requestedToBeYourFriend.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "sender@mindsage.com",
			"Name": "sender",
			"ImageUrl": "sender.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageUrl": "unfriend.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "teacher@mindsage.com",
			"Name": "teacher",
			"ImageUrl": "teacher.jpg",
			"Status": "Unfriend",
			"IsTeacher": true
		},
		{
			"UserProfileId": "selfpurchase@mindsage.com",
			"Name": "selfpurchase",
			"ImageUrl": "selfpurchase.jpg",
			"Status": "Unfriend",
		}
    ]
    """  

@mock  
Scenario: A student who hasn't permission to access the course request friend list Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
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
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' 
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: A student request friend list but the course doesn't existing Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassRoom collection with JSON format are  
    """
    []
    """  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' 
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: Invalid user (unknow) request friend list but the course doesn't existing Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'unknow@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' 
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: Invalid user (empty) request friend list but the course doesn't existing Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile '' request friend list from ClassRoom: 'ClassRoom01' 
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: Invalid user (null) request friend list but the course doesn't existing Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'NULL' request friend list from ClassRoom: 'ClassRoom01' 
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: A student request friend list but used invalid classroom (unknow) Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'   
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'UnknowClassRoom0Id' 
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: A student request friend list but used invalid classroom (empty) Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: '' 
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: A student request friend list but used invalid classroom (null) Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'NULL' 
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: A student request friend list but other users was leave from the course Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "Sakul",
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
		},
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
		},
		{
			"id": "selfpurchase@mindsage.com",
			"Name": "selfpurchase",
			"ImageProfileUrl": "selfpurchase.jpg",
			"Subscriptions":
			[
				{
					"id": "Subscription09",
					"Role": "SelfPurchaser",
					"ClassRoomId": "ClassRoom01",
					"ClassCalendarId": "ClassCalendar01",
					"DeletedDate": "1/1/2016"
				}
			]
		},
    ]
    """   
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' 
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: A student request friend list but other users don't existing Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have UserProfile collection with JSON format are  
    """
    [
		{
			"id": "sakul@mindsage.com",
			"Name": "Sakul",
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
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' 
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: A teacher request friend list Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'teacher@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' 
   Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageUrl": "sakul.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageUrl": "friend01.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageUrl": "friend02.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageUrl": "waitingForRespond.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageUrl": "requestedToBeYourFriend.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "sender@mindsage.com",
			"Name": "sender",
			"ImageUrl": "sender.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageUrl": "unfriend.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "selfpurchase@mindsage.com",
			"Name": "selfpurchase",
			"ImageUrl": "selfpurchase.jpg",
			"Status": "Unfriend",
		}
    ]
    """  

@mock  
Scenario: A SelfPurchase request friend list Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'selfpurchase@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' 
   Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "sakul@mindsage.com",
			"Name": "sakul",
			"ImageUrl": "sakul.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageUrl": "friend01.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageUrl": "friend02.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageUrl": "waitingForRespond.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageUrl": "requestedToBeYourFriend.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "sender@mindsage.com",
			"Name": "sender",
			"ImageUrl": "sender.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageUrl": "unfriend.jpg",
			"Status": "Unfriend",
		},
		{
			"UserProfileId": "teacher@mindsage.com",
			"Name": "teacher",
			"ImageUrl": "teacher.jpg",
			"Status": "Unfriend",
			"IsTeacher": true
		}
    ]
    """  