using System;

namespace Sim.Module.Resources
{
	[Serializable]
	public struct ResourceLocator : IEquatable<ResourceLocator>
	{
		public string Filename;
		public string GroupName;

		public bool Equals(ResourceLocator other)
		{
			return string.Compare(Filename, other.Filename, StringComparison.CurrentCultureIgnoreCase) == 0;
		}

		public override string ToString()
		{
			return string.IsNullOrEmpty(Filename) ? "undefined" : $"{Filename}";
		}
	}
}