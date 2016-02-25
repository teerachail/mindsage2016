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
    public partial class Remove_A_DiscussionFeature : Xunit.IClassFixture<Remove_A_DiscussionFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Remove_A_Discussion.feature"
#line hidden
        
        public Remove_A_DiscussionFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Remove_A_Discussion", "\tIn order to avoid silly mistakes\r\n\tAs a math idiot\r\n\tI want to be told the sum o" +
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
    testRunner.And("System have UserProfile collection with JSON format are", "[\r\n{\r\n\"id\": \"sakul@mindsage.com\",\r\n\"Subscriptions\":\r\n[\r\n{\r\n\t\"id\": \"Subscription01" +
                    "\",\r\n\t\"Role\": \"Teacher\",\r\n\t\"ClassRoomId\": \"ClassRoom01\",\r\n\t\"ClassCalendarId\": \"Cl" +
                    "assCalendar01\",\r\n},\r\n]\r\n},\r\n]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 25
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
                ""LessonCatalogId"": ""LessonCatalog01"",
                ""Order"": 1,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""2/1/2016"",
            }
        ]
    },
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 46
    testRunner.And("System have Comment collection with JSON format are", @"[
    {
        ""Id"": ""Comment01"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""CreatedByUserProfileId"": ""sakul@mindsage.com"",
        ""Description"": ""Hello lesson 1"",
        ""TotalLikes"": 0,
        ""LessonId"": ""Lesson01"",
        ""Discussions"":
[
{
	""id"": ""Discussion01"",
	""Description"": ""This is a discussion"",
	""TotalLikes"": 0,
	""CreatorImageUrl"": ""ImgURL01"",
	""CreatorDisplayName"": ""Sakul jaruthanaset"",
	""CreatedByUserProfileId"": ""sakul@mindsage.com"",
}
]
    }
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 70
    testRunner.And("System have UserActivity collection with JSON format are", @"[
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
	""CreatedCommentAmount"": 1,
	""ParticipationAmount"": 1
}
]
}
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        public virtual void SetFixture(Remove_A_DiscussionFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Remove_A_Discussion")]
        [Xunit.TraitAttribute("Description", "User remove his comment Then system remove his comment")]
        [Xunit.TraitAttribute("Category", "mock")]
        public virtual void UserRemoveHisCommentThenSystemRemoveHisComment()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User remove his comment Then system remove his comment", new string[] {
                        "mock"});
#line 97
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 98
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 99
    testRunner.When("UserProfileId \'sakul@mindsage.com\' remove the discussion \'Discussion01\' from comm" +
                    "ent \'Comment01\' in the lesson \'Lesson01\' of ClassRoom: \'ClassRoom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 100
 testRunner.Then("System update Discussion collection with JSON format in the Comment \'Comment01\' a" +
                    "re", "[\r\n{\r\n\"id\": \"Discussion01\",\r\n\"Description\": \"This is a discussion\",\r\n\"TotalLikes\"" +
                    ": 0,\r\n\"CreatorImageUrl\": \"ImgURL01\",\r\n\"CreatorDisplayName\": \"Sakul jaruthanaset\"" +
                    ",\r\n\"CreatedByUserProfileId\": \"sakul@mindsage.com\",\r\n\"DeletedDate\": \"2/8/2016 00:" +
                    "00 am\"\r\n}\r\n]", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 114
    testRunner.And("System doesn\'t update UserActivity collection with JSON format is", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                Remove_A_DiscussionFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                Remove_A_DiscussionFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
