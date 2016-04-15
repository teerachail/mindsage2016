// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.0.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SpecFlow.GeneratedTests.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class Get_Course_ScheduleFeature : Xunit.IClassFixture<Get_Course_ScheduleFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Get_Course_Schedule.feature"
#line hidden
        
        public Get_Course_ScheduleFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Get_Course_Schedule", "\tIn order to avoid silly mistakes\r\n\tAs a math idiot\r\n\tI want to be told the sum o" +
                    "f two numbers", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line 7
 testRunner.Given("Initialize mocking data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 8
    testRunner.And("System have UserProfile collection with JSON format are", @"[
{
""id"": ""sakul@mindsage.com"",
""Name"": ""Sakul jaruthanaset"",
""ImageProfileUrl"": ""ImgURL01"",
""Subscriptions"":
[
{
	""id"": ""Subscription01"",
	""Role"": ""Teacher"",
	""ClassRoomId"": ""ClassRoom01"",
	""ClassCalendarId"": ""ClassCalendar01"",
},
]
},
{
""id"": ""student@mindsage.com"",
""Name"": ""Student jaruthanaset"",
""ImageProfileUrl"": ""ImgURL02"",
""Subscriptions"":
[
{
	""id"": ""Subscription02"",
	""Role"": ""Student"",
	""ClassRoomId"": ""ClassRoom01"",
	""ClassCalendarId"": ""ClassCalendar01"",
},
]
},
{
""id"": ""selfpurchase@mindsage.com"",
""Name"": ""Selfpurchase jaruthanaset"",
""ImageProfileUrl"": ""ImgURL03"",
""Subscriptions"":
[
{
	""id"": ""Subscription03"",
	""Role"": ""SelfPurchaser"",
	""ClassRoomId"": ""ClassRoom01"",
	""ClassCalendarId"": ""ClassCalendar01"",
},
]
}
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 55
 testRunner.And("System have ClassCalendar collection with JSON format are", @"[
    {
        ""id"": ""ClassCalendar01"",
        ""BeginDate"": ""1/1/2016"",
""ExpiredDate"": ""1/1/2017"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""LessonCalendars"":
        [
            {
                ""Id"": ""LessonCalendar01"",
                ""Order"": 1,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""1/1/2016"",
	""TopicOfTheDays"": [],
                ""LessonId"": ""Lesson01"",
            },
{
                ""Id"": ""LessonCalendar02"",
                ""Order"": 2,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""1/6/2016"",
	""TopicOfTheDays"": [],
                ""LessonId"": ""Lesson01"",
            }
        ],
""Holidays"": [],
""ShiftDays"": []
    }
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        public virtual void SetFixture(Get_Course_ScheduleFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Teacher request course schedule Then system send course schedule to the teacher")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void TeacherRequestCourseScheduleThenSystemSendCourseScheduleToTheTeacher()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Teacher request course schedule Then system send course schedule to the teacher", new string[] {
                        "mock"});
#line 89
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 90
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 91
 testRunner.When("User \'sakul@mindsage.com\' request course schedule from ClassRoomId \'ClassRoom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 92
 testRunner.Then("System send course schedule with JSON format is", @"{
	""IsComplete"": true,
	""BeginDate"": ""1/1/2016"",
	""EndDate"": ""1/1/2017"",
	""Lessons"": [
		{
			""Name"": ""Lesson 1"",
			""BeginDate"": ""1/1/2016"",
		},
		{
			""Name"": ""Lesson 2"",
			""BeginDate"": ""1/6/2016"",
		}
	],
	""Holidays"": [],
	""ShiftDays"": []
}", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Teacher request incorrect course schedule Then system send null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void TeacherRequestIncorrectCourseScheduleThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Teacher request incorrect course schedule Then system send null back", new string[] {
                        "mock"});
#line 114
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 115
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 116
 testRunner.When("User \'sakul@mindsage.com\' request course schedule from ClassRoomId \'IncorrectClas" +
                    "sRoom\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 117
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Teacher request incorrect course schedule (empty) Then system send null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void TeacherRequestIncorrectCourseScheduleEmptyThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Teacher request incorrect course schedule (empty) Then system send null back", new string[] {
                        "mock"});
#line 120
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 121
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 122
 testRunner.When("User \'sakul@mindsage.com\' request course schedule from ClassRoomId \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 123
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Teacher request incorrect course schedule (null) Then system send null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void TeacherRequestIncorrectCourseScheduleNullThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Teacher request incorrect course schedule (null) Then system send null back", new string[] {
                        "mock"});
#line 126
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 127
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 128
 testRunner.When("User \'sakul@mindsage.com\' request course schedule from ClassRoomId \'NULL\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 129
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Unknow user request course schedule Then system send null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void UnknowUserRequestCourseScheduleThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Unknow user request course schedule Then system send null back", new string[] {
                        "mock"});
