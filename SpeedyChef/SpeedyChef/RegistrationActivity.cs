
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System; 
using System.Collections; 

namespace SpeedyChef
{
	[Activity (Theme="@style/MyTheme", Label = "RegistrationActivity", Icon = "@drawable/icon")]		
	public class RegistrationActivity : CustomActivity
	{

		Button login_button;
		Button createAccount_button;
		String username; //Change this to be an arraylist of usernames once you figure
		// out if C# has arraylists
		String password; // This is the password that should go along with the username in the hashmap
		// if you choose to implement one of those.

		ArrayList usernames = new ArrayList();
		ArrayList passwords = new ArrayList(); 

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView(Resource.Layout.Registration);

			// Create your application here
			// Text Input Boxes
			var editTextUsername = FindViewById<EditText> (Resource.Id.editText1);
			var textViewUsername = FindViewById<EditText> (Resource.Id.editText2);

			String enteredUsername = editTextUsername.ToString ();
			String enteredPassword = textViewUsername.ToString ();

			this.login_button = FindViewById<Button> (Resource.Id.button1);
			this.createAccount_button = FindViewById<Button> (Resource.Id.button2); 

			this.login_button.Click += (s, arg) => {
				// Do nothing
				if (usernames.Contains(enteredUsername)) {
					// return an invalid username
					this.username = null;
					this.password = null;
					int indexOfUsername = usernames.IndexOf(enteredUsername);
					if(indexOfUsername == passwords.IndexOf(enteredPassword)) {
						// valid login
						new AlertDialog.Builder(this)
							.SetMessage("You are now logged in")
							.Show();

					}
				} else {
					// Not a valid Login
					new AlertDialog.Builder(this)
						.SetMessage("Your username and password do not match a valid account")
						.Show();
				}
			};

			this.createAccount_button.Click += (s, arg) => {
				var intent = new Intent (this, typeof(CreateanAccountActivity));
				CachedData.Instance.CurrHighLevelType = typeof(CreateanAccountActivity);
				CachedData.Instance.PreviousActivity = this;
				StartActivity (intent);
			};

			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			MenuButtonSetupSuperClass (menu_button);
			//MenuButtonSetup(menu_button);

//			menu_button.Click += (s, arg) => {
//				menu_button.SetBackgroundResource(Resource.Drawable.pressed_lines);
//				PopupMenu menu = new PopupMenu (this, menu_button);
//				menu.Inflate (Resource.Menu.Main_Menu);
//				menu.MenuItemClick += this.MenuButtonClick;
//				menu.DismissEvent += (s2, arg2) => {
//					menu_button.SetBackgroundResource(Resource.Drawable.menu_lines);
//					Console.WriteLine ("menu dismissed");
//				};
//				menu.Show ();
//			};

			//filter_button.Click += (s, arg) => {

		}
	}
}
