
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
using Java.Lang;

namespace SpeedyChef
{

	[Activity (Theme="@style/MyTheme", Label = "StepsActivity", Icon = "@drawable/icon")]			
	public class StepsActivity : CustomActivity
	{

		RecipeStep[] steps; 
		TimerDisplayFrame[] timerFrames;
		TimerPoolHandler timerPoolHandler;
		ViewPager vp;
		int fragmentCount;

		//Called when the page is created
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			int mealId = Intent.GetIntExtra ("mealId", 0);
			Console.WriteLine ("Recipe Id: " + mealId);

			SetContentView (Resource.Layout.Walkthrough);
			vp = FindViewById<ViewPager> (Resource.Id.walkthrough_pager);

			//Store pointers to timer frames to be referenced by the fragments
			timerFrames = new TimerDisplayFrame[5];
			timerFrames [0] = new TimerDisplayFrame(FindViewById<ViewGroup> (Resource.Id.walkthrough_frame_1));
			timerFrames [1] = new TimerDisplayFrame(FindViewById<ViewGroup> (Resource.Id.walkthrough_frame_2));
			timerFrames [2] = new TimerDisplayFrame(FindViewById<ViewGroup> (Resource.Id.walkthrough_frame_3));
			timerFrames [3] = new TimerDisplayFrame(FindViewById<ViewGroup> (Resource.Id.walkthrough_frame_4));
			timerFrames [4] = new TimerDisplayFrame(FindViewById<ViewGroup> (Resource.Id.walkthrough_frame_5));

			timerPoolHandler = new TimerPoolHandler (timerFrames);

			//TODO fix
			steps = WebUtils.getRecipeSteps (mealId);
			fragmentCount = steps.Length + 1;

			vp.Adapter = new StepFragmentPagerAdapter (SupportFragmentManager, steps, timerPoolHandler);

			//Set up the progress dots to appear at the bottom of the screen
			ViewGroup pd = (ViewGroup) FindViewById (Resource.Id.walkthrough_progress_dots);
			NavDot[] progressDots = new NavDot[fragmentCount];
			Drawable open = Resources.GetDrawable (Resource.Drawable.circle_open);

			for (int i = 0; i < progressDots.Length; i++) {
				NavDot dot = new NavDot (this);
				dot.SetMaxWidth(30);
				dot.SetImageDrawable (open);
				pd.AddView (dot);
				dot.Num = i;
				dot.Click += delegate {
					Console.WriteLine("Going to page " + dot.Num);
					vp.SetCurrentItem (dot.Num, true);
				};
				progressDots [i] = dot;
			}
			progressDots[0].SetImageDrawable (Resources.GetDrawable(Resource.Drawable.circle_closed));

			ViewGroup pbs = (ViewGroup)FindViewById (Resource.Id.walkthrough_progress_bars);

			vp.AddOnPageChangeListener (new StepChangeListener (progressDots, open, Resources.GetDrawable(Resource.Drawable.circle_closed)));

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

		public ViewPager GetViewPager() {
			return vp;
		}
	}

	class StepChangeListener : ViewPager.SimpleOnPageChangeListener {
		NavDot[] dots;
		Drawable open;
		Drawable closed;
		int selected;

		public StepChangeListener(NavDot[] dots, Drawable open, Drawable closed) : base() {
			this.dots = dots;
			this.open = open;
			this.closed = closed;
			this.selected = 0;
		}

		public override void OnPageSelected (int position) {
			dots [selected].SetImageDrawable (open);
			selected = position;
			dots [position].SetImageDrawable (closed);
		}
	}

	class NavDot : ImageView {
		public int Num;

		public NavDot(Context c) : base(c) {

		}
	}
}

