Feature: Search_Friends_Self_Purchase
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
			"IsPublic": "true",
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
Scenario: SelfPurchase user search friend with matched keyword (friend only) Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend0' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageUrl": "friend01.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest01-A"
		},
		{
			"UserProfileId": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageUrl": "friend02.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest02-A"
		}
    ]
    """  

@mock  
Scenario: SelfPurchase user search friend with matched keyword (Waiting respond only) Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'waitingForRespond' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageUrl": "waitingForRespond.jpg",
			"Status": "SendRequest",
			"RequestId": "FriendRequest03-A",
		}
    ]
    """  

@mock  
Scenario: SelfPurchase user search friend with matched keyword (Received request only) Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'requestedToBeYourFriend' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageUrl": "requestedToBeYourFriend.jpg",
			"Status": "ReceiveRequest",
			"RequestId": "FriendRequest04-A",
		}
    ]
    """  

@mock  
Scenario: SelfPurchase user search friend with matched keyword (Unfriend only) Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'unfriend' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageUrl": "unfriend.jpg",
			"Status": "Unfriend",
			"RequestId": "FriendRequest05-A",
		}
    ]
    """  

@mock  
Scenario: SelfPurchase user search friend with matched keyword (case sensitive) Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'FRIEnd' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageUrl": "friend01.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest01-A"
		},
		{
			"UserProfileId": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageUrl": "friend02.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest02-A"
		},
		{
			"UserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageUrl": "requestedToBeYourFriend.jpg",
			"Status": "ReceiveRequest",
			"RequestId": "FriendRequest04-A",
		},
		{
			"UserProfileId": "unfriend@mindsage.com",
			"Name": "unfriend",
			"ImageUrl": "unfriend.jpg",
			"Status": "Unfriend",
			"RequestId": "FriendRequest05-A",
		}
    ]
    """  

@mock  
Scenario: SelfPurchase user search friend with no matched keyword Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'UnmatchedKeyword' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend with matched keyword but those users was deleted Then system send matched users list back  
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
		},
    ]
    """  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend0' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend with matched keyword but those users' subscriptions was deleted Then system send matched users list back  
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
					"DeletedDate": "1/1/2016",
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
					"DeletedDate": "1/1/2016",
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
					"DeletedDate": "1/1/2016",
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
					"DeletedDate": "1/1/2016",
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
					"DeletedDate": "1/1/2016",
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
					"DeletedDate": "1/1/2016",
				}
			]
		},
    ]
    """  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend0' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend when user is the only one in the course Then system send matched users list back  
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
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend0' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend with no matched keyword (empty) Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword '' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend with no matched keyword (null) Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'NULL' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: Incorrect user (unknow) search friend with matched keyword Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'unknow@mindsage.com' search friends by keyword 'friend' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: Incorrect user (empty) search friend with matched keyword Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile '' search friends by keyword 'friend' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: Incorrect user (null) search friend with matched keyword Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'NULL' search friends by keyword 'friend' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: Incorrect user (no subscription) search friend Then system send matched users list back  
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
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend but used incorrect ClassRoom (unknow) Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend' from ClassRoom: 'UnknowClassRoom'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend but used incorrect ClassRoom (empty) Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend' from ClassRoom: ''  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend but used incorrect ClassRoom (null) Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend' from ClassRoom: 'NULL'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend but ClassRoom was deleted Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassRoom collection with JSON format are  
    """
    [
        {
            "id": "ClassRoom01",
            "Name": "Emotional literacy",
            "CourseCatalogId": "CourseCatalog01",
            "CreatedDate": "2/1/2016",
            "Message": "Don't forget to comment a lesson!",
			"IsPublic": "true",
			"DeletedDate": "1/1/2016",
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
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend but ClassRoom doesn't existing Then system send matched users list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassRoom collection with JSON format are  
    """
    []
    """  
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase user search friend but its subscription was deleted Then system send matched users list back  
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
    When UserProfile 'sakul@mindsage.com' search friends by keyword 'friend' from ClassRoom: 'ClassRoom01'  
    Then System send friend list with JSON format are  
    """
    []
    """  