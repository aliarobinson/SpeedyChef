itusing System;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;
using System.Linq;

namespace SpeedyChef.UITests
{
	[TestFixture]
	public class CustomActivityTests
	{
		private AndroidApp app;

		[SetUp]
		public void BeforeEachTest ()
		{
			app = ConfigureApp.Android.StartApp ();
		}

		[Test]
		public void correctHomeDisplay()
		{
			// Check home view show
			Func<AppQuery, AppQuery> menu_button_query = e => e.Id("menu_button");
			Func<AppQuery, AppQuery> home_button_query = e => e.Text("Home");
			Func<AppQuery, AppQuery> top_linear_layout = e => e.Id ("top_linear_layout");
			Func<AppQuery, AppQuery> search_container = e => e.Id ("search_container");
			Func<AppQuery, AppQuery> meal_plan_text = e => e.Id ("mealPlanText");

			app.WaitForElement(menu_button_query);
			app.Tap (menu_button_query);
			app.WaitForElement(home_button_query);
			app.Tap (home_button_query);

			app.WaitForElement (top_linear_layout);
			app.WaitForElement (search_container);
			app.WaitForElement (meal_plan_text);

			var has_top_linear_layout = app.Query (top_linear_layout).SingleOrDefault ();
			var has_search_container = app.Query (search_container).SingleOrDefault ();
			var has_mealPlanText = app.Query (meal_plan_text).SingleOrDefault ();

			Assert.IsNotNull (has_top_linear_layout);
			Assert.IsNotNull (has_search_container);
			Assert.IsNotNull (has_mealPlanText);
		}

		[Test]
		public void correctSearchDisplay()
		{
			// Check search view show
			Func<AppQuery, AppQuery> menu_button_query = e => e.Id("menu_button");
			Func<AppQuery, AppQuery> search_button_query = e => e.Text("Search");
			Func<AppQuery, AppQuery> top_linear_layout = e => e.Id ("top_linear_layout");
			Func<AppQuery, AppQuery> search_container = e => e.Id ("search_container");
			Func<AppQuery, AppQuery> filter_button = e => e.Id ("filter_button");

			app.WaitForElement(menu_button_query);
			app.Tap (menu_button_query);
			app.WaitForElement(search_button_query);
			app.Tap (search_button_query);

			app.WaitForElement (top_linear_layout);
			app.WaitForElement (search_container);
			app.WaitForElement (filter_button);

			var has_top_linear_layout = app.Query (top_linear_layout).SingleOrDefault ();
			var has_search_container = app.Query (search_container).SingleOrDefault ();
			var has_filter_button = app.Query (filter_button).SingleOrDefault ();

			Assert.IsNotNull (has_top_linear_layout);
			Assert.IsNotNull (has_search_container);
			Assert.IsNotNull (has_filter_button);
		}

		[Test]
		public void correctBrowseDisplay()
		{
			// Check Browse view show
			Func<AppQuery, AppQuery> menu_button_query = e => e.Id("menu_button");
			Func<AppQuery, AppQuery> browse_button_query = e => e.Text("Browse");
			Func<AppQuery, AppQuery> top_linear_layout = e => e.Id ("top_linear_layout");
			Func<AppQuery, AppQuery> search_container = e => e.Id ("search_container");

			Func<AppQuery, AppQuery> text_italian = e => e.Text ("Italian");
			Func<AppQuery, AppQuery> text_american = e => e.Text ("American");
			Func<AppQuery, AppQuery> text_chinese = e => e.Text ("Chinese");
			Func<AppQuery, AppQuery> text_mexican = e => e.Text ("Mexican");
			Func<AppQuery, AppQuery> text_indian = e => e.Text ("Indian");
			Func<AppQuery, AppQuery> text_japanese = e => e.Text ("Japanese");

			app.WaitForElement(menu_button_query);
			app.Tap (menu_button_query);
			app.WaitForElement(browse_button_query);
			app.Tap (browse_button_query);

			app.WaitForElement (top_linear_layout);
			app.WaitForElement (text_italian);
			app.WaitForElement (text_american);
			app.WaitForElement (text_chinese);
			app.WaitForElement (text_mexican);
			app.WaitForElement (text_indian);
			app.WaitForElement (text_japanese);

			var has_top_linear_layout = app.Query (top_linear_layout).SingleOrDefault ();
			var has_text_italian = app.Query (text_italian).SingleOrDefault ();
			var has_text_american = app.Query (text_american).SingleOrDefault ();
			var has_text_chinese = app.Query (text_chinese).SingleOrDefault ();
			var has_text_mexican = app.Query (text_mexican).SingleOrDefault ();
			var has_text_indian = app.Query (text_indian).SingleOrDefault ();
			var has_text_japanese = app.Query (text_japanese).SingleOrDefault ();

			Assert.IsNotNull (has_top_linear_layout);
			Assert.IsNotNull (has_text_italian);
			Assert.IsNotNull (has_text_american);
			Assert.IsNotNull (has_text_chinese);
			Assert.IsNotNull (has_text_mexican);
			Assert.IsNotNull (has_text_indian);
			Assert.IsNotNull (has_text_japanese);
		}

