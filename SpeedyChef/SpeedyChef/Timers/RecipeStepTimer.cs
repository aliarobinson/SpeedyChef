using System;
using System.Collections.Generic;
using Android.OS;
using Android.Widget;

namespace SpeedyChef
{
	public class RecipeStepTimer : CountDownTimer, ITimerObservable
	{
	/*	private TextView bar_tv;
		private TextView step_tv;
		private ProgressBar pb; */

		//private bool active;
		//private int secondsLeft;
	//	private int maxSeconds;
		private List<ITimerObserver> observers;

		public RecipeStepTimer (int seconds) : base (seconds * 1000, 1000)
		{
			/*this.bar_tv = bartv;
			this.step_tv = steptv;
			this.pb = pb;
			pb.Max = seconds;*/

			//active = false;
			//secondsLeft = seconds;
	//		maxSeconds = seconds;
			observers = new List<ITimerObserver> ();
		}

		public void addObserver(ITimerObserver o) {
			observers.Add (o);
		}
			
		public void removeObserver(ITimerObserver o) {
			observers.Remove (o);
		}

		public void notifyObservers(int seconds) {
			foreach(ITimerObserver ob in observers) {
				ob.timerUpdate (seconds);
			}
		}

		public override void OnTick(long millisUntilFinished) {
			int secondsLeft = (int) (millisUntilFinished / 1000);
			notifyObservers (secondsLeft);
			//TimeUpdate ();
		}

		/*public void TimeUpdate() {
			int seconds = secondsLeft;
			int mins = seconds / 60;
			seconds = seconds % 60;
			string display;
			if (seconds < 10) {
				display = mins + ":0" + seconds;
			} else {
				display = mins + ":" + seconds;
			}

			UpdateTextView (display);

			if (pb != null) {
				pb.IncrementProgressBy (1);
			}
		}*/

		public override void OnFinish() {
			//UpdateTextView ("00:00");
			//active = false;
			notifyObservers(0);
		}

		/*public void activate() {
			active = true;
			Start ();
		}

		public void deactivate() {
			active = false;
			Cancel ();
		}

		public bool IsActive() {
			return active;
		}*/

	/*	public int getSecondsLeft() {
			return secondsLeft;
		}

		public TextView GetStepTextView() {
			return step_tv;
		}

		public TextView GetBarTextView() {
			return bar_tv;
		} 

		public ProgressBar GetProgressBar() {
			return pb;
		}

		public void SetStepTextView(TextView tv) {
			step_tv = tv;
		}

		public void SetBarTextView(TextView tv) {
			bar_tv = tv;
		} 

		public void SetProgressBar(ProgressBar bar) {
			pb = bar;
			pb.Max = maxSeconds;
		}

		public void UpdateTextView(String s) {
			if (bar_tv != null) {
				bar_tv.Text = s;
			}
			if (step_tv != null) {
				step_tv.Text = s;
			}
		} */
	}
}

