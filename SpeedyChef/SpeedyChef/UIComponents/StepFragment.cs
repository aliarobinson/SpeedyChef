using System;
using Android.Views;
using Android.OS;
using Android.Widget;

namespace SpeedyChef
{
	//This class constructs a fragment to display an individual recipe step
	public class StepFragment : Android.Support.V4.App.Fragment {

		private RecipeStep recipeStep;
		private TimerPoolHandler handler;

		public StepFragment(RecipeStep s, TimerPoolHandler h) {
			this.recipeStep = s;
			this.handler = h;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
			ViewGroup rootView = (ViewGroup) inflater.Inflate (Resource.Layout.Step, container, false);

			TextView titleTv = (TextView) rootView.FindViewById (Resource.Id.step_title);
			ImageView imgv = (ImageView) rootView.FindViewById (Resource.Id.step_image);
			TextView descTv = (TextView) rootView.FindViewById (Resource.Id.step_desc);
			TextView timeTv;
			if (this.recipeStep.timeable) { // Add a timer
				rootView.FindViewById (Resource.Id.step_timer_wrapper).Visibility = ViewStates.Visible;
				timeTv = (TextView) rootView.FindViewById (Resource.Id.step_timer_display);
				Button startButton = rootView.FindViewById<Button> (Resource.Id.step_timer_start_button);
				/*if (this.recipeStep.timerHandler.IsActive ()) {
					startButton.SetText (Resource.String.pause);
				} else {
					timeTv.Text = (this.recipeStep.time / 60).ToString () + ":00";

				}
				handler.AssignFragView (this.recipeStep.timerHandler, timeTv, startButton, ((StepsActivity)Activity).GetViewPager());
*/
			}
			else { // Don't add a timer, just display the time estimate
				rootView.FindViewById (Resource.Id.step_static_time).Visibility = ViewStates.Visible;
				timeTv = (TextView) rootView.FindViewById (Resource.Id.step_static_time);
				timeTv.Text = (this.recipeStep.time / 60).ToString() + Resources.GetString(Resource.String.minute_short);
			}

			titleTv.Text = this.recipeStep.title;
			descTv.Text = this.recipeStep.desc;

			return rootView;
		}

	}
}

