// FireAndForgetWithoutLeaking.cs
//
// Starting with the 1.1 release of the .NET Framework, the SDK docs
// now carry a caution that mandates calling EndInvoke on delegates
// you've called BeginInvoke on in order to avoid potential leaks.
// This means you cannot simply "fire-and-forget" a call to BeginInvoke
// without the risk of running the risk of causing problems.
//
// This sample provides an AsyncHelper class with one public method
// called FireAndForget that is intended to support the fire-and-forget
// idiom without the fear of leak.  The usage model is that instead of calling
// BeginInvoke against a delegate, you would instead call
// AsyncHelper.FireAndForget, passing that delegate and it's parameters
// as input.
//
// For example, assuming a delegate defined as follows:
//
//      delegate void CalcAndDisplaySumDelegate( int a, int b );
//
// Instead of doing this to fire-and-forget an async call to some
// target method:
//
// CalcAndDisplaySumDelegate d = new CalcAndDisplaySumDelegate(someCalc.Add);
// d.BeginInvoke(2, 3, null);
//
// You would instead do this:
//
// CalcAndDisplaySumDelegate d = new CalcAndDisplaySumDelegate(someCalc.Add);
// AsyncHelper.FireAndForget(d, 2, 3);
//
// Mike Woodring
// http://staff.develop.com/woodring
//
using System;
using System.Threading;

namespace Subtext.Framework.Threading
{
	public sealed class AsyncHelper
	{
		class TargetInfo
		{
			internal TargetInfo(Delegate d, object[] args)
			{
				Target = d;
				Args = args;
			}

			internal readonly Delegate Target;
			internal readonly object[] Args;
		}

		private static WaitCallback dynamicInvokeShim = new WaitCallback(DynamicInvokeShim);

		public static void FireAndForget(Delegate d, params object[] args)
		{
			ThreadPool.QueueUserWorkItem(dynamicInvokeShim, new TargetInfo(d, args));
		}

		static void DynamicInvokeShim(object o)
		{
			TargetInfo ti = (TargetInfo)o;
			ti.Target.DynamicInvoke(ti.Args);
		}
	}
}