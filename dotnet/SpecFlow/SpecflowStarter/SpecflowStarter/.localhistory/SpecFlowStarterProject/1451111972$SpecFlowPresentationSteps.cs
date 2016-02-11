//using System;
//using TechTalk.SpecFlow;
//using TechTalk.SpecFlow.Assist;
//using Presentation;
//using Microsoft.VisualStudio.TestTools.UnitTesting;


//namespace SpecFlowStarterProject
//{
//    [Binding]
//    public class SpecFlowPresentationSteps
//    {
//        [Given(@"user Marnee is presenting SpecFlow")]
//        public void GivenUserMarneeIsPresentingSpecFlow(Table table)
//        {
//            Presenter presenter = table.CreateInstance<Presenter>();
//            ScenarioContext.Current["presenter"] = presenter;
//        }

//        [When(@"Marnee gives the talk to the group")]
//        public void WhenMarneeGivesTheTalkToTheGroup(Table table)
//        {
//            Presenter presenter = (Presenter)ScenarioContext.Current["presenter"];
//            Group group = table.CreateInstance<Group>();
//            presenter.GivePresentation(ref group);
//            ScenarioContext.Current["group"] = group;
//        }

//        [Then(@"we learn about Behavior Driven Development and SpecFlow")]
//        public void ThenWeLearnAboutBehaviorDrivenDevelopmentAndSpecFlow()
//        {
//            Group group = (Group)ScenarioContext.Current["group"];
//            Assert.IsFalse(group.WeLearnedAllTheThings);
//        }

//    }
//}
