using System;

namespace SpeedyChef
{
	public interface ITimerObserver
	{
		void timerUpdate(int secondsLeft);
	}
}

