using System;
using Android.Views;
using System.Collections.Generic;
using Android.Widget;
using Android.Support.V4.View;

namespace SpeedyChef
{
	public class TimerPoolHandler
	{
		private TimerDisplayFrame[] displays;
		private RecipeStepTimerHandler[] timers;
		private int timerIndex;
		private int maxTimers;

		public TimerPoolHandler (TimerDisplayFrame[] displays)
		{
		//	this.timerFrames = timerFrames;
			this.maxTimers = displays.Length;
			this.timerIndex = 0;
			this.displays = displays;
			this.timers = new RecipeStepTimerHandler[maxTimers];
		}

		/*public RecipeStepTimerHandler[] getTimers() {
			return timers;
		}*/

		/*public void timerUpdate(int secondsLeft) {
			foreach(ITimerObserver ob in this.observers) {
				ob.timerUpdate (secondsLeft);
			}
		}*/

	/*	public void AddTimer(RecipeStep recipeStep, Android.Widget.TextView textView, Button button) {

			//Add timer to list
			/*if (timers.Add (recipeStep.timerHandler)) {
				AssignTimerView (recipeStep, textView, timerFrames [timerIndex], button);
				timerIndex++;
			}

		}*/

		/*private void AssignTimerView(RecipeStep recipeStep, TextView textView, ViewGroup timerFrame, Button button) {
			RecipeStepTimerHandler t = recipeStep.timerHandler;
			t.SetViews (textView, timerFrame);
			timerFrame.FindViewById<TextView> (Resource.Id.walkthrough_text).Text = recipeStep.title;
			AssignButtonFunction (t, button);
		}*/

		/*public void AssignFragView(RecipeStepTimerHandler t, TextView textView, Button button, ViewPager vp) {
			t.SetStepView (textView);
			AssignButtonFunction (t, button, vp);
		}

		private void AssignButtonFunction(RecipeStepTimerHandler t, Button button, ViewPager vp) {
			//ViewGroup timerFrame = t.getTimerFrame ();
			button.Click += delegate {
				if (t.IsActive ()) {
					DeactivateTimer(t, button);
				} else {
					ActivateTimer(t, button);
					//ViewPager vp = ((StepsActivity)Activity).GetViewPager();
					int pos = vp.CurrentItem + 1;
					vp.SetCurrentItem (pos, true);
					/*t.StartTimer ();
					button.SetText (Resource.String.pause);
					timerFrame.Visibility = ViewStates.Visible;
				}
			};
		}*/



		public bool ActivateTimer(RecipeStepTimerHandler t) {
			Console.WriteLine ("ACTIVATING TIMER");
			if (timerIndex >= timers.Length)
				return false;
			this.timers [timerIndex] = t;
			this.displays [timerIndex].setTimer (t);
			t.StartTimer ();
			//button.SetText (Resource.String.pause);

			//ViewGroup timerFrame = observers [timerIndex];
			//timers [timerIndex] = t;
			//t.setTimerFrame (timerFrame);
			//t.setTimerIndex (timerIndex);
			//timerFrame.Visibility = ViewStates.Visible;
			//Console.WriteLine ("Activating timer " + timerIndex);
			timerIndex++;
			return true;
		}

		public bool DeactivateTimer(RecipeStepTimerHandler t) {
			//ViewGroup timerFrame = t.getTimerFrame ();
				int timerPosition = Array.IndexOf(timers, t);
			if (timerPosition < 0)
				return false;
			t.PauseTimer ();
		/*	button.SetText (Resource.String.start);
			Console.WriteLine ("Deactivating timer " + t.getTimerIndex());
			Console.WriteLine ("Total Timer Index: " + timerIndex);
*/

			//Shift all active timers down
			for (int i = timerPosition; i < timerIndex; i++) {
				displays [i].clearTimer ();
				timers [i] = timers [i + 1];
				if (timers [i] != null) {
					displays [i].setTimer (timers [i]);
				}
			}
			timerIndex--;
			return true;
		}
			

		//TODO allow for removing inactive timers
	}
			
}

