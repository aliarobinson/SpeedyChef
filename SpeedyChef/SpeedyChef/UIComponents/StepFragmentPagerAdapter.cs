using System;
using Android.Views;
using Android.OS;
using Android.Widget;

namespace SpeedyChef
{
	public class StepFragmentPagerAdapter : Android.Support.V4.App.FragmentStatePagerAdapter {
		private RecipeStep[] steps;
		private TimerPoolHandler handler;

		public StepFragmentPagerAdapter (Android.Support.V4.App.FragmentManager fm, RecipeStep[] steps, TimerPoolHandler handler) : base(fm) {
			this.steps = steps;
			this.handler = handler;
		}

		public override Android.Support.V4.App.Fragment GetItem(int position) {
			if (position < steps.Length)
				return new StepFragment (steps [position], handler);
			return new FinishedFragment ();
		}

		public override int Count {
			get {
				return steps.Length + 1;
			}
		}

	}

	//Fragment to display when the user has gone through all of the steps
	class FinishedFragment : Android.Support.V4.App.Fragment {

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
			View view = inflater.Inflate (Resource.Layout.FinalStep, container, false);
			Button finishButton = view.FindViewById<Button> (Resource.Id.walkthrough_finish_button);
			finishButton.Click += delegate {
				CachedData.Instance.PreviousActivity = new StepsActivity();
				Activity.Finish();
			};
			return view;
		}
	}
}