#line 132
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 133
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 134
 testRunner.When("User \'unknow@mindsage.com\' request course schedule from ClassRoomId \'ClassRoom01\'" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 135
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Unknow user request incorrect course schedule Then system send null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void UnknowUserRequestIncorrectCourseScheduleThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Unknow user request incorrect course schedule Then system send null back", new string[] {
                        "mock"});
#line 138
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 139
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 140
 testRunner.When("User \'unknow@mindsage.com\' request course schedule from ClassRoomId \'IncorrectCla" +
                    "ssRoom\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 141
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Unknow user (empty) request incorrect course schedule Then system send null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void UnknowUserEmptyRequestIncorrectCourseScheduleThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Unknow user (empty) request incorrect course schedule Then system send null back", new string[] {
                        "mock"});
#line 144
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 145
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 146
 testRunner.When("User \'\' request course schedule from ClassRoomId \'IncorrectClassRoom\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 147
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Unknow user (null) request incorrect course schedule Then system send null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void UnknowUserNullRequestIncorrectCourseScheduleThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Unknow user (null) request incorrect course schedule Then system send null back", new string[] {
                        "mock"});
#line 150
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 151
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 152
 testRunner.When("User \'NULL\' request course schedule from ClassRoomId \'IncorrectClassRoom\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 153
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Student request course schedule Then system send null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void StudentRequestCourseScheduleThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Student request course schedule Then system send null back", new string[] {
                        "mock"});
#line 156
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 157
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 158
 testRunner.When("User \'student@mindsage.com\' request course schedule from ClassRoomId \'ClassRoom01" +
                    "\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 159
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Selfpurchase request course schedule Then system send null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void SelfpurchaseRequestCourseScheduleThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Selfpurchase request course schedule Then system send null back", new string[] {
                        "mock"});
#line 162
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 163
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 164
 testRunner.When("User \'selfpurchase@mindsage.com\' request course schedule from ClassRoomId \'ClassR" +
                    "oom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 165
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Teacher request course schedule when the course doesn\'t set the begin date Then s" +
            "ystem send course schedule to the teacher")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void TeacherRequestCourseScheduleWhenTheCourseDoesnTSetTheBeginDateThenSystemSendCourseScheduleToTheTeacher()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Teacher request course schedule when the course doesn\'t set the begin date Then s" +
                    "ystem send course schedule to the teacher", new string[] {
                        "mock"});
