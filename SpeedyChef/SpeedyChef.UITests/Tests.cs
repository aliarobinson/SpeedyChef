using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;

namespace SpeedyChef.UITests
{
	[TestFixture]
	public class Tests
	{
		AndroidApp app;

		[SetUp]
		public void BeforeEachTest ()
		{
			app = ConfigureApp.Android.StartApp ();
		}

//		[Test]
//		public void ClickingButtonTwiceShouldChangeItsLabel ()
//		{
//			Func<AppQuery, AppQuery> MyButton = c => c.Button ("myButton");
//
//			app.Tap (MyButton);
//			app.Tap (MyButton);
//			AppResult[] results = app.Query (MyButton);
//			app.Screenshot ("Button clicked twice.");
//
//			Assert.AreEqual ("2 clicks!", results [0].Text);
//		}
		[Test]
		public void TestingCreateUser() {
			Func<AppQuery, AppQuery> menu_button_query = e => e.Id("menu_button");
			Func<AppQuery, AppQuery> account_button_query = e => e.Text("Account");

			app.WaitForElement(menu_button_query);
			app.Tap (menu_button_query);
			app.WaitForElement(account_button_query);
			app.Tap (account_button_query);

			app.Tap (e => e.Marked("Create an Account"));
			//app.ClearText (e => e.Marked ("Full Name"));
			//app.ClearText (e => e.Marked ("Email Address"));
			//app.ClearText (e => e.Marked ("Username"));
			//app.ClearText (e => e.Marked ("Password"));
			//app.ClearText (e => e.Marked ("Confirm Password"));

			app.EnterText (e => e.Marked ("FullName"), "greg shackles");
			app.EnterText (e => e.Marked ("EmailAddress"), "greg@gregshackles");
			app.EnterText (e => e.Marked ("Username"), "gre");
			app.EnterText (e => e.Marked ("Password"), "g");
			app.EnterText (e => e.Marked ("ConfirmPassword"), "g");
			app.Tap (e => e.Marked("Create an Account"));

		}
//
		[Test]
		public void TestingLoginUser() {
//			Func<AppQuery, AppQuery> menu_button_query = e => e.Id("menu_button");
//			Func<AppQuery, AppQuery> account_button_query = e => e.Text("Account");
//
//			app.WaitForElement(menu_button_query);
//			app.Tap (menu_button_query);
//			app.WaitForElement(account_button_query);
//			app.Tap (account_button_query);
//
//			app.Tap (e => e.Marked("Create an Account"));
////			app.ClearText (e => e.Marked ("Full Name"));
////			app.ClearText (e => e.Marked ("Email Address"));
////			app.ClearText (e => e.Marked ("Username"));
////			app.ClearText (e => e.Marked ("Password"));
////			app.ClearText (e => e.Marked ("Confirm Password"));
//
//			app.EnterText (e => e.Marked ("Full Name"), "greg shackles");
//			app.EnterText (e => e.Marked ("Email Address"), "greg@gregshackles");
//			app.EnterText (e => e.Marked ("Username"), "gre");
//			app.EnterText (e => e.Marked ("Password"), "g");
//			app.EnterText (e => e.Marked ("Confirm Password"), "g");
//			app.Tap (e => e.Marked("Create an Account"));
//			app.Tap (e => e);
			//app.Tap;

			Func<AppQuery, AppQuery> menu_button_query_two = e => e.Id("menu_button");
			Func<AppQuery, AppQuery> account_button_query_two = e => e.Text("Account");

			app.WaitForElement(menu_button_query_two);
			app.Tap (menu_button_query_two);
			app.WaitForElement(account_button_query_two);
			app.Tap (account_button_query_two);

			app.EnterText (e => e.Marked ("Username"), "gre");
			//app.ClearText (e => e.Marked ("Password"));
			app.EnterText (e => e.Marked ("Password"), "g");
			app.Tap (e => e.Marked("Login"));
		}
	}
}

