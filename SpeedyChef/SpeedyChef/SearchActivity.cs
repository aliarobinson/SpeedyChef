using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using v7Widget = Android.Support.V7.Widget;
using System.Collections.Generic;

using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SpeedyChef
{
	[Activity (Theme="@style/MyTheme", Label = "SpeedyChef", Icon = "@drawable/icon")]
	public class SearchActivity : CustomActivity, SearchView.IOnQueryTextListener, SearchView.IOnSuggestionListener
	{
		v7Widget.RecyclerView mRecyclerView;
		v7Widget.RecyclerView.LayoutManager mLayoutManager;
		RecipeAdapter mAdapter;
		RecipeObject mObject;
		Button filter_button;
		JsonValue jsonDoc;
		string ordertype;
		string asc;
		string mostRecentKeywords;
		private Object thisLock = new Object();


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			this.mostRecentKeywords = "";
			this.asc = "Asc";
			this.ordertype = "Diff";

			//RECYCLER VIEW
			mObject = new RecipeObject ();
			mAdapter = new RecipeAdapter (mObject, this);
			mAdapter.itemClick += this.OnItemClick;
			SetContentView (Resource.Layout.Search);
			mRecyclerView = FindViewById<v7Widget.RecyclerView> (Resource.Id.recyclerView);
			mRecyclerView.SetAdapter (mAdapter);
			mLayoutManager = new v7Widget.LinearLayoutManager (this);
			mRecyclerView.SetLayoutManager (mLayoutManager);

			//SEARCH VIEW
			SearchView searchView = FindViewById<SearchView> (Resource.Id.main_search);
			searchView.SetBackgroundColor (Android.Graphics.Color.DarkOrange);
			searchView.SetOnQueryTextListener ((SearchView.IOnQueryTextListener)this);
			int id = Resources.GetIdentifier ("android:id/search_src_text", null, null);
			TextView textView = (TextView)searchView.FindViewById (id);
			textView.SetTextColor (Android.Graphics.Color.White);
			textView.SetHintTextColor (Android.Graphics.Color.White);
			searchView.SetQueryHint ("Search Recipes...");
			LinearLayout search_container = FindViewById<LinearLayout> (Resource.Id.search_container);
			search_container.Click += (sender, e) => {
				if (searchView.Iconified != false) {
					searchView.Iconified = false;
				}
			};

			//MENU VIEW
			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			MenuButtonSetupSuperClass (menu_button);
			//			menu_button.Click += (s, arg) => {
			//				menu_button.SetBackgroundResource (Resource.Drawable.pressed_lines);
			//				PopupMenu menu = new PopupMenu (this, menu_button);
			//				menu.Inflate (Resource.Menu.Main_Menu);
			//				menu.MenuItemClick += this.MenuButtonClick;
			//				menu.DismissEvent += (s2, arg2) => {
			//					menu_button.SetBackgroundResource (Resource.Drawable.menu_lines);
			//					Console.WriteLine ("menu dismissed");
			//				};
			//				menu.Show ();
			//			};

			this.filter_button = FindViewById<Button> (Resource.Id.filter_button);
			RegisterForContextMenu (filter_button);
			filter_button.Click += (s, arg) => {
				((Button) s).ShowContextMenu();
			};

			showFiltered (null);

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

		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo) {
			base.OnCreateContextMenu (menu, v, menuInfo);

			MenuInflater menuInflater = new MenuInflater (this);
			menuInflater.Inflate (Resource.Menu.Filter_Menu, menu);
		}

		public override bool OnContextItemSelected (IMenuItem item)
		{
			Dictionary<int, string> itemIDtoButtonText = new Dictionary<int, string> () {
				{Resource.Id.Time, "Time"},
				{Resource.Id.Difficulty, "Difficulty"},
				{Resource.Id.Both, "Both"}
			};

			Dictionary<int, string> itemIDtoOrderType = new Dictionary<int, string> () {
				{Resource.Id.Time, "Time"},
				{Resource.Id.Difficulty, "Diff"},
				{Resource.Id.Both, "Both"}
			};

			base.OnContextItemSelected(item);

			string buttonText = itemIDtoButtonText [item.ItemId];
			this.filter_button.Text = buttonText;
			string orderType = itemIDtoOrderType [item.ItemId];
			this.ordertype = orderType;

			showFiltered (null);

			return true;
		}

		public void showFiltered(string input) {
			if (input != null) {
				this.mostRecentKeywords = input.Replace (" ", ",");
			}
			if (CachedData.Instance.PreviousActivity.GetType () == typeof(SubtypeBrowseActivity)) {
				string selectionInput = CachedData.Instance.SelectedSubgenre.Replace (' ', ',');
				string url = "http://speedychef.azurewebsites.net/search/searchbyunion?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc + "&subgenre=" + selectionInput;
				this.ProcessSingleSearchQuery (url, "SearchByUnion");
			} else {
				string url = "http://speedychef.azurewebsites.net/search/search?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc;
				this.ProcessSingleSearchQuery (url, "Search");
			}
		}

		public void OnItemClick (object sender, int position)
		{
			int exampleInt = position + 1;
			System.Diagnostics.Debug.WriteLine (exampleInt);
		}

		public bool OnQueryTextChange(string input)
		{
			showFiltered (input);
			return true;
		}

		public void ProcessSingleSearchQuery(string inURL, string inMethod){
			lock (thisLock) {
				// Create an HTTP web request using the URL:
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (inURL));
				request.ContentType = "application/json";
				request.Method = inMethod;

				// Send the request to the server and wait for the response:
				using (WebResponse response = request.GetResponse ()) {
					// Get a stream representation of the HTTP web response:
					using (Stream stream = response.GetResponseStream ()) {
						// Use this stream to build a JSON document object:
						this.jsonDoc = JsonObject.Load (stream);
					}
				}
				int tempNum = this.mObject.NumElements;
				for (int i = this.mObject.NumElements - 1; i > -1; i--) {
					this.mObject.Remove (i);
					this.mAdapter.NotifyItemRemoved (i);
				}
				for (int k = 0; k < this.jsonDoc.Count; k++) {
					this.mObject.Add (new Tuple<string, string, int, int, int> (this.jsonDoc [k] ["Recname"], this.jsonDoc [k] ["Recdesc"], this.jsonDoc [k] ["Recdiff"], this.jsonDoc[k] ["Rectime"], this.jsonDoc [k] ["Recid"]));
					this.mAdapter.NotifyItemInserted (k);
				}
			}
		}

		public bool OnQueryTextSubmit(string input)
		{
			showFiltered (input);
			return true;
		}

		public bool OnSuggestionSelect (int position)
		{
			return false;
		}

		public bool OnSuggestionClick (int position)
		{
			return false;
		}

	}

	public class RecipeViewHolder : v7Widget.RecyclerView.ViewHolder
	{
		public TextView LeftText { get; private set; }
		public TextView RightText { get; private set; }
		public Activity callingActivity;
		// Locate and cache view references

		public RecipeViewHolder (View itemView, Activity callingActivity, Action<int> listener) : base (itemView)
		{
			this.callingActivity = callingActivity;
			LeftText = itemView.FindViewById<TextView> (Resource.Id.textViewLeft);
			RightText = itemView.FindViewById<TextView> (Resource.Id.textViewRight);
			itemView.Click += (sender, e) => listener (base.AdapterPosition);
		}
	}

	public class RecipeAdapter : v7Widget.RecyclerView.Adapter
	{
		public RecipeObject mRObject;
		public CustomActivity callingActivity; 
		public event EventHandler<int> itemClick;
		public int Recid;

		public RecipeAdapter (RecipeObject inRObject, CustomActivity inActivity)
		{
			this.mRObject = inRObject;
			this.callingActivity = inActivity;
		}

		public override v7Widget.RecyclerView.ViewHolder
		OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From (parent.Context).
				Inflate (Resource.Layout.TwoByTwoResultView, parent, false);
			RecipeViewHolder vh = new RecipeViewHolder (itemView, this.callingActivity, OnClick);
			return vh;
		}

		public void OnClick (int position)
		{
			if (this.itemClick != null) 
			{
				this.itemClick (this, position);
				var intent = new Intent (callingActivity, typeof(RecipeViewActivity));
				CachedData.Instance.PreviousActivity = callingActivity;
				CachedData.Instance.mostRecentRecSel = this.mRObject.RecipeList[position].Item5;
				callingActivity.StartActivity (intent);
			}
		}


		public override void OnBindViewHolder (v7Widget.RecyclerView.ViewHolder holder, int position)
		{
			RecipeViewHolder vh = holder as RecipeViewHolder;
			Tuple<string, string, int, int, int> tupleInQuestion = mRObject.getObjectInPosition (position);
			string tempLeftText = tupleInQuestion.Item1;
			string tempRightText = tupleInQuestion.Item2;
			this.Recid = tupleInQuestion.Item5;

			Dictionary<int, Android.Graphics.Color> intToColor = new Dictionary<int, Android.Graphics.Color> () {
				{ 5, Android.Graphics.Color.Red },
				{ 4, Android.Graphics.Color.Orange },
				{ 3, Android.Graphics.Color.Yellow },
				{ 2, Android.Graphics.Color.GreenYellow },
				{ 1, Android.Graphics.Color.Green }
			};

			vh.LeftText.SetBackgroundColor (intToColor[tupleInQuestion.Item3]);
			vh.RightText.SetBackgroundColor (intToColor[tupleInQuestion.Item3]);

			vh.LeftText.Text = tempLeftText;
			vh.RightText.Text = tempRightText;
		}

		public override int ItemCount
		{
			get { return mRObject.NumElements ; }
		}
	}

	public class RecipeObject
	{
		public int NumElements;
		public List<Tuple<string, string, int, int, int>> RecipeList;
		private Object thisLock = new Object();

		public RecipeObject ()
		{
			this.RecipeList = new List<Tuple<string, string, int, int, int>>();
			this.NumElements = this.RecipeList.Count;
		}

		public RecipeObject (List<Tuple<string, string, int, int, int>> inList)
		{
			this.RecipeList = inList;
			this.NumElements = this.RecipeList.Count;
		}

		public Tuple<string, string, int, int, int> getObjectInPosition(int position)
		{
			return this.RecipeList [position];
		}

		public void Add(Tuple<string, string, int, int, int> newTuple){
			this.RecipeList.Add (newTuple);
			this.NumElements = this.RecipeList.Count;
		}

		public void Remove(int position){
			this.RecipeList.RemoveAt (position);
			this.NumElements -= 1;
		}
	}
}