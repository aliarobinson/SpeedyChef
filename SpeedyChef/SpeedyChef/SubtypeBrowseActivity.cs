﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using v7Widget = Android.Support.V7.Widget;

namespace SpeedyChef
{
	[Activity (Theme="@style/MyTheme", Label = "Browse Nationalities", Icon = "@drawable/icon")]
	public class SubtypeBrowseActivity : CustomActivity
	{
		v7Widget.RecyclerView mRecyclerView;
		v7Widget.RecyclerView.LayoutManager mLayoutManager;
		SideBySideAdapter mAdapter;
		SideBySideObject mObject;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//RECYCLER VIEW
			mObject = new SideBySideObject (CachedData.Instance.TupleDict[CachedData.Instance.SelectedNationality]);
			mAdapter = new SideBySideAdapter (mObject, this, true);
			SetContentView (Resource.Layout.BrowseNationalities);
			mRecyclerView = FindViewById<v7Widget.RecyclerView> (Resource.Id.recyclerView);
			mRecyclerView.SetAdapter (mAdapter);
			mLayoutManager = new v7Widget.LinearLayoutManager (this);
			mRecyclerView.SetLayoutManager (mLayoutManager);

			//MENU VIEW
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

		}

		protected override void OnResume(){
			base.OnResume ();
			CachedData.Instance.CurrHighLevelType = this.GetType ();
		}

		public override void OnBackPressed(){
			base.OnPause ();
			CachedData.Instance.PreviousActivity = this;
			Finish ();
		}

	}
}

