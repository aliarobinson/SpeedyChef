
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

			//			var fullName = FindViewById<EditText> (Resource.Id.editText1);
			//			var emailAddress = FindViewById<EditText> (Resource.Id.editText2);
			//			var username = FindViewById<EditText> (Resource.Id.editText3);
			//			var password = FindViewById<EditText> (Resource.Id.editText4);
			//			var confirmPassword = FindViewById<EditText> (Resource.Id.editText5);
			//
			//			createAccountButton = FindViewById<Button> (Resource.Id.button1);
			//
			//			if (fullNames.Contains (fullName)) {
			//				new AlertDialog.Builder(this)
			//					.SetMessage("A user with the same name already has an account")
			//					.Show();
			//			}
			//			if (emailAddresses.Contains (emailAddress)) {
			//				new AlertDialog.Builder(this)
			//					.SetMessage("That email already has an account with it")
			//					.Show();
			//			}
			//			if (usernames.Contains (username)) {
			//				new AlertDialog.Builder(this)
			//					.SetMessage("That username already exists")
			//					.Show();
			//			}
			//			if (!password.Equals (confirmPassword)) {
			//				new AlertDialog.Builder (this)
			//					.SetMessage ("The passwords do not match")
			//					.Show ();
			//			} else {
			//				fullNames.Add (fullName);
			//				emailAddresses.Add (emailAddress);
			//				usernames.Add (username);
			//				passwords.Add (password);
			//
			//				new AlertDialog.Builder(this)
			//					.SetMessage("Your account has been created")
			//					.Show();
			//			}


			// Create your application here
		}
	}
}
