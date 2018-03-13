using System;
using System.Xml.Serialization;

namespace Sim.Module.Generic {
	public interface IRange<T> : IXmlSerializable where T : IEquatable<T>, IComparable<T>
	{
		T Min { get; }
		T Max { get; }
		T Clamp(T value);
		bool IsInside(T value, bool strictly = false);
		bool IsAbove(T value, bool strictly = false);
		bool IsBelow(T value, bool strictly = false);
	}
}