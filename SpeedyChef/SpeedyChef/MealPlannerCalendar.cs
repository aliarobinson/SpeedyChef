
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

/// <summary>
/// Meal planner calendar page.
/// </summary>
namespace SpeedyChef
{
	[Activity (Theme = "@style/MyTheme", Label = "MealPlannerCalendar")]			
	public class MealPlannerCalendar : CustomActivity
	{
		private Boolean resumeHasRun = false;

		/// Button currently highlighted after being clicked on.
		DateButton selected = null;

		/// Layout where meal information is displayed.
		LinearLayout mealDisplay = null;

		/// The current day of app.
		DateTime current = DateTime.Now;

		/// DateTime for the date to view on screen.
		DateTime viewDate = DateTime.Now;

		/// Debug text bar to use to help present data.
		TextView debug = null;

		/// Month banner that needs to be adjusted with the date.
		TextView monthInfo = null;

		/// Current date TextView object that will be highlighted.
		TextView currentDate = null;

		/// List of all buttons in display area to be selected.
		DateButton[] daysList = null;

		/// The add bar, location of add button.
		RelativeLayout addBar = null;

		LinearLayout mealObject;



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.MealPlannerCalendar);

			// Provides global 
			mealDisplay = FindViewById<LinearLayout> (Resource.Id.mealDisplay);
			debug = FindViewById<TextView> (Resource.Id.debug);
			monthInfo = FindViewById<TextView> (Resource.Id.weekOf);
			daysList = new DateButton[7];
			addBar = FindViewById<RelativeLayout> (Resource.Id.addBar);
			// Makes sure day is selected before you can add a meal
			if (selected == null) {
				addBar.Visibility = Android.Views.ViewStates.Invisible;
				mealDisplay.Visibility = Android.Views.ViewStates.Invisible;
			}

			Button addButton = FindViewById<Button> (Resource.Id.addMeal);
			addButton.Click += (sender, e) => {
				Intent intent = new Intent (this, typeof(MealDesign));
				// Adds Binary field to be parsed later
				intent.PutExtra ("Date", selected.GetDateField ().ToBinary ());
				StartActivityForResult (intent, 0);
			};

			//MENU VIEW
			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			MenuButtonSetupSuperClass (menu_button);

			// Define variables
			Calendar c = Calendar.GetInstance (Java.Util.TimeZone.Default); 
			Button[] shifters = new Button[2];

			// Setting up month bar
			monthInfo.Text = current.ToString ("MMMMMMMMMM") + " of " + current.Year;

			// Retrieve buttons from layouts
			daysList [0] = new DateButton (FindViewById<Button> (Resource.Id.day1));
			daysList [1] = new DateButton (FindViewById<Button> (Resource.Id.day2));
			daysList [2] = new DateButton (FindViewById<Button> (Resource.Id.day3));
			daysList [3] = new DateButton (FindViewById<Button> (Resource.Id.day4));
			daysList [4] = new DateButton (FindViewById<Button> (Resource.Id.day5));
			daysList [5] = new DateButton (FindViewById<Button> (Resource.Id.day6));
			daysList [6] = new DateButton (FindViewById<Button> (Resource.Id.day7));
			// Assigning dates to days
			handleCalendar (current);
			// Adding action listeners
			for (int i = 0; i < daysList.Length; i++) {
				daysList [i].wrappedButton.Click +=
					new EventHandler ((s, e) => dayClick (s, e));
			}

			// Go backwards a week button
			shifters [0] = FindViewById<Button> (Resource.Id.leftShift);
			shifters [0].Click += delegate {
				GoBackWeek ();
			};

