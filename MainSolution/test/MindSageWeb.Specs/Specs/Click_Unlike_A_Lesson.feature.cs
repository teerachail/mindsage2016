// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SpecFlow.GeneratedTests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class Click_Unlike_A_LessonFeature : Xunit.IClassFixture<Click_Unlike_A_LessonFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Click_Unlike_A_Lesson.feature"
#line hidden
        
        public Click_Unlike_A_LessonFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Click_Unlike_A_Lesson", "In order to avoid silly mistakes\r\nAs a math idiot\r\nI want to be told the sum of t" +
                    "wo numbers", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 59
    testRunner.And("System have ClassRoom collection with JSON format are", @"[
    {
        ""id"": ""ClassRoom01"",
        ""Name"": ""Emotional literacy"",
        ""CourseCatalogId"": ""CourseCatalog01"",
        ""CreatedDate"": ""2/1/2016"",
        ""Lessons"":
        [
            {
                ""id"": ""Lesson01"",
                ""TotalLikes"": 2,
                ""LessonCatalogId"": ""LessonCatalog01""
            },
            {
                ""id"": ""Lesson02"",
                ""TotalLikes"": 0,
                ""LessonCatalogId"": ""LessonCatalog02""
            },
        ]
    }
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 83
    testRunner.And("System have LikeLesson collection with JSON format are", @"[
    {
        ""id"": ""LikeLesson01"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""LessonId"": ""Lesson01"",
        ""LikedByUserProfileId"": ""miolynet@perfenterprise.com"",
        ""CreatedDate"": ""2/1/2016"",
    },
    {
        ""id"": ""LikeLesson02"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""LessonId"": ""Lesson01"",
        ""LikedByUserProfileId"": ""sakul@mindsage.com"",
        ""CreatedDate"": ""2/1/2016"",
    },
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 102
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
        
        public virtual void SetFixture(Click_Unlike_A_LessonFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Click_Unlike_A_Lesson")]
        [Xunit.TraitAttribute("Description", "User click unlike a lesson Then system update lesson\'s total like")]
        public virtual void UserClickUnlikeALessonThenSystemUpdateLessonSTotalLike()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User click unlike a lesson Then system update lesson\'s total like", new string[] {
                        "mock"});
#line 129
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 130
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 131
    testRunner.When("UserProfileId \'sakul@mindsage.com\' click the like button in the lesson \'Lesson01\'" +
                    " of ClassRoom: \'ClassRoom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 132
    testRunner.Then("System update total likes in the lesson \'Lesson01\' of ClassRoom \'ClassRoom01\' to " +
                    "\'1\' likes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 133
    testRunner.And("System update LikeLesson by JSON format is", "{\r\n        \"id\": \"LikeLesson02\",\r\n        \"ClassRoomId\": \"ClassRoom01\",\r\n        " +
                    "\"LessonId\": \"Lesson01\",\r\n        \"LikedByUserProfileId\": \"sakul@mindsage.com\",\r\n" +
                    "        \"CreatedDate\": \"2/1/2016\",\r\n        \"DeletedDate\": \"2/8/2016 00:00 am\"\r\n" +
                    "}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
 testRunner.And("System doesn\'t update UserActivity collection with JSON format is", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                Click_Unlike_A_LessonFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                Click_Unlike_A_LessonFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion