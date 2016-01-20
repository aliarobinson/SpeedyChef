using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;

namespace SpeedyChef.UITests
{
	[TestFixture]
	public class TimersTest
	{
		AndroidApp aApp;

		[SetUp]
		public void BeforeEachTest ()
		{
			aApp = ConfigureApp.Android.StartApp ();
		}

		[Test]
		public void ConfirmTimerStartsOnTap ()
		{
			aApp.Repl ();
		}
	}
}

