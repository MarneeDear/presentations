//using system;
//using techtalk.specflow;
//using techtalk.specflow.assist;
//using presentation;
//using microsoft.visualstudio.testtools.unittesting;


//namespace specflowstarterproject
//{
//	[binding]
//	public class specflowpresentationsteps
//	{
//		[given(@"user marnee is presenting specflow")]
//		public void givenusermarneeispresentingspecflow(table table)
//		{
//			presenter presenter = table.createinstance<presenter>();
//			scenariocontext.current["presenter"] = presenter;
//		}

//		[when(@"marnee gives the talk to the group")]
//		public void whenmarneegivesthetalktothegroup(table table)
//		{
//			presenter presenter = (presenter)scenariocontext.current["presenter"];
//			group group = table.createinstance<group>();
//			presenter.givepresentation(ref group);
//			scenariocontext.current["group"] = group;
//		}

//		[then(@"we learn about behavior driven development and specflow")]
//		public void thenwelearnaboutbehaviordrivendevelopmentandspecflow()
//		{
//			group group = (group)scenariocontext.current["group"];
//			assert.isfalse(group.welearnedallthethings);
//		}

//	}
//}
