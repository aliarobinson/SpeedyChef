
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

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Result.Ok) {
				//helloLabel.Text = data.GetStringExtra ("greeting");
				String curUser = data.GetStringExtra ("username");
				String curPass = data.GetStringExtra ("password");

				this.usernames.Add (curUser);
				this.passwords.Add (curPass);
			}
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView(Resource.Layout.Registration);

			var editTextUsername = FindViewById<EditText> (Resource.Id.editText1);
			var textViewUsername = FindViewById<EditText> (Resource.Id.editText2);

			String enteredUsername = null;
			String enteredPassword = null;

			editTextUsername.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				enteredUsername = e.Text.ToString ();
			};

			textViewUsername.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				enteredPassword = e.Text.ToString ();
			};

			this.login_button = FindViewById<Button> (Resource.Id.button1);
			this.createAccount_button = FindViewById<Button> (Resource.Id.button2); 

			this.login_button.Click += (s, arg) => {
				// Do nothing
				Console.WriteLine ("Count User: {0}", usernames.Count);
				Console.WriteLine ("Count User: {0}", passwords.Count);
				Console.WriteLine ("entered username {0}", enteredUsername);
				Console.WriteLine ("entered password {0}", enteredPassword);
				Console.WriteLine(usernames.IndexOf(enteredUsername));
				Console.WriteLine(passwords.IndexOf(enteredPassword));

				if (usernames.Contains(enteredUsername)) {
					// return an invalid username
					//this.username = null;
					//this.password = null;
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
				//StartActivity (intent);

				Console.WriteLine ("Count User create: {0}", usernames.Count);
				Console.WriteLine ("Count pass create : {0}", passwords.Count);


				//var myIntent = new Intent (this, typeof(SecondActivity));
				StartActivityForResult (intent, 0);
			};

			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			MenuButtonSetupSuperClass (menu_button);
		}
	}
}
