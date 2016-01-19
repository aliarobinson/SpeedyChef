using System;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;

namespace SpeedyChef
{
	public class RecipeStepTimerHandler : ITimerObservable, ITimerObserver
	{
		private RecipeStepTimer recipeStepTimer;
		private List<ITimerObserver> observers;
		private int fullTime;
		private int timeLeft;
	//	private ViewGroup timerFrame;
	//	private int timerIndex;
		private string timerName;

		public RecipeStepTimerHandler (string s, int t)
		{
			fullTime = t;
			timeLeft = t;
			timerName = s;
			recipeStepTimer = new RecipeStepTimer (t);
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

		public void timerUpdate(int secondsLeft) {
			timeLeft = secondsLeft;
		}

		/*public void SetViews(TextView stepView, ViewGroup timerFrame) {
			recipeStepTimer.SetStepTextView (stepView);
			setTimerFrame (timerFrame);
			}

		public void SetStepView(TextView stepView) {
			recipeStepTimer.SetStepTextView (stepView);
		}

		public ViewGroup getTimerFrame() {
			return this.timerFrame;
		}

		public void setTimerFrame(ViewGroup timerFrame) {
			this.timerFrame = timerFrame;
			recipeStepTimer.SetBarTextView (timerFrame.FindViewById<TextView> (Resource.Id.walkthrough_time));
			recipeStepTimer.SetProgressBar (timerFrame.FindViewById<ProgressBar> (Resource.Id.walkthrough_bar));
			timerFrame.FindViewById<TextView> (Resource.Id.walkthrough_text).Text = this.timerName;
			recipeStepTimer.TimeUpdate ();
		}

		public int getTimerIndex() {
			return this.timerIndex;
		}

		public void setTimerIndex(int x) {
			this.timerIndex = x;
		}*/

		public string getTimerName() {
			return this.timerName;
		}

		public void getFullTime() {
			return this.fullTime;
		}

	/*	public void setTimerName(string s) {
			this.timerName = s;
		}*/

		public void StartTimer() {
			recipeStepTimer.Start ();
		}
			
		public void PauseTimer() {
//			recipeStepTimer.deactivate ();
		recipeStepTimer.Cancel();
			recipeStepTimer = new RecipeStepTimer (time);
			recipeStepTimer.addObserver (this);
			/*newTimer.SetStepTextView (recipeStepTimer.GetStepTextView ());
			newTimer.SetBarTextView (recipeStepTimer.GetBarTextView ());
			newTimer.SetProgressBar (recipeStepTimer.GetProgressBar ());*/
		}

		/*public bool IsActive() {
			return recipeStepTimer.IsActive ();
		}*/

	}
}

