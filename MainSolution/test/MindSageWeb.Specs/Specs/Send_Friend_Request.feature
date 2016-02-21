Feature: Send_Friend_Request
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
		{
			"id": "earn@mindsage.com",
			"Name": "Earn",
			"ImageProfileUrl": "earn.jpg",
			"Subscriptions": []
		},
    ]
    """  

@mock  
Scenario: User send friend request Then system record the request  
    Given Today is '2/8/2016 00:00 am'  
	And System have FriendRequest collection with JSON format are
    """
    []
    """  
    When UserProfile 'sakul@mindsage.com' send friend request to UserProfile 'earn@mindsage.com'
    Then System upsert FriendRequest with JSON format is
    """
	{
		"FromUserProfileId": "sakul@mindsage.com",
		"ToUserProfileId": "earn@mindsage.com",
		"Status": "SendRequest",
		"CreatedDate": "2/8/2016 00:00 am",
    }
    """ 
	And System upsert FriendRequest with JSON format is
    """
	{
		"FromUserProfileId": "earn@mindsage.com",
		"ToUserProfileId": "sakul@mindsage.com",
		"Status": "ReceiveRequest",
		"CreatedDate": "2/8/2016 00:00 am",
    }
    """  

@mock  
Scenario: User accept friend request Then system record there are friend  
    Given Today is '2/8/2016 00:00 am'  
	And System have FriendRequest collection with JSON format are
    """
    [
		{
			"id": "FriendRequest01",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "earn@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/8/2016 00:00 am",
		},
		{
			"id": "FriendRequest02",
			"FromUserProfileId": "earn@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/8/2016 00:00 am",
		},
	]
    """  
    When UserProfile 'sakul@mindsage.com' respond friend request to UserProfile 'earn@mindsage.com' by RequestId 'FriendRequest01' IsAccept 'true'
    Then System upsert FriendRequest with JSON format is
    """
	{
		"id": "FriendRequest01",
		"FromUserProfileId": "sakul@mindsage.com",
		"ToUserProfileId": "earn@mindsage.com",
		"Status": "Friend",
		"AcceptedDate": "2/8/2016 00:00 am",
		"CreatedDate": "2/8/2016 00:00 am",
	}
    """ 
	And System upsert FriendRequest with JSON format is
    """
	{
		"id": "FriendRequest02",
		"FromUserProfileId": "earn@mindsage.com",
		"ToUserProfileId": "sakul@mindsage.com",
		"Status": "Friend",
		"AcceptedDate": "2/8/2016 00:00 am",
		"CreatedDate": "2/8/2016 00:00 am",
	}
    """  

@mock  
Scenario: User reject friend request Then system record there aren't friend  
    Given Today is '2/8/2016 00:00 am'  
	And System have FriendRequest collection with JSON format are
    """
    [
		{
			"id": "FriendRequest01",
			"FromUserProfileId": "sakul@mindsage.com",
			"ToUserProfileId": "earn@mindsage.com",
			"Status": "ReceiveRequest",
			"CreatedDate": "2/8/2016 00:00 am",
		},
		{
			"id": "FriendRequest02",
			"FromUserProfileId": "earn@mindsage.com",
			"ToUserProfileId": "sakul@mindsage.com",
			"Status": "SendRequest",
			"CreatedDate": "2/8/2016 00:00 am",
		},
	]
    """  
    When UserProfile 'sakul@mindsage.com' respond friend request to UserProfile 'earn@mindsage.com' by RequestId 'FriendRequest01' IsAccept 'false'
    Then System upsert FriendRequest with JSON format is
    """
	{
		"id": "FriendRequest01",
		"FromUserProfileId": "sakul@mindsage.com",
		"ToUserProfileId": "earn@mindsage.com",
		"Status": "Unfriend",
		"CreatedDate": "2/8/2016 00:00 am",
		"DeletedDate": "2/8/2016 00:00 am",
	}
    """ 
	And System upsert FriendRequest with JSON format is
    """
	{
		"id": "FriendRequest02",
		"FromUserProfileId": "earn@mindsage.com",
		"ToUserProfileId": "sakul@mindsage.com",
		"Status": "Unfriend",
		"CreatedDate": "2/8/2016 00:00 am",
		"DeletedDate": "2/8/2016 00:00 am",
	}
    """  