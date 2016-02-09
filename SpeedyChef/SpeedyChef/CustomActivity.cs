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
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Graphics.Drawables;
using Android.Util;
using v7Widget = Android.Support.V7.Widget;

namespace SpeedyChef
{
	[Activity (Theme="@style/MyTheme", Label = "SpeedyChef", Icon = "@drawable/icon")]
	public class CustomActivity : FragmentActivity
	{
		Dictionary<string, string> titleToClassname = new Dictionary<string, string> (){
			{"Browse", "BrowseNationalitiesActivity"},
			{"Plan", "MealPlannerCalendar"},
			{"Walkthrough", "StepsActivity"},
			{"Search", "SearchActivity"},
			{"Preferences", "Allergens"},
			{"Home", "MainActivity"},
			{"Account", "RegistrationActivity"}
		};

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
		}

		public void MenuButtonClick (object s, PopupMenu.MenuItemClickEventArgs arg){
			string classname = titleToClassname [arg.Item.TitleFormatted.ToString ()];
			Type t =  Type.GetType("SpeedyChef."+ classname);
			if (t != this.GetType ()) {
				changeToView (t);
			}
		}

		public void changeToView(Type t)
		{
			var intent = new Intent (this, t);
			CachedData.Instance.CurrHighLevelType = t;
			CachedData.Instance.PreviousActivity = this;
			StartActivity (intent);
		}

