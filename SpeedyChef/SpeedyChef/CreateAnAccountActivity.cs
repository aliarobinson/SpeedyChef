
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

	[Activity (Theme="@style/MyTheme", Label = "CreateAccountActivity", Icon = "@drawable/icon")]			
	public class CreateanAccountActivity : CustomActivity
	{

		Button createAccountButton;

		ArrayList fullNames = new ArrayList();
		ArrayList emailAddresses = new ArrayList();
		ArrayList usernames = new ArrayList();
		ArrayList passwords = new ArrayList();

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView(Resource.Layout.CreateAccount);

			var fullName = FindViewById<EditText> (Resource.Id.editText1);
			var emailAddress = FindViewById<EditText> (Resource.Id.editText2);
			var username = FindViewById<EditText> (Resource.Id.editText3);
			var password = FindViewById<EditText> (Resource.Id.editText4);
			var confirmPassword = FindViewById<EditText> (Resource.Id.editText5);

			String fName = null;
			String eAddress = null;
			String uname = null;
			String pword = null;
			String cword = null;

			fullName.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				fName = e.Text.ToString ();
			};
			emailAddress.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				eAddress = e.Text.ToString ();
			};
			username.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				uname = e.Text.ToString ();
			};
			password.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				pword = e.Text.ToString ();
			};
			confirmPassword.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				cword = e.Text.ToString ();
			};
			
			createAccountButton = FindViewById<Button> (Resource.Id.createAccountButton);

			this.createAccountButton.Click += (s, arg) => {
				if (fullNames.Contains (fName)) {
					new AlertDialog.Builder (this)
						.SetMessage ("A user with the same name already has an account")
						.Show ();
				}
				else if (emailAddresses.Contains (eAddress)) {
					new AlertDialog.Builder (this)
						.SetMessage ("That email already has an account with it")
						.Show ();
				}
				else if (usernames.Contains(uname)) {
					new AlertDialog.Builder (this)
						.SetMessage ("That username already exists")
						.Show ();
				}
				else if (!pword.Equals(cword)) {
					new AlertDialog.Builder (this)
						.SetMessage ("The passwords do not match")
						.Show ();
				} else {
					fullNames.Add (fName);
					emailAddresses.Add (eAddress);
					usernames.Add (uname);
					passwords.Add (pword);

			
	
					new AlertDialog.Builder (this)
					.SetMessage ("Your account has been created")
					.Show ();

					Intent myIntent = new Intent (this, typeof(RegistrationActivity));
					myIntent.PutExtra ("username", uname);
					myIntent.PutExtra("password", pword);
					SetResult (Result.Ok, myIntent);
					Finish();
				}
			};

			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			MenuButtonSetupSuperClass (menu_button);

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
			// Create your application here
		}
	}
}
