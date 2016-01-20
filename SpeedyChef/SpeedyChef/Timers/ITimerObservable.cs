using System;

namespace SpeedyChef
{
	public interface ITimerObservable
	{
		void addObserver(ITimerObserver o);
		void removeObserver(ITimerObserver o);
		void notifyObservers(int seconds);
	}
}