		public void MenuButtonSetupSuperClass(Button m_button) {
			m_button.Click += (s, arg) => {
				m_button.SetBackgroundResource(Resource.Drawable.pressed_lines);
				PopupMenu menu = new PopupMenu (this, m_button);
				menu.Inflate (Resource.Menu.Main_Menu);
				menu.MenuItemClick += this.MenuButtonClick;
				menu.DismissEvent += (s2, arg2) => {
					m_button.SetBackgroundResource(Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};
		}

		public v7Widget.RecyclerView superDeployRecyclerView(PlannedMealAdapter mAdapter, v7Widget.RecyclerView.LayoutManager mLayoutManager,
			v7Widget.RecyclerView mRecyclerView) {

			mRecyclerView = FindViewById<v7Widget.RecyclerView> (Resource.Id.recyclerView);
			mRecyclerView.SetAdapter (mAdapter);
			mRecyclerView.SetLayoutManager (mLayoutManager);
			return mRecyclerView;
		}

		public void SearchViewSuper(SearchView searchView, TextView textView) {
			searchView.SetBackgroundColor (Android.Graphics.Color.DarkOrange);
			searchView.SetOnQueryTextListener ((SearchView.IOnQueryTextListener) this);
			textView.SetTextColor(Android.Graphics.Color.White);
			textView.SetHintTextColor (Android.Graphics.Color.White);
			searchView.SetQueryHint ("Search Recipes...");
			LinearLayout search_container = FindViewById<LinearLayout> (Resource.Id.search_container);
			search_container.Click += (sender, e) => {
				if (searchView.Iconified != false){
					searchView.Iconified = false;
				}
			};
		}

//		public void RecyclerViewSuper(SideBySideObject mObject,SideBySideAdapter mAdapter, v7Widget.RecyclerView mRecyclerView,v7Widget.RecyclerView.LayoutManager mLayoutManager) {
//			//RECYCLER VIEW
//			mObject = new SideBySideObject (CachedData.Instance.TupleDict[CachedData.Instance.SelectedNationality]);
//			mAdapter = new SideBySideAdapter (mObject, this, true);
//			SetContentView (Resource.Layout.BrowseNationalities);
//			mRecyclerView = FindViewById<v7Widget.RecyclerView> (Resource.Id.recyclerView);
//			mRecyclerView.SetAdapter (mAdapter);
//			mLayoutManager = new v7Widget.LinearLayoutManager (this);
//			mRecyclerView.SetLayoutManager (mLayoutManager);
//		}

	}

	public class CachedData
	{

		public Tuple<int, string>[] ItalianFood { get; set; }
		public Tuple<int, string>[] ChineseFood { get; set; }
		public Tuple<int, string>[] FrenchFood { get; set; }
		public Tuple<int, string>[] SpanishFood { get; set; }
		public Tuple<int, string>[] MexicanFood { get; set; }
		public Tuple<int, string>[] GreekFood { get; set; }
		public Tuple<int, string>[] ThaiFood { get; set; }
		public Tuple<int, string>[] IndianFood { get; set; }
		public Tuple<int, string>[] JapaneseFood { get; set; }
		public Tuple<int, string>[] AmericanFood { get; set; }
		public Dictionary<string, Tuple<int, string>[]> TupleDict { get; set; }
		public string SelectedNationality { get; set; }
		public string SelectedSubgenre { get; set; }
		public int mostRecentRecSel { get; set; }
		public int mostRecentMealAdd { get; set; }
		public CustomActivity PreviousActivity { get; set; }
		public int MealDesignMealId { get; set; }
		public int MealDesignMealSize { get; set; }
		public System.Type CurrHighLevelType { get; set; }
		public string LastSubmissionFromMain { get; set; }
		public bool SubmissionFromMain { get; set; }
		public bool FromMealDesign { get; set; }

		private CachedData()
		{
			ItalianFood = new Tuple<int, string>[4];
			TupleDict = new Dictionary<string, Tuple<int, string>[]> ();
			TupleDict.Add ("Italian", ItalianFood);
			ItalianFood [0] = new Tuple<int, string> (Resource.Drawable.ItalianDesserts, "Italian Desserts");
			ItalianFood [1] = new Tuple<int, string> (Resource.Drawable.ItalianMeats, "Italian Meats");
			ItalianFood [2] = new Tuple<int, string> (Resource.Drawable.ItalianVegetables, "Italian Vegetables");
			ItalianFood [3] = new Tuple<int, string>(Resource.Drawable.ItalianPastas, "Italian Pastas");

			ChineseFood = new Tuple<int, string>[4];
			TupleDict.Add ("Chinese", ChineseFood);
			ChineseFood [0] = new Tuple<int, string> (Resource.Drawable.ChineseDesserts, "Chinese Desserts");
			ChineseFood [1] = new Tuple<int, string> (Resource.Drawable.ChineseMeats, "Chinese Meats"); 
			ChineseFood [2] = new Tuple<int, string> (Resource.Drawable.ChineseVegetables, "Chinese Vegetables");
			ChineseFood [3] = new Tuple<int, string> (Resource.Drawable.ChineseRice, "Chinese Rice");

			FrenchFood = new Tuple<int, string>[4];
			TupleDict.Add ("French", FrenchFood);
			FrenchFood[0] = new Tuple<int, string> (Resource.Drawable.FrenchDesserts, "French Desserts");
			FrenchFood[1] = new Tuple<int, string> (Resource.Drawable.FrenchMeats, "French Meats");
			FrenchFood[2] = new Tuple<int, string> (Resource.Drawable.FrenchVegetables, "French Vegetables");
			FrenchFood[3] = new Tuple<int, string> (Resource.Drawable.FrenchSnails, "French Snails");

			SpanishFood = new Tuple<int, string>[4];
			TupleDict.Add ("Spanish", SpanishFood);
			SpanishFood[0] = new Tuple<int, string> (Resource.Drawable.SpanishDesserts, "Spanish Desserts"); 
			SpanishFood[1] = new Tuple<int, string> (Resource.Drawable.SpanishMeats, "Spanish Meats");
			SpanishFood[2] = new Tuple<int, string> (Resource.Drawable.SpanishVegetables, "Spanish Vegetables");
			SpanishFood[3] = new Tuple<int, string> (Resource.Drawable.SpanishOmelettes, "Spanish Omelettes");

			MexicanFood = new Tuple<int, string>[4];
			TupleDict.Add ("Mexican", MexicanFood);
			MexicanFood[0] = new Tuple<int, string> (Resource.Drawable.MexicanDesserts, "Mexican Desserts");
			MexicanFood[1] = new Tuple<int, string> (Resource.Drawable.MexicanMeats, "Mexican Meats");
			MexicanFood[2] = new Tuple<int, string> (Resource.Drawable.MexicanVegetables, "Mexican Vegetables");
			MexicanFood[3] = new Tuple<int, string> (Resource.Drawable.MexicanWraps, "Mexican Wraps");

			AmericanFood = new Tuple<int, string>[4];
			TupleDict.Add ("American", AmericanFood);
			AmericanFood[0] = new Tuple<int, string> (Resource.Drawable.AmericanDesserts, "American Desserts");
			AmericanFood[1] = new Tuple<int, string> (Resource.Drawable.AmericanMeats, "American Meats");
			AmericanFood[2] = new Tuple<int, string> (Resource.Drawable.AmericanVegetables, "American Vegetables");
			AmericanFood[3] = new Tuple<int, string> (Resource.Drawable.AmericanHamburgers, "American Hamburgers");

			JapaneseFood = new Tuple<int, string>[4];
			TupleDict.Add ("Japanese", JapaneseFood);
			JapaneseFood[0] = new Tuple<int, string> (Resource.Drawable.JapaneseDesserts, "Japanese Desserts");
			JapaneseFood[1] = new Tuple<int, string> (Resource.Drawable.JapaneseMeats, "Japanese Meats");
			JapaneseFood[2] = new Tuple<int, string> (Resource.Drawable.JapaneseVegetables, "Japanese Vegetables");
			JapaneseFood[3] = new Tuple<int, string> (Resource.Drawable.JapaneseSushi, "Japanese Sushi");

			GreekFood = new Tuple<int, string>[4];
			TupleDict.Add ("Greek", GreekFood);
			GreekFood[0] = new Tuple<int, string> (Resource.Drawable.GreekDesserts, "Greek Desserts");
			GreekFood[1] = new Tuple<int, string> (Resource.Drawable.GreekMeats, "Greek Meats");
			GreekFood[2] = new Tuple<int, string> (Resource.Drawable.GreekVegetables, "Greek Vegetables");
			GreekFood[3] = new Tuple<int, string> (Resource.Drawable.GreekGyros, "Greek Gyros");

			ThaiFood = new Tuple<int, string>[4];
			TupleDict.Add ("Thai", ThaiFood);
			ThaiFood[0] = new Tuple<int, string> (Resource.Drawable.ThaiDesserts, "American Desserts");
			ThaiFood[1] = new Tuple<int, string> (Resource.Drawable.ThaiMeats, "American Meats");
			ThaiFood[2] = new Tuple<int, string> (Resource.Drawable.ThaiVegetables, "American Vegetables");
			ThaiFood[3] = new Tuple<int, string> (Resource.Drawable.ThaiFoodEx, "Thai Food");

			IndianFood = new Tuple<int, string>[4];
			TupleDict.Add ("Indian", IndianFood);
			IndianFood[0] = new Tuple<int, string> (Resource.Drawable.IndianDesserts, "Indian Desserts");
			IndianFood[1] = new Tuple<int, string> (Resource.Drawable.IndianMeats, "Indian Meats");
			IndianFood[2] = new Tuple<int, string> (Resource.Drawable.IndianVegetables, "Indian Vegetables");
			IndianFood[3] = new Tuple<int, string> (Resource.Drawable.IndianCurry, "Indian Curry");

			SelectedNationality = "American";
			this.PreviousActivity = new MainActivity ();
			this.FromMealDesign = false;
		}

		private static volatile CachedData _instance;
		private static object syncRoot = new Object();

		public static CachedData Instance
		{
			get
			{
				if (_instance == null) 
				{
					lock (syncRoot) 
					{
						if (_instance == null) 
							_instance = new CachedData();
					}
				}

				return _instance;
			}
		}
	}
}

