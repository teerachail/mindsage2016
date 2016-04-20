Feature: Show_Friends_List_Self_Purchase
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
Scenario: SelfPurchase user request friend list Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageUrl": "friend01.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest01-A",
			"IsTeacher": false
		},
		{
			"UserProfileId": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageUrl": "friend02.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest02-A",
			"IsTeacher": false
		},
		{
			"UserProfileId": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageUrl": "waitingForRespond.jpg",
			"Status": "SendRequest",
			"RequestId": "FriendRequest03-A",
			"IsTeacher": false
		},
		{
			"UserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageUrl": "requestedToBeYourFriend.jpg",
			"Status": "ReceiveRequest",
			"RequestId": "FriendRequest04-A",
			"IsTeacher": false
		}
    ]
    """ 

@mock  
Scenario: SelfPurchase user request friend list buy they have no friend requests Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have FriendRequest collection with JSON format are
    """
    []
    """  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: User who hasn't permission to access to the course request friend list Then system send friend list back  
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
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase request friend list but the course doesn't existing Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
	And System have ClassRoom collection with JSON format are  
    """
    []
    """  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: Incorrect user (unknow) request friend list but the course doesn't existing Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'unknow@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: Incorrect user (empty) request friend list but the course doesn't existing Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile '' request friend list from ClassRoom: 'ClassRoom01' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: Incorrect user (null) request friend list but the course doesn't existing Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'NULL' request friend list from ClassRoom: 'ClassRoom01' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase request friend list but the course is incorrect (unknow) Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'unknowClassRoomId' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase request friend list but the course is incorrect (empty) Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: '' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase request friend list but the course is incorrect (null) Then system send friend list back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'NULL' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    []
    """  

@mock  
Scenario: SelfPurchase request friend list but their friends accounts was deleted Then system send friend list back  
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
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    [
		{
			"UserProfileId": "friend01@mindsage.com",
			"Name": "friend01",
			"ImageUrl": "friend01.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest01-A",
			"IsTeacher": false
		},
		{
			"UserProfileId": "friend02@mindsage.com",
			"Name": "friend02",
			"ImageUrl": "friend02.jpg",
			"Status": "Friend",
			"RequestId": "FriendRequest02-A",
			"IsTeacher": false
		},
		{
			"UserProfileId": "waitingForRespond@mindsage.com",
			"Name": "waitingForRespond",
			"ImageUrl": "waitingForRespond.jpg",
			"Status": "SendRequest",
			"RequestId": "FriendRequest03-A",
			"IsTeacher": false
		},
		{
			"UserProfileId": "requestedToBeYourFriend@mindsage.com",
			"Name": "requestedToBeYourFriend",
			"ImageUrl": "requestedToBeYourFriend.jpg",
			"Status": "ReceiveRequest",
			"RequestId": "FriendRequest04-A",
			"IsTeacher": false
		}
    ]
    """ 

@mock  
Scenario: SelfPurchase request friend list but their friends accounts doesn't existing Then system send friend list back  
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
    When UserProfile 'sakul@mindsage.com' request friend list from ClassRoom: 'ClassRoom01' by selfpurchase method  
    Then System send friend list with JSON format are  
    """
    []
    """  