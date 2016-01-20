using System;
using NUnit.Framework;
using SpeedyChef;

namespace UnitTests
{
	public class TimerTests
	{
		MockTimer mockTimer;

		[SetUp]
		public void Setup ()
		{
			mockTimer = new MockTimer ("timerName", 5);
		}


		[TearDown]
		public void Tear ()
		{
			//Fill in if needed
		}

		[Test]
		public void TestTimer ()
		{
			MockTimerObserver ob = new MockTimerObserver ();
			mockTimer.addObserver (ob);
			Assert.AreEqual ("timerName", mockTimer.GetTimerName ());
			Assert.AreEqual (5, mockTimer.GetFullTime ());
			Assert.IsFalse (mockTimer.IsActive ());

			mockTimer.StartTimer ();
			Assert.IsTrue (mockTimer.IsActive ());
			mockTimer.simulateTimerTick ();
			mockTimer.simulateTimerTick ();
			mockTimer.simulateTimerTick ();
			mockTimer.simulateTimerTick ();
			mockTimer.simulateTimerTick ();

			Assert.AreEqual (5, mockTimer.GetFullTime ());
			Assert.IsFalse (mockTimer.IsActive ());

		}

		[Test]
		public void TestTimerHandler() {
			TimerDisplayFrame frame1 = new TimerDisplayFrame (null);
			TimerDisplayFrame[] frames = new TimerDisplayFrame[1];
			frames [0] = frame1;
			TimerPoolHandler poolHandler = new TimerPoolHandler (frames);
			poolHandler.ActivateTimer (mockTimer);
			Assert.IsTrue (mockTimer.IsActive ());
			poolHandler.DeactivateTimer (mockTimer);
			Assert.IsFalse (mockTimer.IsActive ());
		}
	}

	class MockTimer : RecipeStepTimerHandler {

		private int ticks;

		public MockTimer(String name, int t) : base(name, t) {
			this.ticks = t;
		}

		public void simulateTimerTick() {
			this.ticks--;
			timerUpdate (this.ticks);
		}

		// Don't actually use a timer during unit testing
		public override void StartTimer() {
			this.active = true;
		}

		public override void PauseTimer() {
			this.active = false;
		}
	}

	class MockTimerObserver : ITimerObserver {
		int numTimerUpdates = 0;

		public void timerUpdate(int secondsLeft) {
			this.numTimerUpdates++;	
		}

		public int getNumUpdates() {
			return this.numTimerUpdates;
		}
	}
}

