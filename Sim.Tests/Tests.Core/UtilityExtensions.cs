using System;
using System.Diagnostics;

namespace Sim.Tests.Core {
	public static class UtilityExtensions
	{
		public static void LogDiag(this string source)
		{
			Debug.WriteLine(string.IsNullOrEmpty(source)
				? "[null]"
				: source);
		}

		public static void LogAux(this string source)
		{
			Console.WriteLine(string.IsNullOrEmpty(source)
				? "[null]"
				: source);
		}
	}
}