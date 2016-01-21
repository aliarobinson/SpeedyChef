using System;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;
using System.Linq;

namespace SpeedyChef.UITests
{
	[TestFixture]
	public class SearchActivityTests
	{
		private AndroidApp app;

		[SetUp]
		public void BeforeEachTest ()
		{
			app = ConfigureApp.Android.StartApp ();
			Func<AppQuery, AppQuery> menu_button_query = e => e.Id("menu_button");
			app.Tap (menu_button_query);
			Func<AppQuery, AppQuery> search_button = e => e.Text ("Search");
			app.Tap (search_button);
		}

		[Test]
		public void searchResultTest ()
		{
			Func<AppQuery, AppQuery> main_search = e => e.Id ("main_search");
			app.EnterText(main_search, "italian");

			Func<AppQuery, AppQuery> text_view_left = e => e.Id ("textViewLeft");
			app.WaitForElement (text_view_left);

			AppResult[] search_result = app.Query(text_view_left);
			Assert.AreEqual (2, search_result.Length);

			Assert.True(search_result[0].Text.Contains ("Italian Pasta"));
			Assert.True(search_result[1].Text.Contains ("Cannoli"));
		}

		[Test]
		public void filterByTimeTest ()
		{
			Func<AppQuery, AppQuery> main_search = e => e.Id ("main_search");
			app.EnterText(main_search, "italian");

			Func<AppQuery, AppQuery> filter = e => e.Id ("filter_button");
			app.Tap (filter);
			Func<AppQuery, AppQuery> by_time = e => e.Text ("By Time");
			app.Tap(by_time);

			Func<AppQuery, AppQuery> text_view_left = e => e.Id ("textViewLeft");
			app.WaitForElement (text_view_left);

			AppResult[] search_result = app.Query(text_view_left);
			Assert.AreEqual (2, search_result.Length);

			Assert.True(search_result[0].Text.Contains ("Cannoli"));
			Assert.True(search_result[1].Text.Contains ("Italian Pasta"));
		}

		[Test]
		public void filterByDifficultyTest ()
		{
			Func<AppQuery, AppQuery> main_search = e => e.Id ("main_search");
			app.EnterText(main_search, "italian");

			Func<AppQuery, AppQuery> filter = e => e.Id ("filter_button");
			app.Tap (filter);
			Func<AppQuery, AppQuery> by_difficulty = e => e.Text ("By Difficulty");
			app.Tap(by_difficulty);

			Func<AppQuery, AppQuery> text_view_left = e => e.Id ("textViewLeft");
			app.WaitForElement (text_view_left);

			AppResult[] search_result = app.Query(text_view_left);
			Assert.AreEqual (2, search_result.Length);

			Assert.True(search_result[0].Text.Contains ("Italian Pasta"));
			Assert.True(search_result[1].Text.Contains ("Cannoli"));
		}

		[Test]
		public void filterByBothTest ()
		{
			Func<AppQuery, AppQuery> main_search = e => e.Id ("main_search");
			app.EnterText(main_search, "italian");

			Func<AppQuery, AppQuery> filter = e => e.Id ("filter_button");
			app.Tap (filter);
			Func<AppQuery, AppQuery> by_both = e => e.Text ("By Both");
			app.Tap(by_both);

			Func<AppQuery, AppQuery> text_view_left = e => e.Id ("textViewLeft");
			app.WaitForElement (text_view_left);

			AppResult[] search_result = app.Query(text_view_left);
			Assert.AreEqual (2, search_result.Length);

			Assert.True(search_result[0].Text.Contains ("Cannoli"));
			Assert.True(search_result[1].Text.Contains ("Italian Pasta"));
		}
	}
}