		[Test]
		public void correctPlanDisplay()
		{
			// Check Plan view show
			Func<AppQuery, AppQuery> menu_button_query = e => e.Id("menu_button");
			Func<AppQuery, AppQuery> plan_button_query = e => e.Text("Plan");
			Func<AppQuery, AppQuery> top_linear_layout = e => e.Id ("top_linear_layout");
			Func<AppQuery, AppQuery> search_container = e => e.Id ("search_container");

			Func<AppQuery, AppQuery> monthBar = e => e.Id ("monthBar");
			Func<AppQuery, AppQuery> dayViewer = e => e.Id ("dayViewer");

			app.WaitForElement(menu_button_query);
			app.Tap (menu_button_query);
			app.WaitForElement(plan_button_query);
			app.Tap (plan_button_query);

			app.WaitForElement (top_linear_layout);
			app.WaitForElement (dayViewer);

			var has_top_linear_layout = app.Query (top_linear_layout).SingleOrDefault ();
			var has_day_viewer = app.Query (dayViewer).SingleOrDefault ();

			Assert.IsNotNull (has_top_linear_layout);
			Assert.IsNotNull (has_day_viewer);
		}

		[Test]
		public void correctPreferencesDisplay()
		{
			// Check Preferences view show
			Func<AppQuery, AppQuery> menu_button_query = e => e.Id("menu_button");
			Func<AppQuery, AppQuery> preferences_button_query = e => e.Text("Preferences");

			Func<AppQuery, AppQuery> top_linear_layout = e => e.Id ("top_linear_layout");
			Func<AppQuery, AppQuery> tabs_linear_layout = e => e.Id("tabs_linear_layout");
			Func<AppQuery, AppQuery> peanuts = e => e.Id ("checkBox1");
			Func<AppQuery, AppQuery> gluten = e => e.Id ("checkBox2");
			Func<AppQuery, AppQuery> eggs = e => e.Id ("checkBox3");
			Func<AppQuery, AppQuery> soy = e => e.Id ("checkBox4");
			Func<AppQuery, AppQuery> dairy = e => e.Id ("checkBox5");
			Func<AppQuery, AppQuery> tree_nuts = e => e.Id ("checkBox6"); 


			app.WaitForElement(menu_button_query);
			app.Tap (menu_button_query);
			app.WaitForElement(preferences_button_query);
			app.Tap (preferences_button_query);

			app.WaitForElement (top_linear_layout);
			app.WaitForElement (tabs_linear_layout);
			app.WaitForElement (peanuts);
			app.WaitForElement (gluten);
			app.WaitForElement (eggs);
			app.WaitForElement (soy);
			app.WaitForElement (dairy);
			app.WaitForElement (tree_nuts);

			var has_top_linear_layout = app.Query (top_linear_layout).SingleOrDefault ();
			var has_tabs_linear_layout = app.Query (tabs_linear_layout).SingleOrDefault ();
			var has_peanuts = app.Query (peanuts).SingleOrDefault ();
			var has_gluten = app.Query (gluten).SingleOrDefault ();
			var has_eggs = app.Query (eggs).SingleOrDefault ();
			var has_soy = app.Query (soy).SingleOrDefault ();
			var has_dairy = app.Query (dairy).SingleOrDefault ();
			var has_tree_nuts = app.Query (tree_nuts).SingleOrDefault ();

			Assert.IsNotNull (has_top_linear_layout);
			Assert.IsNotNull (has_tabs_linear_layout);
			Assert.IsNotNull (has_peanuts);
			Assert.IsNotNull (has_gluten);
			Assert.IsNotNull (has_eggs);
			Assert.IsNotNull (has_soy);
			Assert.IsNotNull (has_dairy);
			Assert.IsNotNull (has_tree_nuts);
		}

		[Test]
		public void correctAccountDisplay()
		{
			// Check Account view show
		}

		[Test]
		public void correctWalkthroughDisplay()
		{
			// Check  Walkthrough view show
		}
	}
}