			// Advance week button
			shifters [1] = FindViewById<Button> (Resource.Id.rightShift);
			shifters [1].Click += delegate {
				GoForwardWeek ();
			};
			debug.Text = "";
			// LinearLayout ll = FindViewById<LinearLayout> (Resource.Id.MealDisplay);
			// Console.WriteLine (ll.ChildCount + " Look for me");
		}

		protected override void OnActivityResult (int requestCode, 
		                                          Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Result.Ok) {
				// Console.WriteLine (data.GetStringExtra ("Result"));

				RefreshMeals ();
				// Console.WriteLine ("HERE");
				// Console.WriteLine ("Request code = "+ requestCode);
			}
		}

		/// Event handler method to help get date to pass to next object
		protected void dayClick (object sender, EventArgs e)
		{
			if (selected != null) {
				selected.wrappedButton.SetBackgroundColor (Resources.GetColor 
					(Resource.Color.light_gray));
				selected.wrappedButton.SetTextColor (Resources.GetColor 
					(Resource.Color.black_text));
			}
			if (currentDate != null) {
				currentDate.SetBackgroundColor (Resources.GetColor 
					(Resource.Color.current_date));
			}
			
			selected = GetDateButton ((Button)sender);
			mealDisplay.Visibility = Android.Views.ViewStates.Visible;
			selected.wrappedButton.SetBackgroundColor 
				(Resources.GetColor (Resource.Color.selected_date));
			selected.wrappedButton.SetTextColor 
				(Resources.GetColor (Resource.Color.white_text));
			addBar.Visibility = Android.Views.ViewStates.Visible;
			RefreshMeals ();
		}


		protected override void OnResume ()
		{
			base.OnResume ();
			if (!resumeHasRun) {
				resumeHasRun = true;
				// Console.WriteLine ("Comes here");
				return;
			}
		}

		/// Refreshs the meal displaying area after calling Json.
		private async void RefreshMeals ()
		{
			// Below handles connection to the database and the parsing of Json
			LinearLayout mealDisplay = FindViewById<LinearLayout> (Resource.Id.MealDisplay);
			mealDisplay.RemoveAllViews ();
			string user = "tester";
			string useDate = selected.GetDateField ().ToString ("yyyy-MM-dd");
			string url = "http://speedychef.azurewebsites.net/" +
			             "CalendarScreen/GetMealDay?user=" + user + "&date=" + useDate;
			JsonValue json = await FetchMealData (url);

			// System.Diagnostics.Debug.WriteLine (json.ToString ());
			parseMeals (json);
		}

		/// Parses the meals from Json to display on the calendar page. Creates buttons 
		/// and views programmatically.
		private void parseMeals (JsonValue json)
		{
			LinearLayout mealDisplay = FindViewById<LinearLayout> (Resource.Id.MealDisplay);
			// PRINTS
			mealDisplay.RemoveAllViews ();
			for (int i = 0; i < json.Count; i++) {
				makeObjects (json [i], i, mealDisplay);
			}
		}

		/// Makes meal segments for the calendar page
		private async void makeObjects (JsonValue json, 
		                                int count, LinearLayout mealDisplay)
		{
			this.mealObject = new LinearLayout (this);
			LinearLayout.LayoutParams mealObjectLL = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 
					LinearLayout.LayoutParams.WrapContent);
			handleMealObjectCreation (mealObject, 25, 25, mealObjectLL, count, json);
			// Additional Json information to be used
			string user = "tester";
			int mealId = json ["Mealid"];
			string url = "http://speedychef.azurewebsites.net/" +
			             "CalendarScreen/GetRecipesForMeal?user="
			             + user + "&mealId=" + mealId;
			JsonValue recipeResult = await FetchMealData (url);
			this.mealObject.AddView (ButtonView (json, recipeResult, count));
			this.mealObject.SetPadding (0, 0, 0, 40);
			this.mealDisplay.AddView (mealObject);
		}

		public void handleMealObjectCreation(LinearLayout mealObject, int width, int height, LinearLayout.LayoutParams mealObjectLL, int count, JsonValue json) {
			this.mealObject.Orientation = Orientation.Vertical;
			this.mealObject.SetMinimumWidth (width);
			this.mealObject.SetMinimumHeight (height);
			this.mealObject.LayoutParameters = mealObjectLL;
			this.mealObject.Id = count * 20 + 5;
			this.mealObject.AddView (CreateButtonContainer (json, count));
		}

		/// Creates a button view to be added to a meal to start the walkthrough
		private LinearLayout ButtonView (JsonValue json, JsonValue recipeResult, int count)
		{
			LinearLayout walkthroughButton = new LinearLayout (this);
			walkthroughButton.Orientation = Orientation.Vertical;
			LinearLayout.LayoutParams wtll = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 
					LinearLayout.LayoutParams.WrapContent);
			walkthroughButton.LayoutParameters = wtll;
			walkthroughButton.AddView (CreateMealInfo (json, recipeResult, count));
			MealButton button = new MealButton (this);
			handleMealButtonCreation(button, "Mealname", "Mealid", "Mealsize", "Start Walkthrough", 150, json);
			LinearLayout.LayoutParams bll = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 
					LinearLayout.LayoutParams.WrapContent);
			bll.SetMargins (10, 10, 10, 10);
			button.LayoutParameters = bll;
			walkthroughButton.AddView (button);
			return walkthroughButton;
		}

		public void handleMealButtonCreation(MealButton button, String name, String id, 
					String mealSize, String text, int height, JsonValue json) {
			button.mealName = json [name];
			button.mealId = json [id];
			button.mealSize = json [mealSize];
			button.Text = text;
			button.SetHeight (height);
			button.Click += (object sender, EventArgs e) => {
				Intent i = new Intent (this, typeof(StepsActivity));
				// System.Diagnostics.Debug.WriteLine (button.mealId);
				i.PutExtra (id, button.mealId);
				// requestCode of walkthrough is 1
				StartActivityForResult (i, 1);
			};
			button.Gravity = GravityFlags.Center;
			button.SetPadding (0, 0, 0, 10);
		}

		/// Creates the meal info area in the programmitcally generated by Json.
		private LinearLayout CreateMealInfo (JsonValue json, 
		                                     JsonValue recipeResult, int count)
		{
			LinearLayout mealInfo = new LinearLayout (this);
			LinearLayout.LayoutParams mealInfoLL = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 
					LinearLayout.LayoutParams.WrapContent);
			handleMealInfoLayout (mealInfo, 25, 25, mealInfoLL, count, json);
			// Set image icon
			ImageView dinerIcon = new ImageView (this);
			dinerIcon.SetImageResource (Resource.Drawable.gray_person);
			dinerIcon.LayoutParameters = new 
				LinearLayout.LayoutParams (50, LinearLayout.LayoutParams.MatchParent);
			// Finish setting the image icon
			TextView mealSize = new TextView (this);
			TextView recipeInfo = new TextView (this);
			LinearLayout.LayoutParams rill = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, 
					LinearLayout.LayoutParams.WrapContent);
			recipeInfo.LayoutParameters = rill;
			LinearLayout.LayoutParams tvll = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, 
					LinearLayout.LayoutParams.WrapContent);
			handleRecipeInfo (recipeInfo, rill, recipeResult);
			handleMealSize (mealSize, tvll, json, "Mealsize");
			// Add image icon
			mealInfo.AddView (mealSize);
			mealInfo.AddView (dinerIcon);
			mealInfo.AddView (recipeInfo);
			return mealInfo;
		}

		public void handleMealSize (TextView mealSize, LinearLayout.LayoutParams tvll, JsonValue json, String mealSizeString) {
			mealSize.SetTextAppearance (this, Android.Resource.Style.TextAppearanceSmall);
			mealSize.LayoutParameters = tvll;
			mealSize.Text = json [mealSizeString].ToString ();
			mealSize.Gravity = GravityFlags.Right;
		}

		public void handleRecipeInfo (TextView recipeInfo, LinearLayout.LayoutParams rill, JsonValue recipeResult) {
			recipeInfo.Text = handleRecipeJson (recipeResult);
			recipeInfo.SetTextAppearance (this, Android.Resource.Style.TextAppearanceSmall);
			recipeInfo.SetLines (1);
			recipeInfo.SetPadding (10, 0, 0, 0);
		}

		public void handleMealInfoLayout (LinearLayout mealInfo, int width, int height, 
			LinearLayout.LayoutParams mealInfoLL, int count, JsonValue json) {
			mealInfo.Orientation = Orientation.Horizontal;
			mealInfo.SetMinimumWidth (25);
			mealInfo.SetMinimumHeight (25);
			mealInfoLL.SetMargins (5, 5, 5, 5);
			mealInfo.LayoutParameters = mealInfoLL;
			mealInfo.Id = count * 20 + 7;
		}

		/// Creates the button container. Used to clean up code and with button for desiging meal/
		private LinearLayout CreateButtonContainer (JsonValue json, int count)
		{
			LinearLayout buttonCont = new LinearLayout (this);
			LinearLayout.LayoutParams bcll = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 
					LinearLayout.LayoutParams.WrapContent);
			handleMealInfoLayout (buttonCont, 25, 100, bcll, count, json);
			buttonCont.Visibility = Android.Views.ViewStates.Visible;
			MealButton button = new MealButton (this, null, 
				                    Resource.Style.generalButtonStyle); 
			
			LinearLayout.LayoutParams lp = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, 
					LinearLayout.LayoutParams.MatchParent);

			handleMealButtonInButtonContainer (button, "Mealname", "Mealid", 
				"Mealsize", 150, json, lp, "Mealname");
			buttonCont.AddView (button);
			return buttonCont;
		}

		public void handleMealButtonInButtonContainer (MealButton button, String name, String id, 
			String mealSize, int height, JsonValue json, LinearLayout.LayoutParams lp, String text) {
			button.mealName = json [name];
			button.mealSize = (json [mealSize]);
			button.SetHeight (height);
			button.mealId = (json [id]);
			button.Click += (object sender, EventArgs e) => {
				Intent intent = new Intent (this, typeof(MealDesign));
				LinearLayout mealDisplay = FindViewById<LinearLayout> (Resource.Id.MealDisplay);
				// PRINTS
				mealDisplay.RemoveAllViews ();
				intent.PutExtra ("Name", button.mealName);
				intent.PutExtra ("Mealsize", button.mealSize);
				intent.PutExtra ("mealId", button.mealId);
				StartActivityForResult (intent, 3);
				// requestCode for Design page 3

			};

			button.LayoutParameters = lp;
			button.Text = json ["Mealname"];

			button.Visibility = Android.Views.ViewStates.Visible;
			button.SetBackgroundColor (Resources.GetColor (Resource.Color.orange_header));
			button.Gravity = GravityFlags.Center;
		}

		/// Takes the recipes and makes into nice string to display
		public string handleRecipeJson (JsonValue json)
		{
			string finalString = "";
			for (int i = 0; i < json.Count; i++) {
				finalString += json [i] ["Recname"] + ", ";
			}
			if (finalString.Length > 2) {
				finalString = finalString.Substring (0, finalString.Length - 2);
			}
			// System.Diagnostics.Debug.WriteLine (finalString);
			return finalString;
		}
			
		/// Fetchs the meal data.
		private async Task<JsonValue> FetchMealData (string url)
		{
			// Create an HTTP web request using the URL:
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "GET";
			
			// Send the request to the server and wait for the response:
			using (WebResponse response = await request.GetResponseAsync ().ConfigureAwait (true)) {
				// Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream ()) {
					// Use this stream to build a JSON document object:
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream)).ConfigureAwait (true);
					// Return the JSON document:
					return jsonDoc;
				}
			}

		}

		/// Gets the date button.
		private DateButton GetDateButton (Button b)
		{
			for (int i = 0; i < daysList.Length; i++) {
				if (daysList [i].wrappedButton.GetHashCode () == b.GetHashCode ()) {
					return daysList [i];
				}
			}
			// Console.WriteLine ("Should never get here");
			return daysList [0];
		}

		/// Method that handles updating all the boxes in the calendar so that
		/// the dates line up in the week
		public void handleCalendar (DateTime date)
		{
			currentDate = null;
			string day = date.AddDays (-date.DayOfWeek.GetHashCode ())
				.ToString ("M/d");
			DateTime temp = date.AddDays (-date.DayOfWeek.GetHashCode ());
			string weekDay = temp.ToString ("ddd");
			monthInfo.Text = temp.ToString ("MMMMMMMMMM") + " of " + temp.Year;
			for (int i = 0; i < 7; i++) {
				// Determines the day from how far away from the beginning (Sunday)
				// and displays appropriately
				day = date.AddDays (-date.DayOfWeek.GetHashCode () + i).ToString ("MM/d");
				weekDay = date.AddDays (-date.DayOfWeek.GetHashCode () + i)
					.ToString ("ddddddd");
				
				daysList [i].wrappedButton.Text = weekDay.Substring (0, 3) + "\n" + day;
				// Change the above line from a 3 to a 1
				daysList [i].SetDateField (date.AddDays 
					(-date.DayOfWeek.GetHashCode () + i));
				// Sets all the buttons to the default colors
				daysList [i].wrappedButton.SetBackgroundColor (Resources.GetColor
					(Resource.Color.light_gray));
				daysList [i].wrappedButton.SetTextColor (Resources.GetColor 
					(Resource.Color.black_text));
				// Handles setting the highlighting of the current day on the phone
				if (i == current.DayOfWeek.GetHashCode ()
				    && date.Date.Equals (current.Date)) {
					currentDate = daysList [i].wrappedButton;
					daysList [i].wrappedButton.SetBackgroundColor 
							(Resources.GetColor (Resource.Color.selected_date));
					daysList [i].wrappedButton.SetBackgroundColor 
							(Resources.GetColor (Resource.Color.current_date));
					// daysList [i].wrappedButton.SetBackgroundResource ();
				}
			}
			// Removes any selected day
			selected = null;
			mealDisplay.Visibility = Android.Views.ViewStates.Invisible;
			// Makes the add button invisible
			addBar.Visibility = Android.Views.ViewStates.Invisible;
		}

		/// <summary>
		/// Goes back a week.
		/// </summary>
		public void GoBackWeek ()
		{
			LinearLayout mealDisplay = FindViewById<LinearLayout> 
				(Resource.Id.MealDisplay);
			mealDisplay.RemoveAllViews ();
			viewDate = viewDate.AddDays (-7);
			handleCalendar (viewDate);
		}

		/// Goes forward a week.
		public void GoForwardWeek ()
		{
			viewDate = viewDate.AddDays (7);
			LinearLayout mealDisplay = FindViewById<LinearLayout> 
				(Resource.Id.MealDisplay);
			mealDisplay.RemoveAllViews ();
			handleCalendar (viewDate);
		}
	}
		
	/// Wrapper class for button to help handle passing the dates.
	public class DateButton
	{
		/**
		 * Datefield for button container
		 **/
		private DateTime dateField;

		/// The wrapped button for the button container.
		public Button wrappedButton;

		/// Initializes a new instance of the <see cref="SpeedyChef.DateButton"/> class. 
		/// This is a container class.
		public DateButton (Button button)
		{
			wrappedButton = button;
			this.dateField = DateTime.Now.AddDays (-100);
		}

		/// Sets the date field.
		public void SetDateField (DateTime date)
		{
			// Console.WriteLine ("Wrote date");
			this.dateField = date;
		}

		/// Gets the date field.
		public DateTime GetDateField ()
		{
			return this.dateField;
		}
	}
	/// Button class that contains extra fields to be used for getting 
	/// additional information
	public class MealButton : Button
	{
		/// Gets or sets the meal identifier.
		public int mealId { get; set; }

		/// Gets or sets the name of the meal.
		public string mealName { get; set; }

		/// Gets or sets the size of the meal.
		public int mealSize { get; set; }

		/// Initializes a new instance of the <see cref="SpeedyChef.MealButton"/> class.
		public MealButton (Context context) : base (context)
		{
			this.mealId = -1;
			this.mealName = "";
			this.mealSize = -1;
		}

		/// Initializes a new instance of the <see cref="SpeedyChef.MealButton"/> class.
		public MealButton (Context context, 
		                   Android.Util.IAttributeSet set, int style) :
			base (context, set, style)
		{
			this.mealId = -1;
			this.mealName = "";
		}
	}
}