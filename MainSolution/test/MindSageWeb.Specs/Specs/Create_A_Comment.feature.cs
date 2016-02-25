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
    public partial class Create_A_CommentFeature : Xunit.IClassFixture<Create_A_CommentFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Create_A_Comment.feature"
#line hidden
        
        public Create_A_CommentFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Create_A_Comment", "\tIn order to avoid silly mistakes\r\n\tAs a math idiot\r\n\tI want to be told the sum o" +
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
    testRunner.And("System have UserProfile collection with JSON format are", "[\r\n{\r\n\"id\": \"sakul@mindsage.com\",\r\n\"Name\": \"Sakul jaruthanaset\",\r\n\"ImageProfileUr" +
                    "l\": \"ImgURL01\",\r\n\"Subscriptions\":\r\n[\r\n{\r\n\t\"id\": \"Subscription01\",\r\n\t\"Role\": \"Tea" +
                    "cher\",\r\n\t\"ClassRoomId\": \"ClassRoom01\",\r\n\t\"ClassCalendarId\": \"ClassCalendar01\",\r\n" +
                    "},\r\n]\r\n},\r\n]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 27
    testRunner.And("System have ClassCalendar collection with JSON format are", @"[
    {
        ""id"": ""ClassCalendar01"",
        ""BeginDate"": ""2/1/2016"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""LessonCalendars"":
        [
            {
                ""Id"": ""LessonCalendar01"",
                ""LessonId"": ""Lesson01"",
                ""Order"": 1,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""2/1/2016"",
            },
            {
                ""Id"": ""LessonCalendar02"",
                ""LessonId"": ""Lesson02"",
                ""Order"": 2,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""2/8/2016"",
            },
            {
                ""Id"": ""LessonCalendar03"",
                ""LessonId"": ""Lesson03"",
                ""Order"": 3,
                ""SemesterGroupName"": ""B"",
                ""BeginDate"": ""2/15/2016"",
            },
        ]
    },
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 61
    testRunner.And("System have UserActivity collection with JSON format are", @"   [
	{
		""id"": ""UserActivity01"",
		""UserProfileId"": ""sakul@mindsage.com"",
		""ClassRoomId"": ""ClassRoom01"",
		""LessonActivities"":
		[
			{
				""id"": ""LessonActivity01"",
				""LessonId"": ""Lesson01"",

				""TotalContentsAmount"": 1,
				""SawContentIds"": 
				[
					""Content01""
				],
				""CreatedCommentAmount"": 0,
				""ParticipationAmount"": 1
			}
		]
	}
   ]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        public virtual void SetFixture(Create_A_CommentFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Create_A_Comment")]
        [Xunit.TraitAttribute("Description", "User create a new comment Then system create a new comment")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void UserCreateANewCommentThenSystemCreateANewComment()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User create a new comment Then system create a new comment", new string[] {
                        "mock"});
#line 88
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 89
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 90
    testRunner.When("UserProfileId \'sakul@mindsage.com\' create a new comment with a message is \'Hello " +
                    "lesson 1\' in the lesson \'Lesson01\' of ClassRoom: \'ClassRoom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 91
    testRunner.Then("System add new Comment by JSON format is", @"{
    ""ClassRoomId"": ""ClassRoom01"",
    ""CreatedByUserProfileId"": ""sakul@mindsage.com"",
""CreatorDisplayName"": ""Sakul jaruthanaset"",
""CreatorImageUrl"": ""ImgURL01"",
    ""Description"": ""Hello lesson 1"",
    ""TotalLikes"": 0,
    ""LessonId"": ""Lesson01"",
    ""Discussions"": [],
""CreatedDate"": ""2/8/2016 00:00 am""
}", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 105
    testRunner.And("System update UserActivity collection with JSON format is", @"{
""id"": ""UserActivity01"",
""UserProfileId"": ""sakul@mindsage.com"",
""ClassRoomId"": ""ClassRoom01"",
""LessonActivities"":
[
{
""id"": ""LessonActivity01"",
""LessonId"": ""Lesson01"",

""TotalContentsAmount"": 1,
""SawContentIds"": 
[
	""Content01""
],
""CreatedCommentAmount"": 1,
""ParticipationAmount"": 1
}
]
}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                Create_A_CommentFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                Create_A_CommentFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