#line 168
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 169
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 170
 testRunner.And("System have ClassCalendar collection with JSON format are", @"[
    {
        ""id"": ""ClassCalendar01"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""LessonCalendars"":
        [
            {
                ""Id"": ""LessonCalendar01"",
                ""Order"": 1,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""1/1/2016"",
	""TopicOfTheDays"": [],
                ""LessonId"": ""Lesson01"",
            },
{
                ""Id"": ""LessonCalendar02"",
                ""Order"": 2,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""1/6/2016"",
	""TopicOfTheDays"": [],
                ""LessonId"": ""Lesson01"",
            }
        ],
""Holidays"": [],
""ShiftDays"": []
    }
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 200
 testRunner.When("User \'sakul@mindsage.com\' request course schedule from ClassRoomId \'ClassRoom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 201
 testRunner.Then("System send course schedule with JSON format is", "{\r\n\t\"IsComplete\": true,\r\n\t\"Lessons\": [\r\n\t\t{\r\n\t\t\t\"Name\": \"Lesson 1\",\r\n\t\t\t\"BeginDat" +
                    "e\": \"1/1/2016\",\r\n\t\t},\r\n\t\t{\r\n\t\t\t\"Name\": \"Lesson 2\",\r\n\t\t\t\"BeginDate\": \"1/6/2016\",\r" +
                    "\n\t\t}\r\n\t],\r\n\t\"Holidays\": [],\r\n\t\"ShiftDays\": []\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Teacher request course schedule but the ClassCalendar was deleted Then system sen" +
            "d null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void TeacherRequestCourseScheduleButTheClassCalendarWasDeletedThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Teacher request course schedule but the ClassCalendar was deleted Then system sen" +
                    "d null back", new string[] {
                        "mock"});
#line 221
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 222
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 223
 testRunner.And("System have ClassCalendar collection with JSON format are", @"[
    {
        ""id"": ""ClassCalendar01"",
        ""BeginDate"": ""1/1/2016"",
""ExpiredDate"": ""1/1/2017"",
""DeletedDate"": ""1/1/2016"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""LessonCalendars"":
        [
            {
                ""Id"": ""LessonCalendar01"",
                ""Order"": 1,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""1/1/2016"",
	""TopicOfTheDays"": [],
                ""LessonId"": ""Lesson01"",
            },
{
                ""Id"": ""LessonCalendar02"",
                ""Order"": 2,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""1/6/2016"",
	""TopicOfTheDays"": [],
                ""LessonId"": ""Lesson01"",
            }
        ],
""Holidays"": [],
""ShiftDays"": []
    }
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 256
 testRunner.When("User \'sakul@mindsage.com\' request course schedule from ClassRoomId \'ClassRoom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 257
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Teacher request course schedule but the ClassCalendar was closed Then system send" +
            " null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void TeacherRequestCourseScheduleButTheClassCalendarWasClosedThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Teacher request course schedule but the ClassCalendar was closed Then system send" +
                    " null back", new string[] {
                        "mock"});
#line 260
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 261
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 262
 testRunner.And("System have ClassCalendar collection with JSON format are", @"[
    {
        ""id"": ""ClassCalendar01"",
        ""BeginDate"": ""1/1/2016"",
""ExpiredDate"": ""1/1/2017"",
""CloseDate"": ""1/1/2016"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""LessonCalendars"":
        [
            {
                ""Id"": ""LessonCalendar01"",
                ""Order"": 1,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""1/1/2016"",
	""TopicOfTheDays"": [],
                ""LessonId"": ""Lesson01"",
            },
{
                ""Id"": ""LessonCalendar02"",
                ""Order"": 2,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""1/6/2016"",
	""TopicOfTheDays"": [],
                ""LessonId"": ""Lesson01"",
            }
        ],
""Holidays"": [],
""ShiftDays"": []
    }
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 295
 testRunner.When("User \'sakul@mindsage.com\' request course schedule from ClassRoomId \'ClassRoom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 296
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Get_Course_Schedule")]
        [Xunit.TraitAttribute("Description", "Teacher request course schedule but the ClassCalendar doesn\'t existing Then syste" +
            "m send null back")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void TeacherRequestCourseScheduleButTheClassCalendarDoesnTExistingThenSystemSendNullBack()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Teacher request course schedule but the ClassCalendar doesn\'t existing Then syste" +
                    "m send null back", new string[] {
                        "mock"});
#line 299
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 300
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 301
 testRunner.And("System have ClassCalendar collection with JSON format are", "[]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 305
 testRunner.When("User \'sakul@mindsage.com\' request course schedule from ClassRoomId \'ClassRoom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 306
 testRunner.Then("System send null back", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                Get_Course_ScheduleFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                Get_Course_ScheduleFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion